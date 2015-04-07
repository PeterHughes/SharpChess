// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieceRook.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The piece rook.
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
    /// The piece rook.
    /// </summary>
    public class PieceRook : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        /// Simple positional piece-square score values.
        /// </summary>
        private static readonly int[] SquareValues = 
        { 
            10, 10, 10, 10, 10, 10, 10, 10,    0, 0, 0, 0, 0, 0, 0, 0, 
            10, 20, 20, 20, 20, 20, 20, 10,    0, 0, 0, 0, 0, 0, 0, 0, 
            10, 20, 30, 30, 30, 30, 20, 10,    0, 0, 0, 0, 0, 0, 0, 0, 
            10, 20, 30, 40, 40, 30, 20, 10,    0, 0, 0, 0, 0, 0, 0, 0, 
            10, 20, 30, 40, 40, 30, 20, 10,    0, 0, 0, 0, 0, 0, 0, 0, 
            10, 20, 30, 30, 30, 30, 20, 10,    0, 0, 0, 0, 0, 0, 0, 0, 
            10, 20, 20, 20, 20, 20, 20, 10,    0, 0, 0, 0, 0, 0, 0, 0, 
            10, 10, 10, 10, 10, 10, 10, 10,    0, 0, 0, 0, 0, 0, 0, 0
        };

        /// <summary>
        /// Directional vectors of where the piece can go
        /// </summary>
        public static int[] moveVectors = { 1, -1, 16, -16 };

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
                return "R";
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
                return 5;
            }
        }

        /// <summary>
        /// Gets the image index for this piece. Used to determine which graphic image is displayed for thie piece.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.Base.Player.Colour == Player.PlayerColourNames.White ? 3 : 2;
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
                return Piece.PieceNames.Rook;
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

                // After the opening, Rooks are penalized slightly depending on "taxicab" distance to the enemy king.
                if (Game.Stage != Game.GameStageNames.Opening)
                {
                    intPoints -= this.Base.TaxiCabDistanceToEnemyKingPenalty();
                }

                if (Game.Stage != Game.GameStageNames.End)
                {
                    // Rooks are given a bonus of 10(0) points for occupying a file with no friendly pawns and a bonus of 
                    // 4(0) points if no enemy pawns lie on that file. 
                    bool blnHasFiendlyPawn = false;
                    bool blnHasEnemyPawn = false;
                    Square squareThis = Board.GetSquare(this.Base.Square.File, 0);
                    while (squareThis != null)
                    {
                        Piece piece = squareThis.Piece;
                        if (piece != null && piece.Name == Piece.PieceNames.Pawn)
                        {
                            if (piece.Player.Colour == this.Base.Player.Colour)
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
                    if (
                        (this.Base.Player.Colour == Player.PlayerColourNames.White && this.Base.Square.Rank == 6)
                        || 
                        (this.Base.Player.Colour == Player.PlayerColourNames.Black && this.Base.Square.Rank == 1))
                    {
                        intPoints += 30;
                    }
                }

                // Mobility
                Squares squares = new Squares();
                squares.Add(this.Base.Square);
                Board.LineThreatenedBy(this.Base.Player, squares, this.Base.Square, 1);
                Board.LineThreatenedBy(this.Base.Player, squares, this.Base.Square, -1);
                Board.LineThreatenedBy(this.Base.Player, squares, this.Base.Square, 16);
                Board.LineThreatenedBy(this.Base.Player, squares, this.Base.Square, -16);

                int intSquareValue = 0;
                foreach (Square square in squares)
                {
                    intSquareValue += SquareValues[square.Ordinal];
                }

                intPoints += intSquareValue >> 2;

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
                return 5000;
                    
                    // - ((m_Base.Player.PawnsInPlay-5) * 125);  // lower the rook's value by 1/8 for each pawn above five of the side being valued, with the opposite adjustment for each pawn short of five
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

        static private Piece.PieceNames _pieceType = Piece.PieceNames.Rook;

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