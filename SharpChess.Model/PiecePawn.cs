// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PiecePawn.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The piece pawn.
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
    #region Using

    using System;

    using SharpChess.Model.AI;

    #endregion

    /// <summary>
    /// The piece pawn.
    /// </summary>
    public class PiecePawn : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        ///   The pawn advancement bonus.
        /// </summary>
        private static readonly int[] AdvancementBonus = { 0, 0, 0, 45, 75, 120, 240, 999 };

        /// <summary>
        ///   The pawn file bonus.
        /// </summary>
        private static readonly int[] FileBonus = { 0, 6, 16, 32, 32, 16, 6, 0 };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PiecePawn"/> class.
        /// </summary>
        /// <param name="pieceBase">
        /// The piece base.
        /// </param>
        public PiecePawn(Piece pieceBase)
        {
            this.Base = pieceBase;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets Abbreviation.
        /// </summary>
        public string Abbreviation
        {
            get
            {
                return "P";
            }
        }

        /// <summary>
        ///   Gets the base part of the piece. i.e. the bit that sits on the chess square.
        /// </summary>
        public Piece Base { get; private set; }

        /// <summary>
        ///   Gets basic value of the piece. e.g. pawn = 1, bishop = 3, queen = 9
        /// </summary>
        public int BasicValue
        {
            get
            {
                return 1;
            }
        }

        /*
        public int DefencePoints
        {
            get 
            {
                // Add further subtraction, then add on a defense bonus
                if (blnIsIsolatedLeft || blnIsIsolatedRight || blnIsBackward)
                {
                    switch (Game.Stage)
                    {
                        case Game.enmStage.Opening:
                            intPoints += (m_Base.DefensePoints>>1) - 30;
                            break;
                        case Game.enmStage.Middle:
                            intPoints += (m_Base.DefensePoints>>2) - 15;
                            break;
                    }
                }
            }
        }
*/

        /// <summary>
        ///   Gets the image index for this piece. Used to determine which graphic image is displayed for thie piece.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.Base.Player.Colour == Player.PlayerColourNames.White ? 9 : 8;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether the piece is capturable. Kings aren't, everything else is.
        /// </summary>
        public bool IsCapturable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///   Gets the piece's name.
        /// </summary>
        public Piece.PieceNames Name
        {
            get
            {
                return Piece.PieceNames.Pawn;
            }
        }

        /// <summary>
        ///   Gets the positional points assigned to this piece.
        /// </summary>
        public int PositionalPoints
        {
            get
            {
                return this.PositionalPointsCacheable;
            }
        }

        /// <summary>
        ///   Gets the positional points assigned to this piece, which are safe to cache in the Pawn hash table <see cref="HashTablePawn"/>.
        ///   Position values are cachable if they are affected *exclusively* to pawn position.
        /// </summary>
        private int PositionalPointsCacheable
        {
            get
            {
                int intPoints = 0;
                int intIndex;
                Piece piece;

                // PENALITIES

                // Isolated or Doubled pawn
                bool blnIsIsolatedLeft = true;
                bool blnIsIsolatedRight = true;
                bool blnIsDouble = false;
                for (intIndex = this.Base.Player.Pieces.Count - 1; intIndex >= 0; intIndex--)
                {
                    piece = this.Base.Player.Pieces.Item(intIndex);
                    if (piece.Name == Piece.PieceNames.Pawn && piece != this.Base)
                    {
                        if (piece.Square.File == this.Base.Square.File)
                        {
                            blnIsDouble = true;
                        }

                        if (piece.Square.File == this.Base.Square.File - 1)
                        {
                            blnIsIsolatedLeft = false;
                        }

                        if (piece.Square.File == this.Base.Square.File + 1)
                        {
                            blnIsIsolatedRight = false;
                        }
                    }
                }

                if (blnIsIsolatedLeft)
                {
                    switch (this.Base.Square.File)
                    {
                        case 0:
                            intPoints -= 10;
                            break;
                        case 1:
                            intPoints -= 30;
                            break;
                        case 2:
                            intPoints -= 40;
                            break;
                        case 3:
                            intPoints -= 50;
                            break;
                        case 4:
                            intPoints -= 50;
                            break;
                        case 5:
                            intPoints -= 40;
                            break;
                        case 6:
                            intPoints -= 30;
                            break;
                        case 7:
                            intPoints -= 10;
                            break;
                    }
                }

                if (blnIsIsolatedRight)
                {
                    switch (this.Base.Square.File)
                    {
                        case 0:
                            intPoints -= 10;
                            break;
                        case 1:
                            intPoints -= 30;
                            break;
                        case 2:
                            intPoints -= 40;
                            break;
                        case 3:
                            intPoints -= 50;
                            break;
                        case 4:
                            intPoints -= 50;
                            break;
                        case 5:
                            intPoints -= 40;
                            break;
                        case 6:
                            intPoints -= 30;
                            break;
                        case 7:
                            intPoints -= 10;
                            break;
                    }
                }

                if (blnIsDouble)
                {
                    intPoints -= 75;
                }
                else if (blnIsIsolatedLeft && blnIsIsolatedRight)
                {
                    intPoints -= 100;
                }

                // Backward pawn
                bool blnIsBackward = !((piece = Board.GetPiece(this.Base.Square.Ordinal - 1)) != null
                                       && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == this.Base.Player.Colour);

                if (blnIsBackward && (piece = Board.GetPiece(this.Base.Square.Ordinal + 1)) != null
                    && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == this.Base.Player.Colour)
                {
                    blnIsBackward = false;
                }

                if (blnIsBackward
                    &&
                    (piece = Board.GetPiece(this.Base.Square.Ordinal - this.Base.Player.PawnAttackLeftOffset)) != null
                    && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == this.Base.Player.Colour)
                {
                    blnIsBackward = false;
                }

                if (blnIsBackward
                    &&
                    (piece = Board.GetPiece(this.Base.Square.Ordinal - this.Base.Player.PawnAttackRightOffset)) != null
                    && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == this.Base.Player.Colour)
                {
                    blnIsBackward = false;
                }

                if (blnIsBackward)
                {
                    intPoints -= 30;
                }

                if (Game.Stage != Game.GameStageNames.End)
                {
                    // Pawns on D or E files
                    if (this.Base.Square.File == 3 || this.Base.Square.File == 4)
                    {
                        // still at rank 2
                        if ((this.Base.Player.Colour == Player.PlayerColourNames.White && this.Base.Square.Rank == 1)
                            || (this.Base.Player.Colour == Player.PlayerColourNames.Black && this.Base.Square.Rank == 6))
                        {
                            intPoints -= 200;
                        }
                    }
                }

                if (Game.Stage == Game.GameStageNames.Opening)
                {
                    // Pawns on D or E files
                    if (this.Base.Square.File == 3 || this.Base.Square.File == 4)
                    {
                        // Bonus for rank 5
                        if ((this.Base.Player.Colour == Player.PlayerColourNames.White && this.Base.Square.Rank == 3)
                            || (this.Base.Player.Colour == Player.PlayerColourNames.Black && this.Base.Square.Rank == 4))
                        {
                            intPoints += 150;
                        }
                    }
                }

                // BONUSES

                // Encourage capturing towards the centre
                intPoints += FileBonus[this.Base.Square.File];

                // Advancement
                int intAdvancementBonus =
                    AdvancementBonus[
                        this.Base.Player.Colour == Player.PlayerColourNames.White
                            ? this.Base.Square.Rank
                            : 7 - this.Base.Square.Rank];

                // Passed Pawns
                bool blnIsPassed = true;
                for (intIndex = this.Base.Player.OpposingPlayer.Pieces.Count - 1; intIndex >= 0; intIndex--)
                {
                    piece = this.Base.Player.OpposingPlayer.Pieces.Item(intIndex);
                    if (piece.Name == Piece.PieceNames.Pawn)
                    {
                        if (((this.Base.Player.Colour == Player.PlayerColourNames.White
                              && piece.Square.Rank > this.Base.Square.Rank)
                             ||
                             (this.Base.Player.Colour == Player.PlayerColourNames.Black
                              && piece.Square.Rank < this.Base.Square.Rank))
                            &&
                            (piece.Square.File == this.Base.Square.File
                             || piece.Square.File == this.Base.Square.File - 1
                             || piece.Square.File == this.Base.Square.File + 1))
                        {
                            blnIsPassed = false;
                        }
                    }
                }

                if (blnIsPassed)
                {
                    intPoints += 80;
                    intAdvancementBonus <<= 1;
                }

                // Slowly increase advancement bonus as material drops away.
                intAdvancementBonus = intAdvancementBonus * ((Game.MaxMaterialCount - Game.LowestMaterialCount) * 2)
                                      / Game.MaxMaterialCount;

                intPoints += intAdvancementBonus;

                // +this.PawnForkTwoMajorPiecesBonus(); // 15Mar06 Nimzo Added pawn fork bonus
                return intPoints;
            }
        }

        /// <summary>
        ///   Gets the material value of this piece.
        /// </summary>
        public int Value
        {
            get
            {
                return this.Base.Square.Rank == 0 || this.Base.Square.Rank == 7 ? 850 : 1000;
            }
        }

        #endregion

        /*
        private Move.enmName MoveName(Player.enmColour colourPlayer, Square squareTo)
        {
            if (colourPlayer==Player.enmColour.White && squareTo.Rank==7 || colourPlayer==Player.enmColour.Black && squareTo.Rank==0)
            {
                return Move.enmName.PawnPromotion;
            }
            else
            {
                return Move.enmName.Standard;
            }
        }
*/
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
            // Types of promotion to generate. Removed bishop and Rook.
            Move.MoveNames[] promotionTypes = {
                                                  Move.MoveNames.PawnPromotionQueen, Move.MoveNames.PawnPromotionKnight

                                                  // Move.MoveNames.PawnPromotionBishop, Move.MoveNames.PawnPromotionRook // Why bother?
                                              };

            Square square;
            bool isPromotion = (this.Base.Player.Colour == Player.PlayerColourNames.White && this.Base.Square.Rank == 6)
                               ||
                               (this.Base.Player.Colour == Player.PlayerColourNames.Black && this.Base.Square.Rank == 1);

            int intMovesToGenerate = isPromotion ? promotionTypes.Length : 1;

            for (int intIndex = 0; intIndex < intMovesToGenerate; intIndex++)
            {
                // Take right
                if ((square = Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnAttackRightOffset))
                    != null)
                {
                    if (square.Piece != null && square.Piece.Player.Colour != this.Base.Player.Colour
                        && square.Piece.IsCapturable)
                    {
                        moves.Add(
                            0, 
                            0, 
                            isPromotion ? promotionTypes[intIndex] : Move.MoveNames.Standard, 
                            this.Base, 
                            this.Base.Square, 
                            square, 
                            square.Piece, 
                            0, 
                            0);
                    }
                }

                // Take left
                if ((square = Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnAttackLeftOffset)) != null)
                {
                    if (square.Piece != null && square.Piece.Player.Colour != this.Base.Player.Colour
                        && square.Piece.IsCapturable)
                    {
                        moves.Add(
                            0, 
                            0, 
                            isPromotion ? promotionTypes[intIndex] : Move.MoveNames.Standard, 
                            this.Base, 
                            this.Base.Square, 
                            square, 
                            square.Piece, 
                            0, 
                            0);
                    }
                }

                // Forward one
                if (movesType == Moves.MoveListNames.All || isPromotion)
                {
                    if ((square = Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnForwardOffset))
                        != null && square.Piece == null)
                    {
                        moves.Add(
                            0, 
                            0, 
                            isPromotion ? promotionTypes[intIndex] : Move.MoveNames.Standard, 
                            this.Base, 
                            this.Base.Square, 
                            square, 
                            square.Piece, 
                            0, 
                            0);
                    }
                }
            }

            // Forward two
            if (movesType == Moves.MoveListNames.All)
            {
                if (!this.Base.HasMoved)
                {
                    // Check one square ahead is not occupied
                    if ((square = Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnForwardOffset))
                        != null && square.Piece == null)
                    {
                        if (
                            (square =
                             Board.GetSquare(
                                 this.Base.Square.Ordinal + this.Base.Player.PawnForwardOffset
                                 + this.Base.Player.PawnForwardOffset)) != null && square.Piece == null)
                        {
                            moves.Add(
                                0, 0, Move.MoveNames.Standard, this.Base, this.Base.Square, square, square.Piece, 0, 0);
                        }
                    }
                }
            }

            // En Passent 
            if ((this.Base.Square.Rank == 4 && this.Base.Player.Colour == Player.PlayerColourNames.White)
                || (this.Base.Square.Rank == 3 && this.Base.Player.Colour == Player.PlayerColourNames.Black))
            {
                Piece piecePassed;

                // Left
                if ((piecePassed = Board.GetPiece(this.Base.Square.Ordinal - 1)) != null && piecePassed.NoOfMoves == 1
                    && piecePassed.LastMoveTurnNo == Game.TurnNo && piecePassed.Name == Piece.PieceNames.Pawn
                    && piecePassed.Player.Colour != this.Base.Player.Colour)
                {
                    square = Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnAttackLeftOffset);
                    moves.Add(0, 0, Move.MoveNames.EnPassent, this.Base, this.Base.Square, square, piecePassed, 0, 0);
                }

                // Right
                if ((piecePassed = Board.GetPiece(this.Base.Square.Ordinal + 1)) != null && piecePassed.NoOfMoves == 1
                    && piecePassed.LastMoveTurnNo == Game.TurnNo && piecePassed.Name == Piece.PieceNames.Pawn
                    && piecePassed.Player.Colour != this.Base.Player.Colour)
                {
                    square = Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnAttackRightOffset);
                    moves.Add(0, 0, Move.MoveNames.EnPassent, this.Base, this.Base.Square, square, piecePassed, 0, 0);
                }
            }
        }

        public bool CanAttackSquare(Square target_square)
        {
            int intOrdinal = this.Base.Square.Ordinal;
            Square square;

            square = Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnAttackLeftOffset);
            if (square != null && target_square.Ordinal == square.Ordinal)
                return true;
            square = Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnAttackRightOffset);
            if (square != null && target_square.Ordinal == square.Ordinal)
                return true;

            return false;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates a bonus when the pawn folk two major pieces.
        /// </summary>
        /// <returns>
        /// Value of the cheapest forked piece.
        /// </returns>
        private int PawnForkTwoMajorPiecesBonus()
        {
            Square squareLeft;

            if ((squareLeft = Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnAttackLeftOffset)) != null)
            {
                Piece pieceLeft = squareLeft.Piece;
                if (pieceLeft != null && pieceLeft.Player.Colour != this.Base.Player.Colour && pieceLeft.IsCapturable
                    && pieceLeft.Name != Piece.PieceNames.Pawn)
                {
                    Square squareRight;
                    if (
                        (squareRight =
                         Board.GetSquare(this.Base.Square.Ordinal + this.Base.Player.PawnAttackRightOffset)) != null)
                    {
                        Piece pieceRight = squareRight.Piece;
                        if (pieceRight != null && pieceRight.Player.Colour != this.Base.Player.Colour
                            && pieceRight.IsCapturable && pieceRight.Name != Piece.PieceNames.Pawn)
                        {
                            return Math.Min(pieceLeft.Value, pieceRight.Value);
                        }
                    }
                }
            }

            return 0;
        }

        #endregion

        #region Static methods

        static private Piece.PieceNames _pieceType = Piece.PieceNames.Pawn;

        /// <summary>
        ///  static method to determine if a square is attacked by this piece
        /// </summary>
        /// <param name="square"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        static public bool DoesPieceAttackSquare(Square square, Player player)
        {
            Piece piece;
            piece = Board.GetPiece(square.Ordinal - player.PawnAttackLeftOffset);
            if (piece != null && piece.Name == _pieceType && piece.Player.Colour == player.Colour)
            {
                return true;
            }

            piece = Board.GetPiece(square.Ordinal - player.PawnAttackRightOffset);
            if (piece != null && piece.Name == _pieceType && piece.Player.Colour == player.Colour)
            {
                return true;
            }
            return false;
        }

        static public bool DoesPieceAttackSquare(Square square, Player player, out Piece attackingPiece)
        {
            attackingPiece = null;
            Piece piece;
            piece = Board.GetPiece(square.Ordinal - player.PawnAttackLeftOffset);
            if (piece != null && piece.Name == _pieceType && piece.Player.Colour == player.Colour)
            {
                attackingPiece = piece;
                return true;
            }

            piece = Board.GetPiece(square.Ordinal - player.PawnAttackRightOffset);
            if (piece != null && piece.Name == _pieceType && piece.Player.Colour == player.Colour)
            {
                attackingPiece = piece;
                return true;
            }
            return false;
        }

        #endregion     
    }
}