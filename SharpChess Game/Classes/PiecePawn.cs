// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PiecePawn.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The piece pawn.
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
    #region Using

    using System;

    #endregion

    /// <summary>
    /// The piece pawn.
    /// </summary>
    public class PiecePawn : IPieceTop
    {
        #region Constants and Fields

        /// <summary>
        /// The aint advancement bonus.
        /// </summary>
        private static readonly int[] aintAdvancementBonus = { 0, 0, 0, 45, 75, 120, 240, 999 };

        /// <summary>
        /// The aint file bonus.
        /// </summary>
        private static readonly int[] aintFileBonus = { 0, 6, 16, 32, 32, 16, 6, 0 };

        /// <summary>
        /// The m_ base.
        /// </summary>
        private readonly Piece m_Base;

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
                return "P";
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
        /// Gets ImageIndex.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.m_Base.Player.Colour == Player.enmColour.White ? 9 : 8;
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
                return Piece.enmName.Pawn;
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
                int intIndex;
                Piece piece;

                // PENALITIES

                // Isolated or Doubled pawn
                bool blnIsIsolatedLeft = true;
                bool blnIsIsolatedRight = true;
                bool blnIsDouble = false;
                for (intIndex = this.m_Base.Player.Pieces.Count - 1; intIndex >= 0; intIndex--)
                {
                    piece = this.m_Base.Player.Pieces.Item(intIndex);
                    if (piece.Name == Piece.enmName.Pawn && piece != this.m_Base)
                    {
                        if (piece.Square.File == this.m_Base.Square.File)
                        {
                            blnIsDouble = true;
                        }

                        if (piece.Square.File == this.m_Base.Square.File - 1)
                        {
                            blnIsIsolatedLeft = false;
                        }

                        if (piece.Square.File == this.m_Base.Square.File + 1)
                        {
                            blnIsIsolatedRight = false;
                        }
                    }
                }

                if (blnIsIsolatedLeft)
                {
                    switch (this.m_Base.Square.File)
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
                    switch (this.m_Base.Square.File)
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
                bool blnIsBackward = true;
                if (blnIsBackward && (piece = Board.GetPiece(this.m_Base.Square.Ordinal - 1)) != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
                {
                    blnIsBackward = false;
                }

                if (blnIsBackward && (piece = Board.GetPiece(this.m_Base.Square.Ordinal + 1)) != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
                {
                    blnIsBackward = false;
                }

                if (blnIsBackward && (piece = Board.GetPiece(this.m_Base.Square.Ordinal - this.m_Base.Player.PawnAttackLeftOffset)) != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
                {
                    blnIsBackward = false;
                }

                if (blnIsBackward && (piece = Board.GetPiece(this.m_Base.Square.Ordinal - this.m_Base.Player.PawnAttackRightOffset)) != null && piece.Name == Piece.enmName.Pawn && piece.Player.Colour == this.m_Base.Player.Colour)
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
                    if (this.m_Base.Square.File == 3 || this.m_Base.Square.File == 4)
                    {
                        // still at rank 2
                        if (this.m_Base.Player.Colour == Player.enmColour.White && this.m_Base.Square.Rank == 1 || this.m_Base.Player.Colour == Player.enmColour.Black && this.m_Base.Square.Rank == 6)
                        {
                            intPoints -= 200;
                        }
                    }
                }

                if (Game.Stage == Game.GameStageNames.Opening)
                {
                    // Pawns on D or E files
                    if (this.m_Base.Square.File == 3 || this.m_Base.Square.File == 4)
                    {
                        // Bonus for rank 5
                        if (this.m_Base.Player.Colour == Player.enmColour.White && this.m_Base.Square.Rank == 4 || this.m_Base.Player.Colour == Player.enmColour.Black && this.m_Base.Square.Rank == 3)
                        {
                            intPoints -= 300;
                        }
                    }
                }

                // BONUSES

                // Encourage capturing towards the centre
                intPoints += aintFileBonus[this.m_Base.Square.File];

                // Advancement
                int intAdvancementBonus = aintAdvancementBonus[this.m_Base.Player.Colour == Player.enmColour.White ? this.m_Base.Square.Rank : 7 - this.m_Base.Square.Rank];

                // Passed Pawns
                bool blnIsPassed = true;
                for (intIndex = this.m_Base.Player.OtherPlayer.Pieces.Count - 1; intIndex >= 0; intIndex--)
                {
                    piece = this.m_Base.Player.OtherPlayer.Pieces.Item(intIndex);
                    if (piece.Name == Piece.enmName.Pawn)
                    {
                        if ((this.m_Base.Player.Colour == Player.enmColour.White && piece.Square.Rank > this.m_Base.Square.Rank || this.m_Base.Player.Colour == Player.enmColour.Black && piece.Square.Rank < this.m_Base.Square.Rank) && (piece.Square.File == this.m_Base.Square.File || piece.Square.File == this.m_Base.Square.File - 1 || piece.Square.File == this.m_Base.Square.File + 1))
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
                intAdvancementBonus = intAdvancementBonus * ((Game.MaxMaterialCount - Game.LowestMaterialCount) * 2) / Game.MaxMaterialCount;

                intPoints += intAdvancementBonus + this.EvalForkPawn(); // 15Mar06 Nimzo Added pawn fork bonus

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
                return this.m_Base.Square.Rank == 0 || this.m_Base.Square.Rank == 7 ? 850 : 1000;
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
            Move.enmName[] PromotionTypes = { Move.enmName.PawnPromotionQueen, Move.enmName.PawnPromotionRook, Move.enmName.PawnPromotionKnight, Move.enmName.PawnPromotionBishop };
            Square square;
            bool blnIsPromotion = this.m_Base.Player.Colour == Player.enmColour.White && this.m_Base.Square.Rank == 6 || this.m_Base.Player.Colour == Player.enmColour.Black && this.m_Base.Square.Rank == 1;
            int intMovesToGenerate = blnIsPromotion ? PromotionTypes.Length : 1;

            for (int intIndex = 0; intIndex < intMovesToGenerate; intIndex++)
            {
                // Take right
                if ((square = Board.GetSquare(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnAttackRightOffset)) != null)
                {
                    if (square.Piece != null && square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)
                    {
                        moves.Add(0, 0, blnIsPromotion ? PromotionTypes[intIndex] : Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }
                }

                // Take left
                if ((square = Board.GetSquare(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnAttackLeftOffset)) != null)
                {
                    if (square.Piece != null && square.Piece.Player.Colour != this.m_Base.Player.Colour && square.Piece.IsCapturable)
                    {
                        moves.Add(0, 0, blnIsPromotion ? PromotionTypes[intIndex] : Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }
                }

                // Forward one
                if (movesType == Moves.enmMovesType.All || blnIsPromotion)
                {
                    if ((square = Board.GetSquare(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnForwardOffset)) != null && square.Piece == null)
                    {
                        moves.Add(0, 0, blnIsPromotion ? PromotionTypes[intIndex] : Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                    }
                }
            }

            // Forward two
            if (movesType == Moves.enmMovesType.All)
            {
                if (!this.m_Base.HasMoved)
                {
                    // Check one square ahead is not occupied
                    if ((square = Board.GetSquare(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnForwardOffset)) != null && square.Piece == null)
                    {
                        if ((square = Board.GetSquare(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnForwardOffset + this.m_Base.Player.PawnForwardOffset)) != null && square.Piece == null)
                        {
                            moves.Add(0, 0, Move.enmName.Standard, this.m_Base, this.m_Base.Square, square, square.Piece, 0, 0);
                        }
                    }
                }
            }

            // En Passent 
            if (this.m_Base.Square.Rank == 4 && this.m_Base.Player.Colour == Player.enmColour.White || this.m_Base.Square.Rank == 3 && this.m_Base.Player.Colour == Player.enmColour.Black)
            {
                Piece piecePassed;

                // Left
                if ((piecePassed = Board.GetPiece(this.m_Base.Square.Ordinal - 1)) != null && piecePassed.NoOfMoves == 1 && piecePassed.LastMoveTurnNo == Game.TurnNo && piecePassed.Name == Piece.enmName.Pawn && piecePassed.Player.Colour != this.m_Base.Player.Colour)
                {
                    square = Board.GetSquare(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnAttackLeftOffset);
                    moves.Add(0, 0, Move.enmName.EnPassent, this.m_Base, this.m_Base.Square, square, piecePassed, 0, 0);
                }

                // Right
                if ((piecePassed = Board.GetPiece(this.m_Base.Square.Ordinal + 1)) != null && piecePassed.NoOfMoves == 1 && piecePassed.LastMoveTurnNo == Game.TurnNo && piecePassed.Name == Piece.enmName.Pawn && piecePassed.Player.Colour != this.m_Base.Player.Colour)
                {
                    square = Board.GetSquare(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnAttackRightOffset);
                    moves.Add(0, 0, Move.enmName.EnPassent, this.m_Base, this.m_Base.Square, square, piecePassed, 0, 0);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Eval the threat of fork by the pawn
        /// </summary>
        /// <remarks>
        /// 15Mar06 Nimzo
        /// </remarks>
        /// &gt;
        /// <returns>
        /// <para>
        /// Min of the value of attacked pieces if fork
        /// </para>
        /// <para>
        /// 30 if the pawn threatens a piece
        /// </para>
        /// <para>
        /// 0 if not threat
        /// </para>
        /// </returns>
        private int EvalForkPawn()
        {
            const int intTHREAT_PAWN = 30;
            int intBonus = 0;
            Square squareLeft;
            Square squareRight;
            Piece pieceLeft = null;

            if ((squareLeft = Board.GetSquare(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnAttackLeftOffset)) != null)
            {
                pieceLeft = squareLeft.Piece;
                if (pieceLeft != null && pieceLeft.Player.Colour != this.m_Base.Player.Colour && pieceLeft.IsCapturable && pieceLeft.Name != Piece.enmName.Pawn)
                {
                    intBonus = intTHREAT_PAWN; // see where CanBeDrivenAwayByPawn is called
                }
            }

            if ((squareRight = Board.GetSquare(this.m_Base.Square.Ordinal + this.m_Base.Player.PawnAttackRightOffset)) != null)
            {
                Piece pieceRight = squareRight.Piece;
                if (pieceRight != null && pieceRight.Player.Colour != this.m_Base.Player.Colour && pieceRight.IsCapturable && pieceRight.Name != Piece.enmName.Pawn)
                {
                    intBonus = (intBonus == intTHREAT_PAWN) ? Math.Min(pieceLeft.Value, pieceRight.Value) : intTHREAT_PAWN;
                }
            }

            return intBonus;
        }

        #endregion
    }
}