// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Squares.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The squares.
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

    using System.Collections;

    #endregion

    /// <summary>
    /// The squares.
    /// </summary>
    public class Squares : IEnumerable
    {
        #region Constants and Fields

        /// <summary>
        /// The m_col squares.
        /// </summary>
        private readonly ArrayList squareList = new ArrayList(32);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of squares in the list.
        /// </summary>
        public int Count
        {
            get
            {
                return this.squareList.Count;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new square to the list.
        /// </summary>
        /// <param name="square">
        /// The square to add.
        /// </param>
        public void Add(Square square)
        {
            this.squareList.Add(square);
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The enumerator.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return this.squareList.GetEnumerator();
        }

        /// <summary>
        /// Searches for the specified square and returns its index.
        /// </summary>
        /// <param name="square">
        /// The piece to search for.
        /// </param>
        /// <returns>
        /// Index value of the found square. or null if not found.
        /// </returns>
        public int IndexOf(Square square)
        {
            return this.squareList.IndexOf(square);
        }

        /// <summary>
        /// Insert a sqaure into the list. at the specified index position.
        /// </summary>
        /// <param name="ordinal">
        /// The ordinal index position where the square will be inserted.
        /// </param>
        /// <param name="square">
        /// The piece.
        /// </param>
        public void Insert(int ordinal, Square square)
        {
            this.squareList.Insert(ordinal, square);
        }

        /// <summary>
        /// Returns the square at the specified index position in the list.
        /// </summary>
        /// <param name="intIndex">
        /// Index position.
        /// </param>
        /// <returns>
        /// The square at the specified index.
        /// </returns>
        public Square Item(int intIndex)
        {
            return (Square)this.squareList[intIndex];
        }

        /// <summary>
        /// Remove the square from the list.
        /// </summary>
        /// <param name="square">
        /// The piece to remove.
        /// </param>
        public void Remove(Square square)
        {
            this.squareList.Remove(square);
        }

        #endregion
    }
}