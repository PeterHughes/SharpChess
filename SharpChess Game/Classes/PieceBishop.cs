using System;

namespace SharpChess
{
	public class PieceBishop: IPieceTop
	{
		Piece m_Base = null;

		public PieceBishop(Piece pieceBase)
		{
			m_Base = pieceBase;
		}

		public Piece Base
		{
			get { return m_Base; }
		}

		public string Abbreviation
		{
			get {return "B";}
		}

		public Piece.enmName Name
		{
			get {return Piece.enmName.Bishop;}
		}

		public int BasicValue
		{
			get { return 3;	}
		}

		public int Value
		{
			get
			{
				return 3250;
			}
		}

        public static int[] m_aintSquareValues =
		{
			10,10,10,10,10,10,10,10,    0,0,0,0,0,0,0,0,
			10,25,20,20,20,20,25,10,    0,0,0,0,0,0,0,0,
			10,49,30,30,30,30,49,10,    0,0,0,0,0,0,0,0,
			10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
			10,49,30,30,30,30,49,10,    0,0,0,0,0,0,0,0,
			10,25,20,20,20,20,25,10 ,   0,0,0,0,0,0,0,0,
			10,10,10,10,10,10,10,10 ,   0,0,0,0,0,0,0,0
		};

        public int PositionalPoints
		{
			get
			{
				int intPoints = 0;

				intPoints += (m_aintSquareValues[m_Base.Square.Ordinal]<<1);

				if (Game.Stage!=Game.enmStage.End)
				{
					if (m_Base.CanBeDrivenAwayByPawn())
					{
						intPoints-=30;
					}
				}
				
				// Mobility
				Squares squares = new Squares();
				squares.Add(m_Base.Square);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, 15);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, 17);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, -15);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, -17);
				int intSquareValue = 0;
				foreach (Square square in squares)
				{
					intSquareValue += m_aintSquareValues[square.Ordinal];
				}
				intPoints += (intSquareValue >> 2);

				intPoints += m_Base.DefensePoints;			

				return intPoints;
			}
		}

		public int ImageIndex
		{
			get { return (this.m_Base.Player.Colour==Player.enmColour.White ? 1 : 0); }
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
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, 17, movesType);
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, 15, movesType);
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, -15, movesType);
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, -17, movesType);
		}
	}
}
