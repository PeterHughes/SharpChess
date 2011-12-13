using System;

namespace SharpChess
{
	public class PieceRook: IPieceTop
	{
		Piece m_Base = null;

		public PieceRook(Piece pieceBase)
		{
			m_Base = pieceBase;
		}

		public Piece Base
		{
			get { return m_Base; }
		}

		public string Abbreviation
		{
			get {return "R";}
		}

		public Piece.enmName Name
		{
			get {return Piece.enmName.Rook;}
		}

		public int BasicValue
		{
			get { return 5;	}
		}

		public int Value
		{
			get
			{
				return 5000;// - ((m_Base.Player.PawnsInPlay-5) * 125);  // lower the rook's value by 1/8 for each pawn above five of the side being valued, with the opposite adjustment for each pawn short of five
			}
		}

        private static readonly int[] m_aintSquareValues =
        {
			10,10,10,10,10,10,10,10,    0,0,0,0,0,0,0,0,
			10,20,20,20,20,20,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,30,30,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,30,30,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,20,20,20,20,20,10 ,   0,0,0,0,0,0,0,0,
			10,10,10,10,10,10,10,10 ,   0,0,0,0,0,0,0,0
        };

		public int PositionalPoints
		{
			get
			{
				int intPoints = 0;

				// After the opening, Rooks are penalized slightly depending on "taxicab" distance to the enemy king.
				if (Game.Stage != Game.enmStage.Opening)
				{
					intPoints -= this.m_Base.TaxiCabDistanceToEnemyKingPenalty();
				}

				if (Game.Stage != Game.enmStage.End)
				{
					// Rooks are given a bonus of 10(0) points for occupying a file with no friendly pawns and a bonus of 
					// 4(0) points if no enemy pawns lie on that file. 
					bool blnHasFiendlyPawn = false;
					bool blnHasEnemyPawn = false;
					Square squareThis = Board.GetSquare(this.m_Base.Square.File, 0);
					Piece piece;
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.enmName.Pawn)
						{
							if (piece.Player.Colour==this.m_Base.Player.Colour)
							{
								blnHasFiendlyPawn = true;
							}
							else
							{
								blnHasEnemyPawn = true;
							}
							if (blnHasFiendlyPawn && blnHasEnemyPawn) break;
						}
						squareThis = Board.GetSquare(squareThis.Ordinal + 16);
					}
					if (!blnHasFiendlyPawn)				
					{
						intPoints += 20;
					}
					if (!blnHasEnemyPawn)				
					{
						intPoints += 10;
					}

					// 7th rank
					if ( m_Base.Player.Colour==Player.enmColour.White && m_Base.Square.Rank==6
						||
						m_Base.Player.Colour==Player.enmColour.Black && m_Base.Square.Rank==1
						)
					{
						intPoints += 30;
					}

				}

				// Mobility
				Squares squares = new Squares();
				squares.Add(m_Base.Square);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, 1);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, -1);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, 16);
				Board.LineThreatenedBy(m_Base.Player, squares, m_Base.Square, -16);
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
			get { return (this.m_Base.Player.Colour==Player.enmColour.White ? 3 : 2); }
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
			Board.AppendPiecePath(moves, m_Base, m_Base.Player,  1, movesType);
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, -1, movesType);
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, -16, movesType);
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, 16, movesType);
		}

	}
}
