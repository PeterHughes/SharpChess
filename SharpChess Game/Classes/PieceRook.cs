// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieceRook.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The piece rook.
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
    /// The piece rook.
    /// </summary>
    public class PieceRook : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        /// The m_aint square values.
        /// </summary>
        private static readonly int[] m_aintSquareValues = 
        {
            10,10,10,10,10,10,10,10,    0,0,0,0,0,0,0,0,
            10,20,20,20,20,20,20,10,    0,0,0,0,0,0,0,0,
            10,20,30,30,30,30,20,10,    0,0,0,0,0,0,0,0,
            10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
            10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
            10,20,30,30,30,30,20,10,    0,0,0,0,0,0,0,0,
            10,20,20,20,20,20,20,10 ,   0,0,0,0,0,0,0,0,
            10,10,10,10,10,10,10,10 ,   0,0,0,0,0,0,0,0
        };

        /// <summary>
        /// The m_ base.
        /// </summary>
        private readonly Piece m_Base;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceRook"/> class.
        /// </summary>
        /// <param name="pieceBase">
        /// The piece base.
        /// </param>
        public PieceRook(Piece pieceBase)
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
                return "R";
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
                return 5;
            }
        }

        /// <summary>
        /// Gets ImageIndex.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.m_Base.Player.Colour == Player.enmColour.White ? 3 : 2;
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
                return Piece.enmName.Rook;
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

                // After the opening, Rooks are penalized slightly depending on "taxicab" distance to the enemy king.
                if (Game.Stage != Game.GameStageNames.Opening)
                {
                    intPoints -= this.m_Base.TaxiCabDistanceToEnemyKingPenalty();
                }

                if (Game.Stage != Game.GameStageNames.End)
                {
                    // Rooks are given a bonus of 10(0) points for occupying a file with no friendly pawns and a bonus of 
                    // 4(0) points if no enemy pawns lie on that file. 
                    bool blnHasFiendlyPawn = false;
                    bool blnHasEnemyPawn = false;
                    Square squareThis = Board.GetSquare(this.m_Base.Square.File, 0);
                    Piece piece;
                    while (squareThis != null)
                    {
                        piece = squareThis.Piece;
                        if (piece != null && piece.Name == Piece.enmName.Pawn)
                        {
                            if (piece.Player.Colour == this.m_Base.Player.Colour)
                            {
                                blnHasFiendlyPawn = true;
                            }
                            else
                            {
                                blnHasEnemyPawn = true;
                            }

                            if (blnHasFiendlyPawn && blnHasEnemyPawn)
                            {
                                break;
                            }
                        }

                        squareThis = Board.GetSquare(squareThis.Ordinal + 16);
                    }

                    if (!blnHasFiendlyPawn)
                    {
                        intPoints += 20;
                    }

                    if (!blnHasEnemyPawn)
                    {
                        intPoints += 10;
                    }

                    // 7th rank
                    if (this.m_Base.Player.Colour == Player.enmColour.White && this.m_Base.Square.Rank == 6
                        || this.m_Base.Player.Colour == Player.enmColour.Black && this.m_Base.Square.Rank == 1)
                    {
                        intPoints += 30;
                    }
                }

                // Mobility
                Squares squares = new Squares();
                squares.Add(this.m_Base.Square);
                Board.LineThreatenedBy(this.m_Base.Player, squares, this.m_Base.Square, 1);
                Board.LineThreatenedBy(this.m_Base.Player, squares, this.m_Base.Square, -1);
                Board.LineThreatenedBy(this.m_Base.Player, squares, this.m_Base.Square, 16);
                Board.LineThreatenedBy(this.m_Base.Player, squares, this.m_Base.Square, -16);
                int intSquareValue = 0;
                foreach (Square square in squares)
                {
                    intSquareValue += m_aintSquareValues[square.Ordinal];
                }

                intPoints += intSquareValue >> 2;

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
                return 5000;
                    
                    // - ((m_Base.Player.PawnsInPlay-5) * 125);  // lower the rook's value by 1/8 for each pawn above five of the side being valued, with the opposite adjustment for each pawn short of five
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
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, 1, movesType);
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, -1, movesType);
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, -16, movesType);
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, 16, movesType);
        }

        #endregion
    }
}