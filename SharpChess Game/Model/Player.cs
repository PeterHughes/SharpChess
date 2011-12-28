// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The player.
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

namespace SharpChess.Model
{
    #region Using

    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    using SharpChess.Model.AI;

    using ThreadState = System.Threading.ThreadState;

    #endregion

    /// <summary>
    /// The player.
    /// </summary>
    public abstract class Player
    {
        #region Constants and Fields

        /// <summary>
        ///   The ma x_ score.
        /// </summary>
        private const int MaxScore = int.MaxValue;

        /// <summary>
        ///   The mi n_ score.
        /// </summary>
        private const int MinScore = int.MinValue + 1;

        /// <summary>
        ///   The m_int min search depth.
        /// </summary>
        private const int MinSearchDepth = 1;

        /// <summary>
        ///   The m_int minimum search depth.
        /// </summary>
        private const int MinimumSearchDepth = 1;

        /// <summary>
        ///   The m_ulong pondering hash code a.
        /// </summary>
        private static ulong ponderingHashCodeA;

        /// <summary>
        ///   The m_ulong pondering hash code b.
        /// </summary>
        private static ulong ponderingHashCodeB;

        /// <summary>
        ///   The m_bln display move analysis tree.
        /// </summary>
        private bool displayMoveAnalysisTree;

        /// <summary>
        ///   The m_bln force immediate move.
        /// </summary>
        private bool forceAnImmediateMove;

        /// <summary>
        ///   The m_tsn thinking time cutoff.
        /// </summary>
        private TimeSpan thinkingTimeCutoff;

        /// <summary>
        ///   The m_tsn thinking time max allowed.
        /// </summary>
        private TimeSpan thinkingTimeMaxAllowed;

        /// <summary>
        ///   The m_thread thought.
        /// </summary>
        private Thread threadThought;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Player" /> class.
        /// </summary>
        protected Player()
        {
            this.Clock = new PlayerClock();
            this.MaxSearchDepth = 32;
            this.MaterialCount = 7;
            this.PawnCountInPlay = 8;
            this.Pieces = new Pieces();
            this.CapturedEnemyPieces = new Pieces();
            this.Brain = new Brain();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The delegatetype player event.
        /// </summary>
        public delegate void PlayerEvent();

        #endregion

        #region Public Events

        /// <summary>
        ///   The move considered.
        /// </summary>
        public event PlayerEvent MoveConsidered;

        /// <summary>
        ///   The ready to make move.
        /// </summary>
        public event PlayerEvent ReadyToMakeMove;

        /// <summary>
        ///   The thinking beginning.
        /// </summary>
        public event PlayerEvent ThinkingBeginning;

        #endregion

        #region Enums

        /// <summary>
        /// The enm colour.
        /// </summary>
        public enum ColourNames
        {
            /// <summary>
            ///   The white.
            /// </summary>
            White, 

            /// <summary>
            ///   The black.
            /// </summary>
            Black
        }

        /// <summary>
        /// The enm intellegence.
        /// </summary>
        public enum IntellegenceNames
        {
            /// <summary>
            ///   The human.
            /// </summary>
            Human, 

            /// <summary>
            ///   The computer.
            /// </summary>
            Computer
        }

        /// <summary>
        /// The enm status.
        /// </summary>
        public enum StatusNames
        {
            /// <summary>
            ///   The normal.
            /// </summary>
            Normal, 

            /// <summary>
            ///   The in check.
            /// </summary>
            InCheck, 

            /// <summary>
            ///   The in stale mate.
            /// </summary>
            InStaleMate, 

            /// <summary>
            ///   The in check mate.
            /// </summary>
            InCheckMate
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the player's chess brain. Contains all computer AI player logic.
        /// </summary>
        public Brain Brain { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether CanClaimFiftyMoveDraw.
        /// </summary>
        public bool CanClaimFiftyMoveDraw
        {
            get
            {
                return Game.MoveHistory.Count > 0
                           ? Game.MoveHistory.Last.IsFiftyMoveDraw
                           : Game.FiftyMoveDrawBase >= 100;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether CanClaimInsufficientMaterialDraw.
        /// </summary>
        public bool CanClaimInsufficientMaterialDraw
        {
            get
            {
                // Return true if K vs K, K vs K+B, K vs K+N
                if (Game.PlayerWhite.Pieces.Count > 2 || Game.PlayerBlack.Pieces.Count > 2)
                {
                    return false;
                }

                if (Game.PlayerWhite.Pieces.Count == 2 && Game.PlayerBlack.Pieces.Count == 2)
                {
                    return false;
                }

                if (Game.PlayerWhite.Pieces.Count == 1 && Game.PlayerBlack.Pieces.Count == 1)
                {
                    return true;
                }

                Player playerTwoPieces = Game.PlayerWhite.Pieces.Count == 2 ? Game.PlayerWhite : Game.PlayerBlack;
                Piece pieceNotKing = playerTwoPieces.Pieces.Item(0).Name == Piece.PieceNames.King
                                         ? playerTwoPieces.Pieces.Item(1)
                                         : playerTwoPieces.Pieces.Item(0);

                switch (pieceNotKing.Name)
                {
                    case Piece.PieceNames.Bishop:
                    case Piece.PieceNames.Knight:
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether CanClaimThreeMoveRepetitionDraw.
        /// </summary>
        public bool CanClaimThreeMoveRepetitionDraw
        {
            get
            {
                return this.CanClaimMoveRepetitionDraw(3);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether CanMove.
        /// </summary>
        public bool CanMove
        {
            get
            {
                Moves moves;
                foreach (Piece piece in this.Pieces)
                {
                    moves = new Moves();
                    piece.GenerateLegalMoves(moves);
                    if (moves.Count > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        ///   Gets CapturedEnemyPieces.
        /// </summary>
        public Pieces CapturedEnemyPieces { get; protected set; }

        /// <summary>
        ///   Gets CapturedEnemyPiecesTotalBasicValue.
        /// </summary>
        public int CapturedEnemyPiecesTotalBasicValue
        {
            get
            {
                int intValue = 0;
                foreach (Piece piece in this.CapturedEnemyPieces)
                {
                    intValue += piece.BasicValue;
                }

                return intValue;
            }
        }

        /// <summary>
        ///   Gets Clock.
        /// </summary>
        public PlayerClock Clock { get; private set; }

        /// <summary>
        ///   Gets or sets Colour.
        /// </summary>
        public ColourNames Colour { get; set; }

        /// <summary>
        ///   Gets CurrentMove.
        /// </summary>
        public Move CurrentMove { get; private set; }

        /// <summary>
        ///   Gets or sets Evaluations.
        /// </summary>
        public int Evaluations { get; protected set; }

        /// <summary>
        ///   Gets EvaluationsPerSecond.
        /// </summary>
        public double EvaluationsPerSecond
        {
            get
            {
                return this.Evaluations / this.ThinkingTimeElpased.TotalSeconds;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether HasCastled.
        /// </summary>
        public bool HasCastled { get; set; }

        /// <summary>
        ///   The intellegence.
        /// </summary>
        public IntellegenceNames Intellegence { get; set; }

        /// <summary>
        ///   Gets a value indicating whether IsInCheck.
        /// </summary>
        public bool IsInCheck
        {
            get
            {
                return HashTableCheck.QueryandCachePlayerInCheckStatusForPosition(
                    Board.HashCodeA, Board.HashCodeB, this);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether IsInCheckMate.
        /// </summary>
        public bool IsInCheckMate
        {
            get
            {
                if (!this.IsInCheck)
                {
                    return false;
                }

                Moves moves = new Moves();
                this.GenerateLegalMoves(moves);
                return moves.Count == 0;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether IsPondering.
        /// </summary>
        public bool IsPondering { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether IsThinking.
        /// </summary>
        public bool IsThinking
        {
            get
            {
                return this.threadThought != null;
            }
        }

        /// <summary>
        ///   Gets or sets King.
        /// </summary>
        public Piece King { get; protected set; }

        /// <summary>
        ///   Gets MaterialCount.
        /// </summary>
        public int MaterialCount { get; protected set; }

        /// <summary>
        ///   Gets MaxExtensions.
        /// </summary>
        public int MaxExtensions { get; private set; }

        /// <summary>
        ///   Gets MaxQuiesDepth.
        /// </summary>
        public int MaxQuiesenceDepthReached { get; private set; }

        /// <summary>
        ///   Gets MaxSearchDepth.
        /// </summary>
        public int MaxSearchDepth { get; private set; }

        /// <summary>
        ///   Gets OtherPlayer.
        /// </summary>
        public Player OtherPlayer
        {
            get
            {
                return this.Colour == ColourNames.White ? Game.PlayerBlack : Game.PlayerWhite;
            }
        }

        /// <summary>
        ///   Gets PawnAttackLeftOffset.
        /// </summary>
        public abstract int PawnAttackLeftOffset { get; }

        /// <summary>
        ///   Gets PawnAttackRightOffset.
        /// </summary>
        public abstract int PawnAttackRightOffset { get; }

        /// <summary>
        ///   Gets PawnForwardOffset.
        /// </summary>
        public abstract int PawnForwardOffset { get; }

        /*
         * PawmKing points removed because not safe to cache pawn scores that take into account non pawn pieces.
        /// <summary>
        ///   Gets PawnKingPoints.
        /// </summary>
        public int PawnKingPoints
        {
            get
            {
                int intPoints;
                int intIndex;

                if ((intPoints = HashTablePawnKing.ProbeHash(Board.HashCodeA, Board.HashCodeB, this.Colour)) == HashTablePawnKing.NotFoundInHashTable)
                {
                    Piece piece;
                    intPoints = 0;
                    for (intIndex = this.m_colPieces.Count - 1; intIndex >= 0; intIndex--)
                    {
                        piece = this.m_colPieces.Item(intIndex);
                        switch (piece.Name)
                        {
                            case Piece.PieceNames.Pawn:
                            case Piece.PieceNames.King:
                                intPoints += piece.PointsTotal;
                                break;
                        }
                    }

                    HashTablePawnKing.RecordHash(Board.HashCodeA, Board.HashCodeB, intPoints, this.Colour);
                }

                return intPoints;
            }
        }
        
        */

        /// <summary>
        ///   Gets PieceBasicValue.
        /// </summary>
        public int PieceBasicValue
        {
            get
            {
                return this.Pieces.Cast<Piece>().Sum(piece => piece.BasicValue);
            }
        }

        /// <summary>
        ///   Gets Pieces.
        /// </summary>
        public Pieces Pieces { get; protected set; }

        /// <summary>
        ///   Gets Points.
        /// </summary>
        public int Points
        {
            get
            {
                int intPoints = 0;
                int intIndex;
                Piece piece;

                // intPoints += this.PawnKingPoints;
                int intBishopCount = 0;
                int intRookCount = 0;
                for (intIndex = this.Pieces.Count - 1; intIndex >= 0; intIndex--)
                {
                    piece = this.Pieces.Item(intIndex);

                    /*
                    switch (piece.Name)
                    {
                        case Piece.PieceNames.Pawn:
                        case Piece.PieceNames.King:
                            break;
                        default:
                            intPoints += piece.PointsTotal;
                            break;
                    }
                    */
                    intPoints += piece.PointsTotal;

                    switch (piece.Name)
                    {
                        case Piece.PieceNames.Bishop:
                            intBishopCount++;
                            break;

                        case Piece.PieceNames.Rook:
                            intRookCount++;
                            break;
                    }
                }

                if (intBishopCount >= 2)
                {
                    intPoints += 500;
                }

                if (intRookCount >= 2)
                {
                    intPoints += 100;
                }

                // Multiple attack bonus
                // for (intIndex=this.OtherPlayer.m_colPieces.Count-1; intIndex>=0; intIndex--)
                // {
                // piece = this.OtherPlayer.m_colPieces.Item(intIndex);
                // intPoints += m_aintAttackBonus[piece.Square.NoOfAttacksBy(this)];
                // }

                // Factor in human 3 move repition draw condition
                // If this player is "human" then a draw if scored high, else a draw is scored low
                if (Game.MoveHistory.Count > 0 && Game.MoveHistory.Last.IsThreeMoveRepetition)
                {
                    intPoints += this.Intellegence == IntellegenceNames.Human ? 1000000000 : 0;
                }

                if (this.HasCastled)
                {
                    intPoints += 117;
                }
                else
                {
                    if (this.King.HasMoved)
                    {
                        intPoints -= 247;
                    }
                    else
                    {
                        Piece pieceRook;
                        pieceRook = this.Colour == ColourNames.White ? Board.GetPiece(7, 0) : Board.GetPiece(7, 7);
                        if (pieceRook == null || pieceRook.Name != Piece.PieceNames.Rook
                            || pieceRook.Player.Colour != this.Colour || pieceRook.HasMoved)
                        {
                            intPoints -= 107;
                        }

                        pieceRook = this.Colour == ColourNames.White ? Board.GetPiece(0, 0) : Board.GetPiece(0, 7);
                        if (pieceRook == null || pieceRook.Name != Piece.PieceNames.Rook
                            || pieceRook.Player.Colour != this.Colour || pieceRook.HasMoved)
                        {
                            intPoints -= 107;
                        }
                    }
                }

                if (this.IsInCheck)
                {
                    if (this.IsInCheckMate)
                    {
                        intPoints -= 999999999;
                    }
                }

                return intPoints;
            }
        }

        /// <summary>
        ///   Gets PositionPoints.
        /// </summary>
        public int PositionPoints
        {
            get
            {
                int intTotalValue = 0;
                int intIndex;
                for (intIndex = this.Pieces.Count - 1; intIndex >= 0; intIndex--)
                {
                    Piece piece = this.Pieces.Item(intIndex);
                    intTotalValue += piece.PositionalPoints;
                }

                return intTotalValue;
            }
        }

        /// <summary>
        ///   Gets PositionsPerSecond.
        /// </summary>
        public int PositionsPerSecond
        {
            get
            {
                return this.PositionsSearched / Math.Max(Convert.ToInt32(this.ThinkingTimeElpased.TotalSeconds), 1);
            }
        }

        /// <summary>
        ///   Gets PositionsSearched.
        /// </summary>
        public int PositionsSearched { get; protected set; }

        /// <summary>
        ///   Gets PrincipalVariation.
        /// </summary>
        public Moves PrincipalVariation { get; private set; }

        /// <summary>
        ///   Gets PrincipalVariationText.
        /// </summary>
        public string PrincipalVariationText
        {
            get
            {
                string strText = string.Empty;
                if (this.PrincipalVariation != null)
                {
                    for (int intIndex = 0; intIndex < this.PrincipalVariation.Count; intIndex++)
                    {
                        if (intIndex < this.PrincipalVariation.Count)
                        {
                            Move move = this.PrincipalVariation[intIndex];
                            if (move != null)
                            {
                                strText += (move.Piece.Name == Piece.PieceNames.Pawn
                                                ? string.Empty
                                                : move.Piece.Abbreviation) + move.From.Name
                                           + (move.PieceCaptured != null ? "x" : string.Empty) + move.To.Name + " ";
                            }
                        }
                    }
                }

                return strText;
            }
        }

        /// <summary>
        ///   Gets Score.
        /// </summary>
        public int Score
        {
            get
            {
                return this.Points - this.OtherPlayer.Points;
            }
        }

        /// <summary>
        ///   Gets SearchDepth.
        /// </summary>
        public int SearchDepth { get; private set; }

        /// <summary>
        ///   Gets SearchPositionNo.
        /// </summary>
        public int SearchPositionNo { get; protected set; }

        /// <summary>
        ///   Gets Status.
        /// </summary>
        public StatusNames Status
        {
            get
            {
                if (this.IsInCheckMate)
                {
                    return StatusNames.InCheckMate;
                }

                if (!this.CanMove)
                {
                    return StatusNames.InStaleMate;
                }

                if (this.IsInCheck)
                {
                    return StatusNames.InCheck;
                }

                return StatusNames.Normal;
            }
        }

        /// <summary>
        ///   Gets ThinkingTimeAllotted.
        /// </summary>
        public TimeSpan ThinkingTimeAllotted { get; private set; }

        /// <summary>
        ///   Gets ThinkingTimeElpased.
        /// </summary>
        public TimeSpan ThinkingTimeElpased
        {
            get
            {
                return DateTime.Now - this.Clock.TurnStartTime;
            }
        }

        /// <summary>
        ///   Gets ThinkingTimeRemaining.
        /// </summary>
        public TimeSpan ThinkingTimeRemaining
        {
            get
            {
                return this.ThinkingTimeAllotted - this.ThinkingTimeElpased;
            }
        }

        /// <summary>
        ///   Gets TotalPieceValue.
        /// </summary>
        public int TotalPieceValue
        {
            get
            {
                int intValue = 0;
                foreach (Piece piece in this.Pieces)
                {
                    intValue += piece.Value;
                }

                return intValue;
            }
        }

        /// <summary>
        ///   Gets TotalPositionsToSearch.
        /// </summary>
        public int TotalPositionsToSearch { get; protected set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the number of pawns in play.
        /// </summary>
        private int PawnCountInPlay { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The abort thinking.
        /// </summary>
        public void AbortThinking()
        {
            if (this.threadThought != null && this.threadThought.ThreadState == ThreadState.Running)
            {
                this.threadThought.Abort();
                this.threadThought.Join();
                this.threadThought = null;
            }
        }

        /// <summary>
        /// The can claim move repetition draw.
        /// </summary>
        /// <param name="numberOfMoves">
        /// The no of moves.
        /// </param>
        /// <returns>
        /// True if, can claim move repetition draw.
        /// </returns>
        public bool CanClaimMoveRepetitionDraw(int numberOfMoves)
        {
            if (Game.MoveHistory.Count == 0)
            {
                return false;
            }

            // if (this.Colour==Game.MoveHistory.Last.Piece.Player.Colour)
            // {
            // return false;
            // }
            Move move;
            int intRepetitionCount = 1;
            int intIndex = Game.MoveHistory.Count - 1;
            for (; intIndex >= 0; intIndex--, intIndex--)
            {
                move = Game.MoveHistory[intIndex];
                if (move.HashCodeA == Board.HashCodeA && move.HashCodeB == Board.HashCodeB)
                {
                    if (intRepetitionCount >= numberOfMoves)
                    {
                        return true;
                    }

                    intRepetitionCount++;
                }

                if (move.Piece.Name == Piece.PieceNames.Pawn || move.PieceCaptured != null)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// The capture all pieces.
        /// </summary>
        public void CaptureAllPieces()
        {
            for (int intIndex = this.Pieces.Count - 1; intIndex >= 0; intIndex--)
            {
                Piece piece = this.Pieces.Item(intIndex);
                piece.Capture();
            }
        }

        /// <summary>
        /// The decrease material count.
        /// </summary>
        public void DecreaseMaterialCount()
        {
            this.MaterialCount--;
        }

        /// <summary>
        /// The decrease pawn count.
        /// </summary>
        public void DecreasePawnCount()
        {
            this.PawnCountInPlay--;
        }

        /// <summary>
        /// The demote all pieces.
        /// </summary>
        public void DemoteAllPieces()
        {
            for (int intIndex = this.Pieces.Count - 1; intIndex >= 0; intIndex--)
            {
                Piece piece = this.Pieces.Item(intIndex);
                if (piece.HasBeenPromoted)
                {
                    piece.Demote();
                }
            }
        }

        /// <summary>
        /// The determine check status.
        /// </summary>
        /// <returns>
        /// The determine check status.
        /// </returns>
        public bool DetermineCheckStatus()
        {
            return ((PieceKing)this.King.Top).DetermineCheckStatus();
        }

        /// <summary>
        /// The force immediate move.
        /// </summary>
        public void ForceImmediateMove()
        {
            if (this.IsThinking && !this.forceAnImmediateMove)
            {
                this.forceAnImmediateMove = true;
                while (this.threadThought != null)
                {
                    Thread.Sleep(50);
                }
            }
        }

        /// <summary>
        /// The generate lazy moves.
        /// </summary>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <param name="moves">
        /// The moves.
        /// </param>
        /// <param name="movesType">
        /// The moves type.
        /// </param>
        /// <param name="squareAttacking">
        /// The square attacking.
        /// </param>
        public void GenerateLazyMoves(int depth, Moves moves, Moves.MoveListNames movesType, Square squareAttacking)
        {
            // if (squareAttacking==null)
            // {
            // All moves as defined by movesType
            foreach (Piece piece in this.Pieces)
            {
                piece.GenerateLazyMoves(moves, movesType);

                /*
                if (movesType != Moves.MoveListNames.All)
                {
                    int intIndex;
                    for (intIndex = moves.Count - 1; intIndex >= 0; intIndex--)
                    {
                        Move move = moves[intIndex];
                        if (!( 
                             move.Name == Move.MoveNames.PawnPromotionQueen
                             &&
                             move.PieceCaptured == null
                             (move.Name == Move.MoveNames.Standard
                              && move.From.Piece.BasicValue < move.To.Piece.BasicValue)
                             ||
                             (move.Name == Move.MoveNames.Standard
                              && !move.To.PlayerCanMoveToThisSquare(move.Piece.Player.OtherPlayer))
                             ||
                             move.To.Ordinal==squareAttacking.Ordinal 
                             ))
                        {
                            // TODO generating all then removing non-captures must be very slow!
                            moves.Remove(move);
                        }
                    }
                }
                */
            }

            // }
            // else
            // {
            // Just re-capture moves
            // squareAttacking.AttackerMoveList(moves, this);
            // }
        }

        /// <summary>
        /// The generate legal moves.
        /// </summary>
        /// <param name="moves">
        /// The moves.
        /// </param>
        public void GenerateLegalMoves(Moves moves)
        {
            foreach (Piece piece in this.Pieces)
            {
                piece.GenerateLegalMoves(moves);
            }
        }

        /// <summary>
        /// The has piece name.
        /// </summary>
        /// <param name="piecename">
        /// The piecename.
        /// </param>
        /// <returns>
        /// The has piece name.
        /// </returns>
        public bool HasPieceName(Piece.PieceNames piecename)
        {
            if (piecename == Piece.PieceNames.Pawn && this.PawnCountInPlay > 0)
            {
                return true;
            }

            foreach (Piece piece in this.Pieces)
            {
                if (piece.Name == piecename)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The increase material count.
        /// </summary>
        public void IncreaseMaterialCount()
        {
            this.MaterialCount++;
        }

        /// <summary>
        /// The increase pawn count.
        /// </summary>
        public void IncreasePawnCount()
        {
            this.PawnCountInPlay++;
        }

        /*
        public Move MoveFromNotation(string Text)
        {
            Piece piece = null;
            Square from = null;
            Square square;
            Square to = null;
            Piece pieceTaken = null;
            Move.PieceNames MoveName = Move.PieceNames.Standard;
            Player.enmColour colour;
            string strTo = "";
            string strAction="";
            string strFromFile = "";
            string strFromRank = "";
            string strPieceName = "";
            int intPos;

            Text.Trim();

            if (Text=="")
            {
                Text="";
            }

            // Castle king-side
            if (Text.ToUpper()=="OO" || Text.ToUpper()=="O-O") { from=this.King.Square; to=Board.GetSquare(this.King.Square.Ordinal+2); piece=this.King; MoveName=Move.PieceNames.CastleKingSide; goto exithere;}
            // Castle queen-side
            if (Text.ToUpper()=="OOO" || Text.ToUpper()=="O-O-O") { from=this.King.Square; to=Board.GetSquare(this.King.Square.Ordinal-3); piece=this.King; MoveName=Move.PieceNames.CastleQueenSide; goto exithere;}


            intPos = Text.Length;
            // To square;
            intPos-=2;
            strTo = Text.Substring(intPos);
            // Action
            intPos--;
            if (intPos>=0 && Text.Substring(intPos,1).ToUpper()=="X")
            {
                strAction = Text.Substring(intPos,1).ToUpper();
                intPos--; // skip the "x"
            }
            // Rank number
            if (intPos>=0 && Char.IsDigit(Convert.ToChar(Text.Substring(intPos,1))))
            {
                strFromRank = Text.Substring(intPos,1);
                intPos--;
            }
            // File letter
            if (intPos>=0 && Text.Substring(intPos,1)!=Text.Substring(intPos,1).ToUpper())
            {
                strFromFile = Text.Substring(intPos,1);
                intPos--;
            }
            if (intPos>=0)
            {
                strPieceName = Text.Substring(intPos,1);
            }
            else
            {
                strPieceName = "P";
            }

            to=Board.GetSquare(strTo);
            pieceTaken = to.Piece;
            
            switch (strPieceName)
            {
                case "P":
                    if (strAction!="X")
                    {
                        square = Board.GetSquare(to.Ordinal-this.PawnForwardOffset);
                        piece = square.Piece;
                        while (piece==null || piece.Name!=Piece.PieceNames.Pawn || piece.Player.Colour!=this.Colour)
                        {
                            square = Board.GetSquare(square.Ordinal-this.PawnForwardOffset);
                            piece = square.Piece;
                        }
                        from=square; 
                        piece=from.Piece; 
                    }
                    else
                    {
                        piece = Board.GetPiece(to.Ordinal+this.OtherPlayer.PawnAttackLeftOffset);
                        if (piece==null || piece.Name!=Piece.PieceNames.Pawn || piece.Player.Colour!=this.Colour || strFromFile!="" && piece.Square.FileName!=strFromFile)
                        {
                            piece = Board.GetPiece(to.Ordinal+this.OtherPlayer.PawnAttackRightOffset);
                        }
                        // En passent not currently handled
                        from = piece.Square;
                    }
                    break;

                case "N":
                    if ( (square = Board.GetSquare(to.Ordinal+33 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile)) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal+18 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal-14 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal-31 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal-33 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal-18 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal+14 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal+31 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.Knight && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece;
                    from = piece.Square;
                    break;

                case "B":
                    colour = (strAction=="X" ? this.OtherPlayer.Colour : this.Colour);
                    if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Bishop, to, 15))!=null && piece.Name==Piece.PieceNames.Bishop && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Bishop, to, 17))!=null && piece.Name==Piece.PieceNames.Bishop && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Bishop, to, -15))!=null && piece.Name==Piece.PieceNames.Bishop && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Bishop, to, -17))!=null && piece.Name==Piece.PieceNames.Bishop && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else piece=null;
                    from = piece.Square;
                    break;

                case "R":
                    if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Rook, to, 1))!=null && piece.Name==Piece.PieceNames.Rook && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Rook, to, -1))!=null && piece.Name==Piece.PieceNames.Rook && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Rook, to, 16))!=null && piece.Name==Piece.PieceNames.Rook && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Rook, to, -16))!=null && piece.Name==Piece.PieceNames.Rook && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else piece=null;
                    from = piece.Square;
                    break;

                case "Q":
                    if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Queen, to, 15))!=null && piece.Name==Piece.PieceNames.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Queen, to, 17))!=null && piece.Name==Piece.PieceNames.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Queen, to, -15))!=null && piece.Name==Piece.PieceNames.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Queen, to, -17))!=null && piece.Name==Piece.PieceNames.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Queen, to, 1))!=null && piece.Name==Piece.PieceNames.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Queen, to, -1))!=null && piece.Name==Piece.PieceNames.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Queen, to, 16))!=null && piece.Name==Piece.PieceNames.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else
                        if ((piece=Board.LinesFirstPiece(this.Colour, Piece.PieceNames.Queen, to, -16))!=null && piece.Name==Piece.PieceNames.Queen && piece.Player.Colour==this.Colour && (strFromFile=="" || piece.Square.FileName==strFromFile)) piece=piece; else piece=null;
                    from = piece.Square;
                    break;

                case "K":
                    if ( (square = Board.GetSquare(to.Ordinal+15))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile)) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal+17 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal-15 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal-17 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal+ 1 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal- 1 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal+16 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece; else
                        if ( (square = Board.GetSquare(to.Ordinal-16 ))!=null && square.Piece!=null && square.Piece.Name==Piece.PieceNames.King && square.Piece.Player.Colour==this.Colour && (strFromFile=="" || square.FileName==strFromFile) ) piece=square.Piece;
                    from = piece.Square;
                    break;
            }

            exithere:
                return new Move(0, 0, MoveName, piece, from, to, pieceTaken, 0, 0);
        }
*/

        /// <summary>
        /// The perft.
        /// </summary>
        /// <param name="targetDepth">
        /// The target depth.
        /// </param>
        public void Perft(int targetDepth)
        {
            this.PositionsSearched = 0;
            this.Perft_Ply(this, targetDepth);
        }

        /// <summary>
        /// The start pondering.
        /// </summary>
        public void StartPondering()
        {
            if (this.Intellegence == IntellegenceNames.Computer
                && this.OtherPlayer.Intellegence == IntellegenceNames.Computer)
            {
                // Can't both ponder at the same time
                return;
            }

            if (Game.IsInAnalyseMode)
            {
                // Can't ponder when in Analyse mode
                return;
            }

            if (Game.EnablePondering)
            {
                if (ponderingHashCodeA != Board.HashCodeA || ponderingHashCodeB != Board.HashCodeB)
                {
                    ponderingHashCodeA = Board.HashCodeA;
                    ponderingHashCodeB = Board.HashCodeB;
                }

                if (!this.IsThinking && !this.OtherPlayer.IsThinking
                    && this.OtherPlayer.Intellegence == IntellegenceNames.Computer && Game.PlayerToPlay == this)
                {
                    this.IsPondering = true;
                    this.StartThinking();
                }
            }
        }

        /// <summary>
        /// The start thinking.
        /// </summary>
        public void StartThinking()
        {
            // Bail out if unable to move
            if (!this.CanMove)
            {
                return;
            }

            // Send draw result is playing WinBoard
            if (WinBoard.Active && this.Intellegence == IntellegenceNames.Computer)
            {
                if (this.CanClaimThreeMoveRepetitionDraw)
                {
                    WinBoard.SendDrawByRepetition();
                    return;
                }
                else if (this.CanClaimFiftyMoveDraw)
                {
                    WinBoard.SendDrawByFiftyMoveRule();
                    return;
                }
                else if (this.CanClaimInsufficientMaterialDraw)
                {
                    WinBoard.SendDrawByFiftyMoveRule();
                    return;
                }
            }

            this.threadThought = new Thread(this.Think);
            this.threadThought.Name = (++Game.ThreadCounter).ToString();

            this.ThinkingBeginning();
            if (this.IsPondering)
            {
                // m_threadThought.Priority = System.Threading.ThreadPriority.BelowNormal;
                this.threadThought.Priority = ThreadPriority.Normal;
            }
            else
            {
                this.threadThought.Priority = ThreadPriority.Normal;
            }

            this.threadThought.Start();
        }

        /// <summary>
        /// The stop pondering.
        /// </summary>
        public void StopPondering()
        {
            if (this.IsPondering)
            {
                this.AbortThinking();
                this.IsPondering = false;
            }
        }

        /// <summary>
        /// The think.
        /// </summary>
        /// <exception cref="ForceImmediateMoveException">
        /// </exception>
        /// <exception cref="ForceImmediateMoveException">
        /// </exception>
        public void Think()
        {
            // Determine the best move available for "this" player instance, from the current board position.
            Debug.WriteLine(
                string.Format(
                    "Thread {0} is " + (this.IsPondering ? "pondering" : "thinking"), Thread.CurrentThread.Name));

            Player player = this; // Set the player, whose move is to be computed, to "this" player object instance
            this.PrincipalVariation = new Moves(); // Best moves line (Principal Variation) found so far.

            // TimeSpan tsnTimePondered = new TimeSpan();
            int intTurnNo = Game.TurnNo;

            this.displayMoveAnalysisTree = Game.CaptureMoveAnalysisData;

            // Set whether to build a post-analysis tree of positions searched
            try
            {
                if (!this.IsPondering && !Game.IsInAnalyseMode)
                {
                    // Query Simple Opening Book
                    if (Game.UseRandomOpeningMoves)
                    {
                        Move moveBook;
                        if ((moveBook = OpeningBookSimple.SuggestRandomMove(player)) != null)
                        {
                            this.PrincipalVariation.Add(moveBook);
                            this.MoveConsidered();
                            throw new ForceImmediateMoveException();
                        }
                    }

                    /* Query Best Opening Book
                        if ((m_moveBest = OpeningBook.SearchForGoodMove(Board.HashCodeA, Board.HashCodeB, this.Colour) )!=null) 
                        {
                            m_moveCurrent = m_moveBest;
                            this.MoveConsidered();
                            throw new ForceImmediateMoveException();
                        }
                    */
                }

                // Time allowed for this player to think
                if (Game.ClockFixedTimePerMove.TotalSeconds > 0)
                {
                    // Absolute fixed time per move. No time is carried over from one move to the next.
                    this.ThinkingTimeAllotted = Game.ClockFixedTimePerMove;
                }
                else if (Game.ClockIncrementPerMove.TotalSeconds > 0)
                {
                    // Incremental clock
                    this.ThinkingTimeAllotted =
                        new TimeSpan(
                            Game.ClockIncrementPerMove.Ticks
                            +
                            ((Game.ClockIncrementPerMove.Ticks * Game.MoveNo
                              + Game.ClockTime.Ticks * Math.Min(Game.MoveNo, 40) / 40) - this.Clock.TimeElapsed.Ticks)
                            / 3);

                    // Make sure we never think for less than half the "Increment" time
                    this.ThinkingTimeAllotted =
                        new TimeSpan(
                            Math.Max(this.ThinkingTimeAllotted.Ticks, Game.ClockIncrementPerMove.Ticks / 2 + 1));
                }
                else if (Game.ClockMaxMoves == 0 && Game.ClockIncrementPerMove.TotalSeconds == 0)
                {
                    // Fixed game time
                    this.ThinkingTimeAllotted = new TimeSpan(this.Clock.TimeRemaining.Ticks / 30);
                }
                else
                {
                    // Conventional n moves in x minutes time
                    this.ThinkingTimeAllotted = new TimeSpan(this.Clock.TimeRemaining.Ticks / this.Clock.MovesRemaining);
                }

                // Minimum of 1 second thinking time
                if (this.ThinkingTimeAllotted.TotalSeconds < 1)
                {
                    this.ThinkingTimeAllotted = new TimeSpan(0, 0, 1);
                }

                // The computer only stops "thinking" when it has finished a full ply of thought, 
                // UNLESS m_tsnThinkingTimeMaxAllowed is exceeded, or clock runs out, then it stops right away.
                if (Game.ClockFixedTimePerMove.TotalSeconds > 0)
                {
                    // Fixed time per move
                    this.thinkingTimeMaxAllowed = Game.ClockFixedTimePerMove;
                }
                else
                {
                    // Variable time per move
                    this.thinkingTimeMaxAllowed =
                        new TimeSpan(
                            Math.Min(
                                this.ThinkingTimeAllotted.Ticks * 2, 
                                this.Clock.TimeRemaining.Ticks - (new TimeSpan(0, 0, 0, 2)).Ticks));
                }

                // Minimum of 2 seconds thinking time
                if (this.thinkingTimeMaxAllowed.TotalSeconds < 2)
                {
                    this.thinkingTimeMaxAllowed = new TimeSpan(0, 0, 2);
                }

                // A new deeper ply of search will only be started, IF the cutoff time hasnt been reached yet.
                this.thinkingTimeCutoff = new TimeSpan(this.ThinkingTimeAllotted.Ticks / 3);

                this.forceAnImmediateMove = false; // Set to stop thread thinking and return best move
                this.PositionsSearched = 0; // Total number of positions considered so far
                this.Evaluations = 0;

                // Total number of times the evaluation function has been called (May be less than PositonsSearched if hashtable works well)
                if (Game.IsInAnalyseMode)
                {
                    HashTable.Clear();
                    HashTableCheck.Clear();
                    HashTablePawnKing.Clear();
                    History.Clear();
                }
                else
                {
                    if (this.CanClaimMoveRepetitionDraw(2))
                    {
                        // See if' we're in a 2 move repetition position, and if so, clear the hashtable, as old hashtable entries corrupt 3MR detection
                        HashTable.Clear();
                    }
                    else
                    {
                        HashTable.ResetStats(); // Reset the main hash table hit stats
                    }

                    HashTableCheck.ResetStats();

                    // We also have a hash table in which we just store the check status for both players
                    HashTablePawnKing.ResetStats();

                    // And finally a hash table that stores the positional score of just the pawns.
                    History.Clear(); // Clear down the History Heuristic info, at the start of each move.
                }

                if (!this.IsPondering)
                {
                    this.Clock.Start();
                }

                // Set max search depth, as defined is game difficulty settings
                this.MaxSearchDepth = Game.MaximumSearchDepth == 0 ? 32 : Game.MaximumSearchDepth;

                // Here begins the main Iteractive Deepening loop of the entire search algorithm. (Cue dramitic music!)
                int intScore = player.Score;

                for (this.SearchDepth = MinSearchDepth; this.SearchDepth <= this.MaxSearchDepth; this.SearchDepth++)
                {
                    if (this.displayMoveAnalysisTree)
                    {
                        Game.MoveAnalysis = new Moves();
                    }

                    intScore = this.Aspirate(player, this.PrincipalVariation, intScore, Game.MoveAnalysis);

                    // intScore = AlphaBeta(player, m_intSearchDepth, m_intSearchDepth, MIN_SCORE, MAX_SCORE, null, movesPV, intScore);

                    WinBoard.SendThinking(
                        this.SearchDepth, 
                        intScore, 
                        DateTime.Now - this.Clock.TurnStartTime, 
                        this.PositionsSearched, 
                        this.PrincipalVariationText);

                    if (!Game.IsInAnalyseMode && Game.ClockFixedTimePerMove.TotalSeconds == 0 && !this.IsPondering
                        && (DateTime.Now - this.Clock.TurnStartTime) > this.thinkingTimeCutoff)
                    {
                        // #if !DEBUG // Note the exclamation mark
                        throw new ForceImmediateMoveException();

                        // #endif
                    }

                    if (intScore > 99999 || intScore < -99999)
                    {
                        break; // Checkmate found so dont bother searching any deeper
                    }
                }
            }
            catch (ForceImmediateMoveException x)
            {
                // Undo any moves made during thinking
                Debug.WriteLine(x.ToString());
                while (Game.TurnNo > intTurnNo)
                {
                    Move.Undo(Game.MoveHistory.Last);
                }
            }

            if (this.MoveConsidered != null)
            {
                this.MoveConsidered();
            }

            Debug.WriteLine(
                string.Format(
                    "Thread {0} is ending " + (this.IsPondering ? "pondering" : "thinking"), Thread.CurrentThread.Name));

            this.threadThought = null;
            if (this.MoveConsidered != null && !this.IsPondering)
            {
                this.ReadyToMakeMove();
            }

            this.IsPondering = false;

            // Send total elapsed time to generate this move.
            WinBoard.SendMoveTime(DateTime.Now - this.Clock.TurnStartTime);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The set pieces at starting positions.
        /// </summary>
        protected abstract void SetPiecesAtStartingPositions();

        /// <summary>
        /// The alpha beta.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="ply">
        /// The ply.
        /// </param>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <param name="alpha">
        /// The alpha.
        /// </param>
        /// <param name="beta">
        /// The beta.
        /// </param>
        /// <param name="parentMove">
        /// The move analysed.
        /// </param>
        /// <param name="principalVariationMoves">
        /// The moves p v_ parent.
        /// </param>
        /// <param name="intTotalExtensions">
        /// The int total extensions.
        /// </param>
        /// <returns>
        /// The alpha beta.
        /// </returns>
        /// <exception cref="ForceImmediateMoveException">
        /// </exception>
        /// <exception cref="ForceImmediateMoveException">
        /// </exception>
        private int AlphaBeta(
            Player player, 
            int ply, 
            int depth, 
            int alpha, 
            int beta, 
            Move parentMove, 
            Moves principalVariationMoves, 
            int intTotalExtensions,
            Moves analysisParentBranch
            )
        {
            int val = int.MinValue;
            HashTable.HashTypeNames hashType = HashTable.HashTypeNames.Alpha;
            Move moveHash = null;
            Move moveBest = null;
            bool blnPVNode = false;
            int intScoreAtEntry = 0;
            bool blnAllMovesWereGenerated;
            int intLegalMovesAttempted = 0;
            bool blnIsInCheck = player.IsInCheck;

            if (this.forceAnImmediateMove)
            {
                throw new ForceImmediateMoveException();
            }

            Moves movesPV = new Moves();

            this.PositionsSearched++;

            if (parentMove != null && parentMove.IsThreeMoveRepetition)
            {
                return -player.OtherPlayer.Score;
            }

            if ((val = HashTable.ProbeHash(Board.HashCodeA, Board.HashCodeB, ply, alpha, beta, player.Colour))
                != HashTable.NotFoundInHashTable)
            {
                // High values of "val" indicate that a checkmate has been found
                if (val > 1000000 || val < -1000000)
                {
                    if (this.MaxSearchDepth - depth > 0)
                    {
                        val /= this.MaxSearchDepth - depth;
                    }
                }

                principalVariationMoves.Clear();
                if (HashTable.ProbeForBestMove(Board.HashCodeA, Board.HashCodeB, player.Colour) != null)
                {
                    principalVariationMoves.Add(HashTable.ProbeForBestMove(Board.HashCodeA, Board.HashCodeB, player.Colour));
                }

                return val;
            }

            if (intTotalExtensions > this.MaxExtensions)
            {
                this.MaxExtensions = intTotalExtensions;
            }

            // Generate moves
            Moves movesPossible = new Moves();
            blnAllMovesWereGenerated = depth > 0; // || blnIsInCheck); 
            if (blnAllMovesWereGenerated)
            {
                player.GenerateLazyMoves(depth, movesPossible, Moves.MoveListNames.All, null);
            }
            else
            {
                // Captures only
                player.GenerateLazyMoves(
                    depth, movesPossible, Moves.MoveListNames.CapturesChecksPromotions, parentMove.To);
            }

            // Depth <=0 means we're into Quiescence searching
            if (depth <= 0)
            {
                if (depth < this.MaxQuiesenceDepthReached)
                {
                    this.MaxQuiesenceDepthReached = depth;
                }

                intScoreAtEntry = val = -player.OtherPlayer.Score;
                this.Evaluations++;

                if (val > 1000000 || val < -1000000)
                {
                    val /= this.MaxSearchDepth - depth;
                }

                // If there are no more moves, then return val.
                // TODO remove checking moves from count in quiesence only
                if (movesPossible.Count == 0) 
                {
                    return val;
                }
            }

            // Adaptive Null-move forward pruning
            int r = depth > 6 ? 3 : 2;
            if (depth > (r + 1) && parentMove != null && parentMove.Name != Move.MoveNames.NullMove
                && Game.Stage != Game.GameStageNames.End && !blnIsInCheck)
            {
                Move moveNull = new Move(Game.TurnNo, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 0);
                val =
                    -this.AlphaBeta(
                        player.OtherPlayer, 
                        ply - 1, 
                        depth - r - 1, 
                        -beta, 
                        -beta + 1, 
                        moveNull, 
                        movesPV, 
                        intTotalExtensions,
                        null);
                if (val >= beta)
                {
                    return beta;
                }
            }

            // Get last iteration's best move from the Transition Table
            moveHash = HashTable.ProbeForBestMove(Board.HashCodeA, Board.HashCodeB, player.Colour);

            // Get Killers
            Move moveKillerA = KillerMoves.RetrieveA(ply);
            Move moveKillerB = KillerMoves.RetrieveB(ply);
            Move moveKillerA2 = KillerMoves.RetrieveA(ply + 2);
            Move moveKillerB2 = KillerMoves.RetrieveB(ply + 2);

            // Sort moves
            // TODO BUG: Captures are not being sorted be last value piece.
            this.SortBestMoves(movesPossible, moveHash, moveKillerA, moveKillerA2, moveKillerB, moveKillerB2, player);

            if (ply == this.SearchDepth)
            {
                this.TotalPositionsToSearch = movesPossible.Count;
                this.SearchPositionNo = 0;
            }

            foreach (Move move in movesPossible)
            {
                Move moveThis = move.Piece.Move(move.Name, move.To);
                if (ply == this.SearchDepth)
                {
                    this.SearchPositionNo++;
                    this.CurrentMove = moveThis;
                    if (this.MoveConsidered != null)
                    {
                        this.MoveConsidered();
                    }

                    this.MaxQuiesenceDepthReached = this.SearchDepth;

                    // A little counter to record the deepest Quiescence depth searched on this move.
                    this.MaxExtensions = 0;

                    // A little counter to track the number of extensions on this move.
                }

                if (player.IsInCheck)
                {
                    Move.Undo(moveThis);
                    continue;
                }

                intLegalMovesAttempted++;

                if (moveBest == null)
                {
                    moveBest = moveThis;
                }

                if (this.displayMoveAnalysisTree && this.SearchDepth == this.MaxSearchDepth)
                {
                    // Add moves to post-move analysis tree, if option set by user
                    if (parentMove == null || parentMove.Name != Move.MoveNames.NullMove)
                    {
                        analysisParentBranch.Add(moveThis);
                    }
                    moveThis.Moves = new Moves();
                }


                // Search Extensions
                int intExtension = 0;

                if (movesPossible.Count == 1)
                {
                    // Single Response
                    intExtension = 1;
                }
                else if (parentMove != null && parentMove.IsEnemyInCheck)
                {
                    // Check evasion
                    intExtension = 1;
                }
                else if (parentMove != null && parentMove.PieceCaptured != null && moveThis.PieceCaptured != null
                         && parentMove.PieceCaptured.BasicValue == moveThis.PieceCaptured.BasicValue
                         && parentMove.To == moveThis.To)
                {
                    // Recapture piece of same basic value (on the same square)
                    intExtension = 1;
                }
                else if (moveThis.Piece.Name == Piece.PieceNames.Pawn
                         &&
                         (moveThis.Piece.Player.Colour == ColourNames.White && moveThis.To.Rank == 6
                          || moveThis.Piece.Player.Colour == ColourNames.Black && moveThis.To.Rank == 1))
                {
                    // Pawn push to 7th rank
                    intExtension = 1;
                }

                // Reductions
                // Only reduce if this move hasn't been extended.
                if (intExtension == 0)
                {
                    if (depth > 2 && !blnIsInCheck && moveThis.PieceCaptured == null && !moveThis.IsEnemyInCheck)
                    {
                        int[] margin = { 0, 0, 0, 5000, 5000, 7000, 7000, 9000, 9000, 15000, 15000, 15000, 15000, 15000, 15000, 15000, 15000, 15000 };

                        // int intLazyEval = this.TotalPieceValue - this.OtherPlayer.TotalPieceValue;
                        int intLazyEval = player.Score;
                        if (alpha > intLazyEval + margin[depth])
                        {
                            intExtension = -1;
                        }
                    }

                    // Futility Pruning
                    if (!blnIsInCheck)
                    {
                        switch (depth)
                        {
                            case 2:
                            case 3:

                                // case 4:
                                if (moveThis.PieceCaptured == null && !move.IsEnemyInCheck)
                                {
                                    int intLazyEval = player.Score;

                                    switch (depth)
                                    {
                                        case 2:

                                            // Standard Futility Pruning
                                            if (intLazyEval + 3000 <= alpha)
                                            {
                                                intExtension = -1;
                                            }

                                            break;

                                        case 3:

                                            // Extended Futility Pruning
                                            if (intLazyEval + 5000 <= alpha)
                                            {
                                                intExtension = -1;
                                            }

                                            break;

                                        case 4:

                                            // Razoring
                                            if (intLazyEval + 9750 <= alpha)
                                            {
                                                intExtension = -1;
                                            }

                                            break;
                                    }
                                }

                                break;
                        }
                    }
                }

                /*
                if (intExtension>0 && intTotalExtensions>=m_intSearchDepth)
                {
                    intExtension = 0;
                }
                */

                /* #if DEBUG  // Avoid to break in a zero window research so alpha + 1 < beta
                   if ((alpha + 1 < beta) && DebugMatchVariation(m_intSearchDepth - ply, moveThis)) Debugger.Break();
                 #endif */
                if (blnPVNode)
                {
                    val =
                        -this.AlphaBeta(
                            player.OtherPlayer, 
                            ply - 1, 
                            (depth + intExtension) - 1, 
                            -alpha - 1, 
                            -alpha, 
                            moveThis, 
                            movesPV, 
                            intTotalExtensions + intExtension,
                            moveThis.Moves);
                    if ((val > alpha) && (val < beta))
                    {
                        // fail
                        if (this.displayMoveAnalysisTree && this.SearchDepth == this.MaxSearchDepth && parentMove != null && parentMove.Name != Move.MoveNames.NullMove)
                        {
                            moveThis.Moves.Clear();
                        }
                        val =
                            -this.AlphaBeta(
                                player.OtherPlayer, 
                                ply - 1, 
                                (depth + intExtension) - 1, 
                                -beta, 
                                -alpha, 
                                moveThis, 
                                movesPV, 
                                intTotalExtensions + intExtension,
                                moveThis.Moves);
                    }
                }
                else
                {
                    val =
                        -this.AlphaBeta(
                            player.OtherPlayer, 
                            ply - 1, 
                            (depth + intExtension) - 1, 
                            -beta, 
                            -alpha, 
                            moveThis, 
                            movesPV, 
                            intTotalExtensions + intExtension,
                            moveThis.Moves);
                }

                if (!blnAllMovesWereGenerated && val < intScoreAtEntry)
                {
                    // This code is executed mostly in quiescence when not all moves are tried (maybe just captures)
                    // and the best score we've got is worse than the score we had before we considered any moves
                    // then revert to that score, because we dont want the computer to think that it HAS to make a capture
                    val = intScoreAtEntry;
                }

                move.Score = moveThis.Score = val;

                Move.Undo(moveThis);

                if (val >= beta)
                {
                    alpha = beta;
                    moveThis.Beta = beta;
                    hashType = HashTable.HashTypeNames.Beta;
                    moveBest = moveThis;

                    // if (move.Score < 15000)
                    // {
                    History.Record(player.Colour, moveThis.From.Ordinal, moveThis.To.Ordinal, depth * depth);

                    // 15Mar06 Nimzo Don't include captures as killer moves
                    if ((moveThis.PieceCaptured == null)
                        && ((parentMove == null) || (parentMove.Name != Move.MoveNames.NullMove)))
                    {
                        KillerMoves.RecordPossibleKillerMove(ply, moveBest);
                    }

                    // End Nimzo code

                    // }
                    goto Exit;
                }

                if (val > alpha)
                {
                    blnPVNode = true; /* This is a PV node */
                    alpha = val;
                    hashType = HashTable.HashTypeNames.Exact;
                    moveBest = moveThis;

                    // Collect the Prinicial Variation
                    principalVariationMoves.Clear();
                    principalVariationMoves.Add(moveThis);
                    foreach (Move moveCopy in movesPV)
                    {
                        principalVariationMoves.Add(moveCopy);
                    }

                    // #if DEBUG
                    // Debug.WriteLineIf((ply == m_intSearchDepth) && (ply > 1), string.Format("{0} {1} {2}", ply, PvLine(principalVariationMoves), alpha));
                    // #endif
                }

                moveThis.Alpha = alpha;
                moveThis.Beta = beta;

                if (!Game.IsInAnalyseMode && !this.IsPondering && this.SearchDepth > MinimumSearchDepth
                    && (DateTime.Now - this.Clock.TurnStartTime) > this.thinkingTimeMaxAllowed)
                {
                    // #if !DEBUG // Note the exclamation mark
                    throw new ForceImmediateMoveException();

                    // #endif
                }
            }

            // Check for Stalemate
            if (intLegalMovesAttempted == 0)
            {
                // depth>0 && !player.OtherPlayer.IsInCheck
                // alpha = this.Score;
                alpha = -player.OtherPlayer.Score;
            }

            Exit:

            // Record best move
            if (moveBest != null)
            {
                HashTable.RecordHash(
                    Board.HashCodeA, 
                    Board.HashCodeB, 
                    ply, 
                    alpha, 
                    hashType, 
                    moveBest.From.Ordinal, 
                    moveBest.To.Ordinal, 
                    moveBest.Name, 
                    player.Colour);
            }
            else
            {
                HashTable.RecordHash(
                    Board.HashCodeA, 
                    Board.HashCodeB, 
                    ply, 
                    alpha, 
                    hashType, 
                    -1, 
                    -1, 
                    Move.MoveNames.NullMove, 
                    player.Colour);
            }

            return alpha;
        }

        /// <summary>
        /// The aspirate.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="principalVariationMoves">
        /// The moves p v_ parent.
        /// </param>
        /// <param name="intLastIterationsScore">
        /// The int last iteractions score.
        /// </param>
        /// <returns>
        /// The aspirate.
        /// </returns>
        private int Aspirate(Player player, Moves principalVariationMoves, int intLastIterationsScore, Moves analysisParentBranch)
        {
            int alpha = MinScore; // Score of the best move found so far
            int beta = MaxScore; // Score of the best move found by the opponent
            int val = alpha;

            for (int intAttempt = 0; intAttempt < 3; intAttempt++)
            {
                switch (intAttempt)
                {
                    case 0:
                        alpha = intLastIterationsScore - 500;
                        beta = intLastIterationsScore + 500;
                        break;

                    case 1:
                        alpha = intLastIterationsScore - 2000;
                        beta = intLastIterationsScore + 2000;
                        break;

                    case 2:
                        alpha = MinScore;
                        beta = MaxScore;
                        break;
                }

                val = this.AlphaBeta(player, this.SearchDepth, this.SearchDepth, alpha, beta, null, principalVariationMoves, 0, analysisParentBranch);
                if (val > alpha && val < beta)
                {
                    break;
                }
            }

            return val;
        }

        /// <summary>
        /// Evaluates and assigns a move order score to a move
        /// </summary>
        /// <param name="move">
        /// Move to evaluate
        /// </param>
        /// <param name="moveHash">
        /// Best move from hash table.
        /// </param>
        /// <param name="moveKillerA">
        /// Best killer move from this ply.
        /// </param>
        /// <param name="moveKillerA2">
        /// Second best killer move from this ply.
        /// </param>
        /// <param name="moveKillerB">
        /// Best killer move from previous ply.
        /// </param>
        /// <param name="moveKillerB2">
        /// Second best killer move from previous ply.
        /// </param>
        /// <param name="player">
        /// The player.
        /// </param>
        private void AssignMoveOrderScore(
            Move move, 
            Move moveHash, 
            Move moveKillerA, 
            Move moveKillerA2, 
            Move moveKillerB, 
            Move moveKillerB2, 
            Player player)
        {
            move.Score = 0;

            if (moveHash != null && Move.MovesMatch(move, moveHash))
            {
                move.Score += 10000000;
                return;
            }

            switch (move.Name)
            {
                case Move.MoveNames.PawnPromotionQueen:
                    move.Score += 900000;
                    break;
                case Move.MoveNames.PawnPromotionRook:
                    move.Score += 500000;
                    break;
                case Move.MoveNames.PawnPromotionBishop:
                    move.Score += 300000;
                    break;
                case Move.MoveNames.PawnPromotionKnight:
                    move.Score += 300000;
                    break;
            }

            if (move.PieceCaptured != null)
            {
                // Resulty of Static exchange evaluation
                move.Score += this.SEE(move) * 100000;

                if (move.Score != 0)
                {
                    return;
                }

                // "Good" capture
                if (move.From.Piece.Name == Piece.PieceNames.Queen && move.To.Piece.Name == Piece.PieceNames.Queen)
                {
                    move.Score += 99999;
                    return;
                }
                else if (move.From.Piece.Name == Piece.PieceNames.Rook && move.To.Piece.Name == Piece.PieceNames.Rook)
                {
                    move.Score += 99998;
                    return;
                }
                else if (move.From.Piece.Name == Piece.PieceNames.Knight && move.To.Piece.Name == Piece.PieceNames.Bishop)
                {
                    move.Score += 99997;
                    return;
                }
                else if (move.From.Piece.Name == Piece.PieceNames.Bishop
                         && move.To.Piece.Name == Piece.PieceNames.Bishop)
                {
                    move.Score += 99996;
                    return;
                }
                else if (move.From.Piece.Name == Piece.PieceNames.Bishop
                         && move.To.Piece.Name == Piece.PieceNames.Knight)
                {
                    move.Score += 99995;
                    return;
                }
                else if (move.From.Piece.Name == Piece.PieceNames.Pawn
                         &&
                         (move.Name == Move.MoveNames.EnPassent
                          &&
                          Board.GetPiece(move.To.Ordinal - player.PawnForwardOffset).Name
                          == Piece.PieceNames.Pawn || move.To.Piece.Name == Piece.PieceNames.Pawn))
                {
                    move.Score += 99994;
                    return;
                }
            }

            move.Score += History.Retrieve(player.Colour, move.From.Ordinal, move.To.Ordinal);

            // Killer moves
            if (Move.MovesMatch(move, moveKillerA))
            {
                move.Score += 400;
            }

            /*
            if (Move.MovesMatch(move, moveKillerA2))
            {
                move.Score += 20003;
                return;
            }
            */
            if (Move.MovesMatch(move, moveKillerB))
            {
                move.Score += 200;
            }

            /*
            if (Move.MovesMatch(move, moveKillerB2))
            {
                move.Score += 20001;
                return;
            }
            if (move.Score == 0)
            {
                // do something smart
                int x = 0;
            }
            */

            // Score based upon tactical positional value of board square i.e. how close to centre
            move.Score += move.To.Value - move.From.Value;
        }

        /// <summary>
        /// The perft_ ply.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="depth">
        /// The depth.
        /// </param>
        private void Perft_Ply(Player player, int depth)
        {
            if (depth <= 0)
            {
                return;
            }

            Moves moves = new Moves();
            player.GenerateLegalMoves(moves);

            foreach (Move move in moves)
            {
                Move moveUndo = move.Piece.Move(move.Name, move.To);

                this.PositionsSearched++;

                // Debug.WriteLine(move.DebugText + ",");
                this.Perft_Ply(player.OtherPlayer, depth - 1);

                Move.Undo(moveUndo);
            }
        }

        /// <summary>
        /// Performs a Static Exchange Evaluation to determine the value of a move after all possible re-captures are resolved.
        /// </summary>
        /// <param name="moveMade">
        /// move to be evaluated
        /// </param>
        /// <returns>
        /// The see.
        /// </returns>
        private int SEE(Move moveMade)
        {
            // Static Exchange Evaluator

            // Generate moves
            Moves movesFriendly = new Moves();
            Moves movesEnemy = new Moves();

            moveMade.To.AttackersMoveList(movesFriendly, moveMade.Piece.Player);
            moveMade.To.AttackersMoveList(movesEnemy, moveMade.Piece.Player.OtherPlayer);

            // Remove piece I'm going to move first from my list of moves
            foreach (Move move in movesFriendly)
            {
                if (move.Piece == moveMade.Piece)
                {
                    movesFriendly.Remove(move);
                    break;
                }
            }

            // sort remaining moves by piece value
            foreach (Move move in movesFriendly)
            {
                move.Score = 20 - move.Piece.BasicValue;
            }

            movesFriendly.SortByScore();

            if (movesEnemy.Count > 1)
            {
                // sort remaining moves by piece value
                foreach (Move move in movesEnemy)
                {
                    move.Score = 20 - move.Piece.BasicValue;
                }

                movesEnemy.SortByScore();
            }

            int intTotalFriendlyGain = moveMade.PieceCaptured.BasicValue;
            int intLastMovedPieceValue = moveMade.Piece.BasicValue;

            // Now make a virtual move from each players move list, in order, until one of the players has no remaining moves.
            for (int intIndex = 0;; intIndex++)
            {
                if (intIndex >= movesEnemy.Count)
                {
                    break;
                }

                intTotalFriendlyGain -= intLastMovedPieceValue;
                intLastMovedPieceValue = movesEnemy[intIndex].Piece.BasicValue;
                if (intIndex >= movesFriendly.Count)
                {
                    break;
                }

                intTotalFriendlyGain += intLastMovedPieceValue;
                intLastMovedPieceValue = movesFriendly[intIndex].Piece.BasicValue;
            }

            return intTotalFriendlyGain;
        }

        /// <summary>
        /// Sorts moves so that the best moves are first
        /// </summary>
        /// <param name="movesToSort">
        /// </param>
        /// <param name="moveHash">
        /// </param>
        /// <param name="moveKillerA">
        /// Best killer move from this ply.
        /// </param>
        /// <param name="moveKillerA2">
        /// Second best killer move from this ply.
        /// </param>
        /// <param name="moveKillerB">
        /// Best killer move from previous ply.
        /// </param>
        /// <param name="moveKillerB2">
        /// Second best killer move from previous ply.
        /// </param>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="moveHash">
        /// Best move from hash table.
        /// </param>
        private void SortBestMoves(
            Moves movesToSort, 
            Move moveHash, 
            Move moveKillerA, 
            Move moveKillerA2, 
            Move moveKillerB, 
            Move moveKillerB2, 
            Player player)
        {
            foreach (Move movex in movesToSort)
            {
                this.AssignMoveOrderScore(movex, moveHash, moveKillerA, moveKillerA2, moveKillerB, moveKillerB2, player);
            }

            movesToSort.SortByScore();
        }

        #endregion
    }
}