// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Search.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Performs the central move-selection logic for SharpChess, referred to as the Search. http://chessprogramming.wikispaces.com/Search
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region License

// SharpChess
// Copyright (C) 2012 SharpChess.com
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
    ///   Performs the central move-selection logic for SharpChess, referred to as the Search. http://chessprogramming.wikispaces.com/Search
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
        ///   Initializes a new instance of the <see cref="Search" /> class.
        /// </summary>
        /// <param name="brain"> The brain performing this search. </param>
        public Search(Brain brain)
        {
            this.MyBrain = brain;
            this.MaxSearchDepth = 32;
            this.LastPrincipalVariation = new Moves();
        }

        #endregion

        #region Delegates

        /// <summary>
        ///   The delegatetype Search event.
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
        ///   Gets the maximum search time allowed.
        /// </summary>
        public TimeSpan MaxSearchTimeAllowed { get; private set; }

        /// <summary>
        ///   Gets the player's brain performing this search.
        /// </summary>
        public Brain MyBrain { get; private set; }

        /// <summary>
        ///   Gets PositionsPerSecond.
        /// </summary>
        public int PositionsPerSecond
        {
            get
            {
                return this.PositionsSearchedThisTurn
                       / Math.Max(Convert.ToInt32(this.MyBrain.ThinkingTimeElpased.TotalSeconds), 1);
            }
        }

        /// <summary>
        ///   Gets the number of positions searched this iteration.
        /// </summary>
        public int PositionsSearchedThisIteration { get; private set; }

        /// <summary>
        ///   Gets the number of positions searched this turn.
        /// </summary>
        public int PositionsSearchedThisTurn { get; private set; }

        /// <summary>
        ///   Gets the current search depth.
        /// </summary>
        public int SearchDepth { get; private set; }

        /// <summary>
        ///   Gets the current search position number.
        /// </summary>
        public int SearchPositionNo { get; private set; }

        /// <summary>
        ///   Gets the total positions to search.
        /// </summary>
        public int TotalPositionsToSearch { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the Principal Variation from the previous iteration.
        /// </summary>
        private Moves LastPrincipalVariation { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Starts the iterative deepening search to find the best move for this player. http://chessprogramming.wikispaces.com/Iterative+Deepening
        /// </summary>
        /// <param name="player"> The player to play. </param>
        /// <param name="principalVariation"> Move in the principal variation will be added to this list. </param>
        /// <param name="recommendedSearchTime"> Recommended search time allotted. </param>
        /// <param name="maximumSearchTimeAllowed"> The maximum search time allowed. </param>
        /// <returns> The best move for the player. </returns>
        /// <exception cref="ForceImmediateMoveException">Raised when the user requests for thinking to be terminated, and immediate move to made.</exception>
        public int IterativeDeepeningSearch(
            Player player, Moves principalVariation, TimeSpan recommendedSearchTime, TimeSpan maximumSearchTimeAllowed)
        {
            /* A new deeper ply of search will only be started, if the cutoff time hasnt been reached yet. 
             Minimum search time = 100 milli-second */
            TimeSpan searchTimeCutoff = new TimeSpan(recommendedSearchTime.Ticks / 3);
            if (searchTimeCutoff.TotalMilliseconds < 100)
            {
                searchTimeCutoff = new TimeSpan(0, 0, 0, 0, 100);
            }

            this.MaxSearchTimeAllowed = maximumSearchTimeAllowed;

            this.forceExitWithMove = false; // Set to stop thread thinking and return best move
            this.PositionsSearchedThisTurn = 0; // Total number of positions considered so far this turn.
            this.Evaluations = 0;
            this.LastPrincipalVariation.Clear();

            // A little counter to record the deepest Quiescence depth searched on this move.
            this.MaxQuiesenceDepthReached = 0;

            // A little counter to track the number of extensions on this move.
            this.MaxExtensions = 0;

            // Set max search depth, as defined is game difficulty settings
            this.MaxSearchDepth = Game.MaximumSearchDepth == 0 ? 32 : Game.MaximumSearchDepth;

            int score = player.Score;

            for (this.SearchDepth = MinSearchDepth; this.SearchDepth <= this.MaxSearchDepth; this.SearchDepth++)
            {
                this.PositionsSearchedThisIteration = 0; // Reset positions searched this iteration.
                KillerMoves.Clear(); // Clear killer moves from previous iteration.
                HistoryHeuristic.Clear(); // Clear history when from previous iteration. 

                if (Game.CaptureMoveAnalysisData && Game.MoveAnalysis.Count > 0)
                {
                    Game.MoveAnalysis.Clear();
                }

                score = this.Aspirate(player, principalVariation, score, Game.MoveAnalysis);

                if (!Game.IsInAnalyseMode && Game.ClockFixedTimePerMove.TotalSeconds <= 0 && !this.MyBrain.IsPondering
                    && this.MyBrain.ThinkingTimeElpased > searchTimeCutoff)
                {
                    throw new ForceImmediateMoveException();
                }

                // Copy current PV to previous PV.
                this.LastPrincipalVariation.Clear();
                foreach (Move moveCopy in principalVariation)
                {
                    this.LastPrincipalVariation.Add(moveCopy);
                }

                WinBoard.SendThinking(
                    this.SearchDepth,
                    score,
                    DateTime.Now - player.Clock.TurnStartTime,
                    this.PositionsSearchedThisIteration,
                    this.MyBrain.PrincipalVariationText);

                if (score > 99999 || score < -99999)
                {
                    break; // Checkmate found so dont bother searching any deeper
                }
            }

            return score;
        }

        /// <summary>
        ///   Walks the move generation tree and counts all the leaf nodes of a certain depth, which can be compared to predetermined values and used to isolate bugs. http://chessprogramming.wikispaces.com/Perft
        /// </summary>
        /// <param name="player"> The player to play. </param>
        /// <param name="targetDepth"> The target depth. </param>
        public void Perft(Player player, int targetDepth)
        {
            this.PositionsSearchedThisTurn = 0;
            this.PerftPly(player, targetDepth);
        }

        /// <summary>
        ///   The force search to exit with an immediate move.
        /// </summary>
        public void SearchForceExitWithMove()
        {
            this.forceExitWithMove = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apply Search Extensions
        /// </summary>
        /// <param name="extensionOrReduction">Number of extensions or reductions applied in the search node. Extensions positive, reductions negative.</param>
        /// <param name="movesPossible">List of possible modes at this search node.</param>
        /// <param name="moveMade">One of the candidate moves made from this search node.</param>
        /// <param name="parentMove">Move that is the parent of this search node.</param>
        private static void ApplyExtensions(
            ref int extensionOrReduction, Moves movesPossible, Move moveMade, Move parentMove)
        {
            if (Game.EnableExtensions)
            {
                if (movesPossible.Count == 1)
                {
                    // Single Response
                    extensionOrReduction = 1;
                    Comment(moveMade, "E-1REP ");
                }
                else if (parentMove != null && parentMove.IsEnemyInCheck)
                {
                    // Check evasion
                    extensionOrReduction = 1;
                    Comment(moveMade, "E-CHK ");
                }
                else if (parentMove != null && parentMove.PieceCaptured != null && moveMade.PieceCaptured != null
                         && parentMove.PieceCaptured.BasicValue == moveMade.PieceCaptured.BasicValue
                         && parentMove.To == moveMade.To)
                {
                    // Recapture piece of same basic value (on the same square)
                    extensionOrReduction = 1;
                    Comment(moveMade, "E-RECAP ");
                }
                else if (moveMade.Piece.Name == Piece.PieceNames.Pawn
                         &&
                         ((moveMade.Piece.Player.Colour == Player.PlayerColourNames.White && moveMade.To.Rank == 6)
                          ||
                          (moveMade.Piece.Player.Colour == Player.PlayerColourNames.Black && moveMade.To.Rank == 1)))
                {
                    // Pawn push to 7th rank
                    extensionOrReduction = 1;
                    Comment(moveMade, "E-PAWN7 ");
                }
            }
        }

        /// <summary>
        ///   Add a commnent to the Move Analysis move.
        /// </summary>
        /// <param name="move"> Move to comment. </param>
        /// <param name="comment"> Comment to add. </param>
        private static void Comment(Move move, string comment)
        {
            if (Game.CaptureMoveAnalysisData)
            {
                move.DebugComment += comment;
            }
        }

        /// <summary>
        ///   Performs the search foe the best move, using a specialised form of alpha beta search, named Principal Variation Search (PVS) . http://chessprogramming.wikispaces.com/Alpha-Beta http://chessprogramming.wikispaces.com/Principal+Variation+Search
        /// </summary>
        /// <param name="player"> The player to play. The player is alternated at each new ply of search. </param>
        /// <param name="ply"> True depth in plies. Starts at the max search depth and is DECREMENTED as alpha beta get deeper. </param>
        /// <param name="variableDepth"> Variable depth which starts at the max search depth and is DECREMENTED as alpha beta get deeper. Its value is altered by search extension and reductions. Quiesence starts at depth 0. http://chessprogramming.wikispaces.com/Depth </param>
        /// <param name="alpha"> Alpha (α) is the lower bound, representing the minimum score that a node must reach in order to change the value of a previous node. http://chessprogramming.wikispaces.com/Alpha </param>
        /// <param name="beta"> Beta (β) is the upper bound of a score for the node. If the node value exceeds or equals beta, it means that the opponent will avoid this node, since his guaranteed score (Alpha of the parent node) is already greater. Thus, Beta is the best-score the opponent (min-player) could archive so far... http://chessprogramming.wikispaces.com/Beta </param>
        /// <param name="parentMove"> Move from the parent alpha beta call. </param>
        /// <param name="principalVariation"> The Principal variation (PV) is a sequence of moves is considered best and therefore expect to be played. This list of moves is collected during the alpha beta search. http://chessprogramming.wikispaces.com/Principal+variation </param>
        /// <param name="totalExtensionsOrReductions"> Holds a counter indicating the number of search extensions or reductions at the current search depth. A positive nunber indicates there have been extensions in a previous ply, negative indicates a reduction. http://chessprogramming.wikispaces.com/Extensions http://chessprogramming.wikispaces.com/Reductions </param>
        /// <param name="analysisParentBranch"> When move analysis is enabled, a tree of search moves is collected in this variable, which can be viewed in the GUI. </param>
        /// <returns> The score of the best move. </returns>
        /// <exception cref="ForceImmediateMoveException">Raised when the user requests for thinking to be terminated, and immediate move to made.</exception>
        private int AlphaBetaPvs(
            Player player,
            int ply,
            int variableDepth,
            int alpha,
            int beta,
            Move parentMove,
            Moves principalVariation,
            int totalExtensionsOrReductions,
            Moves analysisParentBranch)
        {
            // TODO Try hash and killer moves, before generating all moves. It will save time.

            // Score of the move being examined in the move loop.
            int val;

            // Type of hash entry that will be stored in the Transposition Table. http://chessprogramming.wikispaces.com/Transposition+Table
            HashTable.HashTypeNames hashType = HashTable.HashTypeNames.Alpha;

            // The best move found at this node. Assigned if/when alpha is improved.
            Move bestMove = null;

            // Indicates that this entire node is a PV Node. http://chessprogramming.wikispaces.com/Node+Types
            bool isPvNode = false;

            // A counter of the number of legal moves we've examines in this node so far.
            int legalMovesAttempted = 0;

            // Indicates whether the player-to-play is in check at this node.
            bool isInCheck = player.IsInCheck;

            // Exit immediately, if instructed to do so. i.e. the user has click the "Move Now" button.
            if (this.forceExitWithMove)
            {
                throw new ForceImmediateMoveException();
            }

            // This node has reached 3-move-repetition, so return the current score, and don't bother searching any deeper.
            if (parentMove != null && parentMove.IsThreeMoveRepetition)
            {
                return player.Score;
            }

            if (totalExtensionsOrReductions > this.MaxExtensions)
            {
                this.MaxExtensions = totalExtensionsOrReductions;
            }

            // Check if this node (position) is in the tranposition (hash) table, and if appropriate, return the score stored there.
            if (ply != this.SearchDepth
                &&
                (val = HashTable.ProbeForScore(Board.HashCodeA, Board.HashCodeB, ply, alpha, beta, player.Colour))
                != HashTable.NotFoundInHashTable)
            {
                // High values of "val" indicate that a checkmate has been found
                if (val > 1000000 || val < -1000000)
                {
                    Comment(parentMove, "Mate: " + (this.MaxSearchDepth - variableDepth) + " ");
                    if (this.MaxSearchDepth - variableDepth > 0)
                    {
                        val /= this.MaxSearchDepth - variableDepth;
                    }
                }

                return val;
            }

            // Depth <= 0 means we're into Quiescence searching
            if (variableDepth <= 0)
            {
                return this.Quiesce(
                    player, ply, variableDepth, alpha, beta, parentMove, principalVariation, analysisParentBranch);
            }

            // Generate "lazy" moves (lazy means we include moves that put our own king in check)
            Moves movesPossible = new Moves();
            player.GenerateLazyMoves(movesPossible, Moves.MoveListNames.All);

            // Stores the PV that is local to this node and it's children.
            Moves localPrincipalVariation = new Moves();

            if (Game.EnableNullMovePruning)
            {
                // Adaptive Null-move pruning
                // http://chessprogramming.wikispaces.com/Null+Move+Pruning
                int r = variableDepth > 6 ? 3 : 2;
                if (variableDepth > (r + 1) && parentMove != null && parentMove.Name != Move.MoveNames.NullMove
                    && Game.Stage != Game.GameStageNames.End && !isInCheck)
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
                            localPrincipalVariation,
                            totalExtensionsOrReductions,
                            null);
                    if (val >= beta)
                    {
                        return beta;
                    }
                }
            }

            // Get last iteration's best move from the transposition Table
            Move moveHash = HashTable.ProbeForBestMove(Board.HashCodeA, Board.HashCodeB, player.Colour);

            // Get Killers
            Move moveKillerA = KillerMoves.RetrieveA(ply);
            Move moveKillerB = KillerMoves.RetrieveB(ply);
            Move moveKillerA2 = KillerMoves.RetrieveA(ply + 2);
            Move moveKillerB2 = KillerMoves.RetrieveB(ply + 2);

            // Get move at same ply from previous iteration's principal variation.
            int indexPv = this.SearchDepth - ply;
            Move movePv = null;
            if (indexPv < this.LastPrincipalVariation.Count)
            {
                movePv = this.LastPrincipalVariation[indexPv];
            }

            // Sort moves
            this.SortBestMoves(
                movesPossible,
                variableDepth,
                movePv,
                moveHash,
                moveKillerA,
                moveKillerA2,
                moveKillerB,
                moveKillerB2,
                player);

            if (ply == this.SearchDepth)
            {
                this.TotalPositionsToSearch = movesPossible.Count;
                this.SearchPositionNo = 0;
            }

            foreach (Move move in movesPossible)
            {
                // Make the move
                Move moveMade = move.Piece.Move(move.Name, move.To);

                this.PositionsSearchedThisTurn++;
                this.PositionsSearchedThisIteration++;

                moveMade.DebugComment += move.DebugComment;

                if (ply == this.SearchDepth)
                {
                    this.SearchPositionNo++;
                    this.CurrentMoveSearched = moveMade;
                    if (this.SearchMoveConsideredEvent != null)
                    {
                        this.SearchMoveConsideredEvent();
                    }
                }

                // This move put our player in check, so abort, and skip to next move.
                if (player.IsInCheck)
                {
                    Move.Undo(moveMade);
                    continue;
                }

                legalMovesAttempted++;

                if (bestMove == null)
                {
                    bestMove = moveMade;
                }

                if (Game.CaptureMoveAnalysisData)
                {
                    // Add moves to post-move analysis tree, if option set by user
                    if (parentMove == null || parentMove.Name != Move.MoveNames.NullMove)
                    {
                        if (analysisParentBranch != null)
                        {
                            analysisParentBranch.Add(moveMade);
                        }
                    }

                    moveMade.Moves = new Moves();
                }

                int extensionOrReduction = 0;

                // Extensions
                // http://chessprogramming.wikispaces.com/Extensions
                ApplyExtensions(ref extensionOrReduction, movesPossible, moveMade, parentMove);

                // Reductions
                // http://chessprogramming.wikispaces.com/Reductions
                this.ApplyReductions(
                    ref extensionOrReduction,
                    totalExtensionsOrReductions,
                    ply,
                    variableDepth,
                    isPvNode,
                    isInCheck,
                    alpha,
                    player,
                    moveMade,
                    legalMovesAttempted,
                    localPrincipalVariation);

                if (Game.EnablePvsSearch && isPvNode)
                {
                    // Attempt a Principal Variation Search (PVS) using a zero window. http://chessprogramming.wikispaces.com/Principal+Variation+Search
                    val =
                        -this.AlphaBetaPvs(
                            player.OpposingPlayer,
                            ply - 1,
                            (variableDepth + extensionOrReduction) - 1,
                            -alpha - 1,
                            -alpha,
                            moveMade,
                            localPrincipalVariation,
                            totalExtensionsOrReductions + extensionOrReduction,
                            moveMade.Moves);

                    if ((val > alpha) && (val < beta))
                    {
                        // PVS failed. Have to re-search using a full alpha-beta window.
                        Comment(moveMade, "-PVS-WIN- ");

                        if (Game.CaptureMoveAnalysisData && parentMove != null
                            && parentMove.Name != Move.MoveNames.NullMove)
                        {
                            moveMade.Moves.Clear();
                        }

                        val =
                            -this.AlphaBetaPvs(
                                player.OpposingPlayer,
                                ply - 1,
                                (variableDepth + extensionOrReduction) - 1,
                                -beta,
                                -alpha,
                                moveMade,
                                localPrincipalVariation,
                                totalExtensionsOrReductions + extensionOrReduction,
                                moveMade.Moves);
                    }
                    else
                    {
                        Comment(moveMade, "-F- ");
                    }
                }
                else
                {
                    // Not a PV node, so just do a normal alpha-beta search.
                    val =
                        -this.AlphaBetaPvs(
                            player.OpposingPlayer,
                            ply - 1,
                            (variableDepth + extensionOrReduction) - 1,
                            -beta,
                            -alpha,
                            moveMade,
                            localPrincipalVariation,
                            totalExtensionsOrReductions + extensionOrReduction,
                            moveMade.Moves);
                }

                move.Score = moveMade.Score = val;

                // Take back the move
                Move.Undo(moveMade);

                if (val >= beta)
                {
                    // Test for a beta cut-off http://chessprogramming.wikispaces.com/Beta-Cutoff
                    alpha = beta;
                    moveMade.Beta = beta;
                    hashType = HashTable.HashTypeNames.Beta;
                    bestMove = moveMade;

                    Comment(parentMove, "(CUT) ");

                    if (moveMade.PieceCaptured == null)
                    {
                        // Add this cut move to the history heuristic.
                        HistoryHeuristic.Record(player.Colour, moveMade.From.Ordinal, moveMade.To.Ordinal, ply * ply);

                        // Add this cut move to the killer move heuristic.
                        KillerMoves.RecordPossibleKillerMove(ply, moveMade);
                    }

                    goto Exit;
                }

                if (val > alpha)
                {
                    // Test if the move made can improve alpha. 
                    // If so, then it become the new best move, and local PV for this node. http://chessprogramming.wikispaces.com/Node
                    Comment(moveMade, "*PV* " + alpha + "<" + val);

                    isPvNode = true; /* This is a PV node */
                    alpha = val;
                    hashType = HashTable.HashTypeNames.Exact;
                    bestMove = moveMade;

                    // Collect the Prinicial Variation
                    lock (principalVariation)
                    {
                        principalVariation.Clear();
                        principalVariation.Add(moveMade);
                        foreach (Move moveCopy in localPrincipalVariation)
                        {
                            principalVariation.Add(moveCopy);
                        }
                    }
                }

                moveMade.Alpha = alpha;
                moveMade.Beta = beta;
                if (!Game.IsInAnalyseMode && !this.MyBrain.IsPondering && this.SearchDepth > MinSearchDepth
                    && this.MyBrain.ThinkingTimeElpased > this.MaxSearchTimeAllowed)
                {
                    throw new ForceImmediateMoveException();
                }
            }

            if (!isPvNode && parentMove != null)
            {
                // All positions were search at this node. It's not a PV node, and no beta cut-off occured,
                // therefore it's an ALL node. http://chessprogramming.wikispaces.com/Node+Types#ALL
                Comment(parentMove, "(ALL) ");
            }

            // Check for Stalemate
            if (legalMovesAttempted == 0)
            {
                alpha = player.Score;
            }

            Exit:

            // Record best move
            if (bestMove != null)
            {
                HashTable.RecordHash(
                    Board.HashCodeA,
                    Board.HashCodeB,
                    ply,
                    alpha,
                    hashType,
                    bestMove.From.Ordinal,
                    bestMove.To.Ordinal,
                    bestMove.Name,
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
        /// Apply reductions to the search node.
        /// http://chessprogramming.wikispaces.com/Reductions
        /// </summary>
        /// <param name="extensionOrReduction">Number of extensions or reductions applied in the search node. Extensions positive, reductions negative.</param>
        /// <param name="totalExtensionsOrReductions">Total number of extensions or reductions applied to the entire search tree. Extensions positive, reductions negative.</param>
        /// <param name="ply"> True depth in plies. Starts at the max search depth and is DECREMENTED as alpha beta get deeper. </param>
        /// <param name="variableDepth"> Variable depth which starts at the max search depth and is DECREMENTED as alpha beta get deeper. Its value is altered by search extension and reductions. Quiesence starts at depth 0. http://chessprogramming.wikispaces.com/Depth </param>
        /// <param name="isPvNode">True if the search node is part of the principal variant.</param>
        /// <param name="isInCheck">True if the play-to-play is in check.</param>
        /// <param name="alpha"> Alpha (α) is the lower bound, representing the minimum score that a node must reach in order to change the value of a previous node. http://chessprogramming.wikispaces.com/Alpha </param>
        /// <param name="player"> The player to play. The player is alternated at each new ply of search. </param>
        /// <param name="moveMade">Move made by the player-to-play.</param>
        /// <param name="legalMovesAttempted">Number of legal moves attempted.</param>
        /// <param name="movesPv">Prinipal variant at and below this search node.</param>
        private void ApplyReductions(
            ref int extensionOrReduction,
            int totalExtensionsOrReductions,
            int ply,
            int variableDepth,
            bool isPvNode,
            bool isInCheck,
            int alpha,
            Player player,
            Move moveMade,
            int legalMovesAttempted,
            Moves movesPv)
        {
            if (!Game.EnableReductions)
            {
                // Reductions disabled, so exit.
                return;
            }

            if (isPvNode)
            {
                // We're at a PV node, so not safe to reduce.
                return;
            }

            if (extensionOrReduction != 0)
            {
                // Extension has been applied, so don't reduce.
                return;
            }

            if (moveMade.PieceCaptured != null)
            {
                // Captures are too risky to reduce. 
                // TODO Consider reducing LOSING captures, according to SEE.
                return;
            }

            if (isInCheck)
            {
                // Player-to-play is in check, so not safe to reduce.
                return;
            }

            if (moveMade.IsEnemyInCheck)
            {
                // Don't reduce move that put the enemy in check
                return;
            }

            // Margin futility based reductions
            if (Game.EnableReductionFutilityMargin && variableDepth > 2)
            {
                int[] margin =
                               {
                                   0, 0, 0, 5000, 5000, 7000, 7000, 9000, 9000, 15000, 15000, 15000, 15000, 15000, 15000,
                                   15000, 15000, 15000
                               };

                int intLazyEval = player.Score;
                if (alpha > intLazyEval + margin[variableDepth])
                {
                    extensionOrReduction = -1;
                    Comment(moveMade, "R-MARG" + (margin[variableDepth] / 1000) + " ");
                }
            }

            // Futility Pruning http://chessprogramming.wikispaces.com/Futility+Pruning
            if (Game.EnableReductionFutilityFixedDepth && extensionOrReduction == 0)
            {
                switch (variableDepth)
                {
                    case 2:
                    case 3:
                        // case 4:
                        int intLazyEval = player.Score;

                        switch (variableDepth)
                        {
                            case 2:

                                // Standard Futility Pruning
                                if (intLazyEval + 3000 <= alpha)
                                {
                                    extensionOrReduction = -2;
                                    Comment(moveMade, "R-FUT3 ");
                                }

                                break;

                            case 3:

                                // Extended Futility Pruning
                                if (intLazyEval + 5000 <= alpha)
                                {
                                    extensionOrReduction = -3;
                                    Comment(moveMade, "R-FUT5 ");
                                }

                                break;

                            case 4:

                                // Deep Futility Pruning
                                if (intLazyEval + 9750 <= alpha)
                                {
                                    extensionOrReduction = -4;
                                    Comment(moveMade, "R-FUT9 ");
                                }

                                break;
                        }

                        break;
                }
            }

            // Late Move Reductions http://chessprogramming.wikispaces.com/Late+Move+Reductions
            // Reduce if move is 1) low in the search order and 2) has a poor history score and 3) not in check and 4) not already reduced or extended.
            if (Game.EnableReductionLateMove && extensionOrReduction == 0)
            {
                if (legalMovesAttempted > 3)
                {
                    int historyScore = HistoryHeuristic.Retrieve(
                        player.Colour, moveMade.From.Ordinal, moveMade.To.Ordinal);

                    if (historyScore == 0)
                    {
                        int eval =
                            -this.AlphaBetaPvs(
                                player.OpposingPlayer,
                                ply - 1,
                                variableDepth - 2,
                                -alpha - 1,
                                -alpha,
                                moveMade,
                                movesPv,
                                totalExtensionsOrReductions,
                                null);

                        if (eval < alpha)
                        {
                            extensionOrReduction = -1;
                            Comment(moveMade, "R-LMR ");
                        }
                    }
                }
            }
        }

        /// <summary>
        ///   Aspiration windows are a way to reduce the search space in an alpha-beta search. The technique is to use a guess of the expected value (usually from the last iteration in iterative deepening), and use a window around this as the alpha-beta bounds. Because the window is narrower, more beta cutoffs are achieved, and the search takes a shorter time. The drawback is that if the true score is outside this window, then a costly re-search must be made. Typical window sizes are 1/2 to 1/4 of a pawn on either side of the guess. http://chessprogramming.wikispaces.com/Aspiration+Windows http://chessprogramming.wikispaces.com/PVS+and+aspiration
        /// </summary>
        /// <param name="player"> The player to play. </param>
        /// <param name="principalVariation"> When move analysis is enabled, a tree of search moves is collected in this variable, which can be viewed in the GUI. </param>
        /// <param name="lastIterationsScore"> Score from the previous (iterative deepending) iteration. Used as the centre of the aspiration window. </param>
        /// <param name="analysisParentBranch"> The analysis Parent Branch. When move analysis is enabled, a tree of search moves is collected in this variable, which can be viewed in the GUI. </param>
        /// <returns> Score of the best move found. </returns>
        private int Aspirate(
            Player player, Moves principalVariation, int lastIterationsScore, Moves analysisParentBranch)
        {
            int alpha = MinScore; // Score of the best move found so far
            int beta = MaxScore; // Score of the best move found by the opponent
            int val = alpha;

            // TODO DISABLED: Investigate why aspiration is worse for SharpChess
            for (int intAttempt = Game.EnableAspiration ? 0 : 2; intAttempt < 3; intAttempt++)
            {
                switch (intAttempt)
                {
                    case 0:
                        alpha = lastIterationsScore - 250;
                        beta = lastIterationsScore + 250;
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
                    principalVariation,
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
        ///   Evaluates and assigns a move-order score to a move
        /// </summary>
        /// <param name="move"> Move to evaluate </param>
        /// <param name="variableDepth"> Variable depth which starts at the max search depth and is DECREMENTED as alpha beta get deeper. Its value is altered by search extension and reductions. Quiesence starts at depth 0. http://chessprogramming.wikispaces.com/Depth </param>
        /// <param name="movePv"> Move from previous iteration's principal variation. </param>
        /// <param name="moveHash"> Best move from hash table. </param>
        /// <param name="moveKillerA"> Best killer move from this ply. </param>
        /// <param name="moveKillerA2"> Second best killer move from this ply. </param>
        /// <param name="moveKillerB"> Best killer move from previous ply. </param>
        /// <param name="moveKillerB2"> Second best killer move from previous ply. </param>
        /// <param name="player"> The player. </param>
        private void AssignMoveOrderScore(
            Move move,
            int variableDepth,
            Move movePv,
            Move moveHash,
            Move moveKillerA,
            Move moveKillerA2,
            Move moveKillerB,
            Move moveKillerB2,
            Player player)
        {
            // TODO Create separate Quiescence move-ordering routine, with fewer parameters!
            move.Score = 0;

            if (moveHash != null && Move.MovesMatch(move, moveHash))
            {
                move.Score = 10000000;
                Comment(move, "O-HASH:" + move.Score + " ");
                return;
            }

            switch (move.Name)
            {
                case Move.MoveNames.PawnPromotionQueen:
                    move.Score = 999999;
                    break;
                case Move.MoveNames.PawnPromotionRook:
                    move.Score = 999998;
                    break;
                case Move.MoveNames.PawnPromotionBishop:
                    move.Score = 999997;
                    break;
                case Move.MoveNames.PawnPromotionKnight:
                    move.Score = 999996;
                    break;
            }

            if (move.Score != 0)
            {
                Comment(move, "O-PROM:" + move.Score + " ");
            }

            if (move.PieceCaptured != null)
            {
                // Result of Static exchange evaluation
                move.Score += this.SEE(move) * 100000;
                if (move.Score != 0)
                {
                    Comment(move, "O-SEE:" + move.Score + " ");
                    return;
                }

                // If in Quiescence and SEE is even, then sort by MVV/LVA
                move.Score = (move.PieceCaptured.Value * 100) - move.Piece.Value;
                if (move.Score != 0)
                {
                    Comment(move, "O-MVV:" + move.Score + " ");
                    return;
                }
            }

            // Killer moves
            if (moveKillerA != null && Move.MovesMatch(move, moveKillerA))
            {
                move.Score += 90000;
                Comment(move, "O-KILLA:" + move.Score + " ");
                return;
            }

            if (moveKillerB != null && Move.MovesMatch(move, moveKillerB))
            {
                move.Score += 70000;
                Comment(move, "O-KILLB:" + move.Score + " ");
                return;
            }

            /*
             * Including these makes the node count slighly worse.
            if (moveKillerA2 != null && Move.MovesMatch(move, moveKillerA2))
            {
                move.Score += 80000;
                AddMoveAnalysisComment(move, "O-KILLA-2:" + move.Score + " ");
                return;
            }

             * if (moveKillerB != null && Move.MovesMatch(move, moveKillerB2))
            {
                move.Score += 60000;
                AddMoveAnalysisComment(move, "O-KILLB-2:" + move.Score + " ");
                return;
            }
             * */

            move.Score += ((int)Math.Sqrt(HistoryHeuristic.Retrieve(player.Colour, move.From.Ordinal, move.To.Ordinal)))
                          * 100;
            if (move.Score != 0)
            {
                Comment(move, "O-HIST:" + move.Score + " ");
                return;
            }

            /*
            // Move from the Principal Variation
            if (movePv != null && Move.MovesMatch(move, movePv))
            {
                move.Score += 100;
            }
            */

            // Score based upon tactical positional value of board square i.e. how close to centre
            move.Score += move.To.Value - move.From.Value;
            Comment(move, "O-SV:" + move.Score + " ");
        }

        /// <summary>
        ///   Recursive element of Perft.
        /// </summary>
        /// <param name="player"> The player. </param>
        /// <param name="depth"> The depth. </param>
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

                this.PositionsSearchedThisTurn++;

                // Debug.WriteLine(move.DebugText + ",");
                this.PerftPly(player.OpposingPlayer, depth - 1);

                Move.Undo(moveUndo);
            }
        }

        /// <summary>
        ///   The purpose of quiescence search is to only evaluate "quiet" positions, or positions where there are no winning tactical moves to be made. This search is needed to avoid the horizon effect. http://chessprogramming.wikispaces.com/Quiescence+Search
        /// </summary>
        /// <param name="player"> The player to play. The player is alternated at each new ply of search. </param>
        /// <param name="ply"> True depth in plies. Starts at the max search depth and is DECREMENTED as alpha beta get deeper. </param>
        /// <param name="variableDepth"> Depth which starts at one and INCREASES as the search deepens. Its value is altered by search extension and reductions. Quiesence starts at depth 0. http://chessprogramming.wikispaces.com/Depth </param>
        /// <param name="alpha"> Alpha (α) is the lower bound, representing the minimum score that a node must reach in order to change the value of a previous node. http://chessprogramming.wikispaces.com/Alpha </param>
        /// <param name="beta"> Beta (β) is the upper bound of a score for the node. If the node value exceeds or equals beta, it means that the opponent will avoid this node, since his guaranteed score (Alpha of the parent node) is already greater. Thus, Beta is the best-score the opponent (min-player) could archive so far... http://chessprogramming.wikispaces.com/Beta </param>
        /// <param name="parentMove"> Move from the parent alpha beta call. </param>
        /// <param name="principalVariation"> The Principal variation (PV) is a sequence of moves is considered best and therefore expect to be played. This list of moves is collected during the alpha beta search. http://chessprogramming.wikispaces.com/Principal+variation </param>
        /// <param name="analysisParentBranch"> When move analysis is enabled, a tree of search moves is collected in this variable, which can be viewed in the GUI. </param>
        /// <returns> The best move for the player. </returns>
        private int Quiesce(
            Player player,
            int ply,
            int variableDepth,
            int alpha,
            int beta,
            Move parentMove,
            Moves principalVariation,
            Moves analysisParentBranch)
        {
            // Gather some stats
            if (variableDepth < this.MaxQuiesenceDepthReached)
            {
                this.MaxQuiesenceDepthReached = variableDepth;
            }

            // Calculate the score
            this.Evaluations++;
            int standPat = player.Score;
            if (standPat > 1000000 || standPat < -1000000)
            {
                // TODO Unit test that negative depths produce score that reduce as they get deeper. The purpose here is to make deeper checks score less than shallower ones.
                // TODO Investigate whether reduced mate scores are constantly reduced when going in and out of hashtable.
                standPat /= this.MaxSearchDepth - ply;
            }

            if (standPat >= beta)
            {
                Comment(parentMove, "(Q:PAT-CUT) ");
                return beta;
            }

            if (alpha < standPat)
            {
                // Comment(parentMove, "(Q:PAT-ALPHA) ");
                alpha = standPat;
            }

            // Disable Quiescense is feature not enabled.
            if (!Game.EnableQuiescense)
            {
                return standPat;
            }

            // Generate moves - Captures and promotions only
            Moves movesPossible = new Moves();
            player.GenerateLazyMoves(movesPossible, Moves.MoveListNames.CapturesPromotions);

            // Get move at same ply from previous iteration's principal variation.
            int indexPv = this.MaxSearchDepth - ply;
            Move movePv = null;
            if (indexPv < this.LastPrincipalVariation.Count)
            {
                movePv = this.LastPrincipalVariation[indexPv];
            }

            // Sort moves
            this.SortBestMoves(movesPossible, 0, movePv, null, null, null, null, null, player);

            Moves movesPv = new Moves();

            foreach (Move move in movesPossible)
            {
                if (move.Score < 0)
                {
                    // Losing capture from SEE, so skip this move.
                    continue;
                }

                // Make capture, but skip illegal moves
                Move moveThis = move.Piece.Move(move.Name, move.To);
                if (player.IsInCheck)
                {
                    Move.Undo(moveThis);
                    continue;
                }

                this.PositionsSearchedThisTurn++;
                this.PositionsSearchedThisIteration++;

                // If this is the deepest stage of iterative deepening, then capture move analysis data.
                if (Game.CaptureMoveAnalysisData && this.SearchDepth == this.MaxSearchDepth)
                {
                    // Add moves to post-move analysis tree, if option set by user
                    if (analysisParentBranch != null)
                    {
                        analysisParentBranch.Add(moveThis);
                    }

                    moveThis.Moves = new Moves();
                }

                moveThis.Score =
                    -this.Quiesce(
                        player.OpposingPlayer,
                        ply - 1,
                        variableDepth - 1,
                        -beta,
                        -alpha,
                        parentMove,
                        movesPv,
                        moveThis.Moves);

                // Undo the capture move
                Move.Undo(moveThis);

                if (moveThis.Score >= beta)
                {
                    Comment(parentMove, "(Q:CUT) ");

                    return beta;
                }

                if (moveThis.Score > alpha)
                {
                    alpha = moveThis.Score;

                    // Comment(parentMove, "(Q:^ALPHA) ");

                    // Collect the Prinicial Variation
                    lock (principalVariation)
                    {
                        principalVariation.Clear();
                        principalVariation.Add(moveThis);
                        foreach (Move moveCopy in movesPv)
                        {
                            principalVariation.Add(moveCopy);
                        }
                    }
                }
            }

            return alpha;
        }

        /// <summary>
        ///   Performs a Static Exchange Evaluation to determine the value of a move after all possible re-captures are resolved. http://chessprogramming.wikispaces.com/Static+Exchange+Evaluation
        /// </summary>
        /// <param name="moveMade"> Move to be evaluated </param>
        /// <returns> The see. </returns>
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
        ///   Sorts moves so that the best moves are first
        /// </summary>
        /// <param name="movesToSort"> List of moves to be sorted. </param>
        /// <param name="variableDepth"> Depth which starts at one and INCREASES as the search deepens. Its value is altered by search extension and reductions. Quiesence starts at depth 0. http://chessprogramming.wikispaces.com/Depth </param>
        /// <param name="movePv"> Move from previous iteration's principal variation. </param>
        /// <param name="moveHash"> // Best move from hash table. </param>
        /// <param name="moveKillerA"> Best killer move from this ply. </param>
        /// <param name="moveKillerA2"> Second best killer move from this ply. </param>
        /// <param name="moveKillerB"> Best killer move from previous ply. </param>
        /// <param name="moveKillerB2"> Second best killer move from previous ply. </param>
        /// <param name="player"> The player. </param>
        private void SortBestMoves(
            Moves movesToSort,
            int variableDepth,
            Move movePv,
            Move moveHash,
            Move moveKillerA,
            Move moveKillerA2,
            Move moveKillerB,
            Move moveKillerB2,
            Player player)
        {
            foreach (Move movex in movesToSort)
            {
                this.AssignMoveOrderScore(
                    movex, variableDepth, movePv, moveHash, moveKillerA, moveKillerA2, moveKillerB, moveKillerB2, player);
            }

            movesToSort.SortByScore();
        }

        #endregion
    }
}