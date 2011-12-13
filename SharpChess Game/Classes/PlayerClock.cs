// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerClock.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The player clock.
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

namespace SharpChess
{
    #region Using

    using System;

    #endregion

    /// <summary>
    /// The player clock.
    /// </summary>
    public class PlayerClock
    {
        #region Constants and Fields

        /// <summary>
        ///   The m_bln is ticking.
        /// </summary>
        private bool m_blnIsTicking;

        /// <summary>
        ///   The m_dtm turn start.
        /// </summary>
        private DateTime m_dtmTurnStart;

        /// <summary>
        ///   The m_player.
        /// </summary>
        private Player m_player;

        /// <summary>
        ///   The m_tsn time elapsed.
        /// </summary>
        private TimeSpan m_tsnTimeElapsed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerClock"/> class.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        public PlayerClock(Player player)
        {
            this.m_player = player;
            this.Reset();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets ControlPeriod.
        /// </summary>
        public int ControlPeriod
        {
            get
            {
                return Game.ClockMoves == 0 ? 1 : (Game.MoveNo / Game.ClockMoves) + 1;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether IsTicking.
        /// </summary>
        public bool IsTicking
        {
            get
            {
                return this.m_blnIsTicking;
            }
        }

        /// <summary>
        ///   Gets MovesRemaining.
        /// </summary>
        public int MovesRemaining
        {
            get
            {
                int intRepeatingMoves = Game.ClockMoves * this.ControlPeriod;
                return Math.Max(intRepeatingMoves - Game.MoveNo, 0);
            }
        }

        /// <summary>
        ///   Gets or sets TimeElapsed.
        /// </summary>
        public TimeSpan TimeElapsed
        {
            get
            {
                return this.m_tsnTimeElapsed;
            }

            set
            {
                this.m_tsnTimeElapsed = value;
            }
        }

        /// <summary>
        ///   Gets TimeElapsedDisplay.
        /// </summary>
        public TimeSpan TimeElapsedDisplay
        {
            get
            {
                return this.m_blnIsTicking
                           ? this.m_tsnTimeElapsed + (DateTime.Now - this.m_dtmTurnStart)
                           : this.m_tsnTimeElapsed;
            }
        }

        /// <summary>
        ///   Gets TimeRemaining.
        /// </summary>
        public TimeSpan TimeRemaining
        {
            get
            {
                TimeSpan tsnRepeatingTimeLimit =
                    new TimeSpan(
                        (Game.ClockTime.Ticks * this.ControlPeriod) + Game.ClockIncrementPerMove.Ticks * Game.MoveNo);
                return tsnRepeatingTimeLimit - this.m_tsnTimeElapsed;
            }
        }

        /// <summary>
        ///   Gets TurnStartTime.
        /// </summary>
        public DateTime TurnStartTime
        {
            get
            {
                return this.m_dtmTurnStart;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The reset.
        /// </summary>
        public void Reset()
        {
            this.m_tsnTimeElapsed = new TimeSpan(0, 0, 0);
            this.m_dtmTurnStart = DateTime.Now;
        }

        /// <summary>
        /// The revert.
        /// </summary>
        public void Revert()
        {
            this.m_blnIsTicking = false;
            this.m_dtmTurnStart = DateTime.Now;
        }

        /// <summary>
        /// The start.
        /// </summary>
        public void Start()
        {
            if (!this.m_blnIsTicking)
            {
                this.m_blnIsTicking = true;
                this.m_dtmTurnStart = DateTime.Now;
            }
        }

        /// <summary>
        /// The stop.
        /// </summary>
        public void Stop()
        {
            if (this.m_blnIsTicking)
            {
                this.m_blnIsTicking = false;
                this.m_tsnTimeElapsed += DateTime.Now - this.m_dtmTurnStart;
            }
        }

        #endregion
    }
}