// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieceBishop.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The piece bishop.
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
    /// The piece bishop.
    /// </summary>
    public class PieceBishop : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        /// The m_aint square values.
        /// </summary>
        public static int[] m_aintSquareValues =
        {
			10,10,10,10,10,10,10,10,    0,0,0,0,0,0,0,0,
			10,25,20,20,20,20,25,10,    0,0,0,0,0,0,0,0,
			10,49,30,30,30,30,49,10,    0,0,0,0,0,0,0,0,
			10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
			10,20,30,40,40,30,20,10,    0,0,0,0,0,0,0,0,
			10,49,30,30,30,30,49,10,    0,0,0,0,0,0,0,0,
			10,25,20,20,20,20,25,10 ,   0,0,0,0,0,0,0,0,
			10,10,10,10,10,10,10,10 ,   0,0,0,0,0,0,0,0
        };

        /// <summary>
        /// The m_ base.
        /// </summary>
        private readonly Piece m_Base;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceBishop"/> class.
        /// </summary>
        /// <param name="pieceBase">
        /// The piece base.
        /// </param>
        public PieceBishop(Piece pieceBase)
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
                return "B";
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
                return this.m_Base.Player.Colour == Player.enmColour.White ? 1 : 0;
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
                return Piece.enmName.Bishop;
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

                intPoints += m_aintSquareValues[this.m_Base.Square.Ordinal] << 1;

                if (Game.Stage != Game.enmStage.End)
                {
                    if (this.m_Base.CanBeDrivenAwayByPawn())
                    {
                        intPoints -= 30;
                    }
                }

                // Mobility
                Squares squares = new Squares();
                squares.Add(this.m_Base.Square);
                Board.LineThreatenedBy(this.m_Base.Player, squares, this.m_Base.Square, 15);
                Board.LineThreatenedBy(this.m_Base.Player, squares, this.m_Base.Square, 17);
                Board.LineThreatenedBy(this.m_Base.Player, squares, this.m_Base.Square, -15);
                Board.LineThreatenedBy(this.m_Base.Player, squares, this.m_Base.Square, -17);
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
                return 3250;
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
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, 17, movesType);
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, 15, movesType);
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, -15, movesType);
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, -17, movesType);
        }

        #endregion
    }
}