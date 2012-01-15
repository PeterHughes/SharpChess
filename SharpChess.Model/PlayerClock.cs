// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerClock.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Player chess clock.
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

namespace SharpChess.Model
{
    #region Using

    using System;

    #endregion

    /// <summary>
    /// Player chess clock.
    /// </summary>
    public class PlayerClock
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerClock"/> class.
        /// </summary>
        public PlayerClock()
        {
            this.Reset();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value that is used to automatically reset the "number of moves remaining" back to "Clock Max Moves", 
        /// when the number of player moves exceeds "ClockMaxMoves". e.g. The clock is set at 120 moves in 60 minutes. 
        /// At the beginning of the game we're in Control Period 1. If the player gets to move 121 then we move into Control Period 2.
        /// So the Control Period increments by 1, every 120 moves, effectively allowing play to continue, and the clock to
        /// continue functioning.
        /// </summary>
        public int ControlPeriod
        {
            get
            {
                return Game.ClockMaxMoves == 0 ? 1 : (Game.MoveNo / Game.ClockMaxMoves) + 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the player's clock is ticking. 
        /// A player's clock ticks during their turn, and and is suspended during their opponent's turn.
        /// </summary>
        public bool IsTicking { get; private set; }

        /// <summary>
        /// Gets the number of move that the player has remaining, when the clock has a move limit e.g. 120 moves in 60 minutes.
        /// When the remaining moves run out, then it is automatically re-extended (see ControlPeriod).
        /// </summary>
        public int MovesRemaining
        {
            get
            {
                // N-o-t-e the remaining moves count is auto-extended by the Control Period.
                int remainingMoves = Game.ClockMaxMoves * this.ControlPeriod;
                return Math.Max(remainingMoves - Game.MoveNo, 0);
            }
        }

        /// <summary>
        ///   Gets or sets the players elapsed turn time. The clock ticks during the player's turn and is suspended during the opponents turn.
        /// </summary>
        public TimeSpan TimeElapsed { get; set; }

        /// <summary>
        ///   Gets the clock's elapsed time suitable for textual display.
        /// </summary>
        public TimeSpan TimeElapsedDisplay
        {
            get
            {
                return this.IsTicking
                           ? this.TimeElapsed + (DateTime.Now - this.TurnStartTime)
                           : this.TimeElapsed;
            }
        }

        /// <summary>
        ///   Gets player's remaining game time.
        /// </summary>
        public TimeSpan TimeRemaining
        {
            get
            {
                TimeSpan tsnRepeatingTimeLimit =
                    new TimeSpan(
                        (Game.ClockTime.Ticks * this.ControlPeriod) + (Game.ClockIncrementPerMove.Ticks * Game.MoveNo));
                return tsnRepeatingTimeLimit - this.TimeElapsed;
            }
        }

        /// <summary>
        ///   Gets the time when the current turn started.
        /// </summary>
        public DateTime TurnStartTime { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The resets the clock back to zero.
        /// </summary>
        public void Reset()
        {
            this.TimeElapsed = new TimeSpan(0, 0, 0);
            this.TurnStartTime = DateTime.Now;
        }

        /// <summary>
        /// Stop the clock and reset the turn start time.
        /// </summary>
        public void Revert()
        {
            this.IsTicking = false;
            this.TurnStartTime = DateTime.Now;
        }

        /// <summary>
        /// Start the clock.
        /// </summary>
        public void Start()
        {
            if (!this.IsTicking)
            {
                this.IsTicking = true;
                this.TurnStartTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Stop the clock.
        /// </summary>
        public void Stop()
        {
            if (this.IsTicking)
            {
                this.IsTicking = false;
                this.TimeElapsed += DateTime.Now - this.TurnStartTime;
            }
        }

        #endregion
    }
}