using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SharpChess
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmWinBoard : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView lvwWinBoard;
		private System.Windows.Forms.ColumnHeader colDirection;
		private System.Windows.Forms.ColumnHeader colMessage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public delegate void delegatetypeWinBoardClosed();
		public event delegatetypeWinBoardClosed WinBoardClosedEvent;

		public frmWinBoard()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmWinBoard));
			this.lvwWinBoard = new System.Windows.Forms.ListView();
			this.colDirection = new System.Windows.Forms.ColumnHeader();
			this.colMessage = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lvwWinBoard
			// 
			this.lvwWinBoard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.colDirection,
																						  this.colMessage});
			this.lvwWinBoard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwWinBoard.FullRowSelect = true;
			this.lvwWinBoard.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvwWinBoard.Location = new System.Drawing.Point(0, 0);
			this.lvwWinBoard.Name = "lvwWinBoard";
			this.lvwWinBoard.Size = new System.Drawing.Size(248, 262);
			this.lvwWinBoard.TabIndex = 143;
			this.lvwWinBoard.View = System.Windows.Forms.View.Details;
			// 
			// colDirection
			// 
			this.colDirection.Text = "Dir";
			this.colDirection.Width = 29;
			// 
			// colMessage
			// 
			this.colMessage.Text = "Message";
			this.colMessage.Width = 199;
			// 
			// frmWinBoard
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 262);
			this.Controls.Add(this.lvwWinBoard);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmWinBoard";
			this.Text = "WinBoard Message Log";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmWinBoard_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		public void LogWinBoardMessage(string strDirection, string strMessage)
		{
			string[] lvi = { strDirection, strMessage };
			lvwWinBoard.Items.Add( new ListViewItem( lvi ) );
			lvwWinBoard.EnsureVisible(lvwWinBoard.Items.Count-1);
		}

		private void frmWinBoard_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
			WinBoardClosedEvent();
		}

	}
}
