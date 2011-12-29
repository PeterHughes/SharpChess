// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Search.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   Performs the central move-selection logic for SharpChess, referred to as the Search.
//   http://chessprogramming.wikispaces.com/Search
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region License

// SharpChess
// Copyright (C) 2011 Peter Hughes
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

namespace SharpChess.Model.AI
{
    #region Using

    using System;

    #endregion

    /// <summary>
    /// Performs the central move-selection logic for SharpChess, referred to as the Search.
    ///   http://chessprogramming.wikispaces.com/Search
    /// </summary>
    public class Search
    {
        #region Constants and Fields

        /// <summary>
        ///   Maximum score.
        /// </summary>
        private const int MaxScore = int.MaxValue;

        /// <summary>
        ///   Minimum score
        /// </summary>
        private const int MinScore = int.MinValue + 1;

        /// <summary>
        ///   Minimum search depth.
        /// </summary>
        private const int MinSearchDepth = 1;

        /// <summary>
        ///   When true, instructure search to exit immediately, with the last fully-searched move.
        /// </summary>
        private bool forceExitWithMove;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Search"/> class.
        /// </summary>
        /// <param name="brain">
        /// The brain performing this search.
        /// </param>
        public Search(Brain brain)
        {
            this.MyBrain = brain;
            this.MaxSearchDepth = 32;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The delegatetype Search event.
        /// </summary>
        public delegate void SearchEventDelegate();

        #endregion

        #region Public Events

        /// <summary>
        ///   The move considered.
        /// </summary>
        public event SearchEventDelegate SearchMoveConsideredEvent;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets CurrentMoveSearched.
        /// </summary>
        public Move CurrentMoveSearched { get; private set; }

        /// <summary>
        ///   Gets or sets Evaluations.
        /// </summary>
        public int Evaluations { get; protected set; }

        /// <summary>
        ///   Gets EvaluationsPerSecond.
        /// </summary>
        public double EvaluationsPerSecond
        {
            get
            {
                return this.Evaluations / this.MyBrain.ThinkingTimeElpased.TotalSeconds;
            }
        }

        /// <summary>
        ///   Gets MaxExtensions.
        /// </summary>
        public int MaxExtensions { get; private set; }

        /// <summary>
        ///   Gets MaxQuiesDepth.
        /// </summary>
        public int MaxQuiesenceDepthReached { get; private set; }

        /// <summary>
        ///   Gets MaxSearchDepth.
        /// </summary>
        public int MaxSearchDepth { get; private set; }

        /// <summary>
        ///   Gets PositionsPerSecond.
        /// </summary>
        public int PositionsPerSecond
        {
            get
            {
                return this.PositionsSearched
                       / Math.Max(Convert.ToInt32(this.MyBrain.ThinkingTimeElpased.TotalSeconds), 1);
            }
        }

        /// <summary>
        ///   Gets the number of positions searched.
        /// </summary>
        public int PositionsSearched { get; private set; }

        /// <summary>
        ///   Gets the current search depth.
        /// </summary>
        public int SearchDepth { get; private set; }

        /// <summary>
        ///   Gets the current search position number.
        /// </summary>
        public int SearchPositionNo { get; private set; }

        /// <summary>
        ///   Gets the maximum search time allowed.
        /// </summary>
        public TimeSpan MaxSearchTimeAllowed { get; private set; }

        /// <summary>
        ///   Gets the total positions to search.
        /// </summary>
        public int TotalPositionsToSearch { get; private set; }

        /// <summary>
        ///   Gets the player's brain performing this search.
        /// </summary>
        public Brain MyBrain { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the iterative deepening search to find the best move for this player.
        /// http://chessprogramming.wikispaces.com/Iterative+Deepening
        /// </summary>
        /// <param name="player">
        /// The player to play.
        /// </param>
        /// <param name="principalVariationMoves">
        /// Move in the principal variation will be added to this list.
        /// </param>
        /// <param name="recommendedSearchTime">
        /// Recommended search time allotted.
        /// </param>
        /// <param name="maximumSearchTimeAllowed">
        /// The maximum search time allowed.
        /// </param>
        /// <returns>
        /// The best move for the player.
        /// </returns>
        /// <exception cref="ForceImmediateMoveException">
        /// Raised when the user requests for thinking to be terminated, and immediate move to made.
        /// </exception>
        public int IterativeDeepeningSearch(
            Player player, Moves principalVariationMoves, TimeSpan recommendedSearchTime, TimeSpan maximumSearchTimeAllowed)
        {
            // A new deeper ply of search will only be started, if the cutoff time hasnt been reached yet.
            TimeSpan searchTimeCutoff = new TimeSpan(recommendedSearchTime.Ticks / 3);

            this.MaxSearchTimeAllowed = maximumSearchTimeAllowed;

            this.forceExitWithMove = false; // Set to stop thread thinking and return best move
            this.PositionsSearched = 0; // Total number of positions considered so far
            this.Evaluations = 0;

            // Set max search depth, as defined is game difficulty settings
            this.MaxSearchDepth = Game.MaximumSearchDepth == 0 ? 32 : Game.MaximumSearchDepth;

            int score = player.Score;

            for (this.SearchDepth = MinSearchDepth; this.SearchDepth <= this.MaxSearchDepth; this.SearchDepth++)
            {
                if (Game.CaptureMoveAnalysisData)
                {
                    Game.MoveAnalysis.Clear();
                }

                score = this.Aspirate(player, principalVariationMoves, score, Game.MoveAnalysis);

                /* score = AlphaBeta(player, m_intSearchDepth, m_intSearchDepth, MIN_SCORE, MAX_SCORE, null, movesPV, intScore); */

                if (!Game.IsInAnalyseMode && Game.ClockFixedTimePerMove.TotalSeconds <= 0 && !this.MyBrain.IsPondering
                    && (DateTime.Now - player.Clock.TurnStartTime) > searchTimeCutoff)
                {
                    throw new ForceImmediateMoveException();
                }

                if (score > 99999 || score < -99999)
                {
                    break; // Checkmate found so dont bother searching any deeper
                }
            }

            return score;
        }

        /// <summary>
        /// Walks the move generation tree and counts all the leaf nodes of a certain depth, which can be compared to predetermined values and used to isolate bugs.
        /// http://chessprogramming.wikispaces.com/Perft
        /// </summary>
        /// <param name="player">
        /// The player to play.
        /// </param>
        /// <param name="targetDepth">
        /// The target depth.
        /// </param>
        public void Perft(Player player, int targetDepth)
        {
            this.PositionsSearched = 0;
            this.PerftPly(player, targetDepth);
        }

        /// <summary>
        /// The force search to exit with an immediate move.
        /// </summary>
        public void SearchForceExitWithMove()
        {
            this.forceExitWithMove = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the search foe the best move, using a specialised form of alpha beta search, named Principal Variation Search (PVS) .
        /// http://chessprogramming.wikispaces.com/Alpha-Beta
        /// http://chessprogramming.wikispaces.com/Principal+Variation+Search
        /// </summary>
        /// <param name="player">
        /// The player to play. The player is alternated at each new ply of search.
        /// </param>
        /// <param name="ply">
        /// True depth in plys. Increments each time alpha beta is recursively called.
        /// </param>
        /// <param name="variableDepth">
        /// Depth which can be altered by search extension and reductions.
        /// http://chessprogramming.wikispaces.com/Depth
        /// </param>
        /// <param name="alpha">
        /// Alpha (α) is the lower bound, representing the minimum score that a node  must reach in order to change the value of a previous node.
        /// http://chessprogramming.wikispaces.com/Alpha
        /// </param>
        /// <param name="beta">
        /// Beta (β) is the upper bound  of a score for the node. If the node value exceeds or equals beta, it means that the opponent will avoid this node, 
        /// since his guaranteed score (Alpha of the parent node) is already greater. Thus, Beta is the best-score the opponent (min-player) could archive so far...
        /// http://chessprogramming.wikispaces.com/Beta
        /// </param>
        /// <param name="parentMove">
        /// Move from the parent alpha beta call.
        /// </param>
        /// <param name="principalVariationMoves">
        /// The Principal variation (PV) is a sequence of moves is considered best and therefore expect to be played. 
        /// This list of moves is collected during the alpha beta search.
        /// http://chessprogramming.wikispaces.com/Principal+variation
        /// </param>
        /// <param name="totalExtensionsAndReducations">
        /// Holds a counter indicating the number of search extensions or reductions at the current search depth.
        /// A positive nunber indicates there have been extensions in a previous ply, negative indicates a reduction.
        /// http://chessprogramming.wikispaces.com/Extensions
        /// http://chessprogramming.wikispaces.com/Reductions
        /// </param>
        /// <param name="analysisParentBranch">
        /// When move analysis is enabled, a tree of search moves is collected in this variable, which can be viewed in the GUI.
        /// </param>
        /// <returns>
        /// The score of the best move.
        /// </returns>
        /// <exception cref="ForceImmediateMoveException">
        /// Raised when the user requests for thinking to be terminated, and immediate move to made.
        /// </exception>
        private int AlphaBetaPvs(
            Player player, 
            int ply, 
            int variableDepth,
            int alpha, 
            int beta, 
            Move parentMove, 
            Moves principalVariationMoves, 
            int totalExtensionsAndReducations, 
            Moves analysisParentBranch)
        {
            int val = int.MinValue;
            HashTable.HashTypeNames hashType = HashTable.HashTypeNames.Alpha;
            Move moveHash = null;
            Move moveBest = null;
            bool blnPVNode = false;
            int intScoreAtEntry = 0;
            bool blnAllMovesWereGenerated;
            int intLegalMovesAttempted = 0;
            bool blnIsInCheck = player.IsInCheck;

            if (this.forceExitWithMove)
            {
                throw new ForceImmediateMoveException();
            }

            Moves movesPV = new Moves();

            this.PositionsSearched++;

            if (parentMove != null && parentMove.IsThreeMoveRepetition)
            {
                return -player.OpposingPlayer.Score;
            }

            // TODO all Quiesence hash table depths must be zero
            if ((val = HashTable.ProbeHash(Board.HashCodeA, Board.HashCodeB, ply, alpha, beta, player.Colour))
                != HashTable.NotFoundInHashTable)
            {
                // High values of "val" indicate that a checkmate has been found
                if (val > 1000000 || val < -1000000)
                {
                    if (this.MaxSearchDepth - variableDepth > 0)
                    {
                        val /= this.MaxSearchDepth - variableDepth;
                    }
                }

                principalVariationMoves.Clear();
                if (HashTable.ProbeForBestMove(Board.HashCodeA, Board.HashCodeB, player.Colour) != null)
                {
                    principalVariationMoves.Add(
                        HashTable.ProbeForBestMove(Board.HashCodeA, Board.HashCodeB, player.Colour));
                }

                return val;
            }

            if (totalExtensionsAndReducations > this.MaxExtensions)
            {
                this.MaxExtensions = totalExtensionsAndReducations;
            }

            // Generate moves
            Moves movesPossible = new Moves();
            blnAllMovesWereGenerated = variableDepth > 0; // || blnIsInCheck); 
            if (blnAllMovesWereGenerated)
            {
                player.GenerateLazyMoves(variableDepth, movesPossible, Moves.MoveListNames.All, null);
            }
            else
            {
                // Captures only
                player.GenerateLazyMoves(
                    variableDepth, movesPossible, Moves.MoveListNames.CapturesChecksPromotions, parentMove.To);
            }

            // Depth <=0 means we're into Quiescence searching
            if (variableDepth <= 0)
            {
                if (variableDepth < this.MaxQuiesenceDepthReached)
                {
                    this.MaxQuiesenceDepthReached = variableDepth;
                }

                intScoreAtEntry = val = -player.OpposingPlayer.Score;
                this.Evaluations++;

                if (val > 1000000 || val < -1000000)
                {
                    val /= this.MaxSearchDepth - variableDepth;
                }

                // If there are no more moves, then return val.
                // TODO remove checking moves from count in quiesence only
                if (movesPossible.Count == 0)
                {
                    return val;
                }
            }

            // Adaptive Null-move forward pruning
            int r = variableDepth > 6 ? 3 : 2;
            if (variableDepth > (r + 1) && parentMove != null && parentMove.Name != Move.MoveNames.NullMove
                && Game.Stage != Game.GameStageNames.End && !blnIsInCheck)
            {
                Move moveNull = new Move(Game.TurnNo, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 0);
                val =
                    -this.AlphaBetaPvs(
                        player.OpposingPlayer, 
                        ply - 1, 
                        variableDepth - r - 1, 
                        -beta, 
                        -beta + 1, 
                        moveNull, 
                        movesPV, 
                        totalExtensionsAndReducations, 
                        null);
                if (val >= beta)
                {
                    return beta;
                }
            }

            // Get last iteration's best move from the transposition Table
            moveHash = HashTable.ProbeForBestMove(Board.HashCodeA, Board.HashCodeB, player.Colour);

            // Get Killers
            Move moveKillerA = KillerMoves.RetrieveA(ply);
            Move moveKillerB = KillerMoves.RetrieveB(ply);
            Move moveKillerA2 = KillerMoves.RetrieveA(ply + 2);
            Move moveKillerB2 = KillerMoves.RetrieveB(ply + 2);

            // Sort moves
            // TODO BUG: Captures are not being sorted be least value piece.
            this.SortBestMoves(movesPossible, moveHash, moveKillerA, moveKillerA2, moveKillerB, moveKillerB2, player);

            if (ply == this.SearchDepth)
            {
                this.TotalPositionsToSearch = movesPossible.Count;
                this.SearchPositionNo = 0;
            }

            foreach (Move move in movesPossible)
            {
                Move moveThis = move.Piece.Move(move.Name, move.To);
                if (ply == this.SearchDepth)
                {
                    this.SearchPositionNo++;
                    this.CurrentMoveSearched = moveThis;
                    if (this.SearchMoveConsideredEvent != null)
                    {
                        this.SearchMoveConsideredEvent();
                    }

                    // A little counter to record the deepest Quiescence depth searched on this move.
                    this.MaxQuiesenceDepthReached = this.SearchDepth;

                    // A little counter to track the number of extensions on this move.
                    this.MaxExtensions = 0;
                }

                if (player.IsInCheck)
                {
                    Move.Undo(moveThis);
                    continue;
                }

                intLegalMovesAttempted++;

                if (moveBest == null)
                {
                    moveBest = moveThis;
                }

                if (Game.CaptureMoveAnalysisData && this.SearchDepth == this.MaxSearchDepth)
                {
                    // Add moves to post-move analysis tree, if option set by user
                    if (parentMove == null || parentMove.Name != Move.MoveNames.NullMove)
                    {
                        analysisParentBranch.Add(moveThis);
                    }

                    moveThis.Moves = new Moves();
                }

                // TODO Consider fractional extensions. http://chessprogramming.wikispaces.com/Extensions
                // Search Extensions
                int intExtension = 0;

                if (movesPossible.Count == 1)
                {
                    // Single Response
                    intExtension = 1;
                }
                else if (parentMove != null && parentMove.IsEnemyInCheck)
                {
                    // Check evasion
                    intExtension = 1;
                }
                else if (parentMove != null && parentMove.PieceCaptured != null && moveThis.PieceCaptured != null
                         && parentMove.PieceCaptured.BasicValue == moveThis.PieceCaptured.BasicValue
                         && parentMove.To == moveThis.To)
                {
                    // Recapture piece of same basic value (on the same square)
                    intExtension = 1;
                }
                else if (moveThis.Piece.Name != Piece.PieceNames.Pawn
                         ||
                         ((moveThis.Piece.Player.Colour != Player.PlayerColourNames.White || moveThis.To.Rank != 6)
                          && (moveThis.Piece.Player.Colour != Player.PlayerColourNames.Black || moveThis.To.Rank != 1)))
                {
                    // Pawn push to 7th rank
                    intExtension = 1;
                }

                // Reductions
                // Only reduce if this move hasn't been extended.
                if (intExtension == 0)
                {
                    if (variableDepth > 2 && !blnIsInCheck && moveThis.PieceCaptured == null && !moveThis.IsEnemyInCheck)
                    {
                        int[] margin = 
                                        {
                                           0, 0, 0, 5000, 5000, 7000, 7000, 9000, 9000, 15000, 15000, 15000, 15000, 15000,
                                           15000, 15000, 15000, 15000
                                        };

                        // int intLazyEval = this.TotalPieceValue - this.OtherPlayer.TotalPieceValue;
                        int intLazyEval = player.Score;
                        if (alpha > intLazyEval + margin[variableDepth])
                        {
                            intExtension = -1;
                        }
                    }

                    // Futility Pruning
                    if (!blnIsInCheck)
                    {
                        switch (variableDepth)
                        {
                            case 2:
                            case 3:

                                // case 4:
                                if (moveThis.PieceCaptured == null && !move.IsEnemyInCheck)
                                {
                                    int intLazyEval = player.Score;

                                    switch (variableDepth)
                                    {
                                        case 2:

                                            // Standard Futility Pruning
                                            if (intLazyEval + 3000 <= alpha)
                                            {
                                                intExtension = -1;
                                            }

                                            break;

                                        case 3:

                                            // Extended Futility Pruning
                                            if (intLazyEval + 5000 <= alpha)
                                            {
                                                intExtension = -1;
                                            }

                                            break;

                                        case 4:

                                            // Razoring
                                            if (intLazyEval + 9750 <= alpha)
                                            {
                                                intExtension = -1;
                                            }

                                            break;
                                    }
                                }

                                break;
                        }
                    }
                }

                /*
                if (intExtension>0 && intTotalExtensions>=m_intSearchDepth)
                {
                    intExtension = 0;
                }
                */

                /* #if DEBUG  // Avoid to break in a zero window research so alpha + 1 < beta
                   if ((alpha + 1 < beta) && DebugMatchVariation(m_intSearchDepth - ply, moveThis)) Debugger.Break();
                 #endif */
                if (blnPVNode)
                {
                    val =
                        -this.AlphaBetaPvs(
                            player.OpposingPlayer, 
                            ply - 1, 
                            (variableDepth + intExtension) - 1, 
                            -alpha - 1, 
                            -alpha, 
                            moveThis, 
                            movesPV, 
                            totalExtensionsAndReducations + intExtension, 
                            moveThis.Moves);
                    if ((val > alpha) && (val < beta))
                    {
                        // fail
                        if (Game.CaptureMoveAnalysisData && this.SearchDepth == this.MaxSearchDepth
                            && parentMove != null && parentMove.Name != Move.MoveNames.NullMove)
                        {
                            moveThis.Moves.Clear();
                        }

                        val =
                            -this.AlphaBetaPvs(
                                player.OpposingPlayer, 
                                ply - 1, 
                                (variableDepth + intExtension) - 1, 
                                -beta, 
                                -alpha, 
                                moveThis, 
                                movesPV, 
                                totalExtensionsAndReducations + intExtension, 
                                moveThis.Moves);
                    }
                }
                else
                {
                    val =
                        -this.AlphaBetaPvs(
                            player.OpposingPlayer, 
                            ply - 1, 
                            (variableDepth + intExtension) - 1, 
                            -beta, 
                            -alpha, 
                            moveThis, 
                            movesPV, 
                            totalExtensionsAndReducations + intExtension, 
                            moveThis.Moves);
                }

                if (!blnAllMovesWereGenerated && val < intScoreAtEntry)
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
                    hashType = HashTable.HashTypeNames.Beta;
                    moveBest = moveThis;

                    // if (move.Score < 15000)
                    // {
                    History.Record(player.Colour, moveThis.From.Ordinal, moveThis.To.Ordinal, variableDepth * variableDepth);

                    // 15Mar06 Nimzo Don't include captures as killer moves
                    if ((moveThis.PieceCaptured == null)
                        && ((parentMove == null) || (parentMove.Name != Move.MoveNames.NullMove)))
                    {
                        KillerMoves.RecordPossibleKillerMove(ply, moveBest);
                    }

                    // End Nimzo code

                    // }
                    goto Exit;
                }

                if (val > alpha)
                {
                    blnPVNode = true; /* This is a PV node */
                    alpha = val;
                    hashType = HashTable.HashTypeNames.Exact;
                    moveBest = moveThis;

                    // Collect the Prinicial Variation
                    principalVariationMoves.Clear();
                    principalVariationMoves.Add(moveThis);
                    foreach (Move moveCopy in movesPV)
                    {
                        principalVariationMoves.Add(moveCopy);
                    }

                    // #if DEBUG
                    // Debug.WriteLineIf((ply == m_intSearchDepth) && (ply > 1), string.Format("{0} {1} {2}", ply, PvLine(principalVariationMoves), alpha));
                    // #endif
                }

                moveThis.Alpha = alpha;
                moveThis.Beta = beta;

                if (!Game.IsInAnalyseMode && !this.MyBrain.IsPondering && this.SearchDepth > MinSearchDepth
                    && (DateTime.Now - player.Clock.TurnStartTime) > this.MaxSearchTimeAllowed)
                {
                    throw new ForceImmediateMoveException();
                }
            }

            // Check for Stalemate
            if (intLegalMovesAttempted == 0)
            {
                // depth>0 && !player.OtherPlayer.IsInCheck
                // alpha = this.Score;
                alpha = -player.OpposingPlayer.Score;
            }

            Exit:

            // Record best move
            if (moveBest != null)
            {
                HashTable.RecordHash(
                    Board.HashCodeA, 
                    Board.HashCodeB, 
                    ply, 
                    alpha, 
                    hashType, 
                    moveBest.From.Ordinal, 
                    moveBest.To.Ordinal, 
                    moveBest.Name, 
                    player.Colour);
            }
            else
            {
                HashTable.RecordHash(
                    Board.HashCodeA, 
                    Board.HashCodeB, 
                    ply, 
                    alpha, 
                    hashType, 
                    -1, 
                    -1, 
                    Move.MoveNames.NullMove, 
                    player.Colour);
            }

            return alpha;
        }

        /// <summary>
        /// Aspiration windows are a way to reduce the search space  in an alpha-beta search. 
        /// The technique is to use a guess of the expected value (usually from the last iteration in iterative deepening), 
        /// and use a window around this as the alpha-beta bounds. Because the window is narrower, more beta cutoffs are achieved, 
        /// and the search takes a shorter time. The drawback is that if the true score is outside this window, then a costly re-search must be made. 
        /// Typical window sizes are 1/2 to 1/4 of a pawn on either side of the guess.
        /// http://chessprogramming.wikispaces.com/Aspiration+Windows
        /// http://chessprogramming.wikispaces.com/PVS+and+aspiration
        /// </summary>
        /// <param name="player">
        /// The player to play.
        /// </param>
        /// <param name="principalVariationMoves">
        /// When move analysis is enabled, a tree of search moves is collected in this variable, which can be viewed in the GUI.
        /// </param>
        /// <param name="lastIterationsScore">
        /// Score from the previous (iterative deepending) iteration. Used as the centre of the aspiration window.
        /// </param>
        /// <param name="analysisParentBranch">
        /// The analysis Parent Branch. When move analysis is enabled, a tree of search moves is collected in this variable, which can be viewed in the GUI.
        /// </param>
        /// <returns>
        /// Score of the best move found.
        /// </returns>
        private int Aspirate(
            Player player, Moves principalVariationMoves, int lastIterationsScore, Moves analysisParentBranch)
        {
            int alpha = MinScore; // Score of the best move found so far
            int beta = MaxScore; // Score of the best move found by the opponent
            int val = alpha;

            for (int intAttempt = 0; intAttempt < 3; intAttempt++)
            {
                switch (intAttempt)
                {
                    case 0:
                        alpha = lastIterationsScore - 500;
                        beta = lastIterationsScore + 500;
                        break;

                    case 1:
                        alpha = lastIterationsScore - 2000;
                        beta = lastIterationsScore + 2000;
                        break;

                    case 2:
                        alpha = MinScore;
                        beta = MaxScore;
                        break;
                }

                val = this.AlphaBetaPvs(
                    player, 
                    this.SearchDepth, 
                    this.SearchDepth, 
                    alpha, 
                    beta, 
                    null, 
                    principalVariationMoves, 
                    0, 
                    analysisParentBranch);
                if (val > alpha && val < beta)
                {
                    break;
                }
            }

            return val;
        }

        /// <summary>
        /// Evaluates and assigns a move-order score to a move
        /// </summary>
        /// <param name="move">
        /// Move to evaluate
        /// </param>
        /// <param name="moveHash">
        /// Best move from hash table.
        /// </param>
        /// <param name="moveKillerA">
        /// Best killer move from this ply.
        /// </param>
        /// <param name="moveKillerA2">
        /// Second best killer move from this ply.
        /// </param>
        /// <param name="moveKillerB">
        /// Best killer move from previous ply.
        /// </param>
        /// <param name="moveKillerB2">
        /// Second best killer move from previous ply.
        /// </param>
        /// <param name="player">
        /// The player.
        /// </param>
        private void AssignMoveOrderScore(
            Move move, 
            Move moveHash, 
            Move moveKillerA, 
            Move moveKillerA2, 
            Move moveKillerB, 
            Move moveKillerB2, 
            Player player)
        {
            move.Score = 0;

            if (moveHash != null && Move.MovesMatch(move, moveHash))
            {
                move.Score += 10000000;
                return;
            }

            switch (move.Name)
            {
                case Move.MoveNames.PawnPromotionQueen:
                    move.Score += 900000;
                    break;
                case Move.MoveNames.PawnPromotionRook:
                    move.Score += 500000;
                    break;
                case Move.MoveNames.PawnPromotionBishop:
                    move.Score += 300000;
                    break;
                case Move.MoveNames.PawnPromotionKnight:
                    move.Score += 300000;
                    break;
            }

            if (move.PieceCaptured != null)
            {
                // Resulty of Static exchange evaluation
                move.Score += this.SEE(move) * 100000;

                if (move.Score != 0)
                {
                    return;
                }

                // "Good" capture
                if (move.From.Piece.Name == Piece.PieceNames.Queen && move.To.Piece.Name == Piece.PieceNames.Queen)
                {
                    move.Score += 99999;
                    return;
                }

                if (move.From.Piece.Name == Piece.PieceNames.Rook && move.To.Piece.Name == Piece.PieceNames.Rook)
                {
                    move.Score += 99998;
                    return;
                }

                if (move.From.Piece.Name == Piece.PieceNames.Knight && move.To.Piece.Name == Piece.PieceNames.Bishop)
                {
                    move.Score += 99997;
                    return;
                }

                if (move.From.Piece.Name == Piece.PieceNames.Bishop
                    && move.To.Piece.Name == Piece.PieceNames.Bishop)
                {
                    move.Score += 99996;
                    return;
                }

                if (move.From.Piece.Name == Piece.PieceNames.Bishop
                    && move.To.Piece.Name == Piece.PieceNames.Knight)
                {
                    move.Score += 99995;
                    return;
                }

                if (move.From.Piece.Name == Piece.PieceNames.Pawn && (
                                                                         (move.Name == Move.MoveNames.EnPassent && Board.GetPiece(move.To.Ordinal - player.PawnForwardOffset).Name == Piece.PieceNames.Pawn)
                                                                         || 
                                                                         move.To.Piece.Name == Piece.PieceNames.Pawn))
                {
                    move.Score += 99994;
                    return;
                }
            }

            move.Score += History.Retrieve(player.Colour, move.From.Ordinal, move.To.Ordinal);

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
            if (move.Score == 0)
            {
                // do something smart
                int x = 0;
            }
            */

            // Score based upon tactical positional value of board square i.e. how close to centre
            move.Score += move.To.Value - move.From.Value;
        }

        /// <summary>
        /// Recursive element of Perft.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="depth">
        /// The depth.
        /// </param>
        private void PerftPly(Player player, int depth)
        {
            if (depth <= 0)
            {
                return;
            }

            Moves moves = new Moves();
            player.GenerateLegalMoves(moves);

            foreach (Move move in moves)
            {
                Move moveUndo = move.Piece.Move(move.Name, move.To);

                this.PositionsSearched++;

                // Debug.WriteLine(move.DebugText + ",");
                this.PerftPly(player.OpposingPlayer, depth - 1);

                Move.Undo(moveUndo);
            }
        }

        /// <summary>
        /// Performs a Static Exchange Evaluation to determine the value of a move after all possible re-captures are resolved.
        /// http://chessprogramming.wikispaces.com/Static+Exchange+Evaluation
        /// </summary>
        /// <param name="moveMade">
        /// Move to be evaluated
        /// </param>
        /// <returns>
        /// The see.
        /// </returns>
        private int SEE(Move moveMade)
        {
            // Static Exchange Evaluator

            // Generate moves
            Moves movesFriendly = new Moves();
            Moves movesEnemy = new Moves();

            moveMade.To.AttackersMoveList(movesFriendly, moveMade.Piece.Player);
            moveMade.To.AttackersMoveList(movesEnemy, moveMade.Piece.Player.OpposingPlayer);

            // Remove piece I'm going to move first from my list of moves
            foreach (Move move in movesFriendly)
            {
                if (move.Piece == moveMade.Piece)
                {
                    movesFriendly.Remove(move);
                    break;
                }
            }

            // sort remaining moves by piece value
            foreach (Move move in movesFriendly)
            {
                move.Score = 20 - move.Piece.BasicValue;
            }

            movesFriendly.SortByScore();

            if (movesEnemy.Count > 1)
            {
                // sort remaining moves by piece value
                foreach (Move move in movesEnemy)
                {
                    move.Score = 20 - move.Piece.BasicValue;
                }

                movesEnemy.SortByScore();
            }

            int intTotalFriendlyGain = moveMade.PieceCaptured.BasicValue;
            int intLastMovedPieceValue = moveMade.Piece.BasicValue;

            // Now make a virtual move from each players move list, in order, until one of the players has no remaining moves.
            for (int intIndex = 0;; intIndex++)
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

        /// <summary>
        /// Sorts moves so that the best moves are first
        /// </summary>
        /// <param name="movesToSort">
        /// List of moves to be sorted.
        /// </param>
        /// <param name="moveHash">
        /// // Best move from hash table.
        /// </param>
        /// <param name="moveKillerA">
        /// Best killer move from this ply.
        /// </param>
        /// <param name="moveKillerA2">
        /// Second best killer move from this ply.
        /// </param>
        /// <param name="moveKillerB">
        /// Best killer move from previous ply.
        /// </param>
        /// <param name="moveKillerB2">
        /// Second best killer move from previous ply.
        /// </param>
        /// <param name="player">
        /// The player.
        /// </param>
        private void SortBestMoves(
            Moves movesToSort, 
            Move moveHash, 
            Move moveKillerA, 
            Move moveKillerA2, 
            Move moveKillerB, 
            Move moveKillerB2, 
            Player player)
        {
            foreach (Move movex in movesToSort)
            {
                this.AssignMoveOrderScore(movex, moveHash, moveKillerA, moveKillerA2, moveKillerB, moveKillerB2, player);
            }

            movesToSort.SortByScore();
        }

        #endregion
    }
}