using System;

namespace SharpChess
{
	public class PieceQueen: IPieceTop
	{
		Piece m_Base = null;

		public PieceQueen(Piece pieceBase)
		{
			m_Base = pieceBase;
		}

		public Piece Base
		{
			get { return m_Base; }
		}

		public string Abbreviation
		{
			get {return "Q";}
		}

		public Piece.enmName Name
		{
			get {return Piece.enmName.Queen;}
		}

		public int BasicValue
		{
			get { return 9;	}
		}

		public int Value
		{
			get
			{
				return 9750;
			}
		}

		public int PositionalPoints
		{
			get
			{
				int intPoints = 0;

				// The queen is that after the opening it is penalized slightly for 
				// "taxicab" distance to the enemy king.
				if (Game.Stage == Game.enmStage.Opening)
				{
					if (m_Base.Player.Colour==Player.enmColour.White)
					{
						intPoints -= this.m_Base.Square.Rank * 7;
					}
					else
					{
						intPoints -= (7-this.m_Base.Square.Rank) * 7;
					}
				}
				else
				{
					intPoints -= this.m_Base.TaxiCabDistanceToEnemyKingPenalty();
				}

				intPoints += m_Base.DefensePoints;

				return intPoints;
			}
		}

		public int ImageIndex
		{
			get { return (this.m_Base.Player.Colour==Player.enmColour.White ? 11 : 10); }
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
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, 16, movesType);
			Board.AppendPiecePath(moves, m_Base, m_Base.Player,  1, movesType);
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, -1, movesType);
			Board.AppendPiecePath(moves, m_Base, m_Base.Player, -16, movesType);
		}

	}
}
