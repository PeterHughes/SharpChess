using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpChess_Performance_Tester
{
    public partial class Main : Form
    {
        PerformanceTestRig testRig;
        public delegate void delegatetypeAddLogMessage(string Message);

        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Receive messages from the test rig, and append them to the message log display.
        /// </summary>
        /// <param name="sender">TestRig object</param>
        /// <param name="e">Message</param>
        void HandleTestRigMessageEvent(object sender, PerformanceTestRig.MessageEventArgs e)
        {
            delegatetypeAddLogMessage AddMessageLogPointer = new delegatetypeAddLogMessage(AddLogMessage);
            this.BeginInvoke(AddMessageLogPointer, e.Message);
        }

        void AddLogMessage(string message)
        {
            this.txtLog.Text += message + "\r\n";
            if (message.Contains("movetime "))
            {
                txtTestTimes.Text += message.Substring(29) + "\r\n";
                this.StopTestUI();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // Change ths SharpChess exe path here, if you want it to be somewhere more convenient.
            this.testRig = new PerformanceTestRig(@"..\..\..\SharpChess Game\bin\Release\SharpChess2.exe");
            this.testRig.RaiseMessageEvent += this.HandleTestRigMessageEvent;
            this.testRig.ReportStartupMessages();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.txtLog.Clear();
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.testRig.SearchDepth = (uint)this.numSearchDepth.Value;
            this.testRig.StartTest();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.StopTestUI();
        }

        private void StopTestUI()
        {
            this.testRig.StopTest();
            this.btnStop.Enabled = false;
            this.btnStart.Enabled = true;
            this.numSearchDepth.Focus();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnStop.Enabled)
            {
                this.testRig.StopTest();
            }
        }
    }
}
