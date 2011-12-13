using System;

namespace SharpChess
{
	public class Square
	{
		static int[] m_aintVectors =
		{
			0,  0,  0,  0,  0,  0,  0,  0,   -15,-17,  0,  0,  0,  0,  0,  0,
			-16,  0,  0,  0,  0,  0,  0,-15,     0,  0,-17,  0,  0,  0,  0,  0,
			-16,  0,  0,  0,  0,  0,-15,  0,     0,  0,  0,-17,  0,  0,  0,  0,
			-16,  0,  0,  0,  0,-15,  0,  0,     0,  0,  0,  0,-17,  0,  0,  0,
			-16,  0,  0,  0,-15,  0,  0,  0,     0,  0,  0,  0,  0,-17,  0,  0,
			-16,  0,  0,-15,  0,  0,  0,  0,     0,  0,  0,  0,  0,  0,-17,100,
			-16,100,-15,  0,  0,  0,  0,  0 ,    0,  0,  0,  0,  0,  0,100,-17,
			-16,-15,100,  0,  0,  0,  0,  0 ,    0, -1, -1, -1, -1, -1, -1, -1,
			0,
			1,  1,  1,  1,  1,  1,  1,  0,     0,  0,  0,  0,  0,100, 15, 16,
			17,100,  0,  0,  0,  0,  0,  0,     0,  0,  0,  0,  0, 15,100, 16,
			100, 17,  0,  0,  0,  0,  0,  0,     0,  0,  0,  0, 15,  0,  0, 16,
			0,  0, 17,  0,  0,  0,  0,  0,     0,  0,  0, 15,  0,  0,  0, 16,
			0,  0,  0, 17,  0,  0,  0,  0,     0,  0, 15,  0,  0,  0,  0, 16,
			0,  0,  0,  0, 17,  0,  0,  0,     0, 15,  0,  0,  0,  0,  0, 16,
			0,  0,  0,  0,  0, 17,  0,  0 ,   15,  0,  0,  0,  0,  0,  0, 16,
			0,  0,  0,  0,  0,  0, 17, 15 ,    0,  0,  0,  0,  0,  0,  0,  0
		};

		static char[] m_aintMinorAttackers =
		{
			'.','.','.','.','.','.','.','.',   'B','B','.','.','.','.','.','.',
			'R','.','.','.','.','.','.','B',   '.','.','B','.','.','.','.','.',
			'R','.','.','.','.','.','B','.',   '.','.','.','B','.','.','.','.',
			'R','.','.','.','.','B','.','.',   '.','.','.','.','B','.','.','.',
			'R','.','.','.','B','.','.','.',   '.','.','.','.','.','B','.','.',
			'R','.','.','B','.','.','.','.',   '.','.','.','.','.','.','B','N',
			'R','N','B','.','.','.','.','.' ,  '.','.','.','.','.','.','N','B',
			'R','B','N','.','.','.','.','.' ,  '.','R','R','R','R','R','R','R',
			'.',
			'R','R','R','R','R','R','R','.',   '.','.','.','.','.','N','B','R',
			'B','N','.','.','.','.','.','.',   '.','.','.','.','.','B','N','R',
			'N','B','.','.','.','.','.','.',   '.','.','.','.','B','.','.','R',
			'.','.','B','.','.','.','.','.',   '.','.','.','B','.','.','.','R',
			'.','.','.','B','.','.','.','.',   '.','.','B','.','.','.','.','R',
			'.','.','.','.','B','.','.','.',   '.','B','.','.','.','.','.','R',
			'.','.','.','.','.','B','.','.' ,  'B','.','.','.','.','.','.','R',
			'.','.','.','.','.','.','B','B' ,  '.','.','.','.','.','.','.','.'
		};

		static char[] m_aintQueenAttackers =
		{
			'.','.','.','.','.','.','.','.',   'Q','Q','.','.','.','.','.','.',
			'Q','.','.','.','.','.','.','Q',   '.','.','Q','.','.','.','.','.',
			'Q','.','.','.','.','.','Q','.',   '.','.','.','Q','.','.','.','.',
			'Q','.','.','.','.','Q','.','.',   '.','.','.','.','Q','.','.','.',
			'Q','.','.','.','Q','.','.','.',   '.','.','.','.','.','Q','.','.',
			'Q','.','.','Q','.','.','.','.',   '.','.','.','.','.','.','Q','.',
			'Q','.','Q','.','.','.','.','.' ,  '.','.','.','.','.','.','.','Q',
			'Q','Q','.','.','.','.','.','.' ,  '.','Q','Q','Q','Q','Q','Q','Q',
			'.',
			'Q','Q','Q','Q','Q','Q','Q','.',   '.','.','.','.','.','.','Q','Q',
			'Q','.','.','.','.','.','.','.',   '.','.','.','.','.','Q','.','Q',
			'.','Q','.','.','.','.','.','.',   '.','.','.','.','Q','.','.','Q',
			'.','.','Q','.','.','.','.','.',   '.','.','.','Q','.','.','.','Q',
			'.','.','.','Q','.','.','.','.',   '.','.','Q','.','.','.','.','Q',
			'.','.','.','.','Q','.','.','.',   '.','Q','.','.','.','.','.','Q',
			'.','.','.','.','.','Q','.','.' ,  'Q','.','.','.','.','.','.','Q',
			'.','.','.','.','.','.','Q','Q' ,  '.','.','.','.','.','.','.','.'
		};

