using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SharpChess
{
	/// <summary>
	/// Summary description for frmPieceSelector.
	/// </summary>
	public class frmPieceSelector : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox picWhiteQueen;
		private System.Windows.Forms.PictureBox picWhiteRook;
		private System.Windows.Forms.PictureBox picWhiteBishop;
		private System.Windows.Forms.PictureBox picWhiteKnight;
		private System.Windows.Forms.PictureBox picBlackKnight;
		private System.Windows.Forms.PictureBox picBlackBishop;
		private System.Windows.Forms.PictureBox picBlackRook;
		private System.Windows.Forms.PictureBox picBlackQueen;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private SharpChess.Move.enmName m_MoveNameSelected = SharpChess.Move.enmName.NullMove;
		private SharpChess.Player.enmColour m_Colour = SharpChess.Player.enmColour.Black;

		public Move.enmName MoveNameSelected
		{
			get { return m_MoveNameSelected; }
		}

		public SharpChess.Player.enmColour Colour
		{
			set { m_Colour = value; }
		}

		public frmPieceSelector()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmPieceSelector));
			this.picWhiteQueen = new System.Windows.Forms.PictureBox();
			this.picWhiteRook = new System.Windows.Forms.PictureBox();
			this.picWhiteBishop = new System.Windows.Forms.PictureBox();
			this.picWhiteKnight = new System.Windows.Forms.PictureBox();
			this.picBlackKnight = new System.Windows.Forms.PictureBox();
			this.picBlackBishop = new System.Windows.Forms.PictureBox();
			this.picBlackRook = new System.Windows.Forms.PictureBox();
			this.picBlackQueen = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// picWhiteQueen
			// 
			this.picWhiteQueen.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.picWhiteQueen.Image = ((System.Drawing.Image)(resources.GetObject("picWhiteQueen.Image")));
			this.picWhiteQueen.Location = new System.Drawing.Point(0, 0);
			this.picWhiteQueen.Name = "picWhiteQueen";
			this.picWhiteQueen.Size = new System.Drawing.Size(42, 42);
			this.picWhiteQueen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picWhiteQueen.TabIndex = 0;
			this.picWhiteQueen.TabStop = false;
			this.picWhiteQueen.Tag = "Queen";
			this.picWhiteQueen.Visible = false;
			this.picWhiteQueen.Click += new System.EventHandler(this.picPiece_Click);
			this.picWhiteQueen.MouseEnter += new System.EventHandler(this.picMouseEnter);
			this.picWhiteQueen.MouseLeave += new System.EventHandler(this.picMouseLeave);
			// 
			// picWhiteRook
			// 
			this.picWhiteRook.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.picWhiteRook.Image = ((System.Drawing.Image)(resources.GetObject("picWhiteRook.Image")));
			this.picWhiteRook.Location = new System.Drawing.Point(40, 0);
			this.picWhiteRook.Name = "picWhiteRook";
			this.picWhiteRook.Size = new System.Drawing.Size(42, 42);
			this.picWhiteRook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picWhiteRook.TabIndex = 1;
			this.picWhiteRook.TabStop = false;
			this.picWhiteRook.Tag = "Rook";
			this.picWhiteRook.Visible = false;
			this.picWhiteRook.Click += new System.EventHandler(this.picPiece_Click);
			this.picWhiteRook.MouseEnter += new System.EventHandler(this.picMouseEnter);
			this.picWhiteRook.MouseLeave += new System.EventHandler(this.picMouseLeave);
			// 
			// picWhiteBishop
			// 
			this.picWhiteBishop.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.picWhiteBishop.Image = ((System.Drawing.Image)(resources.GetObject("picWhiteBishop.Image")));
			this.picWhiteBishop.Location = new System.Drawing.Point(80, 0);
			this.picWhiteBishop.Name = "picWhiteBishop";
			this.picWhiteBishop.Size = new System.Drawing.Size(42, 42);
			this.picWhiteBishop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picWhiteBishop.TabIndex = 2;
			this.picWhiteBishop.TabStop = false;
			this.picWhiteBishop.Tag = "Bishop";
			this.picWhiteBishop.Visible = false;
			this.picWhiteBishop.Click += new System.EventHandler(this.picPiece_Click);
			this.picWhiteBishop.MouseEnter += new System.EventHandler(this.picMouseEnter);
			this.picWhiteBishop.MouseLeave += new System.EventHandler(this.picMouseLeave);
			// 
			// picWhiteKnight
			// 
			this.picWhiteKnight.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.picWhiteKnight.Image = ((System.Drawing.Image)(resources.GetObject("picWhiteKnight.Image")));
			this.picWhiteKnight.Location = new System.Drawing.Point(120, 0);
			this.picWhiteKnight.Name = "picWhiteKnight";
			this.picWhiteKnight.Size = new System.Drawing.Size(42, 42);
			this.picWhiteKnight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picWhiteKnight.TabIndex = 3;
			this.picWhiteKnight.TabStop = false;
			this.picWhiteKnight.Tag = "Knight";
			this.picWhiteKnight.Visible = false;
			this.picWhiteKnight.Click += new System.EventHandler(this.picPiece_Click);
			this.picWhiteKnight.MouseEnter += new System.EventHandler(this.picMouseEnter);
			this.picWhiteKnight.MouseLeave += new System.EventHandler(this.picMouseLeave);
			// 
			// picBlackKnight
			// 
			this.picBlackKnight.BackColor = System.Drawing.Color.Transparent;
			this.picBlackKnight.Image = ((System.Drawing.Image)(resources.GetObject("picBlackKnight.Image")));
			this.picBlackKnight.Location = new System.Drawing.Point(120, 0);
			this.picBlackKnight.Name = "picBlackKnight";
			this.picBlackKnight.Size = new System.Drawing.Size(42, 42);
			this.picBlackKnight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picBlackKnight.TabIndex = 7;
			this.picBlackKnight.TabStop = false;
			this.picBlackKnight.Tag = "Knight";
			this.picBlackKnight.Visible = false;
			this.picBlackKnight.Click += new System.EventHandler(this.picPiece_Click);
			this.picBlackKnight.MouseEnter += new System.EventHandler(this.picMouseEnter);
			this.picBlackKnight.MouseLeave += new System.EventHandler(this.picMouseLeave);
			// 
			// picBlackBishop
			// 
			this.picBlackBishop.BackColor = System.Drawing.Color.Transparent;
			this.picBlackBishop.Image = ((System.Drawing.Image)(resources.GetObject("picBlackBishop.Image")));
			this.picBlackBishop.Location = new System.Drawing.Point(80, 0);
			this.picBlackBishop.Name = "picBlackBishop";
			this.picBlackBishop.Size = new System.Drawing.Size(42, 42);
			this.picBlackBishop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picBlackBishop.TabIndex = 6;
			this.picBlackBishop.TabStop = false;
			this.picBlackBishop.Tag = "Bishop";
			this.picBlackBishop.Visible = false;
			this.picBlackBishop.Click += new System.EventHandler(this.picPiece_Click);
			this.picBlackBishop.MouseEnter += new System.EventHandler(this.picMouseEnter);
			this.picBlackBishop.MouseLeave += new System.EventHandler(this.picMouseLeave);
			// 
			// picBlackRook
			// 
			this.picBlackRook.BackColor = System.Drawing.Color.Transparent;
			this.picBlackRook.Image = ((System.Drawing.Image)(resources.GetObject("picBlackRook.Image")));
			this.picBlackRook.Location = new System.Drawing.Point(40, 0);
			this.picBlackRook.Name = "picBlackRook";
			this.picBlackRook.Size = new System.Drawing.Size(42, 42);
			this.picBlackRook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picBlackRook.TabIndex = 5;
			this.picBlackRook.TabStop = false;
			this.picBlackRook.Tag = "Rook";
			this.picBlackRook.Visible = false;
			this.picBlackRook.Click += new System.EventHandler(this.picPiece_Click);
			this.picBlackRook.MouseEnter += new System.EventHandler(this.picMouseEnter);
			this.picBlackRook.MouseLeave += new System.EventHandler(this.picMouseLeave);
			// 
			// picBlackQueen
			// 
			this.picBlackQueen.BackColor = System.Drawing.Color.Transparent;
			this.picBlackQueen.Image = ((System.Drawing.Image)(resources.GetObject("picBlackQueen.Image")));
			this.picBlackQueen.Location = new System.Drawing.Point(0, 0);
			this.picBlackQueen.Name = "picBlackQueen";
			this.picBlackQueen.Size = new System.Drawing.Size(42, 42);
			this.picBlackQueen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picBlackQueen.TabIndex = 4;
			this.picBlackQueen.TabStop = false;
			this.picBlackQueen.Tag = "Queen";
			this.picBlackQueen.Visible = false;
			this.picBlackQueen.Click += new System.EventHandler(this.picPiece_Click);
			this.picBlackQueen.MouseEnter += new System.EventHandler(this.picMouseEnter);
			this.picBlackQueen.MouseLeave += new System.EventHandler(this.picMouseLeave);
			// 
			// frmPieceSelector
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(229)), ((System.Byte)(197)), ((System.Byte)(105)));
			this.ClientSize = new System.Drawing.Size(162, 40);
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
			this.Load += new System.EventHandler(this.frmPieceSelector_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void picPiece_Click(object sender, System.EventArgs e)
		{
			switch ((string)((PictureBox)sender).Tag)
			{
				case "Queen":
					m_MoveNameSelected = SharpChess.Move.enmName.PawnPromotionQueen;
					break;

				case "Rook":
					m_MoveNameSelected = SharpChess.Move.enmName.PawnPromotionRook;
					break;
				
				case "Bishop":
					m_MoveNameSelected = SharpChess.Move.enmName.PawnPromotionBishop;
					break;
				
				case "Knight":
					m_MoveNameSelected = SharpChess.Move.enmName.PawnPromotionKnight;
					break;
			}
			this.Close();
		}

		private void picMouseEnter(object sender, System.EventArgs e)
		{
			((PictureBox)sender).BackColor = Color.Yellow;
		}

		private void picMouseLeave(object sender, System.EventArgs e)
		{
			((PictureBox)sender).BackColor = Color.FromArgb(255,128,0);
		}

		private void frmPieceSelector_Load(object sender, System.EventArgs e)
		{
			switch (m_Colour)
			{
				case SharpChess.Player.enmColour.White:
					picWhiteQueen.Visible=true;
					picWhiteRook.Visible=true;
					picWhiteBishop.Visible=true;
					picWhiteKnight.Visible=true;
					break;

				case SharpChess.Player.enmColour.Black:
					picBlackQueen.Visible=true;
					picBlackRook.Visible=true;
					picBlackBishop.Visible=true;
					picBlackKnight.Visible=true;
					break;

			}
		}
	}
}
