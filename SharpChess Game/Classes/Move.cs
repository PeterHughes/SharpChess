// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Move.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The move.
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
    using System.Text;

    #endregion

    /// <summary>
    /// The move.
    /// </summary>
    public class Move : IComparable
    {
        #region Constants and Fields

        /// <summary>
        /// The move generator points.
        /// </summary>
        public int MoveGeneratorPoints;

        /// <summary>
        /// The m_ from.
        /// </summary>
        private readonly Square m_From;

        /// <summary>
        /// The m_ last move turn no.
        /// </summary>
        private readonly int m_LastMoveTurnNo;

        /// <summary>
        /// The m_ name.
        /// </summary>
        private readonly enmName m_Name;

        /// <summary>
        /// The m_ to.
        /// </summary>
        private readonly Square m_To;

        /// <summary>
        /// The m_ turn no.
        /// </summary>
        private readonly int m_TurnNo;

        /// <summary>
        /// The m_int fifty move draw counter.
        /// </summary>
        private readonly int m_intFiftyMoveDrawCounter;

        /// <summary>
        /// The m_piece captured.
        /// </summary>
        private readonly Piece m_pieceCaptured;

        /// <summary>
        /// The m_piece captured ordinal.
        /// </summary>
        private readonly int m_pieceCapturedOrdinal;

        /// <summary>
        /// The m_ enemy status.
        /// </summary>
        private Player.enmStatus m_EnemyStatus = Player.enmStatus.Normal;

        /// <summary>
        /// The m_ piece.
        /// </summary>
        private Piece m_Piece;

        /// <summary>
        /// The m_ score.
        /// </summary>
        private int m_Score;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class.
        /// </summary>
        /// <param name="TurnNo">
        /// The turn no.
        /// </param>
        /// <param name="LastMoveTurnNo">
        /// The last move turn no.
        /// </param>
        /// <param name="Name">
        /// The name.
        /// </param>
        /// <param name="piece">
        /// The piece.
        /// </param>
        /// <param name="From">
        /// The from.
        /// </param>
        /// <param name="To">
        /// The to.
        /// </param>
        /// <param name="pieceCaptured">
        /// The piece captured.
        /// </param>
        /// <param name="pieceCapturedOrdinal">
        /// The piece captured ordinal.
        /// </param>
        /// <param name="Score">
        /// The score.
        /// </param>
        public Move(int TurnNo, int LastMoveTurnNo, enmName Name, Piece piece, Square From, Square To, Piece pieceCaptured, int pieceCapturedOrdinal, int Score)
        {
            this.m_TurnNo = TurnNo;
            this.m_LastMoveTurnNo = LastMoveTurnNo;
            this.m_Name = Name;
            this.m_Piece = piece;
            this.m_From = From;
            this.m_To = To;
            this.m_pieceCaptured = pieceCaptured;
            this.m_pieceCapturedOrdinal = pieceCapturedOrdinal;
            this.m_Score = Score;
            if (Name != enmName.NullMove && pieceCaptured == null && piece != null && piece.Name != Piece.enmName.Pawn)
            {
                this.m_intFiftyMoveDrawCounter = Game.MoveHistory.Count > 0 ? Game.MoveHistory.Last.FiftyMoveDrawCounter + 1 : Game.FiftyMoveDrawBase / 2 + 1;
            }
        }

        #endregion

        #region Enums

        /// <summary>
        /// The enm name.
        /// </summary>
        public enum enmName
        {
            /// <summary>
            /// The standard.
            /// </summary>
            Standard, 

            /// <summary>
            /// The castle queen side.
            /// </summary>
            CastleQueenSide, 

            /// <summary>
            /// The castle king side.
            /// </summary>
            CastleKingSide, 

            /// <summary>
            /// The pawn promotion queen.
            /// </summary>
            PawnPromotionQueen, 

            /// <summary>
            /// The pawn promotion rook.
            /// </summary>
            PawnPromotionRook, 

            /// <summary>
            /// The pawn promotion knight.
            /// </summary>
            PawnPromotionKnight, 

            /// <summary>
            /// The pawn promotion bishop.
            /// </summary>
            PawnPromotionBishop, 

            /// <summary>
            /// The en passent.
            /// </summary>
            EnPassent, 

            /// <summary>
            /// The null move.
            /// </summary>
            NullMove
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Alpha.
        /// </summary>
        public int Alpha { get; set; }

        /// <summary>
        /// Gets or sets Beta.
        /// </summary>
        public int Beta { get; set; }

        /// <summary>
        /// Gets or sets ChangeInScore.
        /// </summary>
        public int ChangeInScore { get; set; }

        /// <summary>
        /// Gets DebugText.
        /// </summary>
        public string DebugText
        {
            get
            {
                return (this.Piece != null ? this.Piece.Player.Colour.ToString() + " " 
                    + this.Piece.Name.ToString() : string.Empty) + " " 
                    + this.From.Name + (this.pieceCaptured == null ? "-" : "x") + this.To.Name + " " 
                    + (this.pieceCaptured == null ? string.Empty : this.pieceCaptured.Name.ToString()) + " " 
                    + this.Name.ToString(); // + " A: " + this.Alpha + " B: " + this.Beta + " Score: " + this.Score;// + " h: " + this.m_HashEntries.ToString() + " c:" + this.m_HashCaptures.ToString();
            }
        }

        /// <summary>
        /// Gets Description.
        /// </summary>
        public string Description
        {
            get
            {
                StringBuilder strbMove = new StringBuilder();
                switch (this.Name)
                {
                    case enmName.CastleKingSide:
                        strbMove.Append("O-O");
                        break;

                    case enmName.CastleQueenSide:
                        strbMove.Append("O-O-O");
                        break;

                    default:
                        if ((this.Piece.Name != Piece.enmName.Pawn) && !this.Piece.HasBeenPromoted)
                        {
                            strbMove.Append(this.Piece.Abbreviation);
                        }

                        strbMove.Append(this.From.Name);
                        if (this.pieceCaptured != null)
                        {
                            strbMove.Append("x");
                            if (this.pieceCaptured.Name != Piece.enmName.Pawn)
                            {
                                strbMove.Append(this.pieceCaptured.Abbreviation);
                            }
                        }
                        else
                        {
                            strbMove.Append("-");
                        }

                        strbMove.Append(this.To.Name);
                        break;
                }

                if (this.Piece.HasBeenPromoted)
                {
                    strbMove.Append(":");
                    strbMove.Append(this.Piece.Abbreviation);
                }

                switch (this.m_EnemyStatus)
                {
                    case Player.enmStatus.InCheckMate:
                        strbMove.Append((this.m_Piece.Player.Colour == Player.enmColour.White) ? "# 1-0" : "# 0-1");
                        break;

                    case Player.enmStatus.InStaleMate:
                        strbMove.Append(" 1/2-1/2");
                        break;

                    case Player.enmStatus.InCheck:
                        strbMove.Append("+");
                        break;
                }

                if (this.IsThreeMoveRepetition || this.IsFiftyMoveDraw)
                {
                    strbMove.Append(" 1/2-1/2");
                }

                return strbMove.ToString();
            }
        }

        /// <summary>
        /// Gets or sets EnemyStatus.
        /// </summary>
        public Player.enmStatus EnemyStatus
        {
            get
            {
                return this.m_EnemyStatus;
            }

            set
            {
                this.m_EnemyStatus = value;
            }
        }

        /// <summary>
        /// Gets FiftyMoveDrawCounter.
        /// </summary>
        public int FiftyMoveDrawCounter
        {
            get
            {
                return this.m_intFiftyMoveDrawCounter;
            }
        }

        /// <summary>
        /// Gets From.
        /// </summary>
        public Square From
        {
            get
            {
                return this.m_From;
            }
        }

        /// <summary>
        /// Gets or sets HashCodeA.
        /// </summary>
        public ulong HashCodeA { get; set; }

        /// <summary>
        /// Gets or sets HashCodeB.
        /// </summary>
        public ulong HashCodeB { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsEnemyInCheck.
        /// </summary>
        public bool IsEnemyInCheck { get; set; }

        /// <summary>
        /// Gets a value indicating whether IsFiftyMoveDraw.
        /// </summary>
        public bool IsFiftyMoveDraw
        {
            get
            {
                return this.m_intFiftyMoveDrawCounter >= 100;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsInCheck.
        /// </summary>
        public bool IsInCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsThreeMoveRepetition.
        /// </summary>
        public bool IsThreeMoveRepetition { get; set; }

        /// <summary>
        /// Gets LastMoveTurnNo.
        /// </summary>
        public int LastMoveTurnNo
        {
            get
            {
                return this.m_LastMoveTurnNo;
            }
        }

        /// <summary>
        /// Gets MoveNo.
        /// </summary>
        public int MoveNo
        {
            get
            {
                return this.m_TurnNo / 2 + 1;
            }
        }

        /// <summary>
        /// Gets or sets Moves.
        /// </summary>
        public Moves Moves { get; set; }

        /// <summary>
        /// Gets Name.
        /// </summary>
        public enmName Name
        {
            get
            {
                return this.m_Name;
            }
        }

        /// <summary>
        /// Gets or sets Piece.
        /// </summary>
        public Piece Piece
        {
            get
            {
                return this.m_Piece;
            }

            set
            {
                this.m_Piece = value;
            }
        }

        /// <summary>
        /// Gets or sets Score.
        /// </summary>
        public int Score
        {
            get
            {
                return this.m_Score;
            }

            set
            {
                this.m_Score = value;
            }
        }

        /// <summary>
        /// Gets or sets TimeStamp.
        /// </summary>
        public TimeSpan TimeStamp { get; set; }

        /// <summary>
        /// Gets To.
        /// </summary>
        public Square To
        {
            get
            {
                return this.m_To;
            }
        }

        /// <summary>
        /// Gets TurnNo.
        /// </summary>
        public int TurnNo
        {
            get
            {
                return this.m_TurnNo;
            }
        }

        /// <summary>
        /// Gets pieceCaptured.
        /// </summary>
        public Piece pieceCaptured
        {
            get
            {
                return this.m_pieceCaptured;
            }
        }

        /// <summary>
        /// Gets pieceCapturedOrdinal.
        /// </summary>
        public int pieceCapturedOrdinal
        {
            get
            {
                return this.m_pieceCapturedOrdinal;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The is valid.
        /// </summary>
        /// <param name="moveProposed">
        /// The move proposed.
        /// </param>
        /// <returns>
        /// The is valid.
        /// </returns>
        public static bool IsValid(Move moveProposed)
        {
            if (moveProposed.Piece != Board.GetPiece(moveProposed.From.Ordinal))
            {
                return false;
            }

            Moves movesPossible = new Moves();
            moveProposed.Piece.GenerateLazyMoves(movesPossible, Moves.enmMovesType.All);
            foreach (Move move in movesPossible)
            {
                if (moveProposed.Name == move.Name && moveProposed.To.Ordinal == move.To.Ordinal)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The move name from string.
        /// </summary>
        /// <param name="strMoveName">
        /// The str move name.
        /// </param>
        /// <returns>
        /// </returns>
        public static enmName MoveNameFromString(string strMoveName)
        {
            if (strMoveName == enmName.Standard.ToString())
            {
                return enmName.Standard;
            }

            if (strMoveName == enmName.CastleKingSide.ToString())
            {
                return enmName.CastleKingSide;
            }

            if (strMoveName == enmName.CastleQueenSide.ToString())
            {
                return enmName.CastleQueenSide;
            }

            if (strMoveName == enmName.EnPassent.ToString())
            {
                return enmName.EnPassent;
            }

            if (strMoveName == "PawnPromotion")
            {
                return enmName.PawnPromotionQueen;
            }

            if (strMoveName == enmName.PawnPromotionQueen.ToString())
            {
                return enmName.PawnPromotionQueen;
            }

            if (strMoveName == enmName.PawnPromotionRook.ToString())
            {
                return enmName.PawnPromotionRook;
            }

            if (strMoveName == enmName.PawnPromotionBishop.ToString())
            {
                return enmName.PawnPromotionBishop;
            }

            if (strMoveName == enmName.PawnPromotionKnight.ToString())
            {
                return enmName.PawnPromotionKnight;
            }

            return 0;
        }

        /// <summary>
        /// The moves match.
        /// </summary>
        /// <param name="moveA">
        /// The move a.
        /// </param>
        /// <param name="moveB">
        /// The move b.
        /// </param>
        /// <returns>
        /// The moves match.
        /// </returns>
        public static bool MovesMatch(Move moveA, Move moveB)
        {
            return moveA != null 
                && moveB != null 
                && moveA.Piece == moveB.Piece 
                && moveA.From == moveB.From 
                && moveA.To == moveB.To 
                && moveA.Name == moveB.Name 
                && (
                    (moveA.pieceCaptured == null && moveB.pieceCaptured == null)
                    || (moveA.pieceCaptured != null && moveB.pieceCaptured != null && moveA.pieceCaptured == moveB.pieceCaptured));
        }

        /// <summary>
        /// The undo.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        public static void Undo(Move move)
        {
            Board.HashCodeA ^= move.To.Piece.HashCodeA; // un_XOR the piece from where it was previously moved to
            Board.HashCodeB ^= move.To.Piece.HashCodeB; // un_XOR the piece from where it was previously moved to
            if (move.Piece.Name == Piece.enmName.Pawn)
            {
                Board.PawnHashCodeA ^= move.To.Piece.HashCodeA;
                Board.PawnHashCodeB ^= move.To.Piece.HashCodeB;
            }

            move.Piece.Square = move.From; // Set piece board location
            move.From.Piece = move.Piece; // Set piece on board
            move.Piece.LastMoveTurnNo = move.LastMoveTurnNo;
            move.Piece.NoOfMoves--;

            if (move.Name != enmName.EnPassent)
            {
                move.To.Piece = move.pieceCaptured; // Return piece taken
            }
            else
            {
                move.To.Piece = null; // Blank square where this pawn was
                Board.GetSquare(move.To.Ordinal - move.Piece.Player.PawnForwardOffset).Piece = move.pieceCaptured; // Return En Passent pawn taken
            }

            if (move.pieceCaptured != null)
            {
                move.pieceCaptured.Uncapture(move.pieceCapturedOrdinal);
                Board.HashCodeA ^= move.pieceCaptured.HashCodeA; // XOR back into play the piece that was taken
                Board.HashCodeB ^= move.pieceCaptured.HashCodeB; // XOR back into play the piece that was taken
                if (move.pieceCaptured.Name == Piece.enmName.Pawn)
                {
                    Board.PawnHashCodeA ^= move.pieceCaptured.HashCodeA;
                    Board.PawnHashCodeB ^= move.pieceCaptured.HashCodeB;
                }
            }

            Piece pieceRook;
            switch (move.Name)
            {
                case enmName.CastleKingSide:
                    pieceRook = move.Piece.Player.Colour == Player.enmColour.White ? Board.GetPiece(5, 0) : Board.GetPiece(5, 7);
                    Board.HashCodeA ^= pieceRook.HashCodeA;
                    Board.HashCodeB ^= pieceRook.HashCodeB;
                    pieceRook.Square = Board.GetSquare(7, move.Piece.Square.Rank);
                    pieceRook.LastMoveTurnNo = move.LastMoveTurnNo;
                    pieceRook.NoOfMoves--;
                    Board.GetSquare(7, move.Piece.Square.Rank).Piece = pieceRook;
                    Board.GetSquare(5, move.Piece.Square.Rank).Piece = null;
                    move.Piece.Player.HasCastled = false;
                    Board.HashCodeA ^= pieceRook.HashCodeA;
                    Board.HashCodeB ^= pieceRook.HashCodeB;
                    break;

                case enmName.CastleQueenSide:
                    pieceRook = move.Piece.Player.Colour == Player.enmColour.White ? Board.GetPiece(3, 0) : Board.GetPiece(3, 7);
                    Board.HashCodeA ^= pieceRook.HashCodeA;
                    Board.HashCodeB ^= pieceRook.HashCodeB;
                    pieceRook.Square = Board.GetSquare(0, move.Piece.Square.Rank);
                    pieceRook.LastMoveTurnNo = move.LastMoveTurnNo;
                    pieceRook.NoOfMoves--;
                    Board.GetSquare(0, move.Piece.Square.Rank).Piece = pieceRook;
                    Board.GetSquare(3, move.Piece.Square.Rank).Piece = null;
                    move.Piece.Player.HasCastled = false;
                    Board.HashCodeA ^= pieceRook.HashCodeA;
                    Board.HashCodeB ^= pieceRook.HashCodeB;
                    break;

                case enmName.PawnPromotionQueen:
                case enmName.PawnPromotionRook:
                case enmName.PawnPromotionBishop:
                case enmName.PawnPromotionKnight:
                    move.Piece.Demote();
                    break;
            }

            Board.HashCodeA ^= move.From.Piece.HashCodeA; // XOR the piece back into the square it moved back to
            Board.HashCodeB ^= move.From.Piece.HashCodeB; // XOR the piece back into the square it moved back to
            if (move.From.Piece.Name == Piece.enmName.Pawn)
            {
                Board.PawnHashCodeA ^= move.From.Piece.HashCodeA;
                Board.PawnHashCodeB ^= move.From.Piece.HashCodeB;
            }

            if (move.IsThreeMoveRepetition)
            {
                Board.HashCodeA ^= 31;
                Board.HashCodeB ^= 29;
            }

            Game.TurnNo--;

            Game.MoveHistory.RemoveLast();
        }

        /// <summary>
        /// The compare to.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        /// <returns>
        /// The compare to.
        /// </returns>
        public int CompareTo(object move)
        {
            if (this.m_Score < ((Move)move).Score)
            {
                return 1;
            }

            if (this.m_Score > ((Move)move).Score)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Is the move a promotion of pawn
        /// </summary>
        /// <returns>
        /// true if promotion otherwise false
        /// </returns>
        /// <remarks>
        /// Keep the order of the enumeration <see cref="enmName"/>.PawnPromotionQueen before PawnPromotionBishop
        /// </remarks>
        public bool IsPromotion()
        {
            return (this.m_Name >= enmName.PawnPromotionQueen) && (this.m_Name <= enmName.PawnPromotionBishop);
        }

        #endregion

        // end IsPromotion
    }
}