		static char[] m_aintKingAttackers =
		{
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','K',
			'K','K','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','K',
			'.',
			'K','.','.','.','.','.','.','.',   '.','.','.','.','.','.','K','K',
			'K','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','.',
			'.','.','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','.'
		};

		private static int[] m_aintSquareValues =
		{
			1, 1, 1, 1, 1, 1, 1, 1,    0,0,0,0,0,0,0,0,
			1,10,10,10,10,10,10, 1,    0,0,0,0,0,0,0,0,
			1,10,25,25,25,25,10, 1,    0,0,0,0,0,0,0,0,
			1,10,25,50,50,25,10, 1,    0,0,0,0,0,0,0,0,
			1,10,25,50,50,25,10, 1,    0,0,0,0,0,0,0,0,
			1,10,25,25,25,25,10, 1,    0,0,0,0,0,0,0,0,
			1,10,10,10,10,10,10, 1 ,   0,0,0,0,0,0,0,0,
			1, 1, 1, 1, 1, 1, 1, 1 ,   0,0,0,0,0,0,0,0
		};

		public enum enmColour
		{
				White
			,	Black
		}

		private enmColour m_colour = enmColour.White;
		private Piece m_piece = null;
		private int m_intFile = 0;
		private int m_intRank = 0;
		private int m_intOrdinal = 0;

		public Square(int Ordinal)
		{
			m_intOrdinal = Ordinal;
			m_intFile = Ordinal % Board.MATRIX_WIDTH;
			m_intRank = Ordinal / Board.MATRIX_WIDTH;

			if (m_intFile==0 || m_intFile==2 || m_intFile==4 || m_intFile==6)
			{
				if (m_intRank==0 || m_intRank==2 || m_intRank==4 || m_intRank==6)
				{
					m_colour = enmColour.Black;
				}
				else
				{
					m_colour = enmColour.White;
				}
			}
			else
			{
				if (m_intRank==0 || m_intRank==2 || m_intRank==4 || m_intRank==6)
				{
					m_colour = enmColour.White;
				}
				else
				{
					m_colour = enmColour.Black;
				}
			}

		}

		public int Value
		{
			get
			{
				return m_aintSquareValues[this.Ordinal];
			}
		}

		public enmColour Colour
		{
			get { return m_colour; }
		}
	
		public int File
		{
			get { return m_intFile; }
		}

		public int Rank
		{
			get { return m_intRank; }
		}

		public int Ordinal
		{
			get { return m_intOrdinal; }
		}

		public ulong HashCodeA
		{
			get
			{
				return this.Piece==null ? 0UL : this.Piece.HashCodeAForSquareOrdinal(this.Ordinal) ;
			}
		}

		public ulong HashCodeB
		{
			get
			{
				return this.Piece==null ? 0UL : this.Piece.HashCodeBForSquareOrdinal(this.Ordinal) ;
			}
		}

		public string FileName
		{
			get
			{
				string[] FileNames = {"a", "b", "c", "d", "e", "f", "g", "h"};
				return FileNames[m_intFile];
			}
		}

		public string RankName
		{
			get
			{
				return (m_intRank+1).ToString();
			}
		}

		public string Name
		{
			get
			{
				return FileName + RankName;
			}
		}

		public Piece Piece
		{
			get	{ return m_piece; }
			set {  m_piece = value; }
		}

		public bool CanBeMovedToBy(Player player)
		{
			Piece piece;

			// Pawn
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) return true;
				
			// Knight
			piece = Board.GetPiece( m_intOrdinal+33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( m_intOrdinal+18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( m_intOrdinal-14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( m_intOrdinal-31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( m_intOrdinal-33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( m_intOrdinal-18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( m_intOrdinal+14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return true;
			piece = Board.GetPiece( m_intOrdinal+31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return true;

			// Bishop & Queen
			if (Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 15)!=null) return true;
			if (Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 17)!=null) return true;
			if (Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -15)!=null) return true;
			if (Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -17)!=null) return true;
				
			// Rook & Queen
			if (Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 1)!=null) return true;
			if (Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -1)!=null) return true;
			if (Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 16)!=null) return true;
			if (Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -16)!=null) return true;

