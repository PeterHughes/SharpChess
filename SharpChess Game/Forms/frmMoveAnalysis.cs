// --------------------------------------------------------------------------------------------------------------------
// <copyright file="frmMoveAnalysis.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Summary description for frmMoveAnalysis.
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

    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    using SharpChess.Model;

    #endregion

    /// <summary>
    /// Summary description for frmMoveAnalysis.
    /// </summary>
    public class frmMoveAnalysis : Form
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly Container components = null;

        /// <summary>
        /// Holds tree of moves.
        /// </summary>
        private TreeView tvwMoves;

        /// <summary>
        /// Used to count the total nodes at each depth.
        /// </summary>
        private int[] totalNodesPerDepth;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMoveAnalysis"/> class.
        /// </summary>
        public frmMoveAnalysis()
        {
            // Required for Windows Form Designer support
            this.InitializeComponent();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The delegatetype move analysis closed.
        /// </summary>
        public delegate void delegatetypeMoveAnalysisClosed();

        #endregion

        #region Public Events

        /// <summary>
        /// The move analysis closed event.
        /// </summary>
        public event delegatetypeMoveAnalysisClosed MoveAnalysisClosedEvent;

        #endregion

        #region Public Methods

        /// <summary>
        /// The render move analysis.
        /// </summary>
        public void RenderMoveAnalysis()
        {
            this.tvwMoves.Nodes.Clear();
            this.AddBranch(5, Game.MoveAnalysis, this.tvwMoves.Nodes);

            this.CalculateTotalNodesPerDepth();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculate the number of nodes at each depth of the move tree.
        /// </summary>
        private void CalculateTotalNodesPerDepth()
        {
            this.totalNodesPerDepth = new int[32];
            this.GetNodesAtDepth(0, Game.MoveAnalysis);
            this.Text = "Nodes/depth: ";
            int totalNodes = 0;
            foreach (int nodeCount in this.totalNodesPerDepth)
            {
                if (nodeCount > 0)
                {
                    totalNodes += nodeCount;
                    this.Text += nodeCount + " ";
                }
                else
                {
                    break;
                }
            }
            this.Text += " Total: " + totalNodes;
        }

        /// <summary>
        /// Recursive method to walk the move tree, calculating nodes per depth.
        /// </summary>
        /// <param name="depth">Depth of tree node.</param>
        /// <param name="moves">Branch at node of tree.</param>
        private void GetNodesAtDepth(int depth, Moves moves)
        {
            if (moves != null)
            {
                this.totalNodesPerDepth[depth] += moves.Count;
                foreach (Move move in moves)
                {
                    if (move.Moves != null && move.Moves.Count > 0)
                    {
                        this.GetNodesAtDepth(depth + 1, move.Moves);
                    }
                }
            }
        }

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
        /// The add branch.
        /// </summary>
        /// <param name="intDepth">
        /// The int depth.
        /// </param>
        /// <param name="moves">
        /// The moves.
        /// </param>
        /// <param name="treeNodes">
        /// The tree nodes.
        /// </param>
        private void AddBranch(int intDepth, Moves moves, TreeNodeCollection treeNodes)
        {
            if (intDepth == 0)
            {
                return;
            }

            if (moves != null)
            {
                foreach (Move move in moves)
                {
                    TreeNode treeNode = treeNodes.Add(move.DebugText);
                    treeNode.Tag = move;
                    if (move.Moves != null && move.Moves.Count > 0)
                    {
                        this.AddBranch(intDepth - 1, move.Moves, treeNode.Nodes);
                    }
                }
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Move analysis will be recorded when the next move begins. WARNING: drastically sl" +
        "ows computer thinking, and uses LOTS of memory!");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMoveAnalysis));
            this.tvwMoves = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tvwMoves
            // 
            this.tvwMoves.BackColor = System.Drawing.SystemColors.Window;
            this.tvwMoves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwMoves.Location = new System.Drawing.Point(0, 0);
            this.tvwMoves.Name = "tvwMoves";
            treeNode1.Name = "";
            treeNode1.Text = "Move analysis will be recorded when the next move begins. Max depth 6. WARNING: drastically slows computer thinking, and uses LOTS of memory!";
            this.tvwMoves.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tvwMoves.Size = new System.Drawing.Size(475, 592);
            this.tvwMoves.TabIndex = 64;
            this.tvwMoves.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwMoves_BeforeExpand);
            this.tvwMoves.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvwMoves_AfterExpand);
            this.tvwMoves.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwMoves_AfterSelect);
            // 
            // frmMoveAnalysis
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(475, 592);
            this.Controls.Add(this.tvwMoves);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMoveAnalysis";
            this.Text = "Move Analysis Tree";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMoveAnalysis_Closing);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// The frm move analysis_ closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void frmMoveAnalysis_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.MoveAnalysisClosedEvent();
        }

        /// <summary>
        /// The tvw moves_ after expand.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void tvwMoves_AfterExpand(object sender, TreeViewEventArgs e)
        {
            /*
            foreach (TreeNode tn in e.Node.Nodes)
            {
                this.AddBranch(1, ((Move)tn.Tag).Moves, tn.Nodes);
            }
            */
        }

        #endregion

        private void tvwMoves_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

        }

        private void tvwMoves_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}