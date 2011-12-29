// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Brain.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   AI for the computer player.
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
    using System.Diagnostics;
    using System.Threading;

    using ThreadState = System.Threading.ThreadState;

    /// <summary>
    /// AI for the computer player.
    /// </summary>
    public class Brain
    {
        #region Constants and Fields

        /// <summary>
        ///   The m_ulong pondering hash code a.
        /// </summary>
        private static ulong ponderingHashCodeA;

        /// <summary>
        ///   The m_ulong pondering hash code b.
        /// </summary>
        private static ulong ponderingHashCodeB;

        /// <summary>
        ///   The m_thread thought.
        /// </summary>
        private Thread threadThought;

        /// <summary>
        /// Player whose brain this is.
        /// </summary>
        private Player myPlayer { get; set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Brain" /> class.
        /// </summary>
        public Brain(Player player)
        {
            this.myPlayer = player;
            this.Search = new Search(this);
            this.Search.SearchMoveConsideredEvent += this.SearchMoveConsideredHandler;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The delegatetype Brain event.
        /// </summary>
        public delegate void BrainEvent();

        #endregion

        #region Public Events

        /// <summary>
        ///   The ready to make move.
        /// </summary>
        public event BrainEvent ReadyToMakeMoveEvent;

        /// <summary>
        ///   The move considered.
        /// </summary>
        public event BrainEvent MoveConsideredEvent;

        /// <summary>
        ///   The thinking beginning.
        /// </summary>
        public event BrainEvent ThinkingBeginningEvent;

        #endregion

        private void SearchMoveConsideredHandler()
        {
            if (this.MoveConsideredEvent != null)
            {
                this.MoveConsideredEvent();
            }
        }

        #region Public Properties

        /// <summary>
        /// Gets the Search algorithm.
        /// http://chessprogramming.wikispaces.com/Search
        /// </summary>
        public Search Search { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether IsPondering.
        /// </summary>
        public bool IsPondering { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether IsThinking.
        /// </summary>
        public bool IsThinking
        {
            get
            {
                return this.threadThought != null;
            }
        }

        /// <summary>
        ///   Gets PrincipalVariation.
        /// </summary>
        public Moves PrincipalVariation { get; private set; }

        /// <summary>
        ///   Gets PrincipalVariationText.
        /// </summary>
        public string PrincipalVariationText
        {
            get
            {
                string strText = string.Empty;
                if (this.PrincipalVariation != null)
                {
                    for (int intIndex = 0; intIndex < this.PrincipalVariation.Count; intIndex++)
                    {
                        if (intIndex < this.PrincipalVariation.Count)
                        {
                            Move move = this.PrincipalVariation[intIndex];
                            if (move != null)
                            {
                                strText += (move.Piece.Name == Piece.PieceNames.Pawn
                                                ? string.Empty
                                                : move.Piece.Abbreviation) + move.From.Name
                                           + (move.PieceCaptured != null ? "x" : string.Empty) + move.To.Name + " ";
                            }
                        }
                    }
                }

                return strText;
            }
        }

        /// <summary>
        ///   Gets ThinkingTimeAllotted.
        /// </summary>
        public TimeSpan ThinkingTimeAllotted { get; private set; }

        /// <summary>
        ///   Gets ThinkingMaxAllowed
        /// </summary>
        public TimeSpan ThinkingTimeMaxAllowed { get; private set; }

        /// <summary>
        ///   Gets ThinkingTimeElpased.
        /// </summary>
        public TimeSpan ThinkingTimeElpased
        {
            get
            {
                return DateTime.Now - this.myPlayer.Clock.TurnStartTime;
            }
        }

        /// <summary>
        ///   Gets ThinkingTimeRemaining.
        /// </summary>
        public TimeSpan ThinkingTimeRemaining
        {
            get
            {
                return this.ThinkingTimeAllotted - this.ThinkingTimeElpased;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The abort thinking.
        /// </summary>
        public void AbortThinking()
        {
            if (this.threadThought != null && this.threadThought.ThreadState == ThreadState.Running)
            {
                this.threadThought.Abort();
                this.threadThought.Join();
                this.threadThought = null;
            }
        }

        /// <summary>
        /// The force immediate move.
        /// </summary>
        public void ForceImmediateMove()
        {
            if (this.IsThinking)
            {
                this.Search.SearchForceExitWithMove();
                while (this.threadThought != null)
                {
                    Thread.Sleep(50);
                }
            }
        }

        /// <summary>
        /// The start pondering.
        /// </summary>
        public void StartPondering()
        {
            if (this.myPlayer.Intellegence == Player.PlayerIntellegenceNames.Computer
                && this.myPlayer.OpposingPlayer.Intellegence == Player.PlayerIntellegenceNames.Computer)
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
                if (ponderingHashCodeA != Board.HashCodeA || ponderingHashCodeB != Board.HashCodeB)
                {
                    ponderingHashCodeA = Board.HashCodeA;
                    ponderingHashCodeB = Board.HashCodeB;
                }

                if (!this.IsThinking && !this.myPlayer.OpposingPlayer.Brain.IsThinking
                    && this.myPlayer.OpposingPlayer.Intellegence == Player.PlayerIntellegenceNames.Computer && Game.PlayerToPlay == this.myPlayer)
                {
                    this.IsPondering = true;
                    this.StartThinking();
                }
            }
        }

        /// <summary>
        /// The start thinking.
        /// </summary>
        public void StartThinking()
        {
            // Bail out if unable to move
            if (!this.myPlayer.CanMove)
            {
                return;
            }

            // Send draw result is playing WinBoard
            if (WinBoard.Active && this.myPlayer.Intellegence == Player.PlayerIntellegenceNames.Computer)
            {
                if (this.myPlayer.CanClaimThreeMoveRepetitionDraw)
                {
                    WinBoard.SendDrawByRepetition();
                    return;
                }
                else if (this.myPlayer.CanClaimFiftyMoveDraw)
                {
                    WinBoard.SendDrawByFiftyMoveRule();
                    return;
                }
                else if (this.myPlayer.CanClaimInsufficientMaterialDraw)
                {
                    WinBoard.SendDrawByFiftyMoveRule();
                    return;
                }
            }

            this.threadThought = new Thread(this.Think);
            this.threadThought.Name = (++Game.ThreadCounter).ToString();

            this.ThinkingBeginningEvent();
            if (this.IsPondering)
            {
                // m_threadThought.Priority = System.Threading.ThreadPriority.BelowNormal;
                this.threadThought.Priority = ThreadPriority.Normal;
            }
            else
            {
                this.threadThought.Priority = ThreadPriority.Normal;
            }

            this.threadThought.Start();
        }

        /// <summary>
        /// The stop pondering.
        /// </summary>
        public void StopPondering()
        {
            if (this.IsPondering)
            {
                this.AbortThinking();
                this.IsPondering = false;
            }
        }

        /// <summary>
        /// The think.
        /// </summary>
        /// <exception cref="ForceImmediateMoveException">
        /// </exception>
        /// <exception cref="ForceImmediateMoveException">
        /// </exception>
        public void Think()
        {
            // Determine the best move available for "this" player instance, from the current board position.
            Debug.WriteLine(
                string.Format(
                    "Thread {0} is " + (this.IsPondering ? "pondering" : "thinking"), Thread.CurrentThread.Name));

            Player player = this.myPlayer; // Set the player, whose move is to be computed, to "this" player object instance
            this.PrincipalVariation = new Moves(); // Best moves line (Principal Variation) found so far.

            // TimeSpan tsnTimePondered = new TimeSpan();
            int intTurnNo = Game.TurnNo;

            // Set whether to build a post-analysis tree of positions searched
            try
            {
                if (!this.IsPondering && !Game.IsInAnalyseMode)
                {
                    // Query Simple Opening Book
                    if (Game.UseRandomOpeningMoves)
                    {
                        Move moveBook;
                        if ((moveBook = OpeningBookSimple.SuggestRandomMove(player)) != null)
                        {
                            this.PrincipalVariation.Add(moveBook);
                            this.MoveConsideredEvent();
                            throw new ForceImmediateMoveException();
                        }
                    }

                    /* Query Best Opening Book
                        if ((m_moveBest = OpeningBook.SearchForGoodMove(Board.HashCodeA, Board.HashCodeB, this.Colour) )!=null) 
                        {
                            m_moveCurrent = m_moveBest;
                            this.MoveConsidered();
                            throw new ForceImmediateMoveException();
                        }
                    */
                }

                // Time allowed for this player to think
                if (Game.ClockFixedTimePerMove.TotalSeconds > 0)
                {
                    // Absolute fixed time per move. No time is carried over from one move to the next.
                    this.ThinkingTimeAllotted = Game.ClockFixedTimePerMove;
                }
                else if (Game.ClockIncrementPerMove.TotalSeconds > 0)
                {
                    // Incremental clock
                    this.ThinkingTimeAllotted =
                        new TimeSpan(
                            Game.ClockIncrementPerMove.Ticks
                            +
                            ((Game.ClockIncrementPerMove.Ticks * Game.MoveNo
                              + Game.ClockTime.Ticks * Math.Min(Game.MoveNo, 40) / 40) - this.myPlayer.Clock.TimeElapsed.Ticks)
                            / 3);

                    // Make sure we never think for less than half the "Increment" time
                    this.ThinkingTimeAllotted =
                        new TimeSpan(
                            Math.Max(this.ThinkingTimeAllotted.Ticks, Game.ClockIncrementPerMove.Ticks / 2 + 1));
                }
                else if (Game.ClockMaxMoves == 0 && Game.ClockIncrementPerMove.TotalSeconds == 0)
                {
                    // Fixed game time
                    this.ThinkingTimeAllotted = new TimeSpan(this.myPlayer.Clock.TimeRemaining.Ticks / 30);
                }
                else
                {
                    // Conventional n moves in x minutes time
                    this.ThinkingTimeAllotted = new TimeSpan(this.myPlayer.Clock.TimeRemaining.Ticks / this.myPlayer.Clock.MovesRemaining);
                }

                // Minimum of 1 second thinking time
                if (this.ThinkingTimeAllotted.TotalSeconds < 1)
                {
                    this.ThinkingTimeAllotted = new TimeSpan(0, 0, 1);
                }

                // The computer only stops "thinking" when it has finished a full ply of thought, 
                // UNLESS m_tsnThinkingTimeMaxAllowed is exceeded, or clock runs out, then it stops right away.
                if (Game.ClockFixedTimePerMove.TotalSeconds > 0)
                {
                    // Fixed time per move
                    this.ThinkingTimeMaxAllowed = Game.ClockFixedTimePerMove;
                }
                else
                {
                    // Variable time per move
                    this.ThinkingTimeMaxAllowed =
                        new TimeSpan(
                            Math.Min(
                                this.ThinkingTimeAllotted.Ticks * 2,
                                this.myPlayer.Clock.TimeRemaining.Ticks - (new TimeSpan(0, 0, 0, 2)).Ticks));
                }

                // Minimum of 2 seconds thinking time
                if (this.ThinkingTimeMaxAllowed.TotalSeconds < 2)
                {
                    this.ThinkingTimeMaxAllowed = new TimeSpan(0, 0, 2);
                }

                // Total number of times the evaluation function has been called (May be less than PositonsSearched if hashtable works well)
                if (Game.IsInAnalyseMode)
                {
                    HashTable.Clear();
                    HashTableCheck.Clear();
                    HashTablePawnKing.Clear();
                    History.Clear();
                }
                else
                {
                    if (this.myPlayer.CanClaimMoveRepetitionDraw(2))
                    {
                        // See if' we're in a 2 move repetition position, and if so, clear the hashtable, as old hashtable entries corrupt 3MR detection
                        HashTable.Clear();
                    }
                    else
                    {
                        HashTable.ResetStats(); // Reset the main hash table hit stats
                    }

                    HashTableCheck.ResetStats();

                    // We also have a hash table in which we just store the check status for both players
                    HashTablePawnKing.ResetStats();

                    // And finally a hash table that stores the positional score of just the pawns.
                    History.Clear(); // Clear down the History Heuristic info, at the start of each move.
                }

                if (!this.IsPondering)
                {
                    this.myPlayer.Clock.Start();
                }

                int score = this.Search.IterativeDeepening(this.myPlayer, this.PrincipalVariation, this.ThinkingTimeAllotted, this.ThinkingTimeMaxAllowed);

                WinBoard.SendThinking(
                    this.Search.SearchDepth,
                    score,
                    DateTime.Now - player.Clock.TurnStartTime,
                    this.Search.PositionsSearched,
                    this.PrincipalVariationText);
            }
            catch (ForceImmediateMoveException x)
            {
                // Undo any moves made during thinking
                Debug.WriteLine(x.ToString());
                while (Game.TurnNo > intTurnNo)
                {
                    Move.Undo(Game.MoveHistory.Last);
                }
            }

            if (this.MoveConsideredEvent != null)
            {
                this.MoveConsideredEvent();
            }

            Debug.WriteLine(
                string.Format(
                    "Thread {0} is ending " + (this.IsPondering ? "pondering" : "thinking"), Thread.CurrentThread.Name));

            this.threadThought = null;
            if (this.MoveConsideredEvent != null && !this.IsPondering)
            {
                this.ReadyToMakeMoveEvent();
            }

            this.IsPondering = false;

            // Send total elapsed time to generate this move.
            WinBoard.SendMoveTime(DateTime.Now - this.myPlayer.Clock.TurnStartTime);
        }

        #endregion


    }
}