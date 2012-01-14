// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestRig.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   A Performance Test Rig for SharpChess
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region License

// SharpChess
// Copyright (C) 2012 SharpChess.com
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

namespace SharpChess_Performance_Tester
{
    #region Using

    using System;
    using System.Diagnostics;
    using System.Threading;

    #endregion

    /// <summary>
    /// A Performance Test Rig for SharpChess
    /// </summary>
    public class TestRig
    {
        #region Constants and Fields

        /// <summary>
        /// The sharp chess listener thread.
        /// </summary>
        private Thread sharpChessListenerThread;

        /// <summary>
        ///   References the SharpChess exe process
        /// </summary>
        private Process sharpChessProcess;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRig"/> class. 
        /// Creates an instance of a TestRig
        /// </summary>
        /// <param name="sharpChessExePath">
        /// Path to SharChess executable
        /// </param>
        public TestRig(string sharpChessExePath)
        {
            this.SharpChessExePath = sharpChessExePath;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Raises an event describing activity within the test rig.
        /// </summary>
        public event EventHandler<MessageEventArgs> RaiseMessageEvent;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the search depth.
        /// </summary>
        public uint SearchDepth { get; set; }

        /// <summary>
        ///   Gets or sets the full path to the SharpChess executable.
        /// </summary>
        public string SharpChessExePath { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The on raise message event.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        public virtual void OnRaiseMessageEvent(MessageEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<MessageEventArgs> handler = this.RaiseMessageEvent;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                e.Message = DateTime.Now.ToString("hh:mm:ss") + ": " + e.Message;

                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

        /// <summary>
        /// The report message.
        /// </summary>
        /// <param name="Message">
        /// The message.
        /// </param>
        public void ReportMessage(string Message)
        {
            this.OnRaiseMessageEvent(new MessageEventArgs(Message));
        }

        /// <summary>
        /// Reports cosmetic startup messages.
        /// </summary>
        public void ReportStartupMessages()
        {
            this.ReportMessage("SharpChess Test Rig Initialised.");
            this.ReportMessage("SharpChess path: " + this.SharpChessExePath);
        }

        /// <summary>
        /// Start the listening that listens for SharpChess messages
        /// </summary>
        public void StartListener()
        {
            this.ReportMessage("Starting SharpChess listener...");
            this.sharpChessListenerThread = new Thread(this.Listen);
            this.sharpChessListenerThread.Priority = ThreadPriority.Normal;
            this.sharpChessListenerThread.Start();
            this.ReportMessage("SharpChess listener started.");
        }

        /// <summary>
        /// Loads SharpChess, listens for SharpChess messages, and sends appropriate commands
        /// </summary>
        public void StartTest()
        {
            this.ReportMessage("Test starting...");

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = this.SharpChessExePath;

            // info.Arguments = "";
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;

            this.ReportMessage("Loading SharpChess: " + this.SharpChessExePath);

            this.sharpChessProcess = Process.Start(info);

            if (!this.sharpChessProcess.HasExited)
            {
                this.ReportMessage("SharpChess Loaded.");

                Console.SetIn(this.sharpChessProcess.StandardOutput);
                Console.SetOut(this.sharpChessProcess.StandardInput);

                this.StartListener();

                // Set SharpChess into tournament mode
                // this.SendCommand("xboard showgui"); // Use this command to show the SharpChess GUI, when debugging, or just for fun!
                this.SendCommand("xboard");

                // Load test position (a SharpChess save game)
                this.SendCommand(@"load ..\..\..\SharpChess Performance Tester\TestPosition.sharpchess");

                // Set the maximum search depth (plies)
                this.SendCommand("depth " + this.SearchDepth.ToString());

                // Set the maximum search depth (plies)
                this.SendCommand("go");
            }
        }

        /// <summary>
        /// Stop the listening that listens for SharpChess messages
        /// </summary>
        public void StopListener()
        {
            this.ReportMessage("Stopping SharpChess listener...");
            this.sharpChessListenerThread.Abort();

            // this.sharpChessListenerThread.Join();
            this.sharpChessListenerThread = null;
            this.ReportMessage("SharpChess listener stopped.");
        }

        /// <summary>
        /// Loads SharpChess, listens for SharpChess messages, and sends appropriate commands
        /// </summary>
        public void StopTest()
        {
            this.ReportMessage("Stopping test...");
            this.StopListener();
            this.ReportMessage("Killing SharpChess process.");
            this.sharpChessProcess.Kill();
            this.ReportMessage("Test stopped.");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Listen and process SharpChess messages
        /// </summary>
        private void Listen()
        {
            string messageReceived = string.Empty;
            while (true)
            {
                messageReceived = Console.ReadLine();
                if (messageReceived == null)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    this.ProcessResponse(messageReceived);
                }
            }
        }

        /// <summary>
        /// Handle reponse message sent from SharpChess
        /// </summary>
        /// <param name="messageReceived">
        /// Message received
        /// </param>
        private void ProcessResponse(string messageReceived)
        {
            this.ReportMessage("Received: " + messageReceived);
        }

        /// <summary>
        /// Send a command to SharpChess.
        /// </summary>
        /// <param name="command">
        /// Commmand sent
        /// </param>
        private void SendCommand(string command)
        {
            this.ReportMessage("Sending: " + command);
            Console.WriteLine(command);
        }

        #endregion

        // Copied from internet... Stuff for raising an event. 

        /// <summary>
        /// Custom event class containing message from or from SharpChess
        /// </summary>
        public class MessageEventArgs : EventArgs
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="MessageEventArgs"/> class.
            /// </summary>
            /// <param name="s">
            /// The s.
            /// </param>
            public MessageEventArgs(string s)
            {
                this.Message = s;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets or sets Message.
            /// </summary>
            public string Message { get; set; }

            #endregion
        }
    }
}