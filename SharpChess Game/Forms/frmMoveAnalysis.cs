// --------------------------------------------------------------------------------------------------------------------
// <copyright file="frmMoveAnalysis.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   Summary description for frmMoveAnalysis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region License

// SharpChess
// Copyright (C) 2011 Peter Hughes
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
        /// The tvw moves.
        /// </summary>
        private TreeView tvwMoves;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMoveAnalysis"/> class.
        /// </summary>
        public frmMoveAnalysis()
        {
            // Required for Windows Form Designer support
            this.InitializeComponent();

            // TODO: Add any constructor code after InitializeComponent call
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
            this.AddBranch(2, Game.MoveAnalysis, this.tvwMoves.Nodes);
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

            TreeNode treeNode;
            if (moves != null)
            {
                foreach (Move move in moves)
                {
                    treeNode = treeNodes.Add(move.DebugText);
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
            ResourceManager resources = new ResourceManager(typeof(frmMoveAnalysis));
            this.tvwMoves = new TreeView();
            this.SuspendLayout();

            // tvwMoves
            this.tvwMoves.BackColor = System.Drawing.SystemColors.Window;
            this.tvwMoves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwMoves.ImageIndex = -1;
            this.tvwMoves.Location = new Point(0, 0);
            this.tvwMoves.Name = "tvwMoves";
            this.tvwMoves.Nodes.AddRange(
                new[] { new TreeNode("Move analysis will be recorded when the next move begins") });
            this.tvwMoves.SelectedImageIndex = -1;
            this.tvwMoves.Size = new Size(376, 470);
            this.tvwMoves.TabIndex = 64;
            this.tvwMoves.AfterExpand += new TreeViewEventHandler(this.tvwMoves_AfterExpand);

            // frmMoveAnalysis
            this.AutoScaleBaseSize = new Size(5, 13);
            this.ClientSize = new Size(376, 470);
            this.Controls.Add(this.tvwMoves);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "frmMoveAnalysis";
            this.Text = "Move Analysis Tree";
            this.Closing += new CancelEventHandler(this.frmMoveAnalysis_Closing);
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
            foreach (TreeNode tn in e.Node.Nodes)
            {
                this.AddBranch(1, ((Move)tn.Tag).Moves, tn.Nodes);
            }
        }

        #endregion
    }
}