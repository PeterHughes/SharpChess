// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryHeuristic.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
// Represents the History Heuristic used to improve moved ordering.
// http://chessprogramming.wikispaces.com/History+Heuristic
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
    /// <summary>
    /// Represents the History Heuristic used to improve moved ordering.
    /// http://chessprogramming.wikispaces.com/History+Heuristic
    /// </summary>
    public static class HistoryHeuristic
    {
        #region Constants and Fields

        /// <summary>
        /// History table entries for black.
        /// </summary>
        private static readonly int[,] HistoryTableEntriesforBlack = new int[Board.SquareCount, Board.SquareCount];

        /// <summary>
        /// History table entries for white.
        /// </summary>
        private static readonly int[,] HistoryTableEntriesforWhite = new int[Board.SquareCount, Board.SquareCount];

        #endregion

        #region Public Methods

        /// <summary>
        /// Clear all history heuristic values.
        /// </summary>
        public static void Clear()
        {
            for (int i = 0; i < Board.SquareCount; i++)
            {
                for (int j = 0; j < Board.SquareCount; j++)
                {
                    HistoryTableEntriesforWhite[i, j] = 0;
                    HistoryTableEntriesforBlack[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Record a new history entry.
        /// </summary>
        /// <param name="colour">
        /// The player colour.
        /// </param>
        /// <param name="ordinalFrom">
        /// The From square ordinal.
        /// </param>
        /// <param name="ordinalTo">
        /// The To square ordinal.
        /// </param>
        /// <param name="value">
        /// The history heuristic weighting value.
        /// </param>
        public static void Record(Player.PlayerColourNames colour, int ordinalFrom, int ordinalTo, int value)
        {
            // Disable if this feature when switched off.
            if (!Game.EnableHistoryHeuristic)
            {
                return;
            }

            if (colour == Player.PlayerColourNames.White)
            {
                HistoryTableEntriesforWhite[ordinalFrom, ordinalTo] += value;
            }
            else
            {
                HistoryTableEntriesforBlack[ordinalFrom, ordinalTo] += value;
            }
        }

        /// <summary>
        /// Retrieve a value from the History Heuristic table.
        /// </summary>
        /// <param name="colour">
        /// The player colour.
        /// </param>
        /// <param name="ordinalFrom">
        /// The From square ordinal.
        /// </param>
        /// <param name="ordinalTo">
        /// The To square ordinal.
        /// </param>
        /// <returns>
        /// The history heuristic weighting value.
        /// </returns>
        public static int Retrieve(Player.PlayerColourNames colour, int ordinalFrom, int ordinalTo)
        {
            return colour == Player.PlayerColourNames.White ? HistoryTableEntriesforWhite[ordinalFrom, ordinalTo] : HistoryTableEntriesforBlack[ordinalFrom, ordinalTo];
        }

        #endregion
    }
}