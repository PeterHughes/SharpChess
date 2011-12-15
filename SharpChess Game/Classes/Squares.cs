// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Squares.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The squares.
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
        private readonly ArrayList m_colSquares = new ArrayList(24);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.m_colSquares.Count;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="square">
        /// The square.
        /// </param>
        public void Add(Square square)
        {
            this.m_colSquares.Add(square);
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return this.m_colSquares.GetEnumerator();
        }

        /// <summary>
        /// The index of.
        /// </summary>
        /// <param name="square">
        /// The square.
        /// </param>
        /// <returns>
        /// The index of.
        /// </returns>
        public int IndexOf(Square square)
        {
            return this.m_colSquares.IndexOf(square);
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="Ordinal">
        /// The ordinal.
        /// </param>
        /// <param name="square">
        /// The square.
        /// </param>
        public void Insert(int Ordinal, Square square)
        {
            this.m_colSquares.Insert(Ordinal, square);
        }

        /// <summary>
        /// The item.
        /// </summary>
        /// <param name="intIndex">
        /// The int index.
        /// </param>
        /// <returns>
        /// </returns>
        public Square Item(int intIndex)
        {
            return (Square)this.m_colSquares[intIndex];
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="square">
        /// The square.
        /// </param>
        public void Remove(Square square)
        {
            this.m_colSquares.Remove(square);
        }

        #endregion
    }
}