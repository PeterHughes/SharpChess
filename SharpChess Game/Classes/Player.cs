using System;
using System.Threading;
using System.Text; // for StringBuilder
using System.Diagnostics; // for Debug

namespace SharpChess
{
	public abstract class Player
	{
		public delegate void delegatetypePlayerEvent();

		public enum enmColour
		{
				White
			,	Black
		}

		public enum enmIntellegence
		{
				Human
			,	Computer
		}

		public enum enmStatus
		{
				Normal
			,	InCheck
			,	InStaleMate
			,	InCheckMate
		}

		private Thread m_threadThought = null;

		public event delegatetypePlayerEvent MoveConsidered;
		public event delegatetypePlayerEvent ThinkingBeginning;
		public event delegatetypePlayerEvent ReadyToMakeMove;
		private const int MIN_SCORE = int.MinValue+1;
		private const int MAX_SCORE = int.MaxValue;
		protected Piece m_King;
		private Moves m_movesPVBest;
		private Move m_moveCurrent;
		private bool m_blnHasCastled = false;
		protected enmColour m_colour;
		protected Pieces m_colPieces;
		protected Pieces m_colCapturedEnemyPieces;
		protected int m_NoOfPawnsInPlay = 8;
		protected int m_MaterialCount = 7;
		protected int m_intTotalPositionsToSearch = 0;
		protected int m_intSearchPositionNo = 0;
		protected int m_intEvaluations = 0;
		protected int m_intPositionsSearched = 0;
		private const int m_intMinimumSearchDepth = 1;
		private int m_intSearchDepth = 0;
		private bool m_blnForceImmediateMove = false;
		private bool m_blnDisplayMoveAnalysisTree = false;
		private bool m_blnIsPondering = false;
		private static ulong m_ulongPonderingHashCodeA = 0;
		private static ulong m_ulongPonderingHashCodeB = 0;
		private static DateTime m_dtmPonderingStart = new DateTime();

		private TimeSpan m_tsnThinkingTimeMaxAllowed;
		private TimeSpan m_tsnThinkingTimeAllotted;
		private TimeSpan m_tsnThinkingTimeCutoff;

		public enmIntellegence Intellegence;
		private int m_intMaxQDepthReached = 0;
		private int m_intMaxExtensionsReached = 0;
		
		private int m_intMinSearchDepth = 1;
		private int m_intMaxSearchDepth = 32;

		static int[] m_aintAttackBonus = {0, 3, 25, 53, 79, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99 };

		protected PlayerClock m_PlayerClock;

		public void Think()
		{
			//
			//	Determine the best move available for "this" player instance, from the current board position.
			//

			Debug.WriteLine(String.Format("Thread {0} is " + (this.IsPondering ? "pondering" : "thinking"), Thread.CurrentThread.Name ));

			Player player = this;		// Set the player, whose move is to be computed, to "this" player object instance
			m_movesPVBest = new Moves();// Best moves line (Principal Variation) found so far.
			Moves movesPV = new Moves();// Best moves line (Principal Variation) for the previously completed depth.
//			TimeSpan tsnTimePondered = new TimeSpan();
			int intTurnNo = Game.TurnNo;

			m_blnDisplayMoveAnalysisTree = Game.CaptureMoveAnalysisData; // Set whether to build a post-analysis tree of positions searched
			
			try
			{
				if (!m_blnIsPondering && !Game.IsInAnalyseMode)
				{
					// Query Simple Opening Book
					if (Game.UseRandomOpeningMoves)
					{
						Move moveBook;
						if ((moveBook = OpeningBookSimple.SuggestRandomMove(player) )!=null) 
						{
							m_movesPVBest.Add(moveBook);
                            this.MoveConsidered();
							throw new ForceImmediateMoveException();
						}
					}

		/*				// Query Best Opening Book
						if ((m_moveBest = OpeningBook.SearchForGoodMove(Board.HashCodeA, Board.HashCodeB, this.Colour) )!=null) 
						{
							m_moveCurrent = m_moveBest;
							this.MoveConsidered();
							throw new ForceImmediateMoveException();
						}
		*/			
				}

				// Time allowed for this player to think
				if (Game.ClockFixedTimePerMove.TotalSeconds>0)
				{
					// Absolute fixed time per move. No time is carried over from one move to the next.
					m_tsnThinkingTimeAllotted = Game.ClockFixedTimePerMove;
				}
				else if (Game.ClockIncrementPerMove.TotalSeconds>0)
				{
					// Incremental clock
					m_tsnThinkingTimeAllotted = new TimeSpan( Game.ClockIncrementPerMove.Ticks + ( ( Game.ClockIncrementPerMove.Ticks*Game.MoveNo + Game.ClockTime.Ticks*Math.Min(Game.MoveNo,40)/40 ) - this.m_PlayerClock.TimeElapsed.Ticks) / 3 );
					// Make sure we never think for less than half the "Increment" time
					m_tsnThinkingTimeAllotted = new TimeSpan( Math.Max(m_tsnThinkingTimeAllotted.Ticks, Game.ClockIncrementPerMove.Ticks/2+1) );
				}
				else if (Game.ClockMoves==0 && Game.ClockIncrementPerMove.TotalSeconds==0)
				{
					// Fixed game time
					m_tsnThinkingTimeAllotted = new TimeSpan( this.m_PlayerClock.TimeRemaining.Ticks / 30 );
				}
				else
				{
					// Conventional n moves in x minutes time
					m_tsnThinkingTimeAllotted = new TimeSpan( this.m_PlayerClock.TimeRemaining.Ticks / this.m_PlayerClock.MovesRemaining );
				}
				// Minimum of 1 second thinking time
				if (m_tsnThinkingTimeAllotted.TotalSeconds<1)
				{
					m_tsnThinkingTimeAllotted = new TimeSpan(0,0,1);
				}

				// The computer only stops "thinking" when it has finished a full ply of thought, 
				// UNLESS m_tsnThinkingTimeMaxAllowed is exceeded, or clock runs out, then it stops right away.
				if (Game.ClockFixedTimePerMove.TotalSeconds>0)
				{
					// Fixed time per move
					m_tsnThinkingTimeMaxAllowed = Game.ClockFixedTimePerMove;
				}
				else
				{
					// Variable time per move
					m_tsnThinkingTimeMaxAllowed = new TimeSpan( Math.Min(m_tsnThinkingTimeAllotted.Ticks*2, this.Clock.TimeRemaining.Ticks - (new TimeSpan(0,0,0,2)).Ticks ) );
				}
				// Minimum of 2 seconds thinking time
				if (m_tsnThinkingTimeMaxAllowed.TotalSeconds<2)
				{
					m_tsnThinkingTimeMaxAllowed = new TimeSpan(0,0,2);
				}

				// A new deeper ply of search will only be started, IF the cutoff time hasnt been reached yet.
				m_tsnThinkingTimeCutoff = new TimeSpan( m_tsnThinkingTimeAllotted.Ticks/3);

				m_blnForceImmediateMove	= false; // Set to stop thread thinking and return best move
				m_intPositionsSearched = 0; // Total number of positions considered so far
				m_intEvaluations = 0;		// Total number of times the evaluation function has been called (May be less than PositonsSearched if hashtable works well)

				if (Game.IsInAnalyseMode)
				{
					HashTable.Clear();		
					HashTableCheck.Clear();
					HashTablePawnKing.Clear();	
					History.Clear();			
				}
				else
				{

					if (this.CanClaimMoveRepetitionDraw(2)) // See if' we're in a 2 move repetition position, and if so, clear the hashtable, as old hashtable entries corrupt 3MR detection
					{
						HashTable.Clear();
					}
					else
					{
						HashTable.ResetStats();		// Reset the main hash table hit stats
					}

					HashTableCheck.ResetStats();// We also have a hash table in which we just store the check status for both players
					HashTablePawnKing.ResetStats();	// And finally a hash table that stores the positional score of just the pawns.
					History.Clear();			// Clear down the History Heuristic info, at the start of each move.
				}
				if (!this.IsPondering)
				{
					this.Clock.Start();
				}
		
				// Set max search depth, as defined is game difficulty settings
				m_intMaxSearchDepth = Game.MaximumSearchDepth==0 ? 32: Game.MaximumSearchDepth;

				// Here begins the main Iteractive Deepening loop of the entire search algorithm. (Cue dramitic music!)
				int intScore = player.Score;

				for (m_intSearchDepth=m_intMinSearchDepth; m_intSearchDepth<=m_intMaxSearchDepth; m_intSearchDepth++)
				{
					if (m_blnDisplayMoveAnalysisTree)
					{
						Game.MoveAnalysis = new Moves();
					}

                    intScore = Aspirate(player, m_intSearchDepth, ref movesPV, intScore);
                    //intScore = AlphaBeta(player, m_intSearchDepth, m_intSearchDepth, MIN_SCORE, MAX_SCORE, null, movesPV, intScore);

					m_movesPVBest = movesPV; // The best line is then recorded
				
					WinBoard.SendThinking(m_intSearchDepth, intScore, DateTime.Now-m_PlayerClock.TurnStartTime, m_intPositionsSearched, this.PrincipalVariationText); 

					if (!Game.IsInAnalyseMode && Game.ClockFixedTimePerMove.TotalSeconds==0 && !m_blnIsPondering && (DateTime.Now-m_PlayerClock.TurnStartTime) > m_tsnThinkingTimeCutoff )
					{
						//#if !DEBUG // Note the exclamation mark
						   throw new ForceImmediateMoveException();
						//#endif 					
					}

					if (intScore > 99999 || intScore < -99999) break; // Checkmate found so dont bother searching any deeper
				}

			}
			catch (ForceImmediateMoveException x)
			{
				// Undo any moves made during thinking
				Debug.WriteLine(x.ToString());
				while ( Game.TurnNo>intTurnNo )
				{
					Move.Undo( Game.MoveHistory.Last );
				}
			}

            if (this.MoveConsidered != null)
            {
                this.MoveConsidered();
            }
            
			Debug.WriteLine(String.Format("Thread {0} is ending " + (this.IsPondering ? "pondering" : "thinking"), Thread.CurrentThread.Name ));

			m_threadThought = null;
			if (this.MoveConsidered != null && !this.IsPondering)
			{
				ReadyToMakeMove();
			}
			m_blnIsPondering = false;

            // Send total elapsed time to generate this move.
            WinBoard.SendMoveTime(DateTime.Now - m_PlayerClock.TurnStartTime);
		}

