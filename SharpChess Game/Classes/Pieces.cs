// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pieces.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The pieces.
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
    /// The pieces.
    /// </summary>
    public class Pieces : IEnumerable
    {
        #region Constants and Fields

        /// <summary>
        /// The m_col pieces.
        /// </summary>
        private readonly ArrayList m_colPieces = new ArrayList();

        /// <summary>
        /// The m_player.
        /// </summary>
        private readonly Player m_player;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Pieces"/> class.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        public Pieces(Player player)
        {
            this.m_player = player;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.m_colPieces.Count;
            }
        }

        /// <summary>
        /// Gets Player.
        /// </summary>
        public Player Player
        {
            get
            {
                return this.m_player;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="piece">
        /// The piece.
        /// </param>
        public void Add(Piece piece)
        {
            this.m_colPieces.Add(piece);
        }

        /// <summary>
        /// The clone.
        /// </summary>
        /// <returns>
        /// The clone.
        /// </returns>
        public object Clone()
        {
            return this.m_colPieces.Clone();
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return this.m_colPieces.GetEnumerator();
        }

        /// <summary>
        /// The index of.
        /// </summary>
        /// <param name="piece">
        /// The piece.
        /// </param>
        /// <returns>
        /// The index of.
        /// </returns>
        public int IndexOf(Piece piece)
        {
            return this.m_colPieces.IndexOf(piece);
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="Ordinal">
        /// The ordinal.
        /// </param>
        /// <param name="piece">
        /// The piece.
        /// </param>
        public void Insert(int Ordinal, Piece piece)
        {
            this.m_colPieces.Insert(Ordinal, piece);
        }

        /// <summary>
        /// The item.
        /// </summary>
        /// <param name="intIndex">
        /// The int index.
        /// </param>
        /// <returns>
        /// </returns>
        public Piece Item(int intIndex)
        {
            return (Piece)this.m_colPieces[intIndex];
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="piece">
        /// The piece.
        /// </param>
        public void Remove(Piece piece)
        {
            this.m_colPieces.Remove(piece);
        }

        /// <summary>
        /// The sort by score.
        /// </summary>
        public void SortByScore()
        {
            this.m_colPieces.Sort();
        }

        #endregion
    }
}