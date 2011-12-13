using System;

namespace SharpChess
{
	public class PieceKing: IPieceTop
	{
		static int[] CheckValues = { 0, 60, 180, 360, 500};

		Piece m_Base = null;

		public PieceKing(Piece pieceBase)
		{
			m_Base = pieceBase;
		}

		public Piece Base
		{
			get { return m_Base; }
		}

		public int BasicValue
		{
			get
			{
				return 15;
			}
		}

		public int Value
		{
			get
			{
				return 15000;
			}
		}

		private static int[] m_aintSquareValues =
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
	
				if (Game.Stage!=Game.enmStage.Opening && this.Base.Player.OtherPlayer.HasPieceName(Piece.enmName.Queen))
				{
					Piece piece;

					// Penalty for not having pawn directly in front
					piece = Board.GetPiece(this.m_Base.Square.Ordinal+this.m_Base.Player.PawnForwardOffset);
					if (piece==null || piece.Name!=Piece.enmName.Pawn || piece.Player.Colour!=this.m_Base.Player.Colour)
					{
						intPoints -= 75;
						piece = Board.GetPiece(this.m_Base.Square.Ordinal+this.m_Base.Player.PawnForwardOffset*2);
						if (piece==null || piece.Name!=Piece.enmName.Pawn || piece.Player.Colour!=this.m_Base.Player.Colour)
						{
							intPoints -= 150;
						}
					}

                    // Penalty for first movement the king, other than castling. This is to stop the king dancing around its
                    // own pawns in an attempt to get better protection, at the expense of developing other pieces.
                    if (m_Base.Player.HasCastled)
                    {
                        if (this.m_Base.NoOfMoves >= 2)
                        {
                            intPoints -= 200;
                        }
                    }
                    else 
                    {
                        if (this.m_Base.NoOfMoves >= 1)
                        {
                            intPoints -= 200;
                        }
                    }

					// Penalty for number of open lines to king
					intPoints -= Openness(this.m_Base.Square);

					// Penalty for half-open adjacent files
					bool blnHasFiendlyPawn;
					Square squareThis;

					blnHasFiendlyPawn = false;
					squareThis = Board.GetSquare(this.m_Base.Square.File, m_Base.Square.Rank);
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==this.m_Base.Player.Colour)
						{
							blnHasFiendlyPawn = true;
							break;
						}
						squareThis = Board.GetSquare(squareThis.Ordinal + m_Base.Player.PawnForwardOffset);
					}
					if (!blnHasFiendlyPawn)
					{
						intPoints -= 200;
					}

					blnHasFiendlyPawn = false;
					squareThis = Board.GetSquare(this.m_Base.Square.File+1, m_Base.Square.Rank);
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==this.m_Base.Player.Colour)
						{
							blnHasFiendlyPawn = true;
							break;
						}
						squareThis = Board.GetSquare(squareThis.Ordinal + m_Base.Player.PawnForwardOffset);
					}
					if (!blnHasFiendlyPawn)
					{
						intPoints -= 200;
					}

					blnHasFiendlyPawn = false;
					squareThis = Board.GetSquare(this.m_Base.Square.File-1, m_Base.Square.Rank);
					while (squareThis!=null)
					{
						piece = squareThis.Piece;
						if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==this.m_Base.Player.Colour)
						{
							blnHasFiendlyPawn = true;
							break;
						}
						squareThis = Board.GetSquare(squareThis.Ordinal + m_Base.Player.PawnForwardOffset);
					}
					if (!blnHasFiendlyPawn)
					{
						intPoints -= 200;
					}
				}

				switch (Game.Stage)
				{
					case Game.enmStage.End:
						// Bonus for number of moves available
						Moves moves = new Moves();
						this.GenerateLazyMoves(moves, Moves.enmMovesType.All);
						intPoints += moves.Count*10;

						// Bonus for being in centre of board
						intPoints += m_aintSquareValues[m_Base.Square.Ordinal];
						break;

					default: // Opening & Middle
						// Penalty for being in centre of board
						intPoints -= m_aintSquareValues[m_Base.Square.Ordinal];

						break;
				}
				return intPoints;
			}
		}

		private int OpenLinePenalty(Player.enmColour colour, Square squareKing, int intDirectionOffset)
		{
			Square square = Board.GetSquare(squareKing.Ordinal + intDirectionOffset);
			Piece piece;
			int intPenalty = 0;

			while ( square!=null )
			{
				piece = square.Piece;
				if (piece!=null)
				{
					if ( piece.Player.Colour==colour && piece.Name==Piece.enmName.Pawn )
					{
						break;
					}
				}
				intPenalty+=10;
				square = Board.GetSquare(square.Ordinal + intDirectionOffset);
			}
			return intPenalty;
		}

		private void MoveSquares(ref Squares squares)
		{
			Square square;

			square = Board.GetSquare(m_Base.Square.Ordinal-1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) squares.Add(square);
			square = Board.GetSquare(m_Base.Square.Ordinal+15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) squares.Add(square);
			square = Board.GetSquare(m_Base.Square.Ordinal+16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) squares.Add(square);
			square = Board.GetSquare(m_Base.Square.Ordinal+17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) squares.Add(square);
			square = Board.GetSquare(m_Base.Square.Ordinal+1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) squares.Add(square);
			square = Board.GetSquare(m_Base.Square.Ordinal-15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) squares.Add(square);
			square = Board.GetSquare(m_Base.Square.Ordinal-16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) squares.Add(square);
			square = Board.GetSquare(m_Base.Square.Ordinal-17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) squares.Add(square);
		}

		private int Openness(Square squareKing)
		{
			Square square = squareKing;

			int intOpenness = 0;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 16); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
			//			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,  1); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-16); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
			//			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, -1); if (intOpenness>900) goto exitpoint;
			intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
			/*
						square = Board.GetSquare(squareKing.Ordinal-1);
						if (square!=null)
						{
							intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
							intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
							intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
							intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
						}

						square = Board.GetSquare(squareKing.Ordinal+1);
						if (square!=null)
						{
							intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
							intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
							intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
							intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
						}
			*/
			exitpoint:
				return intOpenness;
		}

		private bool PawnIsAdjacent(int intOrdinal)
		{
			Piece piece;
			piece = Board.GetPiece( intOrdinal+15 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal+16 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal+17 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-15 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-16 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-17 ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal+1  ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			piece = Board.GetPiece( intOrdinal-1  ); if (piece!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==m_Base.Player.Colour) return true; 
			return false;
		}

		public int ImageIndex
		{
			get { return (this.m_Base.Player.Colour==Player.enmColour.White ? 5 : 4); }
		}
	
		public bool DetermineCheckStatus()
		{
			return m_Base.Square.CanBeMovedToBy(m_Base.Player.OtherPlayer);
		}

		public bool CanCastleKingSide
		{
			get
			{
				Piece pieceRook;
				// King hasnt moved
				if (m_Base.HasMoved) return false; 
				// Rook is still there i.e. hasnt been taken
				pieceRook = m_Base.Player.Colour==Player.enmColour.White ? Board.GetPiece(7,0):Board.GetPiece(7,7);
				if ( pieceRook==null || pieceRook.Name!=Piece.enmName.Rook || pieceRook.Player.Colour!=this.m_Base.Player.Colour ) return false;
				if ( !pieceRook.IsInPlay ) return false;
				// King's Rook has moved
				if (pieceRook.HasMoved) return false; 
				// All squares between King and Rook are unoccupied
				if ( Board.GetPiece(m_Base.Square.Ordinal+1)!=null ) return false;
				if ( Board.GetPiece(m_Base.Square.Ordinal+2)!=null ) return false;
				// King is not in check
				if (m_Base.Player.IsInCheck) return false;
				// The king does not move over a square that is attacked by an enemy piece during the castling move
				if ( Board.GetSquare(m_Base.Square.Ordinal+1).CanBeMovedToBy(m_Base.Player.OtherPlayer)  ) return false;
				if ( Board.GetSquare(m_Base.Square.Ordinal+2).CanBeMovedToBy(m_Base.Player.OtherPlayer)  ) return false;

				return true;
			}
		}

		public bool CanCastleQueenSide
		{
			get
			{
				Piece pieceRook;
				// King hasnt moved
				if (m_Base.HasMoved) return false; 
				// Rook is still there i.e. hasnt been taken
				pieceRook = m_Base.Player.Colour==Player.enmColour.White ? Board.GetPiece(0,0):Board.GetPiece(0,7);
				if ( pieceRook==null || pieceRook.Name!=Piece.enmName.Rook || pieceRook.Player.Colour!=this.m_Base.Player.Colour ) return false;
				if ( !pieceRook.IsInPlay ) return false;
				// King's Rook hasnt moved
				if (pieceRook.HasMoved) return false; 
				// All squares between King and Rook are unoccupied
				if ( Board.GetPiece(m_Base.Square.Ordinal-1)!=null ) return false;
				if ( Board.GetPiece(m_Base.Square.Ordinal-2)!=null ) return false;
				if ( Board.GetPiece(m_Base.Square.Ordinal-3)!=null ) return false;
				// King is not in check
				if (m_Base.Player.IsInCheck) return false;
				// The king does not move over a square that is attacked by an enemy piece during the castling move
				if ( Board.GetSquare(m_Base.Square.Ordinal-1).CanBeMovedToBy(m_Base.Player.OtherPlayer)  ) return false;
				if ( Board.GetSquare(m_Base.Square.Ordinal-2).CanBeMovedToBy(m_Base.Player.OtherPlayer)  ) return false;

				return true;
			}
		}

		public string Abbreviation
		{
			get {return "K";}
		}

		public Piece.enmName Name
		{
			get {return Piece.enmName.King;}
		}

		public bool IsCapturable
		{
			get
			{
				return false;
			}
		}

		public void GenerateLazyMoves(Moves moves, Moves.enmMovesType movesType)
		{
			Square square;
			switch (movesType)
			{
				case Moves.enmMovesType.All:
					square = Board.GetSquare(m_Base.Square.Ordinal-1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+1); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-15); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-16); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-17); if ( square!=null && (square.Piece==null || (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);

					if (this.CanCastleKingSide)
					{
						moves.Add(0, 0, Move.enmName.CastleKingSide, m_Base, m_Base.Square, Board.GetSquare(m_Base.Square.Ordinal+2), null, 0, 0);
					}
					if (this.CanCastleQueenSide)
					{
						moves.Add(Game.TurnNo, m_Base.LastMoveTurnNo, Move.enmName.CastleQueenSide, m_Base, m_Base.Square, Board.GetSquare(m_Base.Square.Ordinal-2), null, 0, 0);
					}

					break;

				case Moves.enmMovesType.CapturesChecksPromotions:
					square = Board.GetSquare(m_Base.Square.Ordinal-1); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+15); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+16); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+17); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal+1); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-15); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-16); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					square = Board.GetSquare(m_Base.Square.Ordinal-17); if ( square!=null && (square.Piece!=null && (square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable))) moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					break;
			}

		}

	}
}
