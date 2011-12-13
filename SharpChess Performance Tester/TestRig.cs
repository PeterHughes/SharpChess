using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace SharpChess_Performance_Tester
{
    /// <summary>
    /// A Performance Test Rig for SharpChess
    /// </summary>
    public class PerformanceTestRig
    {
        /// <summary>
        /// References the SharpChess exe process
        /// </summary>
        Process sharpChessProcess;
        private Thread sharpChessListenerThread = null;

        /// <summary>
        /// Raises an event describing activity within the test rig.
        /// </summary>
        public event EventHandler<MessageEventArgs> RaiseMessageEvent;

        /// <summary>
        /// Gets or sets the search depth.
        /// </summary>
        public uint SearchDepth { get; set; }

        /// <summary>
        /// Gets or sets the full path to the SharpChess executable.
        /// </summary>
        public string SharpChessExePath { get; set; }

        /// <summary>
        /// Creates an instance of a TestRig
        /// </summary>
        /// <param name="sharpChessExePath"></param>
        public PerformanceTestRig(string sharpChessExePath)
        {
            this.SharpChessExePath = sharpChessExePath;
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
        /// Loads SharpChess, listens for SharpChess messages, and sends appropriate commands
        /// </summary>
        public void StartTest()
        {
            this.ReportMessage("Test starting...");

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = SharpChessExePath;
            //info.Arguments = "";
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;

            this.ReportMessage("Loading SharpChess: " + SharpChessExePath);

            this.sharpChessProcess = Process.Start(info);

            if (!this.sharpChessProcess.HasExited)
            {

                this.ReportMessage("SharpChess Loaded.");

                Console.SetIn(this.sharpChessProcess.StandardOutput);
                Console.SetOut(this.sharpChessProcess.StandardInput);

                this.StartListener();

                // Set SharpChess into tournament mode
//                this.SendCommand("xboard showgui"); // Use this command to show the SharpChess GUI, when debugging, or just for fun!
                this.SendCommand("xboard");

                // Load test position (a SharpChess save game)
                this.SendCommand(@"load ..\..\..\SharpChess Performance Tester\TestPosition.sharpchess");

                // Set the maximum search depth (plies)
                this.SendCommand("depth " + this.SearchDepth.ToString() );

                // Set the maximum search depth (plies)
                this.SendCommand("go");
            }
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

        /// <summary>
        /// Start the listening that listens for SharpChess messages
        /// </summary>
        public void StartListener()
        {
            this.ReportMessage("Starting SharpChess listener...");
            this.sharpChessListenerThread = new Thread(new ThreadStart(Listen));
            this.sharpChessListenerThread.Priority = System.Threading.ThreadPriority.Normal;
            this.sharpChessListenerThread.Start();
            this.ReportMessage("SharpChess listener started.");
        }

        /// <summary>
        /// Stop the listening that listens for SharpChess messages
        /// </summary>
        public void StopListener()
        {
            this.ReportMessage("Stopping SharpChess listener...");
            this.sharpChessListenerThread.Abort();
            //this.sharpChessListenerThread.Join();
            this.sharpChessListenerThread = null;
            this.ReportMessage("SharpChess listener stopped.");
        }

        /// <summary>
        /// Listen and process SharpChess messages
        /// </summary>
        private void Listen()
        {
            string messageReceived = "";
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
        /// Send a command to SharpChess.
        /// </summary>
        /// <param name="command"></param>
        private void SendCommand(string command)
        {
            this.ReportMessage("Sending: " + command);
            Console.WriteLine(command);
        }

        /// <summary>
        /// Handle reponse message sent from SharpChess
        /// </summary>
        /// <param name="messageReceived"></param>
        private void ProcessResponse(string messageReceived)
        {
            this.ReportMessage("Received: " + messageReceived);
        }

        #region MessageEventStuff
        // Copied from internet... Stuff for raising an event. 

        public void ReportMessage(string Message)
        {
            OnRaiseMessageEvent(new MessageEventArgs(Message));
        }

        // Wrap event invocations inside a protected virtual method
        // to allow derived classes to override the event invocation behavior
        public virtual void OnRaiseMessageEvent(MessageEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<MessageEventArgs> handler = RaiseMessageEvent;

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
        /// Custom event class containing message from or from SharpChess
        /// </summary>
        public class MessageEventArgs : EventArgs
        {
            public MessageEventArgs(string s)
            {
                message = s;
            }
            private string message;

            public string Message
            {
                get { return message; }
                set { message = value; }
            }
        }
        #endregion
    }
}
