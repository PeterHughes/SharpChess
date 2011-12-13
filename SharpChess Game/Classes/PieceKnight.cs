using System;

namespace SharpChess
{
	public class PieceKnight: IPieceTop
	{
		Piece m_Base = null;

		public PieceKnight(Piece pieceBase)
		{
			m_Base = pieceBase;
		}

		public Piece Base
		{
			get { return m_Base; }
		}

		public string Abbreviation
		{
			get {return "N";}
		}

		public Piece.enmName Name
		{
			get {return Piece.enmName.Knight;}
		}

		public int BasicValue
		{
			get { return 3;	}
		}

		public int Value
		{
			get
			{
				return 3250; // + ((m_Base.Player.PawnsInPlay-5) * 63);  // raise the knight's value by 1/16 for each pawn above five of the side being valued, with the opposite adjustment for each pawn short of five;
			}
		}

		public static int[] m_aintSquareValues =
		{
			1, 1, 1, 1, 1, 1, 1, 1,    0,0,0,0,0,0,0,0,
			1, 7, 7, 7, 7, 7, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7,18,18,18,18, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7,18,27,27,18, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7,18,27,27,18, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7,18,18,18,18, 7, 1,    0,0,0,0,0,0,0,0,
			1, 7, 7, 7, 7, 7, 7, 1 ,   0,0,0,0,0,0,0,0,
			1, 1, 1, 1, 1, 1, 1, 1 ,   0,0,0,0,0,0,0,0
		};

		public int PositionalPoints
		{
			get
			{
				int intPoints = 0;

				if (Game.Stage==Game.enmStage.End)
				{
					intPoints -= this.m_Base.TaxiCabDistanceToEnemyKingPenalty()<<4;
				}
				else
				{
					intPoints += (m_aintSquareValues[m_Base.Square.Ordinal]<<3);

					if (m_Base.CanBeDrivenAwayByPawn())
					{
						intPoints-=30;
					}
				}

				intPoints += m_Base.DefensePoints;

				return intPoints;
			}
		}

		public int ImageIndex
		{
			get { return (this.m_Base.Player.Colour==Player.enmColour.White ? 7 : 6 ); }
		}
	
		public bool IsCapturable
		{
			get
			{
				return true;
			}
		}

		public void GenerateLazyMoves(Moves moves, Moves.enmMovesType movesType)
		{
			Square square;

			switch (movesType)
			{
				case Moves.enmMovesType.All:
					square = Board.GetSquare(m_Base.Square.Ordinal+33); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+18); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-14); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-31); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-33); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-18); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+14); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+31); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					break;

				case Moves.enmMovesType.CapturesChecksPromotions:
					square = Board.GetSquare(m_Base.Square.Ordinal+33); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+18); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-14); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-31); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-33); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-18); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+14); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+31); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					break;
			}
		}

	}
}
