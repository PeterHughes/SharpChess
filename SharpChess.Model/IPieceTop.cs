// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPieceTop.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   IPieceTop interface. The Piece class represents the base of a chess piece, on which different "tops" can be placed. 
//   The Top of a piece will change when a piece is promoted. e.g. a Pawn is promoted to a Queen, or a Knight.
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
    /// <summary>
    /// IPieceTop interface. The <see cref="Piece"/>  class represents the base of a chess piece, on which different "tops" can be placed. 
    /// The Top of a piece will change when a piece is promoted. e.g. a Pawn is promoted to a Queen, or a Knight.
    /// </summary>
    public interface IPieceTop
    {
        #region Public Properties

        /// <summary>
        /// Gets the Abbreviated name for the piece.
        /// </summary>
        string Abbreviation { get; }

        /// <summary>
        /// Gets the base <see cref="Piece"/>.
        /// </summary>
        Piece Base { get; }

        /// <summary>
        /// Gets the BasicValue for this piece. e.g. 9 for Queen, 1 for a Pawn.
        /// </summary>
        int BasicValue { get; }

        /// <summary>
        /// Gets ImageIndex for the piece.
        /// </summary>
        int ImageIndex { get; }

        /// <summary>
        /// Gets a value indicating whether the piece can be captured.
        /// </summary>
        bool IsCapturable { get; }

        /// <summary>
        /// Gets the name of the piece.
        /// </summary>
        Piece.PieceNames Name { get; }

        /// <summary>
        /// Gets the positional score points of the piece.
        /// </summary>
        int PositionalPoints { get; }

        /// <summary>
        /// Gets the base score value for the piece e.g. 9000 for Queen, 1000 for a Pawn.
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
        void GenerateLazyMoves(Moves moves, Moves.MoveListNames movesType);

        bool CanAttackSquare(Square square);

        #endregion
    }
}