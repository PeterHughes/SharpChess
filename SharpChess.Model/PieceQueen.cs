// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieceQueen.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The piece queen.
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
    /// The piece queen.
    /// </summary>
    public class PieceQueen : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        /// Directional vectors of where the piece can go
        /// </summary>
        private static int[] moveVectors = { 17, -17, 15, -15, 1, -1, 16, -16 };

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
            this.Base = pieceBase;
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
        /// Gets the base part of the piece. i.e. the bit that sits on the chess square.
        /// </summary>
        public Piece Base { get; private set; }

        /// <summary>
        /// Gets basic value of the piece. e.g. pawn = 1, bishop = 3, queen = 9
        /// </summary>
        public int BasicValue
        {
            get
            {
                return 9;
            }
        }

        /// <summary>
        /// Gets the image index for this piece. Used to determine which graphic image is displayed for thie piece.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.Base.Player.Colour == Player.PlayerColourNames.White ? 11 : 10;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the piece is capturable. Kings aren't, everything else is.
        /// </summary>
        public bool IsCapturable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the piece's name.
        /// </summary>
        public Piece.PieceNames Name
        {
            get
            {
                return Piece.PieceNames.Queen;
            }
        }

        /// <summary>
        /// Gets the positional points assigned to this piece.
        /// </summary>
        public int PositionalPoints
        {
            get
            {
                int intPoints = 0;

                // The queen is that after the opening it is penalized slightly for 
                // "taxicab" distance to the enemy king.
                if (Game.Stage == Game.GameStageNames.Opening)
                {
                    if (this.Base.Player.Colour == Player.PlayerColourNames.White)
                    {
                        intPoints -= this.Base.Square.Rank * 7;
                    }
                    else
                    {
                        intPoints -= (7 - this.Base.Square.Rank) * 7;
                    }
                }
                else
                {
                    intPoints -= this.Base.TaxiCabDistanceToEnemyKingPenalty();
                }

                intPoints += this.Base.DefensePoints;

                return intPoints;
            }
        }

        /// <summary>
        /// Gets the material value of this piece.
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
        /// Generate "lazy" moves for this piece, which is all usual legal moves, but also includes moves that put the king in check.
        /// </summary>
        /// <param name="moves">
        /// Moves list that will be populated with lazy moves.
        /// </param>
        /// <param name="movesType">
        /// Types of moves to include. e.g. All, or captures-only.
        /// </param>
        public void GenerateLazyMoves(Moves moves, Moves.MoveListNames movesType)
        {

            for (int i = 0; i < moveVectors.Length; i++)
            {
                Board.AppendPiecePath(moves, this.Base, this.Base.Player, moveVectors[i], movesType);
            }
        
        }

        public bool CanAttackSquare(Square target_square)
        {
            int intOrdinal = this.Base.Square.Ordinal;
            Square square;

            for (int i = 0; i < moveVectors.Length; i++)
            {
                intOrdinal = this.Base.Square.Ordinal + moveVectors[i];
                while ((square = Board.GetSquare(intOrdinal)) != null)
                {
                    if (square.Ordinal == target_square.Ordinal)
                        return true;

                    if (square.Piece == null)
                    {
                        intOrdinal += moveVectors[i];
                        continue;
                    }
                    else
                        break;
                }
            }
            return false;
        }

        #endregion

        #region Static methods

        static private Piece.PieceNames _pieceType = Piece.PieceNames.Queen;

        /// <summary>
        ///  static method to determine if a square is attacked by this piece
        /// </summary>
        /// <param name="square"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        static public bool DoesPieceAttackSquare(Square square, Player player)
        {
            for (int i = 0; i < moveVectors.Length; i++)
            {
                if (Board.LinesFirstPiece(player.Colour, _pieceType, square, moveVectors[i]) != null)
                {
                    return true;
                }
            }
            return false;

        }

        static public bool DoesPieceAttackSquare(Square square, Player player, out Piece attackingPiece)
        {
            attackingPiece = null;
            for (int i = 0; i < moveVectors.Length; i++)
            {
                if (Board.LinesFirstPiece(player.Colour, _pieceType, square, moveVectors[i]) != null)
                {
                    attackingPiece = Board.LinesFirstPiece(player.Colour, _pieceType, square, moveVectors[i]);
                    return true;
                }
            }
            return false;
        }

        #endregion     
    
    }
}