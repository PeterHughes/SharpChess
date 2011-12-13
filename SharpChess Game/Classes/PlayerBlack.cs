using System;

namespace SharpChess
{
	public class PlayerBlack: Player
	{
		public PlayerBlack()
		{
			m_PlayerClock = new PlayerClock(this);

			this.Colour = Player.enmColour.Black;
			this.Intellegence = Player.enmIntellegence.Computer;
			
			SetPiecesAtStartingPositions();
		}

		protected override void SetPiecesAtStartingPositions()
		{
			this.m_colPieces.Add( this.King = (new Piece(Piece.enmName.King, this, 4,7, Piece.enmID.BlackKing )) );

			this.m_colPieces.Add( new Piece(Piece.enmName.Queen, this, 3,7, Piece.enmID.BlackQueen )); 

			this.m_colPieces.Add( new Piece(Piece.enmName.Rook, this, 0,7, Piece.enmID.BlackQueensRook )); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Rook, this, 7,7, Piece.enmID.BlackKingsRook ));  
			
			this.m_colPieces.Add( new Piece(Piece.enmName.Bishop, this, 2,7, Piece.enmID.BlackQueensBishop));  
			this.m_colPieces.Add( new Piece(Piece.enmName.Bishop, this, 5,7, Piece.enmID.BlackKingsBishop )); 
			
			this.m_colPieces.Add( new Piece(Piece.enmName.Knight, this, 1,7, Piece.enmID.BlackQueensKnight )); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Knight, this, 6,7, Piece.enmID.BlackKingsKnight )); 
			
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 0,6, Piece.enmID.BlackPawn1));  
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 1,6, Piece.enmID.BlackPawn2)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 2,6, Piece.enmID.BlackPawn3)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 3,6, Piece.enmID.BlackPawn4)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 4,6, Piece.enmID.BlackPawn5)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 5,6, Piece.enmID.BlackPawn6)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 6,6, Piece.enmID.BlackPawn7)); 
			this.m_colPieces.Add( new Piece(Piece.enmName.Pawn, this, 7,6, Piece.enmID.BlackPawn8)); 
		}

		public override int PawnForwardOffset
		{
			get { return -16; }
		}

		public override int PawnAttackRightOffset
		{
			get { return -15; }
		}

		public override int PawnAttackLeftOffset
		{
			get { return -17; }
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
