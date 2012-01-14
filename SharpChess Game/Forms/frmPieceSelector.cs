// --------------------------------------------------------------------------------------------------------------------
// <copyright file="frmPieceSelector.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Summary description for frmPieceSelector.
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
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    using SharpChess.Model;

    #endregion

    /// <summary>
    /// Summary description for frmPieceSelector.
    /// </summary>
    public class frmPieceSelector : Form
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly Container components = null;

        /// <summary>
        /// The m_ colour.
        /// </summary>
        private Player.PlayerColourNames m_Colour = Player.PlayerColourNames.Black;

        /// <summary>
        /// The m_ move name selected.
        /// </summary>
        private Move.MoveNames m_MoveNameSelected = Model.Move.MoveNames.NullMove;

        /// <summary>
        /// The pic black bishop.
        /// </summary>
        private PictureBox picBlackBishop;

        /// <summary>
        /// The pic black knight.
        /// </summary>
        private PictureBox picBlackKnight;

        /// <summary>
        /// The pic black queen.
        /// </summary>
        private PictureBox picBlackQueen;

        /// <summary>
        /// The pic black rook.
        /// </summary>
        private PictureBox picBlackRook;

        /// <summary>
        /// The pic white bishop.
        /// </summary>
        private PictureBox picWhiteBishop;

        /// <summary>
        /// The pic white knight.
        /// </summary>
        private PictureBox picWhiteKnight;

        /// <summary>
        /// The pic white queen.
        /// </summary>
        private PictureBox picWhiteQueen;

        /// <summary>
        /// The pic white rook.
        /// </summary>
        private PictureBox picWhiteRook;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="frmPieceSelector"/> class.
        /// </summary>
        public frmPieceSelector()
        {
            // Required for Windows Form Designer support
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets Colour.
        /// </summary>
        public Player.PlayerColourNames Colour
        {
            set
            {
                this.m_Colour = value;
            }
        }

        /// <summary>
        /// Gets MoveNameSelected.
        /// </summary>
        public Move.MoveNames MoveNameSelected
        {
            get
            {
                return this.m_MoveNameSelected;
            }
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
            ResourceManager resources = new ResourceManager(typeof(frmPieceSelector));
            this.picWhiteQueen = new PictureBox();
            this.picWhiteRook = new PictureBox();
            this.picWhiteBishop = new PictureBox();
            this.picWhiteKnight = new PictureBox();
            this.picBlackKnight = new PictureBox();
            this.picBlackBishop = new PictureBox();
            this.picBlackRook = new PictureBox();
            this.picBlackQueen = new PictureBox();
            this.SuspendLayout();

            // picWhiteQueen
            this.picWhiteQueen.BackColor = System.Drawing.Color.FromArgb(255, 128, 0);
            this.picWhiteQueen.Image = (System.Drawing.Image)resources.GetObject("picWhiteQueen.Image");
            this.picWhiteQueen.Location = new Point(0, 0);
            this.picWhiteQueen.Name = "picWhiteQueen";
            this.picWhiteQueen.Size = new Size(42, 42);
            this.picWhiteQueen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picWhiteQueen.TabIndex = 0;
            this.picWhiteQueen.TabStop = false;
            this.picWhiteQueen.Tag = "Queen";
            this.picWhiteQueen.Visible = false;
            this.picWhiteQueen.Click += new EventHandler(this.picPiece_Click);
            this.picWhiteQueen.MouseEnter += new EventHandler(this.picMouseEnter);
            this.picWhiteQueen.MouseLeave += new EventHandler(this.picMouseLeave);

            // picWhiteRook
            this.picWhiteRook.BackColor = System.Drawing.Color.FromArgb(255, 128, 0);
            this.picWhiteRook.Image = (System.Drawing.Image)resources.GetObject("picWhiteRook.Image");
            this.picWhiteRook.Location = new Point(40, 0);
            this.picWhiteRook.Name = "picWhiteRook";
            this.picWhiteRook.Size = new Size(42, 42);
            this.picWhiteRook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picWhiteRook.TabIndex = 1;
            this.picWhiteRook.TabStop = false;
            this.picWhiteRook.Tag = "Rook";
            this.picWhiteRook.Visible = false;
            this.picWhiteRook.Click += new EventHandler(this.picPiece_Click);
            this.picWhiteRook.MouseEnter += new EventHandler(this.picMouseEnter);
            this.picWhiteRook.MouseLeave += new EventHandler(this.picMouseLeave);

            // picWhiteBishop
            this.picWhiteBishop.BackColor = System.Drawing.Color.FromArgb(255, 128, 0);
            this.picWhiteBishop.Image = (System.Drawing.Image)resources.GetObject("picWhiteBishop.Image");
            this.picWhiteBishop.Location = new Point(80, 0);
            this.picWhiteBishop.Name = "picWhiteBishop";
            this.picWhiteBishop.Size = new Size(42, 42);
            this.picWhiteBishop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picWhiteBishop.TabIndex = 2;
            this.picWhiteBishop.TabStop = false;
            this.picWhiteBishop.Tag = "Bishop";
            this.picWhiteBishop.Visible = false;
            this.picWhiteBishop.Click += new EventHandler(this.picPiece_Click);
            this.picWhiteBishop.MouseEnter += new EventHandler(this.picMouseEnter);
            this.picWhiteBishop.MouseLeave += new EventHandler(this.picMouseLeave);

            // picWhiteKnight
            this.picWhiteKnight.BackColor = System.Drawing.Color.FromArgb(255, 128, 0);
            this.picWhiteKnight.Image = (System.Drawing.Image)resources.GetObject("picWhiteKnight.Image");
            this.picWhiteKnight.Location = new Point(120, 0);
            this.picWhiteKnight.Name = "picWhiteKnight";
            this.picWhiteKnight.Size = new Size(42, 42);
            this.picWhiteKnight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picWhiteKnight.TabIndex = 3;
            this.picWhiteKnight.TabStop = false;
            this.picWhiteKnight.Tag = "Knight";
            this.picWhiteKnight.Visible = false;
            this.picWhiteKnight.Click += new EventHandler(this.picPiece_Click);
            this.picWhiteKnight.MouseEnter += new EventHandler(this.picMouseEnter);
            this.picWhiteKnight.MouseLeave += new EventHandler(this.picMouseLeave);

            // picBlackKnight
            this.picBlackKnight.BackColor = System.Drawing.Color.Transparent;
            this.picBlackKnight.Image = (System.Drawing.Image)resources.GetObject("picBlackKnight.Image");
            this.picBlackKnight.Location = new Point(120, 0);
            this.picBlackKnight.Name = "picBlackKnight";
            this.picBlackKnight.Size = new Size(42, 42);
            this.picBlackKnight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBlackKnight.TabIndex = 7;
            this.picBlackKnight.TabStop = false;
            this.picBlackKnight.Tag = "Knight";
            this.picBlackKnight.Visible = false;
            this.picBlackKnight.Click += new EventHandler(this.picPiece_Click);
            this.picBlackKnight.MouseEnter += new EventHandler(this.picMouseEnter);
            this.picBlackKnight.MouseLeave += new EventHandler(this.picMouseLeave);

            // picBlackBishop
            this.picBlackBishop.BackColor = System.Drawing.Color.Transparent;
            this.picBlackBishop.Image = (System.Drawing.Image)resources.GetObject("picBlackBishop.Image");
            this.picBlackBishop.Location = new Point(80, 0);
            this.picBlackBishop.Name = "picBlackBishop";
            this.picBlackBishop.Size = new Size(42, 42);
            this.picBlackBishop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBlackBishop.TabIndex = 6;
            this.picBlackBishop.TabStop = false;
            this.picBlackBishop.Tag = "Bishop";
            this.picBlackBishop.Visible = false;
            this.picBlackBishop.Click += new EventHandler(this.picPiece_Click);
            this.picBlackBishop.MouseEnter += new EventHandler(this.picMouseEnter);
            this.picBlackBishop.MouseLeave += new EventHandler(this.picMouseLeave);

            // picBlackRook
            this.picBlackRook.BackColor = System.Drawing.Color.Transparent;
            this.picBlackRook.Image = (System.Drawing.Image)resources.GetObject("picBlackRook.Image");
            this.picBlackRook.Location = new Point(40, 0);
            this.picBlackRook.Name = "picBlackRook";
            this.picBlackRook.Size = new Size(42, 42);
            this.picBlackRook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBlackRook.TabIndex = 5;
            this.picBlackRook.TabStop = false;
            this.picBlackRook.Tag = "Rook";
            this.picBlackRook.Visible = false;
            this.picBlackRook.Click += new EventHandler(this.picPiece_Click);
            this.picBlackRook.MouseEnter += new EventHandler(this.picMouseEnter);
            this.picBlackRook.MouseLeave += new EventHandler(this.picMouseLeave);

            // picBlackQueen
            this.picBlackQueen.BackColor = System.Drawing.Color.Transparent;
            this.picBlackQueen.Image = (System.Drawing.Image)resources.GetObject("picBlackQueen.Image");
            this.picBlackQueen.Location = new Point(0, 0);
            this.picBlackQueen.Name = "picBlackQueen";
            this.picBlackQueen.Size = new Size(42, 42);
            this.picBlackQueen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBlackQueen.TabIndex = 4;
            this.picBlackQueen.TabStop = false;
            this.picBlackQueen.Tag = "Queen";
            this.picBlackQueen.Visible = false;
            this.picBlackQueen.Click += new EventHandler(this.picPiece_Click);
            this.picBlackQueen.MouseEnter += new EventHandler(this.picMouseEnter);
            this.picBlackQueen.MouseLeave += new EventHandler(this.picMouseLeave);

            // frmPieceSelector
            this.AutoScaleBaseSize = new Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(229, 197, 105);
            this.ClientSize = new Size(162, 40);
            this.Controls.Add(this.picBlackKnight);
            this.Controls.Add(this.picBlackBishop);
            this.Controls.Add(this.picBlackRook);
            this.Controls.Add(this.picBlackQueen);
            this.Controls.Add(this.picWhiteKnight);
            this.Controls.Add(this.picWhiteBishop);
            this.Controls.Add(this.picWhiteRook);
            this.Controls.Add(this.picWhiteQueen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPieceSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Promote pawn to?";
            this.Load += new EventHandler(this.frmPieceSelector_Load);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// The frm piece selector_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void frmPieceSelector_Load(object sender, EventArgs e)
        {
            switch (this.m_Colour)
            {
                case Player.PlayerColourNames.White:
                    this.picWhiteQueen.Visible = true;
                    this.picWhiteRook.Visible = true;
                    this.picWhiteBishop.Visible = true;
                    this.picWhiteKnight.Visible = true;
                    break;

                case Player.PlayerColourNames.Black:
                    this.picBlackQueen.Visible = true;
                    this.picBlackRook.Visible = true;
                    this.picBlackBishop.Visible = true;
                    this.picBlackKnight.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// The pic mouse enter.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void picMouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = Color.Yellow;
        }

        /// <summary>
        /// The pic mouse leave.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void picMouseLeave(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = Color.FromArgb(255, 128, 0);
        }

        /// <summary>
        /// The pic piece_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void picPiece_Click(object sender, EventArgs e)
        {
            switch ((string)((PictureBox)sender).Tag)
            {
                case "Queen":
                    this.m_MoveNameSelected = Model.Move.MoveNames.PawnPromotionQueen;
                    break;

                case "Rook":
                    this.m_MoveNameSelected = Model.Move.MoveNames.PawnPromotionRook;
                    break;

                case "Bishop":
                    this.m_MoveNameSelected = Model.Move.MoveNames.PawnPromotionBishop;
                    break;

                case "Knight":
                    this.m_MoveNameSelected = Model.Move.MoveNames.PawnPromotionKnight;
                    break;
            }

            this.Close();
        }

        #endregion
    }
}