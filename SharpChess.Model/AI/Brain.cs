// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Brain.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   AI for the computer player.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region License

// SharpChess
// Copyright (C) 2012 SharpChess.com
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

namespace SharpChess.Model.AI
{
    #region Using

    using System;
    using System.Diagnostics;
    using System.Threading;

    using ThreadState = System.Threading.ThreadState;

    #endregion

    /// <summary>
    ///   AI for the computer player.
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

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="Brain" /> class.
        /// </summary>
        /// <param name="player"> The player. </param>
        public Brain(Player player)
        {
            this.MyPlayer = player;
            this.Search = new Search(this);
            this.Search.SearchMoveConsideredEvent += this.SearchMoveConsideredHandler;
        }

        #endregion

        #region Delegates

        /// <summary>
        ///   The delegatetype Brain event.
        /// </summary>
        public delegate void BrainEvent();

        #endregion

        #region Public Events

        /// <summary>
        ///   The move considered.
        /// </summary>
        public event BrainEvent MoveConsideredEvent;

        /// <summary>
        ///   The ready to make move.
        /// </summary>
        public event BrainEvent ReadyToMakeMoveEvent;

        /// <summary>
        ///   The thinking beginning.
        /// </summary>
        public event BrainEvent ThinkingBeginningEvent;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether to use random opening moves.
        /// </summary>
        public static bool UseRandomOpeningMoves { get; set; }

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
        ///   Gets the Search algorithm. http://chessprogramming.wikispaces.com/Search
        /// </summary>
        public Search Search { get; private set; }

        /// <summary>
        ///   Gets ThinkingTimeAllotted.
        /// </summary>
        public TimeSpan ThinkingTimeAllotted { get; private set; }

        /// <summary>
        ///   Gets ThinkingTimeElpased.
        /// </summary>
        public TimeSpan ThinkingTimeElpased
        {
            get
            {
                return DateTime.Now - this.MyPlayer.Clock.TurnStartTime;
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

        #region Properties

        /// <summary>
        ///   Gets or sets the player whose brain this is.
        /// </summary>
        private Player MyPlayer { get; set; }

        /// <summary>
        ///   Gets or sets ThinkingMaxAllowed
        /// </summary>
        private TimeSpan ThinkingTimeMaxAllowed { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///   The abort thinking.
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
        ///   The force immediate move.
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
        ///   The start pondering.
        /// </summary>
        public void StartPondering()
        {
            if (this.MyPlayer.Intellegence == Player.PlayerIntellegenceNames.Computer
                && this.MyPlayer.OpposingPlayer.Intellegence == Player.PlayerIntellegenceNames.Computer)
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

                if (!this.IsThinking && !this.MyPlayer.OpposingPlayer.Brain.IsThinking
                    && this.MyPlayer.OpposingPlayer.Intellegence == Player.PlayerIntellegenceNames.Computer
                    && Game.PlayerToPlay == this.MyPlayer)
                {
                    this.IsPondering = true;
                    this.StartThinking();
                }
            }
        }

        /// <summary>
        ///   The start thinking.
        /// </summary>
        public void StartThinking()
        {
            // Bail out if unable to move
            if (!this.MyPlayer.CanMove)
            {
                return;
            }

            // Send draw result is playing WinBoard
            if (WinBoard.Active && this.MyPlayer.Intellegence == Player.PlayerIntellegenceNames.Computer)
            {
                if (this.MyPlayer.CanClaimThreeMoveRepetitionDraw)
                {
                    WinBoard.SendDrawByRepetition();
                    return;
                }

                if (this.MyPlayer.CanClaimFiftyMoveDraw)
                {
                    WinBoard.SendDrawByFiftyMoveRule();
                    return;
                }

                if (this.MyPlayer.CanClaimInsufficientMaterialDraw)
                {
                    WinBoard.SendDrawByFiftyMoveRule();
                    return;
                }
            }

            this.threadThought = new Thread(this.Think);
            this.threadThought.Name = (++Game.ThreadCounter).ToString();

            this.ThinkingBeginningEvent();
            this.threadThought.Priority = ThreadPriority.Normal;

            this.threadThought.Start();
        }

        /// <summary>
        ///   The stop pondering.
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
        ///   Instruct the computer to think and make its next move.
        /// </summary>
        public void Think()
        {
            // Determine the best move available for "this" player instance, from the current board position.
            Debug.WriteLine(
                string.Format(
                    "Thread {0} is " + (this.IsPondering ? "pondering" : "thinking"), Thread.CurrentThread.Name));

            Player player = this.MyPlayer;

            // Set the player, whose move is to be computed, to "this" player object instance
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
                    this.ThinkingTimeMaxAllowed = Game.ClockFixedTimePerMove;
                }
                else if (Game.ClockIncrementPerMove.TotalSeconds > 0)
                {
                    // Incremental clock
                    this.ThinkingTimeAllotted =
                        new TimeSpan(
                            Game.ClockIncrementPerMove.Ticks
                            +
                            ((((Game.ClockIncrementPerMove.Ticks * Game.MoveNo)
                               + (Game.ClockTime.Ticks * Math.Min(Game.MoveNo, 40) / 40))
                              - this.MyPlayer.Clock.TimeElapsed.Ticks) / 3));

                    // Make sure we never think for less than half the "Increment" time
                    this.ThinkingTimeAllotted =
                        new TimeSpan(
                            Math.Max(this.ThinkingTimeAllotted.Ticks, (Game.ClockIncrementPerMove.Ticks / 2) + 1));
                    this.ThinkingTimeMaxAllowed = Game.ClockFixedTimePerMove;
                }
                else if (Game.ClockMaxMoves == 0 && Game.ClockIncrementPerMove.TotalSeconds <= 0)
                {
                    // Fixed game time
                    this.ThinkingTimeAllotted = new TimeSpan(this.MyPlayer.Clock.TimeRemaining.Ticks / 30);
                    this.ThinkingTimeMaxAllowed = Game.ClockFixedTimePerMove;
                }
                else
                {
                    // Conventional n moves in x minutes time
                    this.ThinkingTimeAllotted =
                        new TimeSpan(this.MyPlayer.Clock.TimeRemaining.Ticks / this.MyPlayer.Clock.MovesRemaining);

                    this.ThinkingTimeMaxAllowed = new TimeSpan(this.MyPlayer.Clock.TimeRemaining.Ticks)
                                                  - new TimeSpan(0, 0, 0, 0, 100);
                }

                // Minimum of 100 milli-second thinking time
                if (this.ThinkingTimeAllotted.TotalMilliseconds < 100)
                {
                    this.ThinkingTimeAllotted = new TimeSpan(0, 0, 0, 100);
                }

                // The computer only stops "thinking" when it has finished a full ply of thought, 
                // UNLESS m_tsnThinkingTimeMaxAllowed is exceeded, or clock runs out, then it stops right away.
                if (Game.ClockFixedTimePerMove.TotalSeconds > 0)
                {
                    // Fixed time per move
                    this.ThinkingTimeMaxAllowed = Game.ClockFixedTimePerMove - new TimeSpan(0, 0, 0, 0, 100);
                }
                else
                {
                    // Variable time per move
                    this.ThinkingTimeMaxAllowed =
                        new TimeSpan(
                            Math.Min(
                                this.ThinkingTimeAllotted.Ticks * 2,
                                this.MyPlayer.Clock.TimeRemaining.Ticks - (new TimeSpan(0, 0, 0, 0, 100)).Ticks));
                }

                if (Game.IsInAnalyseMode)
                {
                    HashTable.Clear();
                    HashTableCheck.Clear();
                    HashTablePawn.Clear();
                    HistoryHeuristic.Clear();
                }
                else
                {
                    if (this.MyPlayer.CanClaimMoveRepetitionDraw(2))
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
                    HashTablePawn.ResetStats();

                    // And finally a hash table that stores the positional score of just the pawns.
                    HistoryHeuristic.Clear(); // Clear down the History Heuristic info, at the start of each move.
                }

                if (!this.IsPondering)
                {
                    this.MyPlayer.Clock.Start();
                }

                int score = this.Search.IterativeDeepeningSearch(
                    this.MyPlayer, this.PrincipalVariation, this.ThinkingTimeAllotted, this.ThinkingTimeMaxAllowed);
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
            WinBoard.SendMoveTime(DateTime.Now - this.MyPlayer.Clock.TurnStartTime);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The search move considered handler.
        /// </summary>
        private void SearchMoveConsideredHandler()
        {
            if (this.MoveConsideredEvent != null)
            {
                this.MoveConsideredEvent();
            }
        }

        #endregion
    }
}