			// King!
			piece = Board.GetPiece( m_intOrdinal+16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return true; 
			piece = Board.GetPiece( m_intOrdinal+17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return true; 
			piece = Board.GetPiece( m_intOrdinal+1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return true; 
			piece = Board.GetPiece( m_intOrdinal-15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return true; 
			piece = Board.GetPiece( m_intOrdinal-16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return true; 
			piece = Board.GetPiece( m_intOrdinal-17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return true; 
			piece = Board.GetPiece( m_intOrdinal-1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return true; 
			piece = Board.GetPiece( m_intOrdinal+15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return true; 

			return false;
		}
	
        /// <summary>
        /// Appends a list of moves of all the pieces that are attacking this square.
        /// </summary>
        /// <param name="moves">Moves of pieces that are attacking this square.</param>
        /// <param name="player">player whose turn it is</param>
		public void AttackersMoveList(Moves moves, Player player)
		{
			Piece piece;

			// Pawn
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, Board.GetPiece(m_intOrdinal-player.PawnAttackLeftOffset), Board.GetSquare(m_intOrdinal-player.PawnAttackLeftOffset), this, this.Piece, 0, 0);
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, Board.GetPiece(m_intOrdinal-player.PawnAttackRightOffset), Board.GetSquare(m_intOrdinal-player.PawnAttackRightOffset), this, this.Piece, 0, 0);
				
			// Knight
			piece = Board.GetPiece( m_intOrdinal+33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			piece = Board.GetPiece( m_intOrdinal+18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			piece = Board.GetPiece( m_intOrdinal-14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			piece = Board.GetPiece( m_intOrdinal-31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			piece = Board.GetPiece( m_intOrdinal-33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			piece = Board.GetPiece( m_intOrdinal-18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			piece = Board.GetPiece( m_intOrdinal+14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			piece = Board.GetPiece( m_intOrdinal+31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;

			// Bishop & Queen
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 15))!=null) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 17))!=null) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -15))!=null) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -17))!=null) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
				
			// Rook & Queen
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 1))!=null) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -1))!=null) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 16))!=null) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -16))!=null) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);;

			// King!
			piece = Board.GetPiece( m_intOrdinal+16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);; 
			piece = Board.GetPiece( m_intOrdinal+17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);; 
			piece = Board.GetPiece( m_intOrdinal+1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);; 
			piece = Board.GetPiece( m_intOrdinal-15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);; 
			piece = Board.GetPiece( m_intOrdinal-16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);; 
			piece = Board.GetPiece( m_intOrdinal-17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);; 
			piece = Board.GetPiece( m_intOrdinal-1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);; 
			piece = Board.GetPiece( m_intOrdinal+15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) moves.Add(0, 0, Move.enmName.Standard, piece, piece.Square, this, this.Piece, 0, 0);; 
		}

		public bool CanSlideToHereFrom(Square squareStart, int Offset)
		{
			int intOrdinal = squareStart.Ordinal;
			Square square;

			intOrdinal += Offset;
			while ( (square = Board.GetSquare(intOrdinal))!=null )
			{
				if ( square==this )
				{
					return true;
				}
				if ( square.Piece!=null )
				{
					return false;
				}
				intOrdinal += Offset;
			}
			throw new ApplicationException("CanSlideToHereFrom: Hit edge of board!");
		}

		public int DefencePointsFor(Player player)
		{
			Piece piece;
			int Value = 0;
			int BestValue = 0;

			// Pawn
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) return piece.Value;
				
			// Knight
			piece = Board.GetPiece( m_intOrdinal+33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( m_intOrdinal+18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( m_intOrdinal-14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( m_intOrdinal-31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( m_intOrdinal-33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( m_intOrdinal-18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( m_intOrdinal+14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;
			piece = Board.GetPiece( m_intOrdinal+31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece.Value;

			// Bishop & Queen
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 15); Value = piece!=null ? piece.Value : 0; if (Value>0 && Value <9000 ) return Value; if (Value>0) BestValue=Value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 17); Value = piece!=null ? piece.Value : 0; if (Value>0 && Value <9000 ) return Value; if (Value>0) BestValue=Value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -15); Value = piece!=null ? piece.Value : 0; if (Value>0 && Value <9000 ) return Value; if (Value>0) BestValue=Value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -17); Value = piece!=null ? piece.Value : 0; if (Value>0 && Value <9000 ) return Value; if (Value>0) BestValue=Value;

			// Rook & Queen
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 1); Value = piece!=null ? piece.Value : 0; if (Value>0 && Value <9000 ) return Value; if (Value>0) BestValue=Value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -1); Value = piece!=null ? piece.Value : 0; if (Value>0 && Value <9000 ) return Value; if (Value>0) BestValue=Value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 16); Value = piece!=null ? piece.Value : 0; if (Value>0 && Value <9000 ) return Value; if (Value>0) BestValue=Value;
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -16); Value = piece!=null ? piece.Value : 0; if (Value>0 && Value <9000 ) return Value; if (Value>0) BestValue=Value;

			if (BestValue > 0) return BestValue; // This means a queen was found, but not a Bishop or Rook

			// King!
			piece = Board.GetPiece( m_intOrdinal+16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( m_intOrdinal+17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( m_intOrdinal+1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( m_intOrdinal-15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( m_intOrdinal-16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( m_intOrdinal-17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( m_intOrdinal-1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece.Value; 
			piece = Board.GetPiece( m_intOrdinal+15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece.Value; 

			return 15000;
		}

		public Piece DefendedBy(Player player)
		{
			Piece piece;
			Piece pieceBest = null;

			// Pawn
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) return piece;
				
			// Knight
			piece = Board.GetPiece( m_intOrdinal+33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( m_intOrdinal+18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( m_intOrdinal-14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( m_intOrdinal-31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( m_intOrdinal-33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( m_intOrdinal-18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( m_intOrdinal+14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece;
			piece = Board.GetPiece( m_intOrdinal+31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) return piece;

			// Bishop & Queen
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 15); if (piece!=null) { switch (piece.Name) { case Piece.enmName.Bishop: return piece; case Piece.enmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 17); if (piece!=null) { switch (piece.Name) { case Piece.enmName.Bishop: return piece; case Piece.enmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -15); if (piece!=null) { switch (piece.Name) { case Piece.enmName.Bishop: return piece; case Piece.enmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -17); if (piece!=null) { switch (piece.Name) { case Piece.enmName.Bishop: return piece; case Piece.enmName.Queen: pieceBest=piece; break; } }

			// Rook & Queen
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 1); if (piece!=null) { switch (piece.Name) { case Piece.enmName.Rook: return piece; case Piece.enmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -1); if (piece!=null) { switch (piece.Name) { case Piece.enmName.Rook: return piece; case Piece.enmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 16); if (piece!=null) { switch (piece.Name) { case Piece.enmName.Rook: return piece; case Piece.enmName.Queen: pieceBest=piece; break; } }
			piece = Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -16); if (piece!=null) { switch (piece.Name) { case Piece.enmName.Rook: return piece; case Piece.enmName.Queen: pieceBest=piece; break; } }

			if (pieceBest!=null) return pieceBest; // This means a queen was found, but not a Bishop or Rook

			// King!
			piece = Board.GetPiece( m_intOrdinal+16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( m_intOrdinal+17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( m_intOrdinal+1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( m_intOrdinal-15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( m_intOrdinal-16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( m_intOrdinal-17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( m_intOrdinal-1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece; 
			piece = Board.GetPiece( m_intOrdinal+15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) return piece; 

			return null;
		}

		public Pieces AttackersPieceList(Player player)
		{
			Piece piece;
			Pieces pieces = new Pieces(player);

			// Pawn
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackLeftOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( m_intOrdinal-player.PawnAttackRightOffset ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==player.Colour) pieces.Add(piece);
				
			// Knight
			piece = Board.GetPiece( m_intOrdinal+33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( m_intOrdinal+18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( m_intOrdinal-14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( m_intOrdinal-31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( m_intOrdinal-33 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( m_intOrdinal-18 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( m_intOrdinal+14 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);
			piece = Board.GetPiece( m_intOrdinal+31 ); if (piece!=null && piece.Name==Piece.enmName.Knight && piece.Player.Colour==player.Colour) pieces.Add(piece);

			// Bishop & Queen
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 15))!=null) pieces.Add(piece);
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, 17))!=null) pieces.Add(piece);
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -15))!=null) pieces.Add(piece);
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Bishop, this, -17))!=null) pieces.Add(piece);
				
			// Rook & Queen
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 1))!=null) pieces.Add(piece);
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -1))!=null) pieces.Add(piece);
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, 16))!=null) pieces.Add(piece);
			if ((piece=Board.LinesFirstPiece(player.Colour, Piece.enmName.Rook, this, -16))!=null) pieces.Add(piece);

			// King!
			piece = Board.GetPiece( m_intOrdinal+16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( m_intOrdinal+17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( m_intOrdinal+1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( m_intOrdinal-15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( m_intOrdinal-16 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( m_intOrdinal-17 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( m_intOrdinal-1  ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 
			piece = Board.GetPiece( m_intOrdinal+15 ); if (piece!=null && piece.Name==Piece.enmName.King && piece.Player.Colour==player.Colour) pieces.Add(piece); 

			return pieces;
		}



	}
}
