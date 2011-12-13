// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPieceTop.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The i piece top.
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
    /// The i piece top.
    /// </summary>
    public interface IPieceTop
    {
        #region Public Properties

        /// <summary>
        /// Gets Abbreviation.
        /// </summary>
        string Abbreviation { get; }

        /// <summary>
        /// Gets Base.
        /// </summary>
        Piece Base { get; }

        /// <summary>
        /// Gets BasicValue.
        /// </summary>
        int BasicValue { get; }

        /// <summary>
        /// Gets ImageIndex.
        /// </summary>
        int ImageIndex { get; }

        /// <summary>
        /// Gets a value indicating whether IsCapturable.
        /// </summary>
        bool IsCapturable { get; }

        /// <summary>
        /// Gets Name.
        /// </summary>
        Piece.enmName Name { get; }

        /// <summary>
        /// Gets PositionalPoints.
        /// </summary>
        int PositionalPoints { get; }

        /// <summary>
        /// Gets Value.
        /// </summary>
        int Value { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The generate lazy moves.
        /// </summary>
        /// <param name="moves">
        /// The moves.
        /// </param>
        /// <param name="movesType">
        /// The moves type.
        /// </param>
        void GenerateLazyMoves(Moves moves, Moves.enmMovesType movesType);

        #endregion
    }
}