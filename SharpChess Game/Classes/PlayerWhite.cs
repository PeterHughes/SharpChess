using System;

namespace SharpChess
{
	public class PlayerWhite: Player
	{
		public PlayerWhite()
		{
			m_PlayerClock = new PlayerClock(this);

			this.Colour = Player.enmColour.White;
			this.Intellegence = Player.enmIntellegence.Human;
			
			SetPiecesAtStartingPositions();
		}

		protected override void SetPiecesAtStartingPositions()
		{
			this.m_colPieces.Add( this.King = (new Piece(Piece.enmName.King, this, 4,0, Piece.enmID.WhiteKing)) );

			this.m_colPieces.Add( new Piece(Piece.enmName.Queen, this, 3,0, Piece.enmID.WhiteQueen )); 

			this.m_colPieces.Add( new Piece(Piece.enmName.Rook, this, 0,0, Piece.enmID.WhiteQueensRook )); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Rook, this, 7,0, Piece.enmID.WhiteKingsRook )); 
			
			this.m_colPieces.Add( new Piece(Piece.enmName.Bishop, this, 2,0, Piece.enmID.WhiteQueensBishop )); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Bishop, this, 5,0, Piece.enmID.WhiteKingsBishop )); 
			
			this.m_colPieces.Add( new Piece(Piece.enmName.Knight, this, 1,0, Piece.enmID.WhiteQueensKnight )); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Knight, this, 6,0, Piece.enmID.WhiteKingsKnight ));
			
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 0,1, Piece.enmID.WhitePawn1)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 1,1, Piece.enmID.WhitePawn2)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 2,1, Piece.enmID.WhitePawn3)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 3,1, Piece.enmID.WhitePawn4)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 4,1, Piece.enmID.WhitePawn5));  
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 5,1, Piece.enmID.WhitePawn6)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 6,1, Piece.enmID.WhitePawn7)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 7,1, Piece.enmID.WhitePawn8)); 
		}

		public override int PawnForwardOffset
		{
			get { return 16; }
		}

		public override int PawnAttackRightOffset
		{
			get { return 17; }
		}

		public override int PawnAttackLeftOffset
		{
			get { return 15; }
		}

		public override SharpChess.Player.PlayerClock Clock
		{
			get
			{
				return m_PlayerClock;
			}
		}

	}
}
