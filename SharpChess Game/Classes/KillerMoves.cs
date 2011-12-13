// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KillerMoves.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   Summary description for KillerMoves.
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
    /// <summary>
    /// Killer Moves.
    /// </summary>
    public class KillerMoves
    {
        #region Constants and Fields

        /// <summary>
        /// The m_arrmove a.
        /// </summary>
        private static readonly Move[] m_arrmoveA = new Move[64];

        /// <summary>
        /// The m_arrmove b.
        /// </summary>
        private static readonly Move[] m_arrmoveB = new Move[64];

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="KillerMoves"/> class.
        /// </summary>
        static KillerMoves()
        {
            Clear();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The assign a.
        /// </summary>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <param name="move">
        /// The move.
        /// </param>
        public static void AssignA(int depth, Move move)
        {
            m_arrmoveA[depth + 32] = move;
        }

        /// <summary>
        /// The assign b.
        /// </summary>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <param name="move">
        /// The move.
        /// </param>
        public static void AssignB(int depth, Move move)
        {
            m_arrmoveB[depth + 32] = move;
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public static void Clear()
        {
            for (int intIndex = 0; intIndex < 64; intIndex++)
            {
                m_arrmoveA[intIndex] = null;
                m_arrmoveB[intIndex] = null;
            }
        }

        /// <summary>
        /// Adds the move made to the appropriate killer move slot, if it's better than the currnet killer moves
        /// </summary>
        /// <param name="ply">
        /// Search depth
        /// </param>
        /// <param name="moveMade">
        /// Move to be added
        /// </param>
        public static void RecordPossibleKillerMove(int ply, Move moveMade)
        {
            Move moveKillerA;
            Move moveKillerB;
            bool blnAssignedA = false; // Have we assign Slot A?

            moveKillerA = RetrieveA(ply); // Get slot A move
            if (moveKillerA == null)
            {
                // Slot A is blank, so put anything in it.
                AssignA(ply, moveMade);
                blnAssignedA = true;
            }
            else if (moveMade.Score > moveKillerA.Score)
            {
                // Score is better than Slot A, so
                // transfer move in Slot A to Slot B...
                AssignB(ply, moveKillerA);

                // record move is Slot A
                AssignA(ply, moveMade);
                blnAssignedA = true;
            }

            // If the move wasn't assigned to Slot A, then see if it is good enough to go in Slot B
            if (!blnAssignedA)
            {
                moveKillerB = RetrieveB(ply);

                // Slot B is empty, so put anything in!
                if (moveKillerB == null)
                {
                    AssignB(ply, moveMade);
                }
                else if (moveMade.Score > moveKillerB.Score)
                {
                    // Score is better than Slot B, so
                    // record move is Slot B
                    AssignB(ply, moveMade);
                }
            }
        }

        /// <summary>
        /// The retrieve a.
        /// </summary>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <returns>
        /// </returns>
        public static Move RetrieveA(int depth)
        {
            return m_arrmoveA[depth + 32];
        }

        /// <summary>
        /// The retrieve b.
        /// </summary>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <returns>
        /// </returns>
        public static Move RetrieveB(int depth)
        {
            return m_arrmoveB[depth + 32];
        }

        #endregion
    }
}