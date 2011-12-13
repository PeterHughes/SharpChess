// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieceQueen.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The piece queen.
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
    /// The piece queen.
    /// </summary>
    public class PieceQueen : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        /// The m_ base.
        /// </summary>
        private readonly Piece m_Base;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceQueen"/> class.
        /// </summary>
        /// <param name="pieceBase">
        /// The piece base.
        /// </param>
        public PieceQueen(Piece pieceBase)
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
                return "Q";
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
                return 9;
            }
        }

        /// <summary>
        /// Gets ImageIndex.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.m_Base.Player.Colour == Player.enmColour.White ? 11 : 10;
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
                return Piece.enmName.Queen;
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

                // The queen is that after the opening it is penalized slightly for 
                // "taxicab" distance to the enemy king.
                if (Game.Stage == Game.enmStage.Opening)
                {
                    if (this.m_Base.Player.Colour == Player.enmColour.White)
                    {
                        intPoints -= this.m_Base.Square.Rank * 7;
                    }
                    else
                    {
                        intPoints -= (7 - this.m_Base.Square.Rank) * 7;
                    }
                }
                else
                {
                    intPoints -= this.m_Base.TaxiCabDistanceToEnemyKingPenalty();
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
                return 9750;
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
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, 16, movesType);
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, 1, movesType);
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, -1, movesType);
            Board.AppendPiecePath(moves, this.m_Base, this.m_Base.Player, -16, movesType);
        }

        #endregion
    }
}