		private int Aspirate(Player player, int depth, ref Moves movesPV_Parent, int intLastIteractionsScore)
		{
			int alpha = MIN_SCORE;		// The score of the best move found so far
			int beta = MAX_SCORE;		// The score of the best move found by the enemy
			int val = alpha;
			Moves movesPV = new Moves();// Best moves line (Principal Variation) for the previously completed depth.

			for (int intAttempt=0; intAttempt<3; intAttempt++)
			{
				switch (intAttempt)
				{
					case 0:
						alpha = intLastIteractionsScore - 500;
						beta = intLastIteractionsScore + 500;
						break;

					case 1:
						alpha = intLastIteractionsScore - 2000;
						beta = intLastIteractionsScore + 2000;
						break;

					case 2:
						alpha = MIN_SCORE;
						beta = MAX_SCORE;
						break;
				}

				val = AlphaBeta(player, m_intSearchDepth, m_intSearchDepth, alpha, beta, null, movesPV, 0);
				if (val > alpha && val < beta)
				{
					break;
				}

			}

			movesPV_Parent = movesPV;
			return val;
		}

		private int AlphaBeta(Player player, int ply, int depth, int alpha, int beta, Move moveAnalysed, Moves movesPV_Parent, int intTotalExtensions)
		{
            // TODO Add option of STANDING PAT in Quiescence Search.

			int val = int.MinValue;
			HashTable.enmHashType hashType = HashTable.enmHashType.Alpha;
			Move moveHash = null;
			Move moveBest = null;
			bool blnPVNode = false;
			int intScoreAtEntry = 0;
			bool blnAllMovesWereGenerated;
			int intLegalMovesAttempted = 0;
			bool blnIsInCheck = player.IsInCheck; 

			if (m_blnForceImmediateMove)
			{
				throw new ForceImmediateMoveException();
			}

			Moves movesPV = new Moves();

			m_intPositionsSearched++;

			if (moveAnalysed!=null && moveAnalysed.IsThreeMoveRepetition)
			{
				return -player.OtherPlayer.Score;
			}

			if ( (val = HashTable.ProbeHash(Board.HashCodeA, Board.HashCodeB, ply, alpha, beta, player.Colour)) != HashTable.UNKNOWN )
			{
				// High values of "val" indicate that a checkmate has been found
				if (val>1000000 || val<-1000000)
				{
                    if (m_intMaxSearchDepth - depth > 0)
                    {
                        val /= (m_intMaxSearchDepth - depth);
                    }
				}
				movesPV_Parent.Clear();
				if (HashTable.ProbeForBestMove(player.Colour)!=null)
				{
					movesPV_Parent.Add( HashTable.ProbeForBestMove(player.Colour) );
				}

				return val;
			}

			if (intTotalExtensions > m_intMaxExtensionsReached)
			{
				m_intMaxExtensionsReached = intTotalExtensions;
			}

			// Depth <=0 means we're into Quiescence searching
			if (depth <= 0)
			{
				if (depth < m_intMaxQDepthReached)
				{	
					m_intMaxQDepthReached = depth;
				}
				intScoreAtEntry = val = -player.OtherPlayer.Score;	
                m_intEvaluations++;

				if (val>1000000 || val<-1000000) 
				{
					val /= (m_intMaxSearchDepth-depth);
				}
				// Allow a deeper ply of search if a piece was captured or if a pawn was promoted, 
				if ( 
						!( 
						//	blnIsInCheck
						//	||
							moveAnalysed.pieceCaptured != null
							||
							moveAnalysed.Name==Move.enmName.PawnPromotionQueen
						)
					)
				{
					return val;
				}

			}

			if (m_blnDisplayMoveAnalysisTree)
			{
				moveAnalysed.Moves = new Moves();
			}

			Move moveThis = null;

			// Adaptive Null-move forward pruning
			int R = (depth>6 ? 3 : 2);
			if (depth>(R+1) && moveAnalysed!=null && moveAnalysed.Name!=Move.enmName.NullMove && Game.Stage!=Game.enmStage.End && !blnIsInCheck )
			{
				Move moveNull = new Move(Game.TurnNo, 0, Move.enmName.NullMove, null, null, null, null, 0, 0);
				val = -AlphaBeta(player.OtherPlayer, ply-1, depth - R - 1, -beta, -beta + 1, moveNull, movesPV, intTotalExtensions);
				if (val >= beta)
					return beta;
			}

			// Get last iteration's best move from the Transition Table
			moveHash = HashTable.ProbeForBestMove(player.Colour);

			// Get Killers
			Move moveKillerA = KillerMoves.RetrieveA(ply);
			Move moveKillerB = KillerMoves.RetrieveB(ply);
			Move moveKillerA2 = KillerMoves.RetrieveA(ply+2);
			Move moveKillerB2 = KillerMoves.RetrieveB(ply+2);

			// Generate moves
			Moves movesPossible = new Moves();
			blnAllMovesWereGenerated=(depth>0);  //|| blnIsInCheck); 
			if ( blnAllMovesWereGenerated ) 
			{
				player.GenerateLazyMoves(depth, movesPossible, Moves.enmMovesType.All, null);
			}
			else
			{
				// Captures only
				player.GenerateLazyMoves(depth, movesPossible, Moves.enmMovesType.CapturesChecksPromotions, moveAnalysed.To);
			}

			// Sort moves
            this.SortBestMoves(movesPossible, moveHash, moveKillerA, moveKillerA2, moveKillerB, moveKillerB2, player);

            //
            if (ply == m_intSearchDepth)
            {
                m_intTotalPositionsToSearch = movesPossible.Count;
                m_intSearchPositionNo = 0;
            }

			int intExtension;
			foreach (Move move in movesPossible)
			{
				moveThis = move.Piece.Move(move.Name, move.To);
				if (ply==m_intSearchDepth)
				{
					m_intSearchPositionNo++;
					m_moveCurrent = moveThis;
                    if (this.MoveConsidered != null)
                    {
                        this.MoveConsidered();
                    }
					m_intMaxQDepthReached = m_intSearchDepth;	// A little counter to record the deepest Quiescence depth searched on this move.
					m_intMaxExtensionsReached = 0;	// A little counter to track the number of extensions on this move.
				}

				if (player.IsInCheck) { Move.Undo(moveThis); continue; }

				intLegalMovesAttempted++;

				if (moveBest==null)
				{
					moveBest = moveThis;
				}

				if (m_blnDisplayMoveAnalysisTree)
				{
					// Add moves to post-move analysis tree, if option set by user
					moveAnalysed.Moves.Add(moveThis);
				}

				
				intExtension = 0;

				// Search Extensions
				if (movesPossible.Count==1)
				{
					// Single Response
					intExtension = 1;
				}

				else if (moveAnalysed!=null && moveAnalysed.IsEnemyInCheck)
				{
					// Check evasion
					intExtension = 1;
				}


				else if (moveAnalysed != null && moveAnalysed.pieceCaptured != null
						&& moveThis.pieceCaptured != null
						&& moveAnalysed.pieceCaptured.BasicValue == moveThis.pieceCaptured.BasicValue
						&& moveAnalysed.To == moveThis.To
						)
				{
					// Recapture piece of same basic value (on the same square)
					intExtension = 1;
				}

				else if (moveThis.Piece.Name==Piece.enmName.Pawn && (moveThis.Piece.Player.Colour==Player.enmColour.White && moveThis.To.Rank==6 || moveThis.Piece.Player.Colour==Player.enmColour.Black && moveThis.To.Rank==1 ) )
				{
					// Pawn push to 7th rank
					intExtension = 1;
				}

				// Reductions
				if (depth>2 && !blnIsInCheck && moveThis.pieceCaptured==null && !moveThis.IsEnemyInCheck)
				{
					int[] m_aintMargin = {0,   0,   0, 5000, 5000, 7000, 7000, 9000,9000,
										  15000,15000,15000,15000,15000,15000,15000,15000,15000 };
					//int intLazyEval = this.TotalPieceValue - this.OtherPlayer.TotalPieceValue;
					int intLazyEval = player.Score;
					if (alpha > intLazyEval + m_aintMargin[depth] )
					{
						intExtension = -1;
					}
				}

				// Futility Pruning
				if (!blnIsInCheck)
				{
					switch (depth)
					{
						case 2:
						case 3:
						//case 4:
							if (moveThis.pieceCaptured==null && !move.IsEnemyInCheck)
							{
								int intLazyEval = player.Score;

								switch (depth)
								{
									case 2:
										// Standard Futility Pruning
										if (intLazyEval+3000<=alpha)
										{
											intExtension = -1;
										}
										break;

									case 3:
										// Extended Futility Pruning
										if (intLazyEval+5000<=alpha)
										{
											intExtension = -1;
										}
										break;

									case 4:
										// Razoring
										if (intLazyEval+9750<=alpha)
										{
											intExtension = -1;
										}
										break;
								}
							}
							break;					
					}
				}
/*		
				if (intExtension>0 && intTotalExtensions>=m_intSearchDepth)
				{
					intExtension = 0;
				}
*/
				//#if DEBUG   // Avoid to break in a zero window research so alpha + 1 < beta
				//   if ((alpha + 1 < beta) && DebugMatchVariation(m_intSearchDepth - ply, moveThis)) Debugger.Break();
				//#endif
				if (blnPVNode)
				{
					val = -AlphaBeta(player.OtherPlayer, ply-1, (depth+intExtension) - 1, -alpha-1, -alpha, moveThis, movesPV, intTotalExtensions + intExtension);
					if ((val > alpha) && (val < beta)) /* fail */
					{
						val = -AlphaBeta(player.OtherPlayer, ply-1, (depth+intExtension) - 1, -beta, -alpha, moveThis, movesPV, intTotalExtensions + intExtension);
					}
				}
				else
				{
					val = -AlphaBeta(player.OtherPlayer, ply-1, (depth+intExtension) - 1, -beta, -alpha, moveThis, movesPV, intTotalExtensions + intExtension);
				}

				if (!blnAllMovesWereGenerated && val<intScoreAtEntry)
				{
					// This code is executed mostly in quiescence when not all moves are tried (maybe just captures)
					// and the best score we've got is worse than the score we had before we considered any moves
					// then revert to that score, because we dont want the computer to think that it HAS to make a capture
					val = intScoreAtEntry;
				}

				move.Score = moveThis.Score = val;

				Move.Undo(moveThis);

				if (val >= beta)
				{
					alpha = beta;
					moveThis.Beta = beta;
					hashType = HashTable.enmHashType.Beta;
					moveBest = moveThis;
					//if (move.Score < 15000)
					//{
                        History.Record(player.Colour, moveThis.From.Ordinal, moveThis.To.Ordinal, depth*depth);

                        // 15Mar06 Nimzo Don't include captures as killer moves
						if ((moveThis.pieceCaptured == null) &&
						   ((moveAnalysed == null) ||
							(moveAnalysed.Name != Move.enmName.NullMove)))
						{
                            KillerMoves.RecordPossibleKillerMove(ply, moveBest);
						} // End Nimzo code
					//}
					goto Exit;
				}

				if (val > alpha)
				{
					blnPVNode = true; /* This is a PV node */
					alpha = val;
					hashType = HashTable.enmHashType.Exact;
					moveBest = moveThis;
					// Collect the Prinicial Variation
					movesPV_Parent.Clear();
					movesPV_Parent.Add(moveThis);
					foreach (Move moveCopy in movesPV)
					{
						movesPV_Parent.Add(moveCopy);
					}
					//#if DEBUG
					//	Debug.WriteLineIf((ply == m_intSearchDepth) && (ply > 1), string.Format("{0} {1} {2}", ply, PvLine(movesPV_Parent), alpha));
					//#endif
				} 
				moveThis.Alpha = alpha;
				moveThis.Beta = beta;

				if (!Game.IsInAnalyseMode && !m_blnIsPondering && m_intSearchDepth>m_intMinimumSearchDepth && (DateTime.Now-m_PlayerClock.TurnStartTime) > m_tsnThinkingTimeMaxAllowed)
				{
					//#if !DEBUG // Note the exclamation mark
					   throw new ForceImmediateMoveException();
					//#endif 				
				}
			}

			// Check for Stalemate
			if (intLegalMovesAttempted==0) // depth>0 && !player.OtherPlayer.IsInCheck
			{
				//	alpha = this.Score;
				alpha = -player.OtherPlayer.Score;
			}

			Exit:

			// Record best move
			if (moveBest!=null)
			{
				HashTable.RecordHash(Board.HashCodeA, Board.HashCodeB, ply, alpha, hashType, moveBest.From.Ordinal, moveBest.To.Ordinal, moveBest.Name, player.Colour);
			}
			else
			{
				HashTable.RecordHash(Board.HashCodeA, Board.HashCodeB, ply, alpha, hashType, -1, -1, Move.enmName.NullMove, player.Colour);
			}

            return alpha;
		}

