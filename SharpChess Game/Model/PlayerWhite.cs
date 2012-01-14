// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerWhite.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The player white.
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
    /// The player white.
    /// </summary>
    public class PlayerWhite : Player
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerWhite"/> class.
        /// </summary>
        public PlayerWhite()
        {
            this.Colour = PlayerColourNames.White;
            this.Intellegence = PlayerIntellegenceNames.Human;

            this.SetPiecesAtStartingPositions();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets PawnAttackLeftOffset.
        /// </summary>
        public override int PawnAttackLeftOffset
        {
            get
            {
                return 15;
            }
        }

        /// <summary>
        /// Gets PawnAttackRightOffset.
        /// </summary>
        public override int PawnAttackRightOffset
        {
            get
            {
                return 17;
            }
        }

        /// <summary>
        /// Gets PawnForwardOffset.
        /// </summary>
        public override int PawnForwardOffset
        {
            get
            {
                return 16;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The set pieces at starting positions.
        /// </summary>
        protected override sealed void SetPiecesAtStartingPositions()
        {
            this.Pieces.Add(this.King = new Piece(Piece.PieceNames.King, this, 4, 0, Piece.PieceIdentifierCodes.WhiteKing));

            this.Pieces.Add(new Piece(Piece.PieceNames.Queen, this, 3, 0, Piece.PieceIdentifierCodes.WhiteQueen));

            this.Pieces.Add(new Piece(Piece.PieceNames.Rook, this, 0, 0, Piece.PieceIdentifierCodes.WhiteQueensRook));
            this.Pieces.Add(new Piece(Piece.PieceNames.Rook, this, 7, 0, Piece.PieceIdentifierCodes.WhiteKingsRook));

            this.Pieces.Add(new Piece(Piece.PieceNames.Bishop, this, 2, 0, Piece.PieceIdentifierCodes.WhiteQueensBishop));
            this.Pieces.Add(new Piece(Piece.PieceNames.Bishop, this, 5, 0, Piece.PieceIdentifierCodes.WhiteKingsBishop));

            this.Pieces.Add(new Piece(Piece.PieceNames.Knight, this, 1, 0, Piece.PieceIdentifierCodes.WhiteQueensKnight));
            this.Pieces.Add(new Piece(Piece.PieceNames.Knight, this, 6, 0, Piece.PieceIdentifierCodes.WhiteKingsKnight));

            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 0, 1, Piece.PieceIdentifierCodes.WhitePawn1));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 1, 1, Piece.PieceIdentifierCodes.WhitePawn2));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 2, 1, Piece.PieceIdentifierCodes.WhitePawn3));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 3, 1, Piece.PieceIdentifierCodes.WhitePawn4));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 4, 1, Piece.PieceIdentifierCodes.WhitePawn5));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 5, 1, Piece.PieceIdentifierCodes.WhitePawn6));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 6, 1, Piece.PieceIdentifierCodes.WhitePawn7));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 7, 1, Piece.PieceIdentifierCodes.WhitePawn8));
        }

        #endregion
    }
}