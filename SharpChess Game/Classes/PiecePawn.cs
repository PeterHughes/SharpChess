using System;

namespace SharpChess
{
	public class PiecePawn: IPieceTop
	{
		Piece m_Base = null;

		public PiecePawn(Piece pieceBase)
		{
			m_Base = pieceBase;
		}

		public Piece Base
		{
			get { return m_Base; }
		}

		public string Abbreviation
		{
			get {return "P";}
		}

		public Piece.enmName Name
		{
			get {return Piece.enmName.Pawn;}
		}

		public int BasicValue
		{
			get { return 1;	}
		}

		public int Value
		{
			get
			{
				return m_Base.Square.Rank==0 || m_Base.Square.Rank==7 ? 850 : 1000;
			}
		}

		static int[] aintAdvancementBonus = {0,0,0,45,75,120,240,999};
		static int[] aintFileBonus = {0,6,16,32,32,16,6,0};

		public int PositionalPoints
		{
			get
			{
				int intPoints = 0;
				int intIndex;
				Piece piece;

				// PENALITIES

				// Isolated or Doubled pawn
				bool blnIsIsolatedLeft = true;
				bool blnIsIsolatedRight = true;
				bool blnIsDouble = false;
				for (intIndex=this.m_Base.Player.Pieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = this.m_Base.Player.Pieces.Item(intIndex);
					if (piece.Name==Piece.enmName.Pawn && piece!=this.m_Base)
					{
						if (piece.Square.File==this.m_Base.Square.File)
						{
							blnIsDouble = true;
						}
						if (piece.Square.File==this.m_Base.Square.File-1)
						{
							blnIsIsolatedLeft = false;
						}
						if (piece.Square.File==this.m_Base.Square.File+1)
						{
							blnIsIsolatedRight = false;
						}
					}
				}
				if (blnIsIsolatedLeft)
				{
					switch (this.m_Base.Square.File)
					{
						case 0: intPoints -=  10; break;
						case 1: intPoints -=  30; break;
						case 2: intPoints -=  40; break;
						case 3: intPoints -=  50; break;
						case 4: intPoints -=  50; break;
						case 5: intPoints -=  40; break;
						case 6: intPoints -=  30; break;
						case 7: intPoints -=  10; break;
					}
				}
				if (blnIsIsolatedRight)
				{
					switch (this.m_Base.Square.File)
					{
						case 0: intPoints -=  10; break;
						case 1: intPoints -=  30; break;
						case 2: intPoints -=  40; break;
						case 3: intPoints -=  50; break;
						case 4: intPoints -=  50; break;
						case 5: intPoints -=  40; break;
						case 6: intPoints -=  30; break;
						case 7: intPoints -=  10; break;
					}
				}
				if (blnIsDouble)
				{
					intPoints -= 75;
				}
				else if (blnIsIsolatedLeft && blnIsIsolatedRight)
				{
					intPoints -= 100;
				}

				// Backward pawn
				bool blnIsBackward = true;
				if ( blnIsBackward && (piece = Board.GetPiece(this.m_Base.Square.Ordinal-1                                       ))!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==this.m_Base.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(this.m_Base.Square.Ordinal+1                                       ))!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==this.m_Base.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(this.m_Base.Square.Ordinal-this.m_Base.Player.PawnAttackLeftOffset ))!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==this.m_Base.Player.Colour) blnIsBackward = false;
				if ( blnIsBackward && (piece = Board.GetPiece(this.m_Base.Square.Ordinal-this.m_Base.Player.PawnAttackRightOffset))!=null && piece.Name==Piece.enmName.Pawn && piece.Player.Colour==this.m_Base.Player.Colour) blnIsBackward = false;
				if (blnIsBackward)
				{
					intPoints -= 30;
				}

				if (Game.Stage!=Game.enmStage.End)
				{
					//	Pawns on D or E files
					if (this.m_Base.Square.File==3 || this.m_Base.Square.File==4)
					{
						//	still at rank 2
						if (this.m_Base.Player.Colour==Player.enmColour.White && this.m_Base.Square.Rank==1
							||
							this.m_Base.Player.Colour==Player.enmColour.Black && this.m_Base.Square.Rank==6
							)
						{
							intPoints -= 200;
						}
					}
				}

				if (Game.Stage==Game.enmStage.Opening)
				{
					//	Pawns on D or E files
					if (this.m_Base.Square.File==3 || this.m_Base.Square.File==4)
					{
						// Bonus for rank 5
						if (this.m_Base.Player.Colour==Player.enmColour.White && this.m_Base.Square.Rank==4
							||
							this.m_Base.Player.Colour==Player.enmColour.Black && this.m_Base.Square.Rank==3
							)
						{
							intPoints -= 300;
						}

					}
				}

				// BONUSES

				// Encourage capturing towards the centre
				intPoints += aintFileBonus[m_Base.Square.File];

				// Advancement
				int intAdvancementBonus = aintAdvancementBonus[m_Base.Player.Colour==Player.enmColour.White ? m_Base.Square.Rank : 7-m_Base.Square.Rank];

				// Passed Pawns
				bool blnIsPassed = true;
				for (intIndex=this.m_Base.Player.OtherPlayer.Pieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = this.m_Base.Player.OtherPlayer.Pieces.Item(intIndex);
					if (piece.Name==Piece.enmName.Pawn)
					{
						if	(	(
							this.m_Base.Player.Colour==Player.enmColour.White && piece.Square.Rank > this.m_Base.Square.Rank
							||
							this.m_Base.Player.Colour==Player.enmColour.Black && piece.Square.Rank < this.m_Base.Square.Rank
							)
							&&
							(piece.Square.File==this.m_Base.Square.File || piece.Square.File==this.m_Base.Square.File-1 || piece.Square.File==this.m_Base.Square.File+1)
								
							)
						{
							blnIsPassed = false;
						}
					}
				}

				if (blnIsPassed)
				{
					intPoints += 80;
					intAdvancementBonus <<= 1;
				}

				// Slowly increase advancement bonus as material drops away.
				intAdvancementBonus = intAdvancementBonus * ((Game.MaxMaterialCount-Game.LowestMaterialCount) * 2) / Game.MaxMaterialCount;

				intPoints += intAdvancementBonus + EvalForkPawn();  // 15Mar06 Nimzo Added pawn fork bonus

				return intPoints;
			}
		}
/*
		public int DefencePoints
		{
			get 
			{
				// Add further subtraction, then add on a defense bonus
				if (blnIsIsolatedLeft || blnIsIsolatedRight || blnIsBackward)
				{
					switch (Game.Stage)
					{
						case Game.enmStage.Opening:
							intPoints += (m_Base.DefensePoints>>1) - 30;
							break;
						case Game.enmStage.Middle:
							intPoints += (m_Base.DefensePoints>>2) - 15;
							break;
					}
				}
			}
		}
*/

		public int ImageIndex
		{
			get { return (this.m_Base.Player.Colour==Player.enmColour.White ? 9 : 8); }
		}
	
		public bool IsCapturable
		{
			get
			{
				return true;
			}
		}
/*
		private Move.enmName MoveName(Player.enmColour colourPlayer, Square squareTo)
		{
			if (colourPlayer==Player.enmColour.White && squareTo.Rank==7 || colourPlayer==Player.enmColour.Black && squareTo.Rank==0)
			{
				return Move.enmName.PawnPromotion;
			}
			else
			{
				return Move.enmName.Standard;
			}
		}
*/
		public void GenerateLazyMoves(Moves moves, Moves.enmMovesType movesType)
		{
			Move.enmName[] PromotionTypes = { Move.enmName.PawnPromotionQueen, Move.enmName.PawnPromotionRook, Move.enmName.PawnPromotionKnight, Move.enmName.PawnPromotionBishop };
			Square square;
			bool blnIsPromotion = this.m_Base.Player.Colour==Player.enmColour.White && this.m_Base.Square.Rank==6
									||
									this.m_Base.Player.Colour==Player.enmColour.Black && this.m_Base.Square.Rank==1;
			int intMovesToGenerate = blnIsPromotion ? PromotionTypes.Length : 1;

			for (int intIndex=0; intIndex<intMovesToGenerate; intIndex++)
			{
				// Take right
				if ( (square = Board.GetSquare(m_Base.Square.Ordinal+m_Base.Player.PawnAttackRightOffset))!=null)
				{
					if (square.Piece != null && square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable)
					{
						moves.Add(0, 0, (blnIsPromotion ? PromotionTypes[intIndex] : Move.enmName.Standard), this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					}
				}

				// Take left
				if ( (square = Board.GetSquare(m_Base.Square.Ordinal+m_Base.Player.PawnAttackLeftOffset))!=null)
				{
					if (square.Piece != null && square.Piece.Player.Colour!=m_Base.Player.Colour && square.Piece.IsCapturable)
					{
						moves.Add(0, 0, (blnIsPromotion ? PromotionTypes[intIndex] : Move.enmName.Standard), this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					}
				}

				// Forward one
				if (movesType==Moves.enmMovesType.All || blnIsPromotion)
				{
					if ( (square = Board.GetSquare(m_Base.Square.Ordinal+m_Base.Player.PawnForwardOffset))!=null && square.Piece == null)
					{
						moves.Add(0, 0, (blnIsPromotion ? PromotionTypes[intIndex] : Move.enmName.Standard), this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
					}
				}
			}

			// Forward two
			if (movesType==Moves.enmMovesType.All)
			{
				if (!m_Base.HasMoved)
				{
					// Check one square ahead is not occupied
					if ( (square = Board.GetSquare(m_Base.Square.Ordinal+m_Base.Player.PawnForwardOffset))!=null && square.Piece == null)
					{
						if ( (square = Board.GetSquare(m_Base.Square.Ordinal+m_Base.Player.PawnForwardOffset+m_Base.Player.PawnForwardOffset))!=null && square.Piece == null)
						{
							moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
						}
					}
				}
			}

			// En Passent 
			if ( 
				this.m_Base.Square.Rank==4 && this.m_Base.Player.Colour==Player.enmColour.White 
				||
				this.m_Base.Square.Rank==3 && this.m_Base.Player.Colour==Player.enmColour.Black
				)
			{
				Piece piecePassed;
				// Left
				if ((piecePassed = Board.GetPiece(m_Base.Square.Ordinal-1))!=null && piecePassed.NoOfMoves==1 && piecePassed.LastMoveTurnNo==Game.TurnNo && piecePassed.Name==Piece.enmName.Pawn && piecePassed.Player.Colour!=m_Base.Player.Colour)
				{
					square = Board.GetSquare(m_Base.Square.Ordinal+m_Base.Player.PawnAttackLeftOffset);
					moves.Add(0, 0, Move.enmName.EnPassent, this.m_Base, this.m_Base.Square, square, piecePassed, 0, 0);
				}
				// Right
				if ((piecePassed = Board.GetPiece(m_Base.Square.Ordinal+1))!=null && piecePassed.NoOfMoves==1 && piecePassed.LastMoveTurnNo==Game.TurnNo && piecePassed.Name==Piece.enmName.Pawn && piecePassed.Player.Colour!=m_Base.Player.Colour)
				{
					square = Board.GetSquare(m_Base.Square.Ordinal+m_Base.Player.PawnAttackRightOffset);
					moves.Add(0, 0, Move.enmName.EnPassent, this.m_Base, this.m_Base.Square, square, piecePassed, 0, 0);
				}
			}

		}

		/// <summary>Eval the threat of fork by the pawn</summary>
		/// <remarks>15Mar06 Nimzo</remarks>>
		/// <returns><para>Min of the value of attacked pieces if fork</para>
		/// <para>30 if the pawn threatens a piece</para>
		/// <para>0 if not threat</para></returns>
		private int EvalForkPawn()
		{
			const int intTHREAT_PAWN = 30; 
			int intBonus = 0;
			Square squareLeft;
			Square squareRight;
			Piece pieceLeft = null;

			if ((squareLeft = Board.GetSquare(m_Base.Square.Ordinal + m_Base.Player.PawnAttackLeftOffset)) != null)
			{
				pieceLeft = squareLeft.Piece;
				if (pieceLeft != null && pieceLeft.Player.Colour != m_Base.Player.Colour && pieceLeft.IsCapturable && pieceLeft.Name != Piece.enmName.Pawn)
				{
					intBonus = intTHREAT_PAWN; // see where CanBeDrivenAwayByPawn is called
				}
			}
			if ((squareRight = Board.GetSquare(m_Base.Square.Ordinal + m_Base.Player.PawnAttackRightOffset)) != null)
			{
				Piece pieceRight = squareRight.Piece;
				if (pieceRight != null && pieceRight.Player.Colour != m_Base.Player.Colour && pieceRight.IsCapturable && pieceRight.Name != Piece.enmName.Pawn)
				{
					intBonus = (intBonus == intTHREAT_PAWN) ? Math.Min(pieceLeft.Value, pieceRight.Value) : intTHREAT_PAWN;
				}
			}
			return intBonus;

		} // end EvalForkPawn 
	}
}
