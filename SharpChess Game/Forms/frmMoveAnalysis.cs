using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SharpChess
{
	/// <summary>
	/// Summary description for frmMoveAnalysis.
	/// </summary>
	public class frmMoveAnalysis : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TreeView tvwMoves;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public delegate void delegatetypeMoveAnalysisClosed();
		public event delegatetypeMoveAnalysisClosed MoveAnalysisClosedEvent;

		public frmMoveAnalysis()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMoveAnalysis));
			this.tvwMoves = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// tvwMoves
			// 
			this.tvwMoves.BackColor = System.Drawing.SystemColors.Window;
			this.tvwMoves.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwMoves.ImageIndex = -1;
			this.tvwMoves.Location = new System.Drawing.Point(0, 0);
			this.tvwMoves.Name = "tvwMoves";
			this.tvwMoves.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																				 new System.Windows.Forms.TreeNode("Move analysis will be recorded when the next move begins")});
			this.tvwMoves.SelectedImageIndex = -1;
			this.tvwMoves.Size = new System.Drawing.Size(376, 470);
			this.tvwMoves.TabIndex = 64;
			this.tvwMoves.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvwMoves_AfterExpand);
			// 
			// frmMoveAnalysis
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(376, 470);
			this.Controls.Add(this.tvwMoves);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmMoveAnalysis";
			this.Text = "Move Analysis Tree";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMoveAnalysis_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		public void RenderMoveAnalysis()
		{
			tvwMoves.Nodes.Clear();
			AddBranch(2, Game.MoveAnalysis, tvwMoves.Nodes);
		}

		private void AddBranch(int intDepth, Moves moves, TreeNodeCollection treeNodes)
		{
			if (intDepth == 0) return;

			TreeNode treeNode;
			if (moves!=null)
			{
				foreach(Move move in moves)
				{
					treeNode = treeNodes.Add( move.DebugText );
					treeNode.Tag = move;
					if (move.Moves!=null && move.Moves.Count>0)
					{
						AddBranch(intDepth-1, move.Moves, treeNode.Nodes);
					}
				}
			}
		}

		private void tvwMoves_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			foreach (TreeNode tn in e.Node.Nodes)
			{
				AddBranch(1, ((Move)tn.Tag).Moves, tn.Nodes);
			}
		}

		private void frmMoveAnalysis_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
			MoveAnalysisClosedEvent();
		}

	}
}
