// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieceKing.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The piece king.
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
    /// The piece king.
    /// </summary>
    public class PieceKing : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        /// The m_aint square values.
        /// </summary>
        private static readonly int[] m_aintSquareValues =
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

        /// <summary>
        /// The check values.
        /// </summary>
        private static int[] CheckValues = { 0, 60, 180, 360, 500 };

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
                return "K";
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
                return 15;
            }
        }

        /// <summary>
        /// Gets a value indicating whether CanCastleKingSide.
        /// </summary>
        public bool CanCastleKingSide
        {
            get
            {
                Piece pieceRook;

                // King hasnt moved
                if (this.m_Base.HasMoved)
                {
                    return false;
                }

                // Rook is still there i.e. hasnt been taken
                pieceRook = this.m_Base.Player.Colour == Player.enmColour.White ? Board.GetPiece(7, 0) : Board.GetPiece(7, 7);
                if (pieceRook == null || pieceRook.Name != Piece.enmName.Rook || pieceRook.Player.Colour != this.m_Base.Player.Colour)
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
                if (Board.GetPiece(this.m_Base.Square.Ordinal + 1) != null)
                {
                    return false;
                }

                if (Board.GetPiece(this.m_Base.Square.Ordinal + 2) != null)
                {
                    return false;
                }

                // King is not in check
                if (this.m_Base.Player.IsInCheck)
                {
                    return false;
                }

                // The king does not move over a square that is attacked by an enemy piece during the castling move
                if (Board.GetSquare(this.m_Base.Square.Ordinal + 1).CanBeMovedToBy(this.m_Base.Player.OtherPlayer))
                {
                    return false;
                }

                if (Board.GetSquare(this.m_Base.Square.Ordinal + 2).CanBeMovedToBy(this.m_Base.Player.OtherPlayer))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether CanCastleQueenSide.
        /// </summary>
        public bool CanCastleQueenSide
        {
            get
            {
                Piece pieceRook;

                // King hasnt moved
                if (this.m_Base.HasMoved)
                {
                    return false;
                }

                // Rook is still there i.e. hasnt been taken
                pieceRook = this.m_Base.Player.Colour == Player.enmColour.White ? Board.GetPiece(0, 0) : Board.GetPiece(0, 7);
                if (pieceRook == null || pieceRook.Name != Piece.enmName.Rook || pieceRook.Player.Colour != this.m_Base.Player.Colour)
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
                if (Board.GetPiece(this.m_Base.Square.Ordinal - 1) != null)
                {
                    return false;
                }

                if (Board.GetPiece(this.m_Base.Square.Ordinal - 2) != null)
                {
                    return false;
                }

                if (Board.GetPiece(this.m_Base.Square.Ordinal - 3) != null)
                {
                    return false;
                }

                // King is not in check
                if (this.m_Base.Player.IsInCheck)
                {
                    return false;
                }

                // The king does not move over a square that is attacked by an enemy piece during the castling move
                if (Board.GetSquare(this.m_Base.Square.Ordinal - 1).CanBeMovedToBy(this.m_Base.Player.OtherPlayer))
                {
                    return false;
                }

                if (Board.GetSquare(this.m_Base.Square.Ordinal - 2).CanBeMovedToBy(this.m_Base.Player.OtherPlayer))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets ImageIndex.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.m_Base.Player.Colour == Player.enmColour.White ? 5 : 4;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsCapturable.
        /// </summary>
        public bool IsCapturable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets Name.
        /// </summary>
        public Piece.enmName Name
        {
            get
            {
                return Piece.enmName.King;
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

                if (Game.Stage != Game.GameStageNames.Opening && this.Base.Player.OtherPlayer.HasPieceName(Piece.enmName.Queen))
                {
                    Piece piece;

                    // Penalty for not having pawn directly in front
                    piece = Board.GetPiece(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnForwardOffset);
                    if (piece == null || piece.Name != Piece.enmName.Pawn || piece.Player.Colour != this.m_Base.Player.Colour)
                    {
                        intPoints -= 75;
                        piece = Board.GetPiece(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnForwardOffset * 2);
                        if (piece == null || piece.Name != Piece.enmName.Pawn || piece.Player.Colour != this.m_Base.Player.Colour)
                        {
                            intPoints -= 150;
                        }
                    }

                    // Penalty for first movement the king, other than castling. This is to stop the king dancing around its
                    // own pawns in an attempt to get better protection, at the expense of developing other pieces.
                    if (this.m_Base.Player.HasCastled)
                    {
                        if (this.m_Base.NoOfMoves >= 2)
                        {
                            intPoints -= 200;
                        }
                    }
                    else
                    {
                        if (this.m_Base.NoOfMoves >= 1)
                        {
                            intPoints -= 200;
                        }
                    }

                    // Penalty for number of open lines to king
                    intPoints -= this.Openness(this.m_Base.Square);

                    // Penalty for half-open adjacent files
                    bool blnHasFiendlyPawn;
                    Square squareThis;

                    blnHasFiendlyPawn = false;
                    squareThis = Board.GetSquare(this.m_Base.Square.File, this.m_Base.Square.Rank);
                    while (squareThis != null)
                    {
                        piece = squareThis.Piece;
                        if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
                        {
                            blnHasFiendlyPawn = true;
                            break;
                        }

                        squareThis = Board.GetSquare(squareThis.Ordinal + this.m_Base.Player.PawnForwardOffset);
                    }

                    if (!blnHasFiendlyPawn)
                    {
                        intPoints -= 200;
                    }

                    blnHasFiendlyPawn = false;
                    squareThis = Board.GetSquare(this.m_Base.Square.File + 1, this.m_Base.Square.Rank);
                    while (squareThis != null)
                    {
                        piece = squareThis.Piece;
                        if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
                        {
                            blnHasFiendlyPawn = true;
                            break;
                        }

                        squareThis = Board.GetSquare(squareThis.Ordinal + this.m_Base.Player.PawnForwardOffset);
                    }

                    if (!blnHasFiendlyPawn)
                    {
                        intPoints -= 200;
                    }

                    blnHasFiendlyPawn = false;
                    squareThis = Board.GetSquare(this.m_Base.Square.File - 1, this.m_Base.Square.Rank);
                    while (squareThis != null)
                    {
                        piece = squareThis.Piece;
                        if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
                        {
                            blnHasFiendlyPawn = true;
                            break;
                        }

                        squareThis = Board.GetSquare(squareThis.Ordinal + this.m_Base.Player.PawnForwardOffset);
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
                        this.GenerateLazyMoves(moves, Moves.enmMovesType.All);
                        intPoints += moves.Count * 10;

                        // Bonus for being in centre of board
                        intPoints += m_aintSquareValues[this.m_Base.Square.Ordinal];
                        break;

                    default: // Opening & Middle

                        // Penalty for being in centre of board
                        intPoints -= m_aintSquareValues[this.m_Base.Square.Ordinal];

                        break;
                }

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
                return 15000;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The determine check status.
        /// </summary>
        /// <returns>
        /// The determine check status.
        /// </returns>
        public bool DetermineCheckStatus()
        {
            return this.m_Base.Square.CanBeMovedToBy(this.m_Base.Player.OtherPlayer);
        }

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
                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 1);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 15);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 16);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 17);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 1);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 15);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 16);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 17);
                    if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    if (this.CanCastleKingSide)
                    {
                        moves.Add(0, 0, Move.enmName.CastleKingSide, this.m_Base, this.m_Base.Square, Board.GetSquare(this.m_Base.Square.Ordinal + 2), null, 0, 0);
                    }

                    if (this.CanCastleQueenSide)
                    {
                        moves.Add(Game.TurnNo, this.m_Base.LastMoveTurnNo, Move.enmName.CastleQueenSide, this.m_Base, this.m_Base.Square, Board.GetSquare(this.m_Base.Square.Ordinal - 2), null, 0, 0);
                    }

                    break;

                case Moves.enmMovesType.CapturesChecksPromotions:
                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 1);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 15);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 16);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 17);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal + 1);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 15);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 16);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    square = Board.GetSquare(this.m_Base.Square.Ordinal - 17);
                    if (square != null && (square.Piece != null && (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
                    {
                        moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }

                    break;
            }
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

            square = Board.GetSquare(this.m_Base.Square.Ordinal - 1);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.m_Base.Square.Ordinal + 15);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.m_Base.Square.Ordinal + 16);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.m_Base.Square.Ordinal + 17);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.m_Base.Square.Ordinal + 1);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.m_Base.Square.Ordinal - 15);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.m_Base.Square.Ordinal - 16);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }

            square = Board.GetSquare(this.m_Base.Square.Ordinal - 17);
            if (square != null && (square.Piece == null || (square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)))
            {
                squares.Add(square);
            }
        }

        /// <summary>
        /// The open line penalty.
        /// </summary>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <param name="squareKing">
        /// The square king.
        /// </param>
        /// <param name="intDirectionOffset">
        /// The int direction offset.
        /// </param>
        /// <returns>
        /// The open line penalty.
        /// </returns>
        private int OpenLinePenalty(Player.enmColour colour, Square squareKing, int intDirectionOffset)
        {
            Square square = Board.GetSquare(squareKing.Ordinal + intDirectionOffset);
            Piece piece;
            int intPenalty = 0;

            while (square != null)
            {
                piece = square.Piece;
                if (piece != null)
                {
                    if (piece.Player.Colour == colour && piece.Name == Piece.enmName.Pawn)
                    {
                        break;
                    }
                }

                intPenalty += 10;
                square = Board.GetSquare(square.Ordinal + intDirectionOffset);
            }

            return intPenalty;
        }

        /// <summary>
        /// The openness.
        /// </summary>
        /// <param name="squareKing">
        /// The square king.
        /// </param>
        /// <returns>
        /// The openness.
        /// </returns>
        private int Openness(Square squareKing)
        {
            Square square = squareKing;

            int intOpenness = 0;
            intOpenness += Board.OpenLinePenalty(this.m_Base.Player.Colour, square, 16);
            if (intOpenness > 900)
            {
                goto exitpoint;
            }

            intOpenness += Board.OpenLinePenalty(this.m_Base.Player.Colour, square, 17);
            if (intOpenness > 900)
            {
                goto exitpoint;
            }

            // intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,  1); if (intOpenness>900) goto exitpoint;
            intOpenness += Board.OpenLinePenalty(this.m_Base.Player.Colour, square, -15);
            if (intOpenness > 900)
            {
                goto exitpoint;
            }

            intOpenness += Board.OpenLinePenalty(this.m_Base.Player.Colour, square, -16);
            if (intOpenness > 900)
            {
                goto exitpoint;
            }

            intOpenness += Board.OpenLinePenalty(this.m_Base.Player.Colour, square, -17);
            if (intOpenness > 900)
            {
                goto exitpoint;
            }

            // intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, -1); if (intOpenness>900) goto exitpoint;
            intOpenness += Board.OpenLinePenalty(this.m_Base.Player.Colour, square, 15);
            if (intOpenness > 900)
            {
                goto exitpoint;
            }

            /*
                        square = Board.GetSquare(squareKing.Ordinal-1);
                        if (square!=null)
                        {
                            intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
                            intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
                            intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
                            intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
                        }

                        square = Board.GetSquare(squareKing.Ordinal+1);
                        if (square!=null)
                        {
                            intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 17); if (intOpenness>900) goto exitpoint;
                            intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-15); if (intOpenness>900) goto exitpoint;
                            intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square,-17); if (intOpenness>900) goto exitpoint;
                            intOpenness += Board.LineIsOpen(this.m_Base.Player.Colour, square, 15); if (intOpenness>900) goto exitpoint;
                        }
            */
            exitpoint:
            return intOpenness;
        }

        /// <summary>
        /// The pawn is adjacent.
        /// </summary>
        /// <param name="intOrdinal">
        /// The int ordinal.
        /// </param>
        /// <returns>
        /// The pawn is adjacent.
        /// </returns>
        private bool PawnIsAdjacent(int intOrdinal)
        {
            Piece piece;
            piece = Board.GetPiece(intOrdinal + 15);
            if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
            {
                return true;
            }

            piece = Board.GetPiece(intOrdinal + 16);
            if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
            {
                return true;
            }

            piece = Board.GetPiece(intOrdinal + 17);
            if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
            {
                return true;
            }

            piece = Board.GetPiece(intOrdinal - 15);
            if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
            {
                return true;
            }

            piece = Board.GetPiece(intOrdinal - 16);
            if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
            {
                return true;
            }

            piece = Board.GetPiece(intOrdinal - 17);
            if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
            {
                return true;
            }

            piece = Board.GetPiece(intOrdinal + 1);
            if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
            {
                return true;
            }

            piece = Board.GetPiece(intOrdinal - 1);
            if (piece != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}