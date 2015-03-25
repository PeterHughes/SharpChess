// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The main.
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
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// The main.
    /// </summary>
    public partial class Main : Form
    {
        #region Constants and Fields

        /// <summary>
        /// The test rig.
        /// </summary>
        private TestRig testRig;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> class.
        /// </summary>
        public Main()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The delegatetype add log message.
        /// </summary>
        /// <param name="Message">
        /// The message.
        /// </param>
        public delegate void delegatetypeAddLogMessage(string Message);

        #endregion

        #region Methods

        /// <summary>
        /// The add log message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void AddLogMessage(string message)
        {
            this.txtLog.Text += message + "\r\n";
            if (message.Contains("movetime "))
            {
                this.txtTestTimes.Text += message.Substring(29) + "\r\n";
                this.StopTestUI();
            }
        }

        /// <summary>
        /// Receive messages from the test rig, and append them to the message log display.
        /// </summary>
        /// <param name="sender">
        /// TestRig object
        /// </param>
        /// <param name="e">
        /// Message
        /// </param>
        private void HandleTestRigMessageEvent(object sender, TestRig.MessageEventArgs e)
        {
            delegatetypeAddLogMessage AddMessageLogPointer = this.AddLogMessage;
            this.BeginInvoke(AddMessageLogPointer, e.Message);
        }

        /// <summary>
        /// The main_ form closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.btnStop.Enabled)
            {
                this.testRig.StopTest();
            }
        }

        /// <summary>
        /// The main_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Main_Load(object sender, EventArgs e)
        {
            // Change ths SharpChess exe path here, if you want it to be somewhere more convenient.
//            this.testRig = new TestRig(@"..\..\..\SharpChess Game\bin\Release\SharpChess2.exe");

            this.testRig = new TestRig(@"C:\Users\zass\Documents\Visual Studio 2013\Projects\SharpChess\SharpChess Game\bin\Release\SharpChess2.exe");
            
//C:\Users\zass\Documents\Visual Studio 2013\Projects\SharpChess\SharpChess Game\bin\Release            
            this.testRig.RaiseMessageEvent += this.HandleTestRigMessageEvent;
            this.testRig.ReportStartupMessages();
        }

        /// <summary>
        /// The stop test ui.
        /// </summary>
        private void StopTestUI()
        {
            this.testRig.StopTest();
            this.btnStop.Enabled = false;
            this.btnStart.Enabled = true;
            this.numSearchDepth.Focus();
        }

        /// <summary>
        /// The btn start_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.txtLog.Clear();
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.testRig.SearchDepth = (uint)this.numSearchDepth.Value;
            this.testRig.StartTest();
        }

        /// <summary>
        /// The btn stop_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.StopTestUI();
        }

        #endregion
    }
}