// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerBlack.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The player black.
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
    /// The player black.
    /// </summary>
    public class PlayerBlack : Player
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBlack"/> class.
        /// </summary>
        public PlayerBlack()
        {
            this.Colour = PlayerColourNames.Black;
            this.Intellegence = PlayerIntellegenceNames.Computer;

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
                return -17;
            }
        }

        /// <summary>
        /// Gets PawnAttackRightOffset.
        /// </summary>
        public override int PawnAttackRightOffset
        {
            get
            {
                return -15;
            }
        }

        /// <summary>
        /// Gets PawnForwardOffset.
        /// </summary>
        public override int PawnForwardOffset
        {
            get
            {
                return -16;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The set pieces at starting positions.
        /// </summary>
        protected override sealed void SetPiecesAtStartingPositions()
        {
            this.Pieces.Add(this.King = new Piece(Piece.PieceNames.King, this, 4, 7, Piece.PieceIdentifierCodes.BlackKing));

            this.Pieces.Add(new Piece(Piece.PieceNames.Queen, this, 3, 7, Piece.PieceIdentifierCodes.BlackQueen));

            this.Pieces.Add(new Piece(Piece.PieceNames.Rook, this, 0, 7, Piece.PieceIdentifierCodes.BlackQueensRook));
            this.Pieces.Add(new Piece(Piece.PieceNames.Rook, this, 7, 7, Piece.PieceIdentifierCodes.BlackKingsRook));

            this.Pieces.Add(new Piece(Piece.PieceNames.Bishop, this, 2, 7, Piece.PieceIdentifierCodes.BlackQueensBishop));
            this.Pieces.Add(new Piece(Piece.PieceNames.Bishop, this, 5, 7, Piece.PieceIdentifierCodes.BlackKingsBishop));

            this.Pieces.Add(new Piece(Piece.PieceNames.Knight, this, 1, 7, Piece.PieceIdentifierCodes.BlackQueensKnight));
            this.Pieces.Add(new Piece(Piece.PieceNames.Knight, this, 6, 7, Piece.PieceIdentifierCodes.BlackKingsKnight));

            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 0, 6, Piece.PieceIdentifierCodes.BlackPawn1));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 1, 6, Piece.PieceIdentifierCodes.BlackPawn2));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 2, 6, Piece.PieceIdentifierCodes.BlackPawn3));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 3, 6, Piece.PieceIdentifierCodes.BlackPawn4));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 4, 6, Piece.PieceIdentifierCodes.BlackPawn5));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 5, 6, Piece.PieceIdentifierCodes.BlackPawn6));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 6, 6, Piece.PieceIdentifierCodes.BlackPawn7));
            this.Pieces.Add(new Piece(Piece.PieceNames.Pawn, this, 7, 6, Piece.PieceIdentifierCodes.BlackPawn8));
        }

        #endregion
    }
}