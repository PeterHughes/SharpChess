// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieceKing.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The piece king.
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
    /// The piece king.
    /// </summary>
    public class PieceKing : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        /// Simple positional piece-square score values.
        /// </summary>
        private static readonly int[] SquareValues =
        {
            1, 1,  1,  1,  1,  1, 1, 1,   0, 0, 0, 0, 0, 0, 0, 0, 
            1, 7,  7,  7,  7,  7, 7, 1,   0, 0, 0, 0, 0, 0, 0, 0, 
            1, 7, 18, 18, 18, 18, 7, 1,   0, 0, 0, 0, 0, 0, 0, 0, 
            1, 7, 18, 27, 27, 18, 7, 1,   0, 0, 0, 0, 0, 0, 0, 0, 
            1, 7, 18, 27, 27, 18, 7, 1,   0, 0, 0, 0, 0, 0, 0, 0, 
            1, 7, 18, 18, 18, 18, 7, 1,   0, 0, 0, 0, 0, 0, 0, 0, 
            1, 7,  7,  7,  7,  7, 7, 1,   0, 0, 0, 0, 0, 0, 0, 0, 
            1, 1,  1,  1,  1,  1, 1, 1,   0, 0, 0, 0, 0, 0, 0, 0
        };

        /// <summary>
        /// Directional vectors of where the piece can go
        /// </summary>
        public static int[] moveVectors = { 1, 15, 16, 17, -1, -15, -16, -17 };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PieceKing"/> class.
        /// </summary>
        /// <param name="pieceBase">
        /// The piece base.
        /// </param>
        public PieceKing(Piece pieceBase)
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
                return "K";
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
                return 15;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the player can castle on the king side.
        /// </summary>
        public bool CanCastleKingSide
        {
            get
            {
                // King hasnt moved
                if (this.Base.HasMoved)
                {
                    return false;
                }

                // Rook is still there i.e. hasnt been taken
                Piece pieceRook = this.Base.Player.Colour == Player.PlayerColourNames.White ? Board.GetPiece(7, 0) : Board.GetPiece(7, 7);
                if (pieceRook == null || pieceRook.Name != Piece.PieceNames.Rook || pieceRook.Player.Colour != this.Base.Player.Colour)
                {
                    return false;
                }

                if (!pieceRook.IsInPlay)
                {
                    return false;
                }

                // King's Rook has moved
                if (pieceRook.HasMoved)
                {
                    return false;
                }

                // All squares between King and Rook are unoccupied
                if (Board.GetPiece(this.Base.Square.Ordinal + 1) != null)
                {
                    return false;
                }

                if (Board.GetPiece(this.Base.Square.Ordinal + 2) != null)
                {
                    return false;
                }

                // King is not in check
                if (this.Base.Player.IsInCheck)
                {
                    return false;
                }

                // The king does not move over a square that is attacked by an enemy piece during the castling move
                if (Board.GetSquare(this.Base.Square.Ordinal + 1).PlayerCanAttackSquare(this.Base.Player.OpposingPlayer))
                {
                    return false;
                }

                if (Board.GetSquare(this.Base.Square.Ordinal + 2).PlayerCanAttackSquare(this.Base.Player.OpposingPlayer))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the player can castle on the queen side.
        /// </summary>
        public bool CanCastleQueenSide
        {
            get
            {
                // King hasnt moved
                if (this.Base.HasMoved)
                {
                    return false;
                }

                // Rook is still there i.e. hasnt been taken
                Piece pieceRook = this.Base.Player.Colour == Player.PlayerColourNames.White ? Board.GetPiece(0, 0) : Board.GetPiece(0, 7);
                if (pieceRook == null || pieceRook.Name != Piece.PieceNames.Rook || pieceRook.Player.Colour != this.Base.Player.Colour)
                {
                    return false;
                }

                if (!pieceRook.IsInPlay)
                {
                    return false;
                }

                // King's Rook hasnt moved
                if (pieceRook.HasMoved)
                {
                    return false;
                }

                // All squares between King and Rook are unoccupied
                if (Board.GetPiece(this.Base.Square.Ordinal - 1) != null)
                {
                    return false;
                }

                if (Board.GetPiece(this.Base.Square.Ordinal - 2) != null)
                {
                    return false;
                }

                if (Board.GetPiece(this.Base.Square.Ordinal - 3) != null)
                {
                    return false;
                }

                // King is not in check
                if (this.Base.Player.IsInCheck)
                {
                    return false;
                }

                // The king does not move over a square that is attacked by an enemy piece during the castling move
                if (Board.GetSquare(this.Base.Square.Ordinal - 1).PlayerCanAttackSquare(this.Base.Player.OpposingPlayer))
                {
                    return false;
                }

                if (Board.GetSquare(this.Base.Square.Ordinal - 2).PlayerCanAttackSquare(this.Base.Player.OpposingPlayer))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the image index for this piece. Used to determine which graphic image is displayed for thie piece.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.Base.Player.Colour == Player.PlayerColourNames.White ? 5 : 4;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the piece is capturable. Kings aren't, everything else is.
        /// </summary>
        public bool IsCapturable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the piece's name.
        /// </summary>
        public Piece.PieceNames Name
        {
            get
            {
                return Piece.PieceNames.King;
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

                if (Game.Stage != Game.GameStageNames.Opening && this.Base.Player.OpposingPlayer.HasPieceName(Piece.PieceNames.Queen))
                {
                    // Penalty for not having pawn directly in front
                    Piece piece = Board.GetPiece(this.Base.Square.Ordinal + this.Base.Player.PawnForwardOffset);
                    if (piece == null || piece.Name != Piece.PieceNames.Pawn || piece.Player.Colour != this.Base.Player.Colour)
                    {
                        intPoints -= 75;
                        piece = Board.GetPiece(this.Base.Square.Ordinal + (this.Base.Player.PawnForwardOffset * 2));
                        if (piece == null || piece.Name != Piece.PieceNames.Pawn || piece.Player.Colour != this.Base.Player.Colour)
                        {
                            intPoints -= 150;
                        }
                    }

                    // Penalty for first movement the king, other than castling. This is to stop the king dancing around its
                    // own pawns in an attempt to get better protection, at the expense of developing other pieces.
                    if (this.Base.Player.HasCastled)
                    {
                        if (this.Base.NoOfMoves >= 2)
                        {
                            intPoints -= 200;
                        }
                    }
                    else
                    {
                        if (this.Base.NoOfMoves >= 1)
                        {
                            intPoints -= 200;
                        }
                    }

                    // Penalty for number of open lines to king
                    intPoints -= this.Openness(this.Base.Square);

                    // Penalty for half-open adjacent files
                    bool blnHasFiendlyPawn = false;
                    Square squareThis = Board.GetSquare(this.Base.Square.File, this.Base.Square.Rank);
                    while (squareThis != null)
                    {
                        piece = squareThis.Piece;
                        if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == this.Base.Player.Colour)
                        {
                            blnHasFiendlyPawn = true;
                            break;
                        }

                        squareThis = Board.GetSquare(squareThis.Ordinal + this.Base.Player.PawnForwardOffset);
                    }

                    if (!blnHasFiendlyPawn)
                    {
                        intPoints -= 200;
                    }

                    blnHasFiendlyPawn = false;
                    squareThis = Board.GetSquare(this.Base.Square.File + 1, this.Base.Square.Rank);
                    while (squareThis != null)
                    {
                        piece = squareThis.Piece;
                        if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == this.Base.Player.Colour)
                        {
                            blnHasFiendlyPawn = true;
                            break;
                        }

                        squareThis = Board.GetSquare(squareThis.Ordinal + this.Base.Player.PawnForwardOffset);
                    }

                    if (!blnHasFiendlyPawn)
                    {
                        intPoints -= 200;
                    }

                    blnHasFiendlyPawn = false;
                    squareThis = Board.GetSquare(this.Base.Square.File - 1, this.Base.Square.Rank);
                    while (squareThis != null)
                    {
                        piece = squareThis.Piece;
                        if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == this.Base.Player.Colour)
                        {
                            blnHasFiendlyPawn = true;
                            break;
                        }

                        squareThis = Board.GetSquare(squareThis.Ordinal + this.Base.Player.PawnForwardOffset);
                    }

                    if (!blnHasFiendlyPawn)
                    {
                        intPoints -= 200;
                    }
                }

                switch (Game.Stage)
                {
                    case Game.GameStageNames.End:

                        // Bonus for number of moves available
                        Moves moves = new Moves();
                        this.GenerateLazyMoves(moves, Moves.MoveListNames.All);
                        intPoints += moves.Count * 10;

                        // Bonus for being in centre of board
                        intPoints += SquareValues[this.Base.Square.Ordinal];
                        break;

                    default: // Opening & Middle

                        // Penalty for being in centre of board
                        intPoints -= SquareValues[this.Base.Square.Ordinal];

                        break;
                }

                return intPoints;
            }
        }

        /// <summary>
        /// Gets the material value of the piece.
        /// </summary>
        public int Value
        {
            get
            {
                return 15000;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determine if the king is in check.
        /// </summary>
        /// <returns>
        /// Returns true if the king is in check.
        /// </returns>
        public bool DetermineCheckStatus()
        {
            return this.Base.Square.PlayerCanAttackSquare(this.Base.Player.OpposingPlayer);
        }

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
            Square square;
            switch (movesType)
            {
                case Moves.MoveListNames.All:
                    for (int i = 0; i < moveVectors.Length; i++)
                    {
                        square = Board.GetSquare(this.Base.Square.Ordinal + moveVectors[i]);
                        if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
                        {
                            moves.Add(0, 0, Move.MoveNames.Standard, this.Base, this.Base.Square, square, square.Piece, 0, 0);
                        }
                    } 

                    if (this.CanCastleKingSide)
                    {
                        moves.Add(0, 0, Move.MoveNames.CastleKingSide, this.Base, this.Base.Square, Board.GetSquare(this.Base.Square.Ordinal + 2), null, 0, 0);
                    }

                    if (this.CanCastleQueenSide)
                    {
                        moves.Add(Game.TurnNo, this.Base.LastMoveTurnNo, Move.MoveNames.CastleQueenSide, this.Base, this.Base.Square, Board.GetSquare(this.Base.Square.Ordinal - 2), null, 0, 0);
                    }

                    break;

                case Moves.MoveListNames.CapturesPromotions:
                    for (int i = 0; i < moveVectors.Length; i++)
                    {
                        square = Board.GetSquare(this.Base.Square.Ordinal + moveVectors[i]);
                        if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
                        {
                            moves.Add(0, 0, Move.MoveNames.Standard, this.Base, this.Base.Square, square, square.Piece, 0, 0);
                        }
                    }
                    break;                    
            }
        }

        public bool CanAttackSquare(Square target_square)
        {
            Square square;
            for (int i = 0; i < moveVectors.Length; i++)
            {
                square = Board.GetSquare(this.Base.Square.Ordinal + moveVectors[i]);
                if (square != null && square.Ordinal == target_square.Ordinal)
                    return true;
            }
            return false;
        }

        #endregion

        #region Static methods

        static private Piece.PieceNames _pieceType = Piece.PieceNames.King;

        /// <summary>
        ///  static method to determine if a square is attacked by this piece
        /// </summary>
        /// <param name="square"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        static public bool DoesPieceAttackSquare(Square square, Player player)            
        {
            return Piece.DoesLeaperPieceTypeAttackSquare(square, player, _pieceType, moveVectors);
        }

        static public bool DoesPieceAttackSquare(Square square, Player player, out Piece attackingPiece)
        {
            return Piece.DoesLeaperPieceTypeAttackSquare(square, player, _pieceType, moveVectors, out attackingPiece);
        }

        #endregion 

        #region Methods

        /// <summary>
        /// The move squares.
        /// </summary>
        /// <param name="squares">
        /// The squares.
        /// </param>
        private void MoveSquares(ref Squares squares)
        {
            Square square;

            square = Board.GetSquare(this.Base.Square.Ordinal - 1);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.Base.Square.Ordinal + 15);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.Base.Square.Ordinal + 16);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.Base.Square.Ordinal + 17);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.Base.Square.Ordinal + 1);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.Base.Square.Ordinal - 15);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.Base.Square.Ordinal - 16);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.Base.Square.Ordinal - 17);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }
        }

        /// <summary>
        /// Calculate a positonal score penalty based upon the openness of the King.
        /// </summary>
        /// <param name="squareKing">
        /// The square king.
        /// </param>
        /// <returns>
        /// The openness penalty.
        /// </returns>
        private int Openness(Square squareKing)
        {
            Square square = squareKing;

            int intOpenness = 0;

            for (int i = 0; i < moveVectors.Length; i++)
            {
                intOpenness += Board.OpenLinePenalty(this.Base.Player.Colour, square, moveVectors[i]);
                if (intOpenness > 900)
                    return intOpenness;
            }
            return intOpenness;
        }
        #endregion
    }
}
