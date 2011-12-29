// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Search.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   Performs the central move-selection logic for SharpChess, referred to as "the Search".
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
    using System;

    /// <summary>
    /// Performs the central move-selection logic for SharpChess, referred to as the Search.
    ///  http://chessprogramming.wikispaces.com/Search
    /// </summary>
    public class Search
    {
        /// <summary>
        /// The delegatetype Search event.
        /// </summary>
        public delegate void SearchEvent();

        /// <summary>
        ///   The move considered.
        /// </summary>
        public event SearchEvent SearchMoveConsideredEvent;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Search" /> class.
        /// </summary>
        public Search(Brain brain)
        {
            this.myBrain = brain;
            this.MaxSearchDepth = 32;
        }

        /// <summary>
        ///   The ma x_ score.
        /// </summary>
        private const int MaxScore = Int32.MaxValue;

        /// <summary>
        ///   The mi n_ score.
        /// </summary>
        private const int MinScore = Int32.MinValue + 1;

        /// <summary>
        ///   The m_int min search depth.
        /// </summary>
        private const int MinSearchDepth = 1;

        /// <summary>
        ///   The m_int minimum search depth.
        /// </summary>
        private const int MinimumSearchDepth = 1;

        /// <summary>
        ///   The m_bln force immediate move.
        /// </summary>
        private bool forceExitWithMove;

        /// <summary>
        ///   Gets CurrentMoveSearched.
        /// </summary>
        public Move CurrentMoveSearched { get; private set; }

        /// <summary>
        ///   Gets the player's brain performing this search.
        /// </summary>
        public Brain myBrain { get; private set; }

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
                return this.Evaluations / this.myBrain.ThinkingTimeElpased.TotalSeconds;
            }
        }

        /// <summary>
        ///   Gets TotalPositionsToSearch.
        /// </summary>
        public int TotalPositionsToSearch { get; protected set; }

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
                return this.PositionsSearched / Math.Max(Convert.ToInt32(this.myBrain.ThinkingTimeElpased.TotalSeconds), 1);
            }
        }

        /// <summary>
        ///   The m_tsn thinking time max allowed.
        /// </summary>
        public TimeSpan SearchTimeMaxAllowed { get; private set; }

        /// <summary>
        ///   Gets PositionsSearched.
        /// </summary>
        public int PositionsSearched { get; protected set; }

        /// <summary>
        ///   Gets SearchDepth.
        /// </summary>
        public int SearchDepth { get; private set; }

        /// <summary>
        ///   Gets SearchPositionNo.
        /// </summary>
        public int SearchPositionNo { get; protected set; }

        /// <summary>
        /// The force search to exit with an immediate move.
        /// </summary>
        public void SearchForceExitWithMove()
        {
            this.forceExitWithMove = true;
        }


        public int IterativeDeepening(Player player, Moves principalVariationMoves, 
            TimeSpan searchTimeAllotted, TimeSpan searchTimeMaxAllowed)
        {
            // A new deeper ply of search will only be started, if the cutoff time hasnt been reached yet.
            TimeSpan searchTimeCutoff = new TimeSpan(searchTimeAllotted.Ticks / 3);

            this.SearchTimeMaxAllowed = searchTimeMaxAllowed;

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

                // score = AlphaBeta(player, m_intSearchDepth, m_intSearchDepth, MIN_SCORE, MAX_SCORE, null, movesPV, intScore);

                if (!Game.IsInAnalyseMode && Game.ClockFixedTimePerMove.TotalSeconds <= 0 && !this.myBrain.IsPondering
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
        /// The aspirate.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="principalVariationMoves">
        /// The moves p v_ parent.
        /// </param>
        /// <param name="intLastIterationsScore">
        /// The int last iteractions score.
        /// </param>
        /// <returns>
        /// The aspirate.
        /// </returns>
        private int Aspirate(Player player, Moves principalVariationMoves, int intLastIterationsScore, Moves analysisParentBranch)
        {
            int alpha = MinScore; // Score of the best move found so far
            int beta = MaxScore; // Score of the best move found by the opponent
            int val = alpha;

            for (int intAttempt = 0; intAttempt < 3; intAttempt++)
            {
                switch (intAttempt)
                {
                    case 0:
                        alpha = intLastIterationsScore - 500;
                        beta = intLastIterationsScore + 500;
                        break;

                    case 1:
                        alpha = intLastIterationsScore - 2000;
                        beta = intLastIterationsScore + 2000;
                        break;

                    case 2:
                        alpha = MinScore;
                        beta = MaxScore;
                        break;
                }

                val = this.AlphaBeta(player, this.SearchDepth, this.SearchDepth, alpha, beta, null, principalVariationMoves, 0, analysisParentBranch);
                if (val > alpha && val < beta)
                {
                    break;
                }
            }

            return val;
        }

        private int AlphaBeta(
            Player player,
            int ply,
            int depth,
            int alpha,
            int beta,
            Move parentMove,
            Moves principalVariationMoves,
            int intTotalExtensions,
            Moves analysisParentBranch
            )
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
                    if (this.MaxSearchDepth - depth > 0)
                    {
                        val /= this.MaxSearchDepth - depth;
                    }
                }

                principalVariationMoves.Clear();
                if (HashTable.ProbeForBestMove(Board.HashCodeA, Board.HashCodeB, player.Colour) != null)
                {
                    principalVariationMoves.Add(HashTable.ProbeForBestMove(Board.HashCodeA, Board.HashCodeB, player.Colour));
                }

                return val;
            }

            if (intTotalExtensions > this.MaxExtensions)
            {
                this.MaxExtensions = intTotalExtensions;
            }

            // Generate moves
            Moves movesPossible = new Moves();
            blnAllMovesWereGenerated = depth > 0; // || blnIsInCheck); 
            if (blnAllMovesWereGenerated)
            {
                player.GenerateLazyMoves(depth, movesPossible, Moves.MoveListNames.All, null);
            }
            else
            {
                // Captures only
                player.GenerateLazyMoves(
                    depth, movesPossible, Moves.MoveListNames.CapturesChecksPromotions, parentMove.To);
            }

            // Depth <=0 means we're into Quiescence searching
            if (depth <= 0)
            {
                if (depth < this.MaxQuiesenceDepthReached)
                {
                    this.MaxQuiesenceDepthReached = depth;
                }

                intScoreAtEntry = val = -player.OpposingPlayer.Score;
                this.Evaluations++;

                if (val > 1000000 || val < -1000000)
                {
                    val /= this.MaxSearchDepth - depth;
                }

                // If there are no more moves, then return val.
                // TODO remove checking moves from count in quiesence only
                if (movesPossible.Count == 0)
                {
                    return val;
                }
            }

            // Adaptive Null-move forward pruning
            int r = depth > 6 ? 3 : 2;
            if (depth > (r + 1) && parentMove != null && parentMove.Name != Move.MoveNames.NullMove
                && Game.Stage != Game.GameStageNames.End && !blnIsInCheck)
            {
                Move moveNull = new Move(Game.TurnNo, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 0);
                val =
                    -this.AlphaBeta(
                        player.OpposingPlayer,
                        ply - 1,
                        depth - r - 1,
                        -beta,
                        -beta + 1,
                        moveNull,
                        movesPV,
                        intTotalExtensions,
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
                else if (moveThis.Piece.Name == Piece.PieceNames.Pawn
                         &&
                         (moveThis.Piece.Player.Colour == Player.PlayerColourNames.White && moveThis.To.Rank == 6
                          || moveThis.Piece.Player.Colour == Player.PlayerColourNames.Black && moveThis.To.Rank == 1))
                {
                    // Pawn push to 7th rank
                    intExtension = 1;
                }

                // Reductions
                // Only reduce if this move hasn't been extended.
                if (intExtension == 0)
                {
                    if (depth > 2 && !blnIsInCheck && moveThis.PieceCaptured == null && !moveThis.IsEnemyInCheck)
                    {
                        int[] margin = { 0, 0, 0, 5000, 5000, 7000, 7000, 9000, 9000, 15000, 15000, 15000, 15000, 15000, 15000, 15000, 15000, 15000 };

                        // int intLazyEval = this.TotalPieceValue - this.OtherPlayer.TotalPieceValue;
                        int intLazyEval = player.Score;
                        if (alpha > intLazyEval + margin[depth])
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

                                // case 4:
                                if (moveThis.PieceCaptured == null && !move.IsEnemyInCheck)
                                {
                                    int intLazyEval = player.Score;

                                    switch (depth)
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
                        -this.AlphaBeta(
                            player.OpposingPlayer,
                            ply - 1,
                            (depth + intExtension) - 1,
                            -alpha - 1,
                            -alpha,
                            moveThis,
                            movesPV,
                            intTotalExtensions + intExtension,
                            moveThis.Moves);
                    if ((val > alpha) && (val < beta))
                    {
                        // fail
                        if (Game.CaptureMoveAnalysisData && this.SearchDepth == this.MaxSearchDepth && parentMove != null && parentMove.Name != Move.MoveNames.NullMove)
                        {
                            moveThis.Moves.Clear();
                        }
                        val =
                            -this.AlphaBeta(
                                player.OpposingPlayer,
                                ply - 1,
                                (depth + intExtension) - 1,
                                -beta,
                                -alpha,
                                moveThis,
                                movesPV,
                                intTotalExtensions + intExtension,
                                moveThis.Moves);
                    }
                }
                else
                {
                    val =
                        -this.AlphaBeta(
                            player.OpposingPlayer,
                            ply - 1,
                            (depth + intExtension) - 1,
                            -beta,
                            -alpha,
                            moveThis,
                            movesPV,
                            intTotalExtensions + intExtension,
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
                    History.Record(player.Colour, moveThis.From.Ordinal, moveThis.To.Ordinal, depth * depth);

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

                if (!Game.IsInAnalyseMode && !this.myBrain.IsPondering && this.SearchDepth > MinimumSearchDepth
                    && (DateTime.Now - player.Clock.TurnStartTime) > this.SearchTimeMaxAllowed)
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
        /// Evaluates and assigns a move order score to a move
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
                else if (move.From.Piece.Name == Piece.PieceNames.Rook && move.To.Piece.Name == Piece.PieceNames.Rook)
                {
                    move.Score += 99998;
                    return;
                }
                else if (move.From.Piece.Name == Piece.PieceNames.Knight && move.To.Piece.Name == Piece.PieceNames.Bishop)
                {
                    move.Score += 99997;
                    return;
                }
                else if (move.From.Piece.Name == Piece.PieceNames.Bishop
                         && move.To.Piece.Name == Piece.PieceNames.Bishop)
                {
                    move.Score += 99996;
                    return;
                }
                else if (move.From.Piece.Name == Piece.PieceNames.Bishop
                         && move.To.Piece.Name == Piece.PieceNames.Knight)
                {
                    move.Score += 99995;
                    return;
                }
                else if (move.From.Piece.Name == Piece.PieceNames.Pawn
                         &&
                         (move.Name == Move.MoveNames.EnPassent
                          &&
                          Board.GetPiece(move.To.Ordinal - player.PawnForwardOffset).Name
                          == Piece.PieceNames.Pawn || move.To.Piece.Name == Piece.PieceNames.Pawn))
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
        /// Performs a Static Exchange Evaluation to determine the value of a move after all possible re-captures are resolved.
        /// </summary>
        /// <param name="moveMade">
        /// move to be evaluated
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
            for (int intIndex = 0; ; intIndex++)
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
        /// </param>
        /// <param name="moveHash">
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
        /// <param name="moveHash">
        /// Best move from hash table.
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

        /// <summary>
        /// The perft.
        /// </summary>
        /// <param name="targetDepth">
        /// The target depth.
        /// </param>
        public void Perft(Player player, int targetDepth)
        {
            this.PositionsSearched = 0;
            this.Perft_Ply(player, targetDepth);
        }

        /// <summary>
        /// The perft_ ply.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="depth">
        /// The depth.
        /// </param>
        private void Perft_Ply(Player player, int depth)
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
                this.Perft_Ply(player.OpposingPlayer, depth - 1);

                Move.Undo(moveUndo);
            }
        }
    }
}