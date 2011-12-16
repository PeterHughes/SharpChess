// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieceKnight.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The piece knight.
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
    /// The piece knight.
    /// </summary>
    public class PieceKnight : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        /// The m_aint square values.
        /// </summary>
        public static readonly int[] m_aintSquareValues =
        {
            1, 1, 1, 1, 1, 1, 1, 1,    0,0,0,0,0,0,0,0,
            1, 7, 7, 7, 7, 7, 7, 1,    0,0,0,0,0,0,0,0,
            1, 7,18,18,18,18, 7, 1,    0,0,0,0,0,0,0,0,
            1, 7,18,27,27,18, 7, 1,    0,0,0,0,0,0,0,0,
            1, 7,18,27,27,18, 7, 1,    0,0,0,0,0,0,0,0,
            1, 7,18,18,18,18, 7, 1,    0,0,0,0,0,0,0,0,
            1, 7, 7, 7, 7, 7, 7, 1 ,   0,0,0,0,0,0,0,0,
            1, 1, 1, 1, 1, 1, 1, 1 ,   0,0,0,0,0,0,0,0
        };

        /// <summary>
        /// The m_ base.
        /// </summary>
        private readonly Piece m_Base;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceKnight"/> class.
        /// </summary>
        /// <param name="pieceBase">
        /// The piece base.
        /// </param>
        public PieceKnight(Piece pieceBase)
        {
            this.m_Base = pieceBase;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Abbreviation.
        /// </summary>
        public string Abbreviation
        {
            get
            {
                return "N";
            }
        }

        /// <summary>
        /// Gets Base.
        /// </summary>
        public Piece Base
        {
            get
            {
                return this.m_Base;
            }
        }

        /// <summary>
        /// Gets BasicValue.
        /// </summary>
        public int BasicValue
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// Gets ImageIndex.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.m_Base.Player.Colour == Player.enmColour.White ? 7 : 6;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsCapturable.
        /// </summary>
        public bool IsCapturable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets Name.
        /// </summary>
        public Piece.enmName Name
        {
            get
            {
                return Piece.enmName.Knight;
            }
        }

        /// <summary>
        /// Gets PositionalPoints.
        /// </summary>
        public int PositionalPoints
        {
            get
            {
                int intPoints = 0;

                if (Game.Stage == Game.GameStageNames.End)
                {
                    intPoints -= this.m_Base.TaxiCabDistanceToEnemyKingPenalty() << 4;
                }
                else
                {
                    intPoints += m_aintSquareValues[this.m_Base.Square.Ordinal] << 3;

                    if (this.m_Base.CanBeDrivenAwayByPawn())
                    {
                        intPoints -= 30;
                    }
                }

                intPoints += this.m_Base.DefensePoints;

                return intPoints;
            }
        }

        /// <summary>
        /// Gets Value.
        /// </summary>
        public int Value
        {
            get
            {
                return 3250; // + ((m_Base.Player.PawnsInPlay-5) * 63);  // raise the knight's value by 1/16 for each pawn above five of the side being valued, with the opposite adjustment for each pawn short of five;
            }
        }

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
        public void GenerateLazyMoves(Moves moves, Moves.enmMovesType movesType)
        {
            Square square;

            switch (movesType)
            {
                case Moves.enmMovesType.All:
                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 33);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 18);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 14);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 31);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 33);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 18);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 14);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 31);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    break;

                case Moves.enmMovesType.CapturesChecksPromotions:
                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 33);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 18);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 14);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 31);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 33);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 18);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 14);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 31);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    break;
            }
        }

        #endregion
    }
}