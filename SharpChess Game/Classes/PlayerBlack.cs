// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerBlack.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The player black.
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
            this.m_PlayerClock = new PlayerClock(this);

            this.Colour = enmColour.Black;
            this.Intellegence = enmIntellegence.Computer;

            this.SetPiecesAtStartingPositions();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Clock.
        /// </summary>
        public override PlayerClock Clock
        {
            get
            {
                return this.m_PlayerClock;
            }
        }

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
        protected override void SetPiecesAtStartingPositions()
        {
            this.m_colPieces.Add(this.King = new Piece(Piece.enmName.King, this, 4, 7, Piece.enmID.BlackKing));

            this.m_colPieces.Add(new Piece(Piece.enmName.Queen, this, 3, 7, Piece.enmID.BlackQueen));

            this.m_colPieces.Add(new Piece(Piece.enmName.Rook, this, 0, 7, Piece.enmID.BlackQueensRook));
            this.m_colPieces.Add(new Piece(Piece.enmName.Rook, this, 7, 7, Piece.enmID.BlackKingsRook));

            this.m_colPieces.Add(new Piece(Piece.enmName.Bishop, this, 2, 7, Piece.enmID.BlackQueensBishop));
            this.m_colPieces.Add(new Piece(Piece.enmName.Bishop, this, 5, 7, Piece.enmID.BlackKingsBishop));

            this.m_colPieces.Add(new Piece(Piece.enmName.Knight, this, 1, 7, Piece.enmID.BlackQueensKnight));
            this.m_colPieces.Add(new Piece(Piece.enmName.Knight, this, 6, 7, Piece.enmID.BlackKingsKnight));

            this.m_colPieces.Add(new Piece(Piece.enmName.Pawn, this, 0, 6, Piece.enmID.BlackPawn1));
            this.m_colPieces.Add(new Piece(Piece.enmName.Pawn, this, 1, 6, Piece.enmID.BlackPawn2));
            this.m_colPieces.Add(new Piece(Piece.enmName.Pawn, this, 2, 6, Piece.enmID.BlackPawn3));
            this.m_colPieces.Add(new Piece(Piece.enmName.Pawn, this, 3, 6, Piece.enmID.BlackPawn4));
            this.m_colPieces.Add(new Piece(Piece.enmName.Pawn, this, 4, 6, Piece.enmID.BlackPawn5));
            this.m_colPieces.Add(new Piece(Piece.enmName.Pawn, this, 5, 6, Piece.enmID.BlackPawn6));
            this.m_colPieces.Add(new Piece(Piece.enmName.Pawn, this, 6, 6, Piece.enmID.BlackPawn7));
            this.m_colPieces.Add(new Piece(Piece.enmName.Pawn, this, 7, 6, Piece.enmID.BlackPawn8));
        }

        #endregion
    }
}