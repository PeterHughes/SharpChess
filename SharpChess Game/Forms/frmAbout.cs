// --------------------------------------------------------------------------------------------------------------------
// <copyright file="frmAbout.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Summary description for Form1.
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

namespace SharpChess
{
    #region Using

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmAbout : Form
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly Container components = null;

        /// <summary>
        /// The btn ok.
        /// </summary>
        private Button btnOK;

        /// <summary>
        /// The label 1.
        /// </summary>
        private Label label1;

        /// <summary>
        /// The label 2.
        /// </summary>
        private Label label2;

        /// <summary>
        /// The label 3.
        /// </summary>
        private Label label3;

        /// <summary>
        /// The label 4.
        /// </summary>
        private Label label4;

        /// <summary>
        /// The label 5.
        /// </summary>
        private Label label5;

        /// <summary>
        /// The lbl product name.
        /// </summary>
        private Label lblProductName;

        /// <summary>
        /// The lbl version.
        /// </summary>
        private Label lblVersion;

        /// <summary>
        /// The lbl web site.
        /// </summary>
        private LinkLabel lblWebSite;

        /// <summary>
        /// The picture box 1.
        /// </summary>
        private PictureBox pictureBox1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="frmAbout"/> class.
        /// </summary>
        public frmAbout()
        {
            // Required for Windows Form Designer support
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager resources = new ResourceManager(typeof(frmAbout));
            this.lblVersion = new Label();
            this.pictureBox1 = new PictureBox();
            this.lblProductName = new Label();
            this.label2 = new Label();
            this.btnOK = new Button();
            this.label3 = new Label();
            this.label4 = new Label();
            this.lblWebSite = new LinkLabel();
            this.label1 = new Label();
            this.label5 = new Label();
            this.SuspendLayout();

            // lblVersion
            this.lblVersion.Location = new Point(88, 96);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new Size(88, 23);
            this.lblVersion.TabIndex = 0;
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // pictureBox1
            this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            this.pictureBox1.Location = new Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(48, 48);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;

            // lblProductName
            this.lblProductName.Font = new Font(
                "Microsoft Sans Serif", 
                12F, 
                System.Drawing.FontStyle.Bold, 
                System.Drawing.GraphicsUnit.Point, 
                0);
            this.lblProductName.Location = new Point(56, 16);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new Size(120, 23);
            this.lblProductName.TabIndex = 2;
            this.lblProductName.Text = "SharpChess";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            // label2
            this.label2.Location = new Point(8, 96);
            this.label2.Name = "label2";
            this.label2.Size = new Size(72, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Version:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // btnOK
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new Point(55, 160);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);

            // label3
            this.label3.Location = new Point(8, 48);
            this.label3.Name = "label3";
            this.label3.Size = new Size(64, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Created By:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // label4
            this.label4.Location = new Point(88, 48);
            this.label4.Name = "label4";
            this.label4.Size = new Size(88, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "Peter Hughes";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // lblWebSite
            this.lblWebSite.Location = new Point(0, 128);
            this.lblWebSite.Name = "lblWebSite";
            this.lblWebSite.Size = new Size(184, 23);
            this.lblWebSite.TabIndex = 7;
            this.lblWebSite.TabStop = true;
            this.lblWebSite.Text = "www.SharpChess.com";
            this.lblWebSite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblWebSite.LinkClicked += new LinkLabelLinkClickedEventHandler(this.llbWebSite_LinkClicked);

            // label1
            this.label1.Location = new Point(8, 72);
            this.label1.Name = "label1";
            this.label1.Size = new Size(72, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Contributors:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // label5
            this.label5.Location = new Point(88, 72);
            this.label5.Name = "label5";
            this.label5.Size = new Size(88, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Nimzo";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // frmAbout
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            this.CancelButton = this.btnOK;
            this.ClientSize = new Size(184, 190);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblWebSite);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblVersion);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About SharpChess";
            this.Load += new EventHandler(this.frmAbout_Load);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// The btn o k_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The frm about_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void frmAbout_Load(object sender, EventArgs e)
        {
            this.lblProductName.Text = Application.ProductName;
            this.lblVersion.Text = Application.ProductVersion;
        }

        /// <summary>
        /// The llb web site_ link clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void llbWebSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://" + this.lblWebSite.Text);
        }

        #endregion
    }
}