// --------------------------------------------------------------------------------------------------------------------
// <copyright file="History.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The history.
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
    /// The history.
    /// </summary>
    public class History
    {
        #region Constants and Fields

        /// <summary>
        /// The a history entry black.
        /// </summary>
        private static readonly int[,] aHistoryEntryBlack = new int[Board.SquareCount, Board.SquareCount];

        /// <summary>
        /// The a history entry white.
        /// </summary>
        private static readonly int[,] aHistoryEntryWhite = new int[Board.SquareCount, Board.SquareCount];

        #endregion

        #region Public Methods

        /// <summary>
        /// The clear.
        /// </summary>
        public static void Clear()
        {
            for (int i = 0; i < Board.SquareCount; i++)
            {
                for (int j = 0; j < Board.SquareCount; j++)
                {
                    aHistoryEntryWhite[i, j] = 0;
                    aHistoryEntryBlack[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// The record.
        /// </summary>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <param name="OrdinalFrom">
        /// The ordinal from.
        /// </param>
        /// <param name="OrdinalTo">
        /// The ordinal to.
        /// </param>
        /// <param name="Value">
        /// The value.
        /// </param>
        public static void Record(Player.enmColour colour, int OrdinalFrom, int OrdinalTo, int Value)
        {
            if (colour == Player.enmColour.White)
            {
                aHistoryEntryWhite[OrdinalFrom, OrdinalTo] += Value;
            }
            else
            {
                aHistoryEntryBlack[OrdinalFrom, OrdinalTo] += Value;
            }
        }

        /// <summary>
        /// The retrieve.
        /// </summary>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <param name="OrdinalFrom">
        /// The ordinal from.
        /// </param>
        /// <param name="OrdinalTo">
        /// The ordinal to.
        /// </param>
        /// <returns>
        /// The retrieve.
        /// </returns>
        public static int Retrieve(Player.enmColour colour, int OrdinalFrom, int OrdinalTo)
        {
            return colour == Player.enmColour.White ? aHistoryEntryWhite[OrdinalFrom, OrdinalTo] : aHistoryEntryBlack[OrdinalFrom, OrdinalTo];
        }

        #endregion
    }
}