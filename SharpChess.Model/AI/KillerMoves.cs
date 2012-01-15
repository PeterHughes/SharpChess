// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KillerMoves.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Represents the Killer Heuristic used to improve move ordering.
//   http://chessprogramming.wikispaces.com/Killer+Heuristic
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
    /// <summary>
    /// Represents the Killer Heuristic used to improve move ordering.
    ///   http://chessprogramming.wikispaces.com/Killer+Heuristic
    /// </summary>
    public static class KillerMoves
    {
        #region Constants and Fields

        /// <summary>
        ///   List of primary (A) Killer Moves indexed by search depth.
        /// </summary>
        private static readonly Move[] PrimaryKillerMovesA = new Move[64];

        /// <summary>
        ///   List of secondary (B) Killer Moves indexed by search depth.
        /// </summary>
        private static readonly Move[] SecondaryKillerMovesB = new Move[64];

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "KillerMoves" /> class.
        /// </summary>
        static KillerMoves()
        {
            Clear();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The clear.
        /// </summary>
        public static void Clear()
        {
            for (int intIndex = 0; intIndex < 64; intIndex++)
            {
                PrimaryKillerMovesA[intIndex] = null;
                SecondaryKillerMovesB[intIndex] = null;
            }
        }

        /// <summary>
        /// Adds the move made to the appropriate killer move slot, if it's better than the current killer moves
        /// </summary>
        /// <param name="ply">
        /// Search depth
        /// </param>
        /// <param name="moveMade">
        /// Move to be added
        /// </param>
        public static void RecordPossibleKillerMove(int ply, Move moveMade)
        {
            // Disable if this feature when switched off.
            if (!Game.EnableKillerMoves)
            {
                return;
            }

            bool blnAssignedA = false; // Have we assign Slot A?

            Move moveKillerA = RetrieveA(ply);
            Move moveKillerB = RetrieveB(ply);

            if (moveKillerA == null)
            {
                // Slot A is blank, so put anything in it.
                AssignA(ply, moveMade);
                blnAssignedA = true;
            }
            else if ((moveMade.Score > moveKillerA.Score && !Move.MovesMatch(moveMade, moveKillerB)) || Move.MovesMatch(moveMade, moveKillerA))
            {
                // Move's score is better than A and isn't B, or the move IS A, 
                blnAssignedA = true;
                if (Move.MovesMatch(moveMade, moveKillerA))
                {
                    // Re-record move in Slot A, but only if it's better
                    if (moveMade.Score > moveKillerA.Score)
                    {
                        AssignA(ply, moveMade);
                    }
                }
                else
                {
                    // Score is better than Slot A

                    // transfer move in Slot A to Slot B...
                    AssignB(ply, moveKillerA);

                    // record move is Slot A
                    AssignA(ply, moveMade);
                }

                moveKillerA = RetrieveA(ply);
            }

            // If the move wasn't assigned to Slot A, then see if it is good enough to go in Slot B, or if move IS B
            if (!blnAssignedA)
            {
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

                moveKillerB = RetrieveB(ply);
            }

            // Finally check if B score is better than and A score, and if so swap.
            if (moveKillerA != null && moveKillerB != null && moveKillerB.Score > moveKillerA.Score)
            {
                Move swap = moveKillerA;
                AssignA(ply, moveKillerB);
                AssignB(ply, swap);
            }
        }

        /// <summary>
        /// Retrieve primary (A) killer move for specified search depth.
        /// </summary>
        /// <param name="depth">
        /// Search depth (ply).
        /// </param>
        /// <returns>
        /// Move for specified depth
        /// </returns>
        public static Move RetrieveA(int depth)
        {
            return PrimaryKillerMovesA[depth + 32];
        }

        /// <summary>
        /// Retrieve secondary (B) killer move for specified search depth.
        /// </summary>
        /// <param name="depth">
        /// Search depth (ply).
        /// </param>
        /// <returns>
        /// Move for specified depth
        /// </returns>
        public static Move RetrieveB(int depth)
        {
            return SecondaryKillerMovesB[depth + 32];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Assign killer move A (primary)
        /// </summary>
        /// <param name="depth">
        /// The search depth (ply).
        /// </param>
        /// <param name="move">
        /// The move to assign.
        /// </param>
        private static void AssignA(int depth, Move move)
        {
            PrimaryKillerMovesA[depth + 32] = move;
        }

        /// <summary>
        /// Assign killer move B (secondary)
        /// </summary>
        /// <param name="depth">
        /// The search depth (ply).
        /// </param>
        /// <param name="move">
        /// The move to assign.
        /// </param>
        private static void AssignB(int depth, Move move)
        {
            SecondaryKillerMovesB[depth + 32] = move;
        }

        #endregion
    }
}