        /// <summary>
        /// Sorts moves so that the best moves are first
        /// </summary>
        /// <param name="movesToSort"></param>
        /// <param name="moveHash"></param>
        /// <param name="moveHash">Best move from hash table.</param>
        /// <param name="moveKillerA">Best killer move from this ply.</param>
        /// <param name="moveKillerA2">Second best killer move from this ply.</param>
        /// <param name="moveKillerB">Best killer move from previous ply.</param>
        /// <param name="moveKillerB2">Second best killer move from previous ply.</param>
        /// <param name="pawnForwardOffset"></param>
        private void SortBestMoves(Moves movesToSort, Move moveHash, Move moveKillerA, Move moveKillerA2, Move moveKillerB, Move moveKillerB2, Player player)
        {
            foreach (Move movex in movesToSort)
            {
                AssignMoveOrderScore(movex, moveHash, moveKillerA, moveKillerA2, moveKillerB, moveKillerB2, player);
            }
            movesToSort.SortByScore();
        }

        /// <summary>
        /// Evaluates and assigns a move order score to a move
        /// </summary>
        /// <param name="move">Move to evaluate</param>
        /// <param name="moveHash">Best move from hash table.</param>
        /// <param name="moveKillerA">Best killer move from this ply.</param>
        /// <param name="moveKillerA2">Second best killer move from this ply.</param>
        /// <param name="moveKillerB">Best killer move from previous ply.</param>
        /// <param name="moveKillerB2">Second best killer move from previous ply.</param>
        /// <param name="pawnForwardOffset"></param>
        private void AssignMoveOrderScore(Move move, Move moveHash, Move moveKillerA, Move moveKillerA2, Move moveKillerB, Move moveKillerB2, Player player)
        {
            move.Score = 0;

            if (moveHash != null && Move.MovesMatch(move, moveHash))
            {
                move.Score += 10000000;
                return;
            }

            switch (move.Name)
            {
                case Move.enmName.PawnPromotionQueen:
                    move.Score += 900000;
                    break;
                case Move.enmName.PawnPromotionRook:
                    move.Score += 500000;
                    break;
                case Move.enmName.PawnPromotionBishop:
                    move.Score += 300000;
                    break;
                case Move.enmName.PawnPromotionKnight:
                    move.Score += 300000;
                    break;
            }

            if (move.pieceCaptured != null)
            {
                // Resulty of Static exchange evaluation
                move.Score += SEE(move) * 100000;

                if (move.Score != 0)
                {
                    return;
                }

                /*
                // "Good" capture
                if (move.From.Piece.Name == Piece.enmName.Queen && move.To.Piece.Name == Piece.enmName.Queen)
                {
                    move.Score += 99999;
                    return;
                }
                else if (move.From.Piece.Name == Piece.enmName.Rook && move.To.Piece.Name == Piece.enmName.Rook)
                {
                    move.Score += 99998;
                    return;
                }
                else if (move.From.Piece.Name == Piece.enmName.Knight && move.To.Piece.Name == Piece.enmName.Bishop)
                {
                    move.Score += 99997;
                    return;
                }
                else if (move.From.Piece.Name == Piece.enmName.Bishop && move.To.Piece.Name == Piece.enmName.Bishop)
                {
                    move.Score += 99996;
                    return;
                }
                else if (move.From.Piece.Name == Piece.enmName.Bishop && move.To.Piece.Name == Piece.enmName.Knight)
                {
                    move.Score += 99995;
                    return;
                }
                else if (move.From.Piece.Name == Piece.enmName.Pawn && (move.Name == Move.enmName.EnPassent && Board.GetPiece(move.To.Ordinal - player.PawnForwardOffset).Name == Piece.enmName.Pawn || move.To.Piece.Name == Piece.enmName.Pawn))
                {
                    move.Score += 99994;
                    return;
                }
                */
            }

            move.Score += (History.Retrieve(player.Colour, move.From.Ordinal, move.To.Ordinal));

            // Killer moves
            if (Move.MovesMatch(move, moveKillerA))
            {
                move.Score += 400;
            }
            /*
            if (Move.MovesMatch(move, moveKillerA2))
            {
                move.Score += 20003;
                return;
            }
            */
            if (Move.MovesMatch(move, moveKillerB))
            {
                move.Score += 200;
            }
            /*
            if (Move.MovesMatch(move, moveKillerB2))
            {
                move.Score += 20001;
                return;
            }
            */


            if (move.Score == 0)
            {
                // do something smart
                int x = 0;
            }

            // Score based upon tactical positional value of board square i.e. how close to centre
            //move.Score += move.To.Value - move.From.Value;


        }


        /// <summary>
        /// Performs a Static Exchange Evaluation to determine the value of a move after all possible re-captures are resolved.
        /// </summary>
        /// <param name="moveMade">move to be evaluated</param>
        /// <returns></returns>
		private int SEE(Move moveMade)
		{
			// Static Exchange Evaluator

			// Generate moves
			Moves movesFriendly = new Moves();
			Moves movesEnemy = new Moves();

			moveMade.To.AttackersMoveList(movesFriendly, moveMade.Piece.Player);
			moveMade.To.AttackersMoveList(movesEnemy   , moveMade.Piece.Player.OtherPlayer);

			// Remove piece I'm going to move first from my list of moves
			foreach (Move move in movesFriendly)
			{
				if (move.Piece==moveMade.Piece)
				{
					movesFriendly.Remove(move);
					break;
				}
			}

			// sort remaining moves by piece value
			foreach (Move move in movesFriendly)
			{
				move.Score  = 20-move.Piece.BasicValue;
			}
			movesFriendly.SortByScore();

			if (movesEnemy.Count>1)
			{
				// sort remaining moves by piece value
				foreach (Move move in movesEnemy)
				{
					move.Score  = 20-move.Piece.BasicValue;
				}
				movesEnemy.SortByScore();
			}

			int intTotalFriendlyGain = moveMade.pieceCaptured.BasicValue;
			int intLastMovedPieceValue = moveMade.Piece.BasicValue;

			// Now make a virtual move from each players move list, in order, until one of the players has no remaining moves
			
			for (int intIndex=0; ; intIndex++)
			{
				if (intIndex >= movesEnemy.Count)
				{
					break;
				}
				intTotalFriendlyGain -= intLastMovedPieceValue;
				intLastMovedPieceValue = movesEnemy[intIndex].Piece.BasicValue;

				if (intIndex >= movesFriendly.Count)
				{
					break;
				}
				intTotalFriendlyGain += intLastMovedPieceValue;
				intLastMovedPieceValue = movesFriendly[intIndex].Piece.BasicValue;
			}

			return intTotalFriendlyGain;
		}


		public void GenerateLegalMoves(Moves moves)
		{
			foreach (Piece piece in m_colPieces)
			{
				piece.GenerateLegalMoves(moves);
			}
		}

		public void GenerateLazyMoves(int depth, Moves moves, Moves.enmMovesType movesType, Square squareAttacking)
		{
//			if (squareAttacking==null)
//			{
				// All moves as defined by movesType
				foreach (Piece piece in this.m_colPieces)
				{
					piece.GenerateLazyMoves(moves, movesType);

					if (movesType!=Moves.enmMovesType.All)
					{
						Move move;
						int intIndex;
						for (intIndex=moves.Count-1; intIndex>=0; intIndex--)
						{
							move = moves[intIndex];
							if ( !(
//								move.To.Ordinal==squareAttacking.Ordinal 
//								||
								move.Name==Move.enmName.PawnPromotionQueen
								||
								(move.Name==Move.enmName.Standard && move.From.Piece.BasicValue<move.To.Piece.BasicValue)
								||
								(move.Name==Move.enmName.Standard && !move.To.CanBeMovedToBy(move.Piece.Player.OtherPlayer)) 
								)
								)  
							{
								moves.Remove(move);
							}
						}
					}

				}
//			}
//			else
//			{
				// Just re-capture moves
//				squareAttacking.AttackerMoveList(moves, this);
//			}
		}

		public Player OtherPlayer
		{
			get { return this.m_colour==Player.enmColour.White ? Game.PlayerBlack : Game.PlayerWhite; }
		}

		public enmColour Colour
		{
			get { return m_colour; }
			set	{ m_colour = value;	}
		}

		public void CaptureAllPieces()
		{
			for (int intIndex=this.m_colPieces.Count-1; intIndex>=0; intIndex--)
			{
				Piece piece = this.m_colPieces.Item(intIndex);
				piece.Capture();
			}
		}

		public void DemoteAllPieces()
		{
			for (int intIndex=this.m_colPieces.Count-1; intIndex>=0; intIndex--)
			{
				Piece piece = this.m_colPieces.Item(intIndex);
				if (piece.HasBeenPromoted)
				{
					piece.Demote();
				}
			}
		}

		public class PlayerClock
		{
			private TimeSpan m_tsnTimeElapsed;
			private DateTime m_dtmTurnStart;
			private bool m_blnIsTicking = false;
			Player m_player;

			public PlayerClock(Player player)
			{
				m_player = player;
				Reset();
			}

			public DateTime TurnStartTime
			{
				get { return m_dtmTurnStart; }
			}

			public bool IsTicking
			{
				get { return m_blnIsTicking; }
			}

			public TimeSpan TimeElapsedDisplay
			{
				get
				{
					return m_blnIsTicking ? m_tsnTimeElapsed + (DateTime.Now - m_dtmTurnStart) : m_tsnTimeElapsed;
				}
			}

			public TimeSpan TimeElapsed
			{
				get
				{
					return m_tsnTimeElapsed;
				}
				set
				{
					m_tsnTimeElapsed = value;
				}
			}

			public int ControlPeriod
			{
				get { return Game.ClockMoves==0 ? 1 : (Game.MoveNo / Game.ClockMoves) + 1; }
			}

			public TimeSpan TimeRemaining
			{
				get
				{
					TimeSpan tsnRepeatingTimeLimit = new TimeSpan( (Game.ClockTime.Ticks * this.ControlPeriod) + Game.ClockIncrementPerMove.Ticks*Game.MoveNo );
					return tsnRepeatingTimeLimit - m_tsnTimeElapsed;
				}
			}

			public int MovesRemaining
			{
				get
				{
					int intRepeatingMoves = Game.ClockMoves * this.ControlPeriod;
					return Math.Max(intRepeatingMoves - Game.MoveNo, 0);
				}
			}

			public void Reset()
			{
				m_tsnTimeElapsed = new TimeSpan(0,0,0);
				m_dtmTurnStart = DateTime.Now;
			}

			public void Start()
			{
				if (!m_blnIsTicking )
				{
					m_blnIsTicking = true;
					m_dtmTurnStart = DateTime.Now;
				}
			}

			public void Stop()
			{
				if (m_blnIsTicking)
				{
					m_blnIsTicking = false;
					m_tsnTimeElapsed += (DateTime.Now - m_dtmTurnStart);
				}
			}

			public void Revert()
			{
				m_blnIsTicking = false;
				m_dtmTurnStart = DateTime.Now;
			}
		}

		public abstract PlayerClock Clock
		{
			get;
		}

		public abstract int PawnForwardOffset
		{
			get;
		}

		public abstract int PawnAttackRightOffset
		{
			get;
		}

		public abstract int PawnAttackLeftOffset
		{
			get;
		}

		protected abstract void SetPiecesAtStartingPositions();

		public bool IsThinking
		{
			get {return m_threadThought!=null;}
		}

		public bool IsPondering
		{
			get {return m_blnIsPondering;}
		}

		public int Evaluations
		{
			get {return m_intEvaluations;}
		}

		public int PositionsSearched
		{
			get {return m_intPositionsSearched;}
		}
		
		public int PositionsPerSecond
		{
			get {return m_intPositionsSearched / Math.Max(Convert.ToInt32(this.ThinkingTimeElpased.TotalSeconds),1);}
		}
		
		public double EvaluationsPerSecond
		{
			get {return m_intEvaluations / this.ThinkingTimeElpased.TotalSeconds;}
		}
		
		public int MaxSearchDepth
		{
			get { return m_intMaxSearchDepth; }
		}

		public int SearchDepth
		{
			get { return m_intSearchDepth; }
		}

		public TimeSpan ThinkingTimeAllotted
		{
			get { return m_tsnThinkingTimeAllotted; }
		}

		public TimeSpan ThinkingTimeElpased
		{
			get { return DateTime.Now-m_PlayerClock.TurnStartTime; }
		}

		public TimeSpan ThinkingTimeRemaining
		{
			get { return m_tsnThinkingTimeAllotted-this.ThinkingTimeElpased; }
		}

		public enmStatus Status
		{
			get
			{
				if (this.IsInCheckMate ) return Player.enmStatus.InCheckMate;
				if (!this.CanMove) return Player.enmStatus.InStaleMate;
				if (this.IsInCheck) return Player.enmStatus.InCheck;
				return Player.enmStatus.Normal;
			}
		}

		public int TotalPieceValue
		{
			get
			{
				int intValue = 0;
				foreach (Piece piece in this.m_colPieces)
				{
					intValue += piece.Value;
				}
				return intValue;
			}
		}

		public int MaterialCount
		{
			get
			{
				return this.m_MaterialCount;
			}
		}

		public int PositionPoints
		{
			get
			{
				int intTotalValue = 0;
				int intIndex;
				Piece piece;
				for (intIndex=this.m_colPieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = this.m_colPieces.Item(intIndex);
					intTotalValue += piece.PositionalPoints;
				}
				return intTotalValue;
			}
		}


		public int PawnKingPoints
		{
			get
			{
				int intPoints;
				int intIndex;

				if ( (intPoints = HashTablePawnKing.ProbeHash(this.Colour)) == HashTablePawnKing.UNKNOWN )
				{
					Piece piece;
					intPoints = 0;
					for (intIndex=this.m_colPieces.Count-1; intIndex>=0; intIndex--)
					{
						piece = this.m_colPieces.Item(intIndex);
						switch (piece.Name)
						{
							case Piece.enmName.Pawn:
							case Piece.enmName.King:
							intPoints += piece.PointsTotal;
							break;
						}
					}
					HashTablePawnKing.RecordHash(intPoints, this.Colour);
				}

				return intPoints;
			}
		}

		public bool HasCastled
		{
			get { return m_blnHasCastled; }
			set { m_blnHasCastled = value; }
		}

		public int TotalPositionsToSearch
		{
			get { return m_intTotalPositionsToSearch; }
		}

		public int MaxQuiesDepth
		{
			get { return m_intMaxQDepthReached; }
		}

		public int MaxExtensions
		{
			get { return m_intMaxExtensionsReached; }
		}

		public Move CurrentMove
		{
			get { return m_moveCurrent; }
		}

		public Moves PrincipalVariation
		{
			get { return m_movesPVBest; }
		}

		public string PrincipalVariationText
		{
			get 
			{ 
				string strText = "";
				if (m_movesPVBest!=null)
				{
					for (int intIndex = 0; intIndex<m_movesPVBest.Count; intIndex++)
					{
						if (intIndex<m_movesPVBest.Count)
						{
							Move move = m_movesPVBest[intIndex];
							if (move!=null)
							{
								strText += (move.Piece.Name==Piece.enmName.Pawn ? "" : move.Piece.Abbreviation) + move.From.Name + (move.pieceCaptured!=null ? "x" : "") + move.To.Name + " ";
							}
						}
					}
				}
				return strText;
			}
		}

		public int SearchPositionNo
		{
			get { return m_intSearchPositionNo; }
		}


		public Player()
		{
			m_colPieces = new Pieces(this);
			m_colCapturedEnemyPieces = new Pieces(this);
		}

		public int PawnsInPlay
		{
			get { return m_NoOfPawnsInPlay; } 
		}

		public void DecreasePawnCount()
		{
			m_NoOfPawnsInPlay--;
		}

		public void IncreasePawnCount()
		{
			m_NoOfPawnsInPlay++;
		}

		public void DecreaseMaterialCount()
		{
			m_MaterialCount--;
		}

		public void IncreaseMaterialCount()
		{
			m_MaterialCount++;
		}

		public int PieceBasicValue
		{
			get
			{
				int intBasicValue = 0;
				foreach (Piece piece in this.Pieces)
				{
					intBasicValue += piece.BasicValue;
				}
				return intBasicValue;
			}
		}

		public bool CanMove
		{
			get
			{
				Moves moves;
				foreach (Piece piece in m_colPieces )
				{
					moves = new Moves();
					piece.GenerateLegalMoves(moves);
					if (moves.Count>0)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool DetermineCheckStatus()
		{
			return ((PieceKing)this.King.Top).DetermineCheckStatus();
		}

		public bool IsInCheck
		{
			get
			{
				return HashTableCheck.IsPlayerInCheck(this);
			}
		}

		public bool IsInCheckMate
		{
			get
			{ 
				if (!IsInCheck) return false;

				Moves moves = new Moves();
				GenerateLegalMoves(moves);
				return (moves.Count==0);
			}
		}

		public void ForceImmediateMove()
		{
			if (this.IsThinking && !m_blnForceImmediateMove)
			{
				m_blnForceImmediateMove = true;
				while (m_threadThought!=null)
				{
					Thread.Sleep(50);
				}
//				m_threadThought.Join();
			}
		}

		public void StartPondering()
		{
			if (this.Intellegence==Player.enmIntellegence.Computer && this.OtherPlayer.Intellegence==Player.enmIntellegence.Computer)
			{
				// Can't both ponder at the same time
				return;
			}

			if (Game.IsInAnalyseMode)
			{
				// Can't ponder when in Analyse mode
				return;
			}

			if (Game.EnablePondering)
			{
				if (m_ulongPonderingHashCodeA!=Board.HashCodeA || m_ulongPonderingHashCodeB!=Board.HashCodeB)
				{
					m_ulongPonderingHashCodeA = Board.HashCodeA;
					m_ulongPonderingHashCodeB = Board.HashCodeB;
					m_dtmPonderingStart = DateTime.Now;
				}

				if (!this.IsThinking && !this.OtherPlayer.IsThinking && this.OtherPlayer.Intellegence==Player.enmIntellegence.Computer && Game.PlayerToPlay==this)
				{
					m_blnIsPondering = true;
					StartThinking();
				}
			}
		}

		public void StopPondering()
		{
			if (this.IsPondering)
			{
				AbortThinking();
				m_blnIsPondering = false;
			}
		}

		public void StartThinking()
		{
			// Bail out if unable to move
			if (!this.CanMove)
			{
				return;
			}

			// Send draw result is playing WinBoard
			if (WinBoard.Active && this.Intellegence==enmIntellegence.Computer)
			{
				if (this.CanClaimThreeMoveRepetitionDraw)
				{
					WinBoard.SendDrawByRepetition();
					return;
				}
				else if (this.CanClaimFiftyMoveDraw)
				{
					WinBoard.SendDrawByFiftyMoveRule();
					return;
				}
				else if (this.CanClaimInsufficientMaterialDraw)
				{
					WinBoard.SendDrawByFiftyMoveRule();
					return;
				}
			}

			m_threadThought = new Thread(new ThreadStart(Think));
			m_threadThought.Name = (++Game.ThreadCounter).ToString();

			ThinkingBeginning();
			if (this.IsPondering)
			{
//				m_threadThought.Priority = System.Threading.ThreadPriority.BelowNormal;
				m_threadThought.Priority = System.Threading.ThreadPriority.Normal;
			}
			else
			{
				m_threadThought.Priority = System.Threading.ThreadPriority.Normal;
			}
			m_threadThought.Start();
		}
		
		public void AbortThinking()
		{
			if (m_threadThought!=null && m_threadThought.ThreadState==System.Threading.ThreadState.Running)
			{
				m_threadThought.Abort();
				m_threadThought.Join();
				m_threadThought = null;
			}
		}

		public Pieces Pieces
		{
			get	{ return m_colPieces; }
		}

		public Pieces CapturedEnemyPieces
		{
			get	{ return m_colCapturedEnemyPieces; }
		}

		public int CapturedEnemyPiecesTotalBasicValue
		{
			get
			{
				int intValue = 0;
				foreach (Piece piece in m_colCapturedEnemyPieces)
				{
					intValue += piece.BasicValue;
				}
				return intValue;
			}
		}

		public bool CanClaimInsufficientMaterialDraw
		{
			get
			{
				// Return true if K vs K, K vs K+B, K vs K+N

				if (Game.PlayerWhite.Pieces.Count>2 || Game.PlayerBlack.Pieces.Count>2) 
				{
					return false;
				}
				if (Game.PlayerWhite.Pieces.Count == 2 && Game.PlayerBlack.Pieces.Count == 2)
				{
					return false;
				}
				if (Game.PlayerWhite.Pieces.Count == 1 && Game.PlayerBlack.Pieces.Count == 1) 
				{
					return true;
				}

				Player playerTwoPieces = Game.PlayerWhite.Pieces.Count==2 ? Game.PlayerWhite : Game.PlayerBlack;
				Piece pieceNotKing = playerTwoPieces.Pieces.Item(0).Name==Piece.enmName.King ? playerTwoPieces.Pieces.Item(1) : playerTwoPieces.Pieces.Item(0);

				switch (pieceNotKing.Name)
				{
					case Piece.enmName.Bishop:
					case Piece.enmName.Knight:
						return true;
				}

				return false;
			}
		}

		public bool CanClaimFiftyMoveDraw
		{
			get
			{
				return Game.MoveHistory.Count>0 ? Game.MoveHistory.Last.IsFiftyMoveDraw : Game.FiftyMoveDrawBase>=100;
			}
		}

		public bool CanClaimThreeMoveRepetitionDraw
		{
			get
			{
				return CanClaimMoveRepetitionDraw(3);
			}
		}

		public bool CanClaimMoveRepetitionDraw(int NoOfMoves)
		{
				if (Game.MoveHistory.Count==0)
				{
					return false;
				}
//				if (this.Colour==Game.MoveHistory.Last.Piece.Player.Colour)
//				{
//					return false;
//				}
				Move move;
				int intRepetitionCount = 1;
				int intIndex=Game.MoveHistory.Count-1;
				for (; intIndex>=0; intIndex--, intIndex--)
				{
					move = Game.MoveHistory[intIndex];
					if (move.HashCodeA==Board.HashCodeA && move.HashCodeB==Board.HashCodeB)
					{
						if (intRepetitionCount>=NoOfMoves)
						{
							return true;
						}
						intRepetitionCount++;
					}
					if (move.Piece.Name==Piece.enmName.Pawn || move.pieceCaptured!=null)
					{
						return false;
					}
				}
				return false;
		}

		public int Points
		{
			get	
			{ 
				int intPoints = 0;
				int intIndex;
				Piece piece;

				intPoints += this.PawnKingPoints;

				int intBishopCount = 0;
				int intRookCount = 0;
				for (intIndex=this.m_colPieces.Count-1; intIndex>=0; intIndex--)
				{
					piece = this.m_colPieces.Item(intIndex);
					switch (piece.Name)
					{
						case Piece.enmName.Pawn:
						case Piece.enmName.King:
							break;
						default:
							intPoints += piece.PointsTotal;
							break;
					}
					switch (piece.Name)
					{
						case Piece.enmName.Bishop:
							intBishopCount++;
							break;

						case Piece.enmName.Rook:
							intRookCount++;
							break;
					}
				}
				if (intBishopCount>=2)
				{
					intPoints += 500;
				}
				if (intRookCount>=2)
				{
					intPoints += 100;
				}

				// Multiple attack bonus
//				for (intIndex=this.OtherPlayer.m_colPieces.Count-1; intIndex>=0; intIndex--)
//				{
//					piece = this.OtherPlayer.m_colPieces.Item(intIndex);
//					intPoints += m_aintAttackBonus[piece.Square.NoOfAttacksBy(this)];
//				}

				// Factor in human 3 move repition draw condition
				// If this player is "human" then a draw if scored high, else a draw is scored low
				if (Game.MoveHistory.Count>0 && Game.MoveHistory.Last.IsThreeMoveRepetition)
				{
					intPoints += (this.Intellegence==Player.enmIntellegence.Human ? 1000000000: 0 );
				}

				if (this.HasCastled)
				{
					intPoints += 117;
				}
				else
				{
					if (this.King.HasMoved)
					{
						intPoints -= 247;
					}
					else
					{
						Piece pieceRook;
						pieceRook = this.Colour==Player.enmColour.White ? Board.GetPiece(7,0):Board.GetPiece(7,7);
						if (pieceRook==null || pieceRook.Name!=Piece.enmName.Rook || pieceRook.Player.Colour!=this.Colour || pieceRook.HasMoved)
						{
							intPoints -= 107;
						}
						pieceRook = this.Colour==Player.enmColour.White ? Board.GetPiece(0,0):Board.GetPiece(0,7);
						if (pieceRook==null || pieceRook.Name!=Piece.enmName.Rook || pieceRook.Player.Colour!=this.Colour || pieceRook.HasMoved)
						{
							intPoints -= 107;
						}
					}
				}


				if (this.IsInCheck)
				{
					if (this.IsInCheckMate)
					{
						intPoints -= 999999999;
					}

				}

				return intPoints;
			}

		}

		public int Score
		{
			get { return this.Points - this.OtherPlayer.Points; }
		}

		public Piece King
		{
			get { return m_King; }
			set { m_King = value; }
		}

		public bool HasPieceName(Piece.enmName piecename)
		{
			if (piecename==Piece.enmName.Pawn && this.PawnsInPlay > 0) return true;
			foreach (Piece piece in this.m_colPieces)
			{
				if (piece.Name==piecename)
				{
					return true;
				}
			}
			return false;
		}

/*
		public Move MoveFromNotation(string Text)
		{
			Piece piece = null;
			Square from = null;
			Square square;
			Square to = null;
			Piece pieceTaken = null;
			Move.enmName MoveName = Move.enmName.Standard;
			Player.enmColour colour;
			string strTo = "";
			string strAction="";
			string strFromFile = "";
			string strFromRank = "";
			string strPieceName = "";
			int intPos;

			Text.Trim();

			if (Text=="")
			{
				Text="";
			}

			// Castle king-side
			if (Text.ToUpper()=="OO" || Text.ToUpper()=="O-O") { from=this.King.Square; to=Board.GetSquare(this.King.Square.Ordinal+2); piece=this.King; MoveName=Move.enmName.CastleKingSide; goto exithere;}
			// Castle queen-side
			if (Text.ToUpper()=="OOO" || Text.ToUpper()=="O-O-O") { from=this.King.Square; to=Board.GetSquare(this.King.Square.Ordinal-3); piece=this.King; MoveName=Move.enmName.CastleQueenSide; goto exithere;}


			intPos = Text.Length;
			// To square;
			intPos-=2;
			strTo = Text.Substring(intPos);
			// Action
			intPos--;
			if (intPos>=0 && Text.Substring(intPos,1).ToUpper()=="X")
			{
				strAction = Text.Substring(intPos,1).ToUpper();
				intPos--; // skip the "x"
			}
			// Rank number
			if (intPos>=0 && Char.IsDigit(Convert.ToChar(Text.Substring(intPos,1))))
			{
				strFromRank = Text.Substring(intPos,1);
				intPos--;
			}
			// File letter
			if (intPos>=0 && Text.Substring(intPos,1)!=Text.Substring(intPos,1).ToUpper())
			{
				strFromFile = Text.Substring(intPos,1);
				intPos--;
			}
			if (intPos>=0)
			{
				strPieceName = Text.Substring(intPos,1);
			}
			else
			{
				strPieceName = "P";
			}

			to=Board.GetSquare(strTo);
			pieceTaken = to.Piece;
			
			switch (strPieceName)
			{
				case "P":
					if (strAction!="X")
					{
						square = Board.GetSquare(to.Ordinal-this.PawnForwardOffset);
						piece = square.Piece;
						while (piece==null || piece.Name!=Piece.enmName.Pawn || piece.Player.Colour!=this.Colour)
						{
							square = Board.GetSquare(square.Ordinal-this.PawnForwardOffset);
							piece = square.Piece;
						}
						from=square; 
						piece=from.Piece; 
					}
					else
					{
						piece = Board.GetPiece(to.Ordinal+this.OtherPlayer.PawnAttackLeftOffset);
						if (piece==null || piece.Name!=Piece.enmName.Pawn || piece.Player.Colour!=this.Colour || strFromFile!="" && piece.Square.FileName!=strFromFile)
						{
							piece = Board.GetPiece(to.Ordinal+this.OtherPlayer.PawnAttackRightOffset);
						}
						// En passent not currently handled
						from = piece.Square;
					}
					break;

				case "N":
					if ( (square = Board.GetSquare(to.Ordinal+33 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile)) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal+18 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal-14 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal-31 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal-33 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal-18 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal+14 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal+31 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece;
					from = piece.Square;					
					break;

				case "B":
					colour = (strAction=="X" ? this.OtherPlayer.Colour : this.Colour);
					if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Bishop, to, 15))!=null && piece.Name==Piece.enmName.Bishop && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Bishop, to, 17))!=null && piece.Name==Piece.enmName.Bishop && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Bishop, to, -15))!=null && piece.Name==Piece.enmName.Bishop && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Bishop, to, -17))!=null && piece.Name==Piece.enmName.Bishop && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else piece=null;
					from = piece.Square;					
					break;

				case "R":
					if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Rook, to, 1))!=null && piece.Name==Piece.enmName.Rook && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Rook, to, -1))!=null && piece.Name==Piece.enmName.Rook && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Rook, to, 16))!=null && piece.Name==Piece.enmName.Rook && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Rook, to, -16))!=null && piece.Name==Piece.enmName.Rook && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else piece=null;
					from = piece.Square;					
					break;

				case "Q":
					if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Queen, to, 15))!=null && piece.Name==Piece.enmName.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Queen, to, 17))!=null && piece.Name==Piece.enmName.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Queen, to, -15))!=null && piece.Name==Piece.enmName.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Queen, to, -17))!=null && piece.Name==Piece.enmName.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Queen, to, 1))!=null && piece.Name==Piece.enmName.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Queen, to, -1))!=null && piece.Name==Piece.enmName.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Queen, to, 16))!=null && piece.Name==Piece.enmName.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
						if ((piece=Board.LinesFirstPiece(this.Colour, Piece.enmName.Queen, to, -16))!=null && piece.Name==Piece.enmName.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else piece=null;
					from = piece.Square;					
					break;

				case "K":
					if ( (square = Board.GetSquare(to.Ordinal+15))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile)) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal+17 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal-15 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal-17 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal+ 1 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal- 1 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal+16 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
						if ( (square = Board.GetSquare(to.Ordinal-16 ))!=null && square.Piece!=null && square.Piece.Name==Piece.enmName.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece;
					from = piece.Square;					
					break;
			}

			exithere:
				return new Move(0, 0, MoveName, piece, from, to, pieceTaken, 0, 0);
		}
*/
		public void Perft(int TargetDepth)
		{
			m_intPositionsSearched = 0;
			Perft_Ply(this, TargetDepth);
		}

		private void Perft_Ply(Player player, int depth)
		{
			if (depth<=0) return;
	
			Moves moves = new Moves();
			player.GenerateLegalMoves(moves);

			foreach (Move move in moves)
			{
				Move moveUndo = move.Piece.Move(move.Name, move.To);

				m_intPositionsSearched++;

//				Debug.WriteLine(move.DebugText + ",");

				Perft_Ply(player.OtherPlayer, depth-1);

				Move.Undo(moveUndo);
			}
		}

		public class ForceImmediateMoveException: ApplicationException
		{
		}


#if DEBUG
      /// <summary>Internal buffer to convert the PV to a string</summary>
      StringBuilder m_strbPV = new StringBuilder(50);

      /// <summary>Convert the Principal Variation to a string</summary>
      /// <param name="moveList">the list of moves of the variation</param>
      /// <returns>the string of the Principal Variation. Ex: 5 Bb3a4 Bc8d7 Ba4xc6</returns>
      private string PvLine(Moves moveList)
      {
         if (moveList != null)
         {
            m_strbPV.Remove(0, m_strbPV.Length);
            for (int intIndex = 0; intIndex < moveList.Count; intIndex++)
            {
               Move move = moveList[intIndex];
               if (move != null)
               {
                  m_strbPV.Append(move.Piece.Abbreviation);
                  m_strbPV.Append(move.From.Name);
                  if (move.pieceCaptured != null)
                     m_strbPV.Append("x");
                  m_strbPV.Append(move.To.Name);
                  m_strbPV.Append(" ");
               }
            }
         }
         return m_strbPV.ToString();         
      } // end PvLine

	  /// <summary>Break on the variation at the given iteration</summary>
	  /// <param name="iPly">the positive or null ply of halfmove. Don't confuse with iDepth</param>
	  /// <param name="moveThis">the current move</param>
	  /// <returns>true if the position is reached otherwise false</returns>
	  private bool DebugMatchVariation(int iPly, Move moveThis)
	  {   // Syntax of the string strVariation: <iteration> Move1 Move2 ...
		#if !SKIP_MATCH_LINE // Add or remove the exclamation mark before SKIP_MATCH_LINE
			return DebugMatchLine("5 Bb3a4 Bc8d7 Ba4xc6 Bd7xc6 Rf1e1 Bf8e7 Bc1d2", iPly, moveThis); // The variation/line you want to debug!
		#else
			return false; // Do not break on the variation
		#endif // SKIP_MATCH_LINE
	  } // end DebugMatchVariation

	  /// <summary>Level of depth of the variation</summary>
	  int m_iDbgLevel = 0;
	  /// <summary>Number of iteration of AlphaBeta at the top level</summary>
	  int m_iDbgIteration;
	  /// <summary>Unambiguous descriptive variation after conversion of the PGN variation</summary>
	  string m_strDbgLine = "";

	  /// <summary>Does the current position match the specified variation?</summary>
	  /// <param name="strVariation">the iteration and the variation. Ex: "5 Rb4b5 Pf4f5 Pe5f6"</param>
	  /// <param name="iPly">number positive or 0 of halfmove. Do not confuse with iDepth</param>
	  /// <param name="moveThis">the current move at the beginning of the research</param>
	  /// <returns>true if the variation is recognized otherwise false</returns>
	  /// <remarks>Must be called after moveThis.DoMove() in AlphaBeta</remarks>
	  private bool DebugMatchLine(string strVariation, int iPly, Move moveThis)
	  {
		  const int iSAN_LENGTH = 5; // Length of Abbreviation of the piece + From square + To square
		  if (m_iDbgLevel == iPly) // Is the level of depth of the variation reached?
		  {
			  if (m_strDbgLine.Length == 0) // Interpret dynamically the variation
			  {   // In this version, strVariation contains unambiguous descriptive moves
				  int indPos = 0; // Evaluate the number of iteration and parse the variation
				  while (char.IsNumber(strVariation[indPos])) indPos++;
				  m_iDbgIteration = Convert.ToInt32(strVariation.Substring(0, indPos));
				  m_strDbgLine = strVariation.Substring(indPos);   // Parse the variation
				  m_strDbgLine = m_strDbgLine.Replace(" ", "");   // removing all whitespaces
				  m_strDbgLine = m_strDbgLine.Replace("x", "");   // removing all "x"
			  }
			  if (m_intSearchDepth == m_iDbgIteration) // Number of iteration of AlphaBeta at the top level
			  {
				  int indPiece = iPly * iSAN_LENGTH; // Index where begins the notation of the move
				  int iLenVar = m_strDbgLine.Length;
				  string strMoveDescr = moveThis.Piece.Abbreviation + moveThis.From.Name + moveThis.To.Name;
				  if ((indPiece <= iLenVar - iSAN_LENGTH) &&
					 (strMoveDescr == m_strDbgLine.Substring(indPiece, iSAN_LENGTH)))
				  {
					  if (++m_iDbgLevel == iLenVar / iSAN_LENGTH) // Number of moves in the variation
					  {
						  m_iDbgLevel = m_intMaxSearchDepth + 1; // Do not recall this utility
						  Board.DebugDisplay(); // Display the current position in the "Output Window"
						  Debug.WriteLine("\nPosition after: " + strVariation);
						  return true;   // The current position matches the wished variation
					  }
				  }
			  }
		  }
		  return false;

	  } // end DbgMatchLine 

#endif // DEBUG

  }
}
