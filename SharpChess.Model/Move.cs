// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Move.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Represents a chess move.
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
    using System.Text;

    #endregion

    /// <summary>
    /// Represents a chess move.
    /// </summary>
    public class Move : IComparable
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class.
        /// </summary>
        /// <param name="turnNo">
        /// The turn no.
        /// </param>
        /// <param name="lastMoveTurnNo">
        /// The last move turn no.
        /// </param>
        /// <param name="moveName">
        /// The move name.
        /// </param>
        /// <param name="piece">
        /// The piece moving.
        /// </param>
        /// <param name="from">
        /// The square the peice is moving from.
        /// </param>
        /// <param name="to">
        /// The square the peice is moving to.
        /// </param>
        /// <param name="pieceCaptured">
        /// The piece being captured.
        /// </param>
        /// <param name="pieceCapturedOrdinal">
        /// Ordinal position of the piece being captured.
        /// </param>
        /// <param name="score">
        /// The positional score.
        /// </param>
        public Move(int turnNo, int lastMoveTurnNo, MoveNames moveName, Piece piece, Square from, Square to, Piece pieceCaptured, int pieceCapturedOrdinal, int score)
        {
            this.EnemyStatus = Player.PlayerStatusNames.Normal;
            this.TurnNo = turnNo;
            this.LastMoveTurnNo = lastMoveTurnNo;
            this.Name = moveName;
            this.Piece = piece;
            this.From = from;
            this.To = to;
            this.PieceCaptured = pieceCaptured;
            this.PieceCapturedOrdinal = pieceCapturedOrdinal;
            this.Score = score;
            if (moveName != MoveNames.NullMove && pieceCaptured == null && piece != null && piece.Name != Piece.PieceNames.Pawn)
            {
                this.FiftyMoveDrawCounter = Game.MoveHistory.Count > 0 ? Game.MoveHistory.Last.FiftyMoveDrawCounter + 1 : (Game.FiftyMoveDrawBase / 2) + 1;
            }
        }

        #endregion

        #region Enums

        /// <summary>
        /// Move type names.
        /// </summary>
        public enum MoveNames
        {
            /// <summary>
            /// Standard move.
            /// </summary>
            Standard, 

            /// <summary>
            /// Castling queen side.
            /// </summary>
            CastleQueenSide, 

            /// <summary>
            /// Castling king side.
            /// </summary>
            CastleKingSide, 

            /// <summary>
            /// Pawn promotion to queen.
            /// </summary>
            PawnPromotionQueen, 

            /// <summary>
            /// Pawn promotion to rook.
            /// </summary>
            PawnPromotionRook, 

            /// <summary>
            /// Pawn promotion to knight.
            /// </summary>
            PawnPromotionKnight, 

            /// <summary>
            /// Pawn promotion to bishop.
            /// </summary>
            PawnPromotionBishop, 

            /// <summary>
            /// En passent move.
            /// </summary>
            EnPassent, 

            /// <summary>
            /// A null move.
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
        /// Gets text for the move useful in debugging.
        /// </summary>
        public string DebugText
        {
            get
            {
                return (this.Piece != null ? this.Piece.Player.Colour.ToString() + " " 
                    + this.Piece.Name.ToString() : string.Empty) + " " 
                    + this.From.Name 
                    + (this.PieceCaptured == null ? "-" : "x") + this.To.Name + " " 
                    + (this.PieceCaptured == null ? string.Empty : this.PieceCaptured.Name.ToString()) + " " // + this.Name
                    + " A: " + this.Alpha 
                    + " B: " + this.Beta 
                    + " Score: " + this.Score 
                    + " " + this.DebugComment;  // + " h: " + this.m_HashEntries.ToString() + " c:" + this.m_HashCaptures.ToString();
            }
        }

        /// <summary>
        /// Gets or sets a comment string containing useful debug info.
        /// </summary>
        public string DebugComment { get; set; }

        /// <summary>
        /// Gets a texual description of the move.
        /// </summary>
        public string Description
        {
            get
            {
                StringBuilder strbMove = new StringBuilder();
                switch (this.Name)
                {
                    case MoveNames.CastleKingSide:
                        strbMove.Append("O-O");
                        break;

                    case MoveNames.CastleQueenSide:
                        strbMove.Append("O-O-O");
                        break;

                    default:
                        if ((this.Piece.Name != Piece.PieceNames.Pawn) && !this.Piece.HasBeenPromoted)
                        {
                            strbMove.Append(this.Piece.Abbreviation);
                        }

                        strbMove.Append(this.From.Name);
                        if (this.PieceCaptured != null)
                        {
                            strbMove.Append("x");
                            if (this.PieceCaptured.Name != Piece.PieceNames.Pawn)
                            {
                                strbMove.Append(this.PieceCaptured.Abbreviation);
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

                switch (this.EnemyStatus)
                {
                    case Player.PlayerStatusNames.InCheckMate:
                        strbMove.Append((this.Piece.Player.Colour == Player.PlayerColourNames.White) ? "# 1-0" : "# 0-1");
                        break;

                    case Player.PlayerStatusNames.InStalemate:
                        strbMove.Append(" 1/2-1/2");
                        break;

                    case Player.PlayerStatusNames.InCheck:
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
        /// Gets or sets status of the enemy e.g. In check, stalemate, checkmate etc.
        /// </summary>
        public Player.PlayerStatusNames EnemyStatus { get; set; }

        /// <summary>
        /// Gets a counter indicating closeness to a fifty-move-draw condition.
        /// </summary>
        public int FiftyMoveDrawCounter { get; private set; }

        /// <summary>
        /// Gets the move From square.
        /// </summary>
        public Square From { get; private set; }

        /// <summary>
        /// Gets or sets the board position HashCodeA.
        /// </summary>
        public ulong HashCodeA { get; set; }

        /// <summary>
        /// Gets or sets the board position HashCodeB.
        /// </summary>
        public ulong HashCodeB { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the enemy is in check.
        /// </summary>
        public bool IsEnemyInCheck { get; set; }

        /// <summary>
        /// Gets a value indicating whether a fifty-move-draw condition has been reached.
        /// </summary>
        public bool IsFiftyMoveDraw
        {
            get
            {
                return this.FiftyMoveDrawCounter >= 100;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the player-to-play is in check.
        /// </summary>
        public bool IsInCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether three-move-repetition applied to this move.
        /// </summary>
        public bool IsThreeMoveRepetition { get; set; }

        /// <summary>
        /// Gets last move turn-number.
        /// </summary>
        public int LastMoveTurnNo { get; private set; }

        /// <summary>
        /// Gets the move number.
        /// </summary>
        public int MoveNo
        {
            get
            {
                return (this.TurnNo / 2) + 1;
            }
        }

        /// <summary>
        /// Gets or sets Moves.
        /// </summary>
        public Moves Moves { get; set; }

        /// <summary>
        /// Gets the move name.
        /// </summary>
        public MoveNames Name { get; private set; }

        /// <summary>
        /// Gets the Piece being moved.
        /// </summary>
        public Piece Piece { get; private set; }

        /// <summary>
        /// Gets or sets the score relating to this move. Ususally used for assigning a move-ordering weighting.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets TimeStamp.
        /// </summary>
        public TimeSpan TimeStamp { get; set; }

        /// <summary>
        /// Gets the move To square.
        /// </summary>
        public Square To { get; private set; }

        /// <summary>
        /// Gets the turn number.
        /// </summary>
        public int TurnNo { get; private set; }

        /// <summary>
        /// Gets the piece being captured.
        /// </summary>
        public Piece PieceCaptured { get; private set; }

        /// <summary>
        /// Gets the ordinal of the piece being captured.
        /// </summary>
        public int PieceCapturedOrdinal { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets move name from text.
        /// </summary>
        /// <param name="moveName">
        /// The move name text.
        /// </param>
        /// <returns>
        /// The Move Name.
        /// </returns>
        public static MoveNames MoveNameFromString(string moveName)
        {
            if (moveName == MoveNames.Standard.ToString())
            {
                return MoveNames.Standard;
            }

            if (moveName == MoveNames.CastleKingSide.ToString())
            {
                return MoveNames.CastleKingSide;
            }

            if (moveName == MoveNames.CastleQueenSide.ToString())
            {
                return MoveNames.CastleQueenSide;
            }

            if (moveName == MoveNames.EnPassent.ToString())
            {
                return MoveNames.EnPassent;
            }

            if (moveName == "PawnPromotion")
            {
                return MoveNames.PawnPromotionQueen;
            }

            if (moveName == MoveNames.PawnPromotionQueen.ToString())
            {
                return MoveNames.PawnPromotionQueen;
            }

            if (moveName == MoveNames.PawnPromotionRook.ToString())
            {
                return MoveNames.PawnPromotionRook;
            }

            if (moveName == MoveNames.PawnPromotionBishop.ToString())
            {
                return MoveNames.PawnPromotionBishop;
            }

            if (moveName == MoveNames.PawnPromotionKnight.ToString())
            {
                return MoveNames.PawnPromotionKnight;
            }

            return 0;
        }

        /// <summary>
        /// Determine where two moves are identical moves.
        /// </summary>
        /// <param name="moveA">
        /// Move A.
        /// </param>
        /// <param name="moveB">
        /// Move B.
        /// </param>
        /// <returns>
        /// True if moves match.
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
                    (moveA.PieceCaptured == null && moveB.PieceCaptured == null)
                    || (moveA.PieceCaptured != null && moveB.PieceCaptured != null && moveA.PieceCaptured == moveB.PieceCaptured));
        }

        /// <summary>
        /// Undo the specified move.
        /// </summary>
        /// <param name="move">
        /// Move to undo.
        /// </param>
        public static void Undo(Move move)
        {
            Board.HashCodeA ^= move.To.Piece.HashCodeA; // un_XOR the piece from where it was previously moved to
            Board.HashCodeB ^= move.To.Piece.HashCodeB; // un_XOR the piece from where it was previously moved to
            if (move.Piece.Name == Piece.PieceNames.Pawn)
            {
                Board.PawnHashCodeA ^= move.To.Piece.HashCodeA;
                Board.PawnHashCodeB ^= move.To.Piece.HashCodeB;
            }

            move.Piece.Square = move.From; // Set piece board location
            move.From.Piece = move.Piece; // Set piece on board
            move.Piece.LastMoveTurnNo = move.LastMoveTurnNo;
            move.Piece.NoOfMoves--;

            if (move.Name != MoveNames.EnPassent)
            {
                move.To.Piece = move.PieceCaptured; // Return piece taken
            }
            else
            {
                move.To.Piece = null; // Blank square where this pawn was
                Board.GetSquare(move.To.Ordinal - move.Piece.Player.PawnForwardOffset).Piece = move.PieceCaptured; // Return En Passent pawn taken
            }

            if (move.PieceCaptured != null)
            {
                move.PieceCaptured.Uncapture(move.PieceCapturedOrdinal);
                Board.HashCodeA ^= move.PieceCaptured.HashCodeA; // XOR back into play the piece that was taken
                Board.HashCodeB ^= move.PieceCaptured.HashCodeB; // XOR back into play the piece that was taken
                if (move.PieceCaptured.Name == Piece.PieceNames.Pawn)
                {
                    Board.PawnHashCodeA ^= move.PieceCaptured.HashCodeA;
                    Board.PawnHashCodeB ^= move.PieceCaptured.HashCodeB;
                }
            }

            Piece pieceRook;
            switch (move.Name)
            {
                case MoveNames.CastleKingSide:
                    pieceRook = move.Piece.Player.Colour == Player.PlayerColourNames.White ? Board.GetPiece(5, 0) : Board.GetPiece(5, 7);
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

                case MoveNames.CastleQueenSide:
                    pieceRook = move.Piece.Player.Colour == Player.PlayerColourNames.White ? Board.GetPiece(3, 0) : Board.GetPiece(3, 7);
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

                case MoveNames.PawnPromotionQueen:
                case MoveNames.PawnPromotionRook:
                case MoveNames.PawnPromotionBishop:
                case MoveNames.PawnPromotionKnight:
                    move.Piece.Demote();
                    break;
            }

            Board.HashCodeA ^= move.From.Piece.HashCodeA; // XOR the piece back into the square it moved back to
            Board.HashCodeB ^= move.From.Piece.HashCodeB; // XOR the piece back into the square it moved back to
            if (move.From.Piece.Name == Piece.PieceNames.Pawn)
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
        /// Compare the score of this move, and the specified move.
        /// </summary>
        /// <param name="move">
        /// Nove to compare.
        /// </param>
        /// <returns>
        /// 1 if specified move score is less, -1 if more, otherwise 0
        /// </returns>
        public int CompareTo(object move)
        {
            if (this.Score < ((Move)move).Score)
            {
                return 1;
            }

            if (this.Score > ((Move)move).Score)
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
        /// Keep the order of the enumeration <see cref="MoveNames"/>.PawnPromotionQueen before PawnPromotionBishop
        /// </remarks>
        public bool IsPromotion()
        {
            return (this.Name >= MoveNames.PawnPromotionQueen) && (this.Name <= MoveNames.PawnPromotionBishop);
        }

        #endregion
    }
}