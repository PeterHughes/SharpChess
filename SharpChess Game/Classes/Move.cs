using System;

namespace SharpChess
{
	public class Move: IComparable
	{
		public enum enmName
		{
				Standard
			,	CastleQueenSide
			,	CastleKingSide
			,	PawnPromotionQueen
			,	PawnPromotionRook
			,	PawnPromotionKnight
			,	PawnPromotionBishop
			,	EnPassent
			,	NullMove
		}

		private Piece m_Piece;
		private Square m_From;
		private Square m_To;
		private Piece m_pieceCaptured;
		private Moves m_moves;
		private enmName m_Name;
		private int m_TurnNo;
		private int m_LastMoveTurnNo;
		private int m_pieceCapturedOrdinal;
		private int m_Score;
		private int m_Alpha;
		private int m_Beta;
		private ulong m_HashCodeA;
		private ulong m_HashCodeB;
		private bool m_IsInCheck = false;
		private bool m_IsEnemyInCheck = false;
		private Player.enmStatus m_EnemyStatus = Player.enmStatus.Normal;
		private TimeSpan m_TimeStamp;
		private bool m_IsThreeMoveRepetition = false;
		private int m_intFiftyMoveDrawCounter = 0;
		private int m_intChangeInScore = 0;

		public int MoveGeneratorPoints = 0;

		public Move(int TurnNo, int LastMoveTurnNo, Move.enmName Name, Piece piece, Square From, Square To, Piece pieceCaptured, int pieceCapturedOrdinal, int Score)
		{
			m_TurnNo = TurnNo;
			m_LastMoveTurnNo = LastMoveTurnNo;
			m_Name = Name;
			m_Piece = piece;
			m_From = From;
			m_To = To;
			m_pieceCaptured = pieceCaptured;
			m_pieceCapturedOrdinal = pieceCapturedOrdinal;
			m_Score = Score;
			if (Name != Move.enmName.NullMove && pieceCaptured == null && piece!=null && piece.Name != Piece.enmName.Pawn)
			{
				m_intFiftyMoveDrawCounter = Game.MoveHistory.Count > 0 ? Game.MoveHistory.Last.FiftyMoveDrawCounter + 1 : Game.FiftyMoveDrawBase / 2 + 1;
			}
		}

		public int CompareTo(object move)
		{
			if ( this.m_Score < ((Move)move).Score) return 1;
			if ( this.m_Score > ((Move)move).Score) return -1;
			return 0;
		}

		public string DebugText
		{
			get
			{
				return (Piece!=null ? this.Piece.Player.Colour.ToString() + " " + this.Piece.Name.ToString() : "") + " " + this.From.Name+(this.pieceCaptured==null ? "-" : "x")+this.To.Name + " " + (this.pieceCaptured==null ? "" : this.pieceCaptured.Name.ToString()) + " " + this.Name.ToString(); // + " A: " + this.Alpha + " B: " + this.Beta + " Score: " + this.Score;// + " h: " + this.m_HashEntries.ToString() + " c:" + this.m_HashCaptures.ToString();
			}
		}

		public int FiftyMoveDrawCounter
		{
			get { return m_intFiftyMoveDrawCounter; }
		}

		public bool IsFiftyMoveDraw
		{
			get { return m_intFiftyMoveDrawCounter>=100; }
		}

		public string Description
		{
			get
			{
				System.Text.StringBuilder strbMove = new System.Text.StringBuilder();
				switch (this.Name)
				{
					case Move.enmName.CastleKingSide:
						strbMove.Append("O-O");
						break;

					case Move.enmName.CastleQueenSide:
						strbMove.Append("O-O-O");
						break;

					default:
						if ((this.Piece.Name != Piece.enmName.Pawn) &&
							!this.Piece.HasBeenPromoted)
							strbMove.Append(this.Piece.Abbreviation);
						strbMove.Append(this.From.Name);
						if (this.pieceCaptured != null)
						{
							strbMove.Append("x");
							if (this.pieceCaptured.Name != Piece.enmName.Pawn)
								strbMove.Append(this.pieceCaptured.Abbreviation);
						}
						else
							strbMove.Append("-");
						strbMove.Append(this.To.Name);
						break;
				}

				if (this.Piece.HasBeenPromoted)
				{
					strbMove.Append(":");
					strbMove.Append(this.Piece.Abbreviation);
				}
				switch (m_EnemyStatus)
				{
					case Player.enmStatus.InCheckMate:
						strbMove.Append((this.m_Piece.Player.Colour == Player.enmColour.White) ?
							"# 1-0" : "# 0-1");   break;

					case Player.enmStatus.InStaleMate:
						strbMove.Append(" 1/2-1/2"); break;
               
					case Player.enmStatus.InCheck:
						strbMove.Append("+"); break;
				}
				if (this.IsThreeMoveRepetition || this.IsFiftyMoveDraw)
					strbMove.Append(" 1/2-1/2");

				return strbMove.ToString();
			}
		}

		public Moves Moves
		{
			get { return m_moves; }
			set { m_moves = value; }
		}

		public int TurnNo
		{
			get {return m_TurnNo;}
		}

		public int LastMoveTurnNo
		{
			get {return m_LastMoveTurnNo;}
		}

		public int MoveNo
		{
			get { return m_TurnNo/2+1; }
		}

		public enmName Name
		{
			get {return m_Name;}
		}

		public Piece Piece
		{
			get {return m_Piece;}
			set {m_Piece = value;}
		}

		public Player.enmStatus EnemyStatus
		{
			get {return m_EnemyStatus;}
			set {m_EnemyStatus = value;}
		}

		public Square From
		{
			get {return m_From;}
		}

		public Square To
		{
			get {return m_To;}
		}

		public Piece pieceCaptured
		{
			get {return m_pieceCaptured;}
		}

		public int pieceCapturedOrdinal
		{
			get {return m_pieceCapturedOrdinal;}
		}

		public ulong HashCodeA
		{
			get {return m_HashCodeA;}
			set {m_HashCodeA = value;}
		}

		public ulong HashCodeB
		{
			get {return m_HashCodeB;}
			set {m_HashCodeB = value;}
		}

		public int Score
		{
			get {return m_Score;}
			set {m_Score = value;}
		}

		public int ChangeInScore
		{
			get {return m_intChangeInScore;}
			set {m_intChangeInScore= value;}
		}

		public int Alpha
		{
			get {return m_Alpha;}
			set {m_Alpha = value;}
		}

		public int Beta
		{
			get {return m_Beta;}
			set {m_Beta = value;}
		}

		public bool IsInCheck
		{
			get {return m_IsInCheck;}
			set {m_IsInCheck = value;}
		}

		public bool IsEnemyInCheck
		{
			get {return m_IsEnemyInCheck;}
			set {m_IsEnemyInCheck = value;}
		}

		public TimeSpan TimeStamp
		{
			get { return m_TimeStamp; }
			set { m_TimeStamp = value; }
		}

		public bool IsThreeMoveRepetition
		{
			get {return m_IsThreeMoveRepetition;}
			set {m_IsThreeMoveRepetition = value;}
		}

		public static void Undo(Move move)
		{
			Board.HashCodeA ^= move.To.Piece.HashCodeA; // un_XOR the piece from where it was previously moved to
			Board.HashCodeB ^= move.To.Piece.HashCodeB; // un_XOR the piece from where it was previously moved to
			if (move.Piece.Name==Piece.enmName.Pawn) 
			{
				Board.PawnHashCodeA ^= move.To.Piece.HashCodeA;
				Board.PawnHashCodeB ^= move.To.Piece.HashCodeB;
			}

			move.Piece.Square = move.From;			// Set piece board location
			move.From.Piece = move.Piece;			// Set piece on board
			move.Piece.LastMoveTurnNo = move.LastMoveTurnNo;
			move.Piece.NoOfMoves--;

			if (move.Name!=Move.enmName.EnPassent)
			{
				move.To.Piece = move.pieceCaptured;	// Return piece taken
			}
			else
			{
				move.To.Piece = null;	// Blank square where this pawn was
				Board.GetSquare(move.To.Ordinal - move.Piece.Player.PawnForwardOffset ).Piece = move.pieceCaptured; // Return En Passent pawn taken
			}

			if (move.pieceCaptured != null)
			{
				move.pieceCaptured.Uncapture(move.pieceCapturedOrdinal);
				Board.HashCodeA ^= move.pieceCaptured.HashCodeA; // XOR back into play the piece that was taken
				Board.HashCodeB ^= move.pieceCaptured.HashCodeB; // XOR back into play the piece that was taken
				if (move.pieceCaptured.Name==Piece.enmName.Pawn) 
				{
					Board.PawnHashCodeA ^= move.pieceCaptured.HashCodeA;
					Board.PawnHashCodeB ^= move.pieceCaptured.HashCodeB;
				}
			}

			Piece pieceRook;
			switch (move.Name)
			{
				case Move.enmName.CastleKingSide:
					pieceRook = move.Piece.Player.Colour==Player.enmColour.White ? Board.GetPiece(5,0):Board.GetPiece(5,7);
					Board.HashCodeA ^= pieceRook.HashCodeA;
					Board.HashCodeB ^= pieceRook.HashCodeB;
					pieceRook.Square = Board.GetSquare(7, move.Piece.Square.Rank);
					pieceRook.LastMoveTurnNo = move.LastMoveTurnNo;
					pieceRook.NoOfMoves--;
					Board.GetSquare(7, move.Piece.Square.Rank).Piece = pieceRook;
					Board.GetSquare(5, move.Piece.Square.Rank).Piece = null;
					move.Piece.Player.HasCastled = false;
					Board.HashCodeA ^= pieceRook.HashCodeA;
					Board.HashCodeB ^= pieceRook.HashCodeB;
					break;

				case Move.enmName.CastleQueenSide:
					pieceRook = move.Piece.Player.Colour==Player.enmColour.White ? Board.GetPiece(3,0):Board.GetPiece(3,7);
					Board.HashCodeA ^= pieceRook.HashCodeA;
					Board.HashCodeB ^= pieceRook.HashCodeB;
					pieceRook.Square = Board.GetSquare(0, move.Piece.Square.Rank);
					pieceRook.LastMoveTurnNo = move.LastMoveTurnNo;
					pieceRook.NoOfMoves--;
					Board.GetSquare(0, move.Piece.Square.Rank).Piece = pieceRook;
					Board.GetSquare(3, move.Piece.Square.Rank).Piece = null;
					move.Piece.Player.HasCastled = false;
					Board.HashCodeA ^= pieceRook.HashCodeA;
					Board.HashCodeB ^= pieceRook.HashCodeB;
					break;

				case Move.enmName.PawnPromotionQueen:
				case Move.enmName.PawnPromotionRook:
				case Move.enmName.PawnPromotionBishop:
				case Move.enmName.PawnPromotionKnight:
					move.Piece.Demote();
					break;
			}

			Board.HashCodeA ^= move.From.Piece.HashCodeA; // XOR the piece back into the square it moved back to
			Board.HashCodeB ^= move.From.Piece.HashCodeB; // XOR the piece back into the square it moved back to
			if (move.From.Piece.Name==Piece.enmName.Pawn) 
			{
				Board.PawnHashCodeA ^= move.From.Piece.HashCodeA;
				Board.PawnHashCodeB ^= move.From.Piece.HashCodeB;
			}

			if (move.IsThreeMoveRepetition)
			{
				Board.HashCodeA ^= 31;
				Board.HashCodeB ^= 29;
			}

			Game.TurnNo--;

			Game.MoveHistory.RemoveLast();
		}
		
		public static Move.enmName MoveNameFromString(string strMoveName)
		{
			if (strMoveName==Move.enmName.Standard.ToString()) return Move.enmName.Standard;
			if (strMoveName==Move.enmName.CastleKingSide.ToString()) return Move.enmName.CastleKingSide;
			if (strMoveName==Move.enmName.CastleQueenSide.ToString()) return Move.enmName.CastleQueenSide;
			if (strMoveName==Move.enmName.EnPassent.ToString()) return Move.enmName.EnPassent;
			if (strMoveName=="PawnPromotion") return Move.enmName.PawnPromotionQueen;
			if (strMoveName==Move.enmName.PawnPromotionQueen.ToString()) return Move.enmName.PawnPromotionQueen;
			if (strMoveName==Move.enmName.PawnPromotionRook.ToString()) return Move.enmName.PawnPromotionRook;
			if (strMoveName==Move.enmName.PawnPromotionBishop.ToString()) return Move.enmName.PawnPromotionBishop;
			if (strMoveName==Move.enmName.PawnPromotionKnight.ToString()) return Move.enmName.PawnPromotionKnight;
			return 0;
		}

		public static bool IsValid(Move moveProposed)
		{
			if (moveProposed.Piece != Board.GetPiece(moveProposed.From.Ordinal) ) return false;

			Moves movesPossible = new Moves();
			moveProposed.Piece.GenerateLazyMoves(movesPossible, Moves.enmMovesType.All);
			foreach (Move move in movesPossible)
			{
				if ( moveProposed.Name==move.Name && moveProposed.To.Ordinal==move.To.Ordinal )
				return true;
			}

			return false;
		}

		public static bool MovesMatch(Move moveA, Move moveB)
		{
			return (moveA!=null && moveB!=null && moveA.Piece==moveB.Piece && moveA.From==moveB.From && moveA.To==moveB.To && moveA.Name==moveB.Name && ( moveA.pieceCaptured==null && moveB.pieceCaptured==null || moveA.pieceCaptured!=null && moveB.pieceCaptured!=null && moveA.pieceCaptured==moveB.pieceCaptured  ) );
		}

		  /// <summary>Is the move a promotion of pawn</summary>
		  /// <returns>true if promotion otherwise false</returns>
		  /// <remarks>Keep the order of the enumeration <see cref="enmName"/>.PawnPromotionQueen before PawnPromotionBishop</remarks>
		  public bool IsPromotion()
		  {
			 return   (m_Name >= SharpChess.Move.enmName.PawnPromotionQueen) &&
				(m_Name <= SharpChess.Move.enmName.PawnPromotionBishop);

		  } // end IsPromotion
	
	}
}
