// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Piece.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Represents the base of a chess piece i.e. the but that sits on a chess square. A piece can have different tops e.g. A Pawn, Queen, Bishop etc.
//   The top of a piece is changed when a piece is promoted. e.g. A pawn is promoted to a Queen.
//   The piece "base" determines its location. The piece "top" determines it type (Queen, pawn etc) and value.
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

    #endregion

    /// <summary>
    /// Represents the base of a chess piece i.e. the but that sits on a chess square. A piece can have different tops e.g. A Pawn, Queen, Bishop etc. 
    ///   The top of a piece is changed when a piece is promoted. e.g. A pawn is promoted to a Queen. 
    ///   The piece "base" determines its location. The piece "top" determines it type (Queen, pawn etc) and value.
    /// </summary>
    public class Piece : IPieceTop
    {
        #region Constructors and Destructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="Piece"/> class.
        /// </summary>
        public Piece()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Piece"/> class.
        /// </summary>
        /// <param name="name">
        /// The piece name e.g. bishop, queen.
        /// </param>
        /// <param name="player">
        /// The player that owns the piece.
        /// </param>
        /// <param name="file">
        /// Board file that the piece starts on.
        /// </param>
        /// <param name="rank">
        /// Board rank that the piece starts on.
        /// </param>
        /// <param name="identifier">
        /// Piece identifier.
        /// </param>
        public Piece(PieceNames name, Player player, int file, int rank, PieceIdentifierCodes identifier)
        {
            this.LastMoveTurnNo = -1;
            this.IsInPlay = true;
            Square square = Board.GetSquare(file, rank);

            this.Player = player;
            this.StartLocation = this.Square = square;
            square.Piece = this;
            this.IdentifierCode = identifier;

            switch (name)
            {
                case PieceNames.Pawn:
                    this.Top = new PiecePawn(this);
                    break;

                case PieceNames.Bishop:
                    this.Top = new PieceBishop(this);
                    break;

                case PieceNames.Knight:
                    this.Top = new PieceKnight(this);
                    break;

                case PieceNames.Rook:
                    this.Top = new PieceRook(this);
                    break;

                case PieceNames.Queen:
                    this.Top = new PieceQueen(this);
                    break;

                case PieceNames.King:
                    this.Top = new PieceKing(this);
                    break;
            }
        }

        #endregion

        #region Enums

        /// <summary>
        /// The enm id.
        /// </summary>
        public enum PieceIdentifierCodes
        {
            /// <summary>
            ///   The white queens rook.
            /// </summary>
            WhiteQueensRook, 

            /// <summary>
            ///   The white queens knight.
            /// </summary>
            WhiteQueensKnight, 

            /// <summary>
            ///   The white queens bishop.
            /// </summary>
            WhiteQueensBishop, 

            /// <summary>
            ///   The white queen.
            /// </summary>
            WhiteQueen, 

            /// <summary>
            ///   The white king.
            /// </summary>
            WhiteKing, 

            /// <summary>
            ///   The white kings bishop.
            /// </summary>
            WhiteKingsBishop, 

            /// <summary>
            ///   The white kings knight.
            /// </summary>
            WhiteKingsKnight, 

            /// <summary>
            ///   The white kings rook.
            /// </summary>
            WhiteKingsRook, 

            /// <summary>
            ///   The white pawn 1.
            /// </summary>
            WhitePawn1, 

            /// <summary>
            ///   The white pawn 2.
            /// </summary>
            WhitePawn2, 

            /// <summary>
            ///   The white pawn 3.
            /// </summary>
            WhitePawn3, 

            /// <summary>
            ///   The white pawn 4.
            /// </summary>
            WhitePawn4, 

            /// <summary>
            ///   The white pawn 5.
            /// </summary>
            WhitePawn5, 

            /// <summary>
            ///   The white pawn 6.
            /// </summary>
            WhitePawn6, 

            /// <summary>
            ///   The white pawn 7.
            /// </summary>
            WhitePawn7, 

            /// <summary>
            ///   The white pawn 8.
            /// </summary>
            WhitePawn8, 

            /// <summary>
            ///   The black queens rook.
            /// </summary>
            BlackQueensRook, 

            /// <summary>
            ///   The black queens knight.
            /// </summary>
            BlackQueensKnight, 

            /// <summary>
            ///   The black queens bishop.
            /// </summary>
            BlackQueensBishop, 

            /// <summary>
            ///   The black queen.
            /// </summary>
            BlackQueen, 

            /// <summary>
            ///   The black king.
            /// </summary>
            BlackKing, 

            /// <summary>
            ///   The black kings bishop.
            /// </summary>
            BlackKingsBishop, 

            /// <summary>
            ///   The black kings knight.
            /// </summary>
            BlackKingsKnight, 

            /// <summary>
            ///   The black kings rook.
            /// </summary>
            BlackKingsRook, 

            /// <summary>
            ///   The black pawn 1.
            /// </summary>
            BlackPawn1, 

            /// <summary>
            ///   The black pawn 2.
            /// </summary>
            BlackPawn2, 

            /// <summary>
            ///   The black pawn 3.
            /// </summary>
            BlackPawn3, 

            /// <summary>
            ///   The black pawn 4.
            /// </summary>
            BlackPawn4, 

            /// <summary>
            ///   The black pawn 5.
            /// </summary>
            BlackPawn5, 

            /// <summary>
            ///   The black pawn 6.
            /// </summary>
            BlackPawn6, 

            /// <summary>
            ///   The black pawn 7.
            /// </summary>
            BlackPawn7, 

            /// <summary>
            ///   The black pawn 8.
            /// </summary>
            BlackPawn8
        }

        /// <summary>
        /// The enm name.
        /// </summary>
        public enum PieceNames
        {
            /// <summary>
            ///   The pawn.
            /// </summary>
            Pawn, 

            /// <summary>
            ///   The bishop.
            /// </summary>
            Bishop, 

            /// <summary>
            ///   The knight.
            /// </summary>
            Knight, 

            /// <summary>
            ///   The rook.
            /// </summary>
            Rook, 

            /// <summary>
            ///   The queen.
            /// </summary>
            Queen, 

            /// <summary>
            ///   The king.
            /// </summary>
            King
        }

        #endregion
       

        #region Public Properties

        /// <summary>
        ///   Gets the piece's abbreviation.
        /// </summary>
        public string Abbreviation
        {
            get
            {
                return this.Top.Abbreviation;
            }
        }

        /// <summary>
        ///   Gets the base part of the piece. i.e. the bit that sits on the chess square.
        /// </summary>
        public Piece Base
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        ///   Gets basic value of the piece. e.g. pawn = 1, bishop = 3, queen = 9
        /// </summary>
        public int BasicValue
        {
            get
            {
                return this.Top.BasicValue;
            }
        }

        /// <summary>
        ///   Gets a score based upon how well defended this piece is.
        /// </summary>
        public int DefensePoints
        {
            get
            {
                Piece piece = this.Square.CheapestPieceDefendingThisSquare(this.Player);
                if (piece != null)
                {
                    switch (piece.Name)
                    {
                        case PieceNames.Pawn:
                            return 60;

                        case PieceNames.Knight:
                        case PieceNames.Bishop:
                            return 45;

                        case PieceNames.Rook:
                            return 30;

                        case PieceNames.Queen:
                            return 20;

                        case PieceNames.King:
                            return 20;
                    }
                }

                return 0;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this piece has been promoted yet.
        /// </summary>
        public bool HasBeenPromoted { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether this piece has been moved yet.
        /// </summary>
        public bool HasMoved
        {
            get
            {
                return this.NoOfMoves != 0 || !this.IsInPlay;
            }
        }

        /// <summary>
        /// Gets the board position HashCodeA for this piece.
        /// </summary>
        public ulong HashCodeA
        {
            get
            {
                return this.HashCodeAForSquareOrdinal(this.Square.Ordinal);
            }
        }

        /// <summary>
        /// Gets the board position HashCodeB for this piece.
        /// </summary>
        public ulong HashCodeB
        {
            get
            {
                return this.HashCodeBForSquareOrdinal(this.Square.Ordinal);
            }
        }

        /// <summary>
        ///  Gets the image index for this piece. Used to determine which graphic image is displayed for thie piece.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this.Top.ImageIndex;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the piece is capturable. Kings aren't, everything else is.
        /// </summary>
        public bool IsCapturable
        {
            get
            {
                return this.Top.IsCapturable;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is still in-play. A piece leaves play when it has been captured.
        /// </summary>
        public bool IsInPlay { get; private set; }

        /// <summary>
        ///   Gets or sets the turn number in which the piece was last moved.
        /// </summary>
        public int LastMoveTurnNo { get; set; }

        /// <summary>
        /// Gets the piece's name.
        /// </summary>
        public PieceNames Name
        {
            get
            {
                return this.Top.Name;
            }
        }

        /// <summary>
        ///   Gets or sets number of moves the piece has made.
        /// </summary>
        public int NoOfMoves { get; set; }

        /// <summary>
        /// Gets the player who owns the piece.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Gets the positional points assigned to this piece.
        /// </summary>
        public int PositionalPoints
        {
            get
            {
                return this.Top.PositionalPoints;
            }
        }

        /// <summary>
        ///   Gets or sets square that this piece is located on.
        /// </summary>
        public Square Square { get; set; }

        /// <summary>
        ///   Gets start location for this piece.
        /// </summary>
        public Square StartLocation { get; private set; }

        /// <summary>
        ///   Gets Top.
        /// </summary>
        public IPieceTop Top { get; set; }

        /// <summary>
        /// Gets the material value of this piece.
        /// </summary>
        public int Value
        {
            get
            {
                return this.Top.Value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the identifier code for the piece. e.g. WhitePawn1
        /// </summary>
        private PieceIdentifierCodes IdentifierCode { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///  can a given player's piece name attack a given square?
        /// </summary>
        /// <param name="square"></param>
        /// <param name="player"></param>
        /// <param name="PieceName"></param>
        /// <returns></returns>
        public static bool CanPlayerPieceNameAttackSquare(Square square, Player player, Piece.PieceNames PieceName)
        {
            switch (PieceName)
            {
                case PieceNames.Bishop:
                    return PieceBishop.DoesPieceAttackSquare(square, player);
                case PieceNames.King:
                    return PieceKing.DoesPieceAttackSquare(square, player);
                case PieceNames.Knight:
                    return PieceKnight.DoesPieceAttackSquare(square, player);
                case PieceNames.Pawn:
                    return PiecePawn.DoesPieceAttackSquare(square, player);
                case PieceNames.Queen:
                    return PieceQueen.DoesPieceAttackSquare(square, player);
                case PieceNames.Rook:
                    return PieceRook.DoesPieceAttackSquare(square, player);
            }
            return false;
        }

        public static bool CanPlayerPieceNameAttackSquare(Square square, Player player, Piece.PieceNames PieceName, out Piece attackingPiece)
        {
            attackingPiece = null;
            switch (PieceName)
            {
                case PieceNames.Bishop:
                    return PieceBishop.DoesPieceAttackSquare(square, player,out attackingPiece);
                case PieceNames.King:
                    return PieceKing.DoesPieceAttackSquare(square, player, out attackingPiece);
                case PieceNames.Knight:
                    return PieceKnight.DoesPieceAttackSquare(square, player, out attackingPiece);
                case PieceNames.Pawn:
                    return PiecePawn.DoesPieceAttackSquare(square, player, out attackingPiece);
                case PieceNames.Queen:
                    return PieceQueen.DoesPieceAttackSquare(square, player, out attackingPiece);
                case PieceNames.Rook:
                    return PieceRook.DoesPieceAttackSquare(square, player, out attackingPiece);
            }
            return false;
        }

        static public bool DoesLeaperPieceTypeAttackSquare(Square square, Player player, PieceNames pieceName, int[] vector)
        {
            Piece piece;
            for (int i = 0; i < vector.Length; i++)
            {
                piece = Board.GetPiece(square.Ordinal + vector[i]);
                if (piece != null && piece.Name == pieceName && piece.Player.Colour == player.Colour)
                {
                    return true;
                }
            }
            return false;
        }

        static public bool DoesLeaperPieceTypeAttackSquare(Square square, Player player, PieceNames pieceName, int[] vector, out Piece attackingPiece)
        {
            Piece piece;
            attackingPiece = null;
            for (int i = 0; i < vector.Length; i++)
            {
                piece = Board.GetPiece(square.Ordinal + vector[i]);
                if (piece != null && piece.Name == pieceName && piece.Player.Colour == player.Colour)
                {
                    attackingPiece = piece;
                    return true;
                }
            }
            return false;
        }



        public bool CanAttackSquare(Square square)
        {
            return this.Top.CanAttackSquare(square);
        }

        /// <summary>
        /// Indicates whether the piece would be attackable by a nearby enemy pawm, if the enemy pawn were to advance.
        /// </summary>
        /// <returns>
        /// True, if can be driven away.
        /// </returns>
        public bool CanBeDrivenAwayByPawn()
        {
            Piece piece;

            piece =
                Board.GetPiece(this.Square.Ordinal + this.Player.PawnAttackLeftOffset + this.Player.PawnForwardOffset);
            if (piece != null && piece.Player.Colour != this.Player.Colour && piece.Name == PieceNames.Pawn)
            {
                return true;
            }

            piece =
                Board.GetPiece(this.Square.Ordinal + this.Player.PawnAttackRightOffset + this.Player.PawnForwardOffset);
            if (piece != null && piece.Player.Colour != this.Player.Colour && piece.Name == PieceNames.Pawn)
            {
                return true;
            }

            piece =
                Board.GetPiece(
                    this.Square.Ordinal + this.Player.PawnAttackLeftOffset + this.Player.PawnForwardOffset
                    + this.Player.PawnForwardOffset);
            if (piece != null && piece.Player.Colour != this.Player.Colour && piece.Name == PieceNames.Pawn
                && !piece.HasMoved)
            {
                return true;
            }

            piece =
                Board.GetPiece(
                    this.Square.Ordinal + this.Player.PawnAttackRightOffset + this.Player.PawnForwardOffset
                    + this.Player.PawnForwardOffset);
            if (piece != null && piece.Player.Colour != this.Player.Colour && piece.Name == PieceNames.Pawn
                && !piece.HasMoved)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Capture this piece.
        /// </summary>
        public void Capture()
        {
            this.Player.OpposingPlayer.CapturedEnemyPieces.Add(this);
            this.Player.Pieces.Remove(this);
            this.Square.Piece = null;
            this.IsInPlay = false;
            if (this.Name == PieceNames.Pawn)
            {
                this.Player.DecreasePawnCount();
            }
            else
            {
                this.Player.DecreaseMaterialCount();
            }

            return;
        }

        /// <summary>
        /// Demote this piece back to a pawn.
        /// </summary>
        /// <exception cref="ApplicationException">
        /// Indicates failure to demote this piece.
        /// </exception>
        public void Demote()
        {
            if (!this.HasBeenPromoted)
            {
                throw new ApplicationException("Cannot demote a piece that hasn't yet been promoted!");
            }

            this.Top = new PiecePawn(this);
            this.Player.IncreasePawnCount();
            this.Player.DecreaseMaterialCount();
            this.HasBeenPromoted = false;
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
            this.Top.GenerateLazyMoves(moves, movesType);
        }

        /// <summary>
        /// The generate legal moves.
        /// </summary>
        /// <param name="moves">
        /// The moves.
        /// </param>
        public void GenerateLegalMoves(Moves moves)
        {
            this.GenerateLazyMoves(moves, Moves.MoveListNames.All);
            for (int intIndex = moves.Count - 1; intIndex >= 0; intIndex--)
            {
                Move move = moves[intIndex];
                Move moveUndo = move.Piece.Move(move.Name, move.To);
                if (move.Piece.Player.IsInCheck)
                {
                    moves.Remove(move);
                }

                Model.Move.Undo(moveUndo);
            }
        }

        /// <summary>
        /// The hash code A for square ordinal.
        /// </summary>
        /// <param name="ordinal">
        /// The ordinal.
        /// </param>
        /// <returns>
        /// Hash code A.
        /// </returns>
        public ulong HashCodeAForSquareOrdinal(int ordinal)
        {
            ulong ulongPromotionModifier = 0;
            if (this.HasBeenPromoted)
            {
                switch (this.Top.Name)
                {
                    case PieceNames.Queen:
                        ulongPromotionModifier = 830859827498573475;
                        break;
                    case PieceNames.Rook:
                        ulongPromotionModifier = 37500384876452947;
                        break;
                    case PieceNames.Bishop:
                        ulongPromotionModifier = 448573857309865743;
                        break;
                    case PieceNames.Knight:
                        ulongPromotionModifier = 294375032850265937;
                        break;
                }
            }

            return PieceHashCodes.MasterHashCodesA[(((uint)this.IdentifierCode) << 7) + ordinal] + ulongPromotionModifier;
        }

        /// <summary>
        /// The hash code b for square ordinal.
        /// </summary>
        /// <param name="ordinal">
        /// The ordinal.
        /// </param>
        /// <returns>
        /// Hash code B.
        /// </returns>
        public ulong HashCodeBForSquareOrdinal(int ordinal)
        {
            ulong ulongPromotionModifier = 0;
            if (this.HasBeenPromoted)
            {
                switch (this.Top.Name)
                {
                    case PieceNames.Queen:
                        ulongPromotionModifier = 790423450762398573;
                        break;
                    case PieceNames.Rook:
                        ulongPromotionModifier = 394756026094872034;
                        break;
                    case PieceNames.Bishop:
                        ulongPromotionModifier = 629385632983478593;
                        break;
                    case PieceNames.Knight:
                        ulongPromotionModifier = 283469276067858673;
                        break;
                }
            }

            return PieceHashCodes.MasterHashCodesB[(((uint)this.IdentifierCode) << 7) + ordinal] + ulongPromotionModifier;
        }

        /// <summary>
        /// Move the piece to a new square, after testing that the move is valid.
        /// </summary>
        /// <param name="moveName">
        /// The move name.
        /// </param>
        /// <param name="square">
        /// The square.
        /// </param>
        /// <returns>
        /// Move made, or null if move is not valid.
        /// </returns>
        public Move TestAndMakeMove(Move.MoveNames moveName, Square square)
        {
            return null;
        }

        /// <summary>
        /// Move the piece to a new square.
        /// </summary>
        /// <param name="moveName">
        /// The move name.
        /// </param>
        /// <param name="square">
        /// The square.
        /// </param>
        /// <returns>
        /// Move made.
        /// </returns>
        public Move Move(Move.MoveNames moveName, Square square)
        {
            Square squarepieceCaptured = square;

            if (moveName == Model.Move.MoveNames.EnPassent)
            {
                // Override when en passent
                squarepieceCaptured = Board.GetSquare(square.Ordinal - this.Player.PawnForwardOffset);
            }

            Board.HashCodeA ^= this.HashCodeA; // Un-XOR current piece position
            Board.HashCodeB ^= this.HashCodeB; // Un-XOR current piece position
            if (this.Name == PieceNames.Pawn)
            {
                Board.PawnHashCodeA ^= this.HashCodeA;
                Board.PawnHashCodeB ^= this.HashCodeB;
            }

            Move move = new Move(
                Game.TurnNo, 
                this.LastMoveTurnNo, 
                moveName, 
                this, 
                this.Square, 
                square, 
                squarepieceCaptured.Piece, 
                squarepieceCaptured.Piece == null ? -1 : squarepieceCaptured.Piece.Player.Pieces.IndexOf(squarepieceCaptured.Piece), 
                0);

            if (square.Piece != null)
            {
                if (squarepieceCaptured.Piece != null)
                {
                    Board.HashCodeA ^= squarepieceCaptured.Piece.HashCodeA; // un-XOR the piece taken
                    Board.HashCodeB ^= squarepieceCaptured.Piece.HashCodeB; // un-XOR the piece taken
                    if (squarepieceCaptured.Piece.Name == PieceNames.Pawn)
                    {
                        Board.PawnHashCodeA ^= squarepieceCaptured.Piece.HashCodeA;
                        Board.PawnHashCodeB ^= squarepieceCaptured.Piece.HashCodeB;
                    }

                    squarepieceCaptured.Piece.Capture();
                }
            }

            Game.TurnNo++;

            this.Square.Piece = null;
            square.Piece = this;
            this.Square = square;

            this.LastMoveTurnNo = Game.TurnNo;
            this.NoOfMoves++;

            Piece pieceRook;
            switch (moveName)
            {
                case Model.Move.MoveNames.CastleKingSide:
                    pieceRook = move.Piece.Player.Colour == Player.PlayerColourNames.White
                                    ? Board.GetPiece(7, 0)
                                    : Board.GetPiece(7, 7);
                    Board.HashCodeA ^= pieceRook.HashCodeA;
                    Board.HashCodeB ^= pieceRook.HashCodeB;
                    pieceRook.Square.Piece = null;
                    pieceRook.LastMoveTurnNo = Game.TurnNo;
                    pieceRook.NoOfMoves++;
                    Board.GetSquare(5, square.Rank).Piece = pieceRook;
                    pieceRook.Square = Board.GetSquare(5, square.Rank);
                    Board.HashCodeA ^= pieceRook.HashCodeA;
                    Board.HashCodeB ^= pieceRook.HashCodeB;
                    this.Player.HasCastled = true;
                    break;

                case Model.Move.MoveNames.CastleQueenSide:
                    pieceRook = move.Piece.Player.Colour == Player.PlayerColourNames.White
                                    ? Board.GetPiece(0, 0)
                                    : Board.GetPiece(0, 7);
                    Board.HashCodeA ^= pieceRook.HashCodeA;
                    Board.HashCodeB ^= pieceRook.HashCodeB;
                    pieceRook.Square.Piece = null;
                    pieceRook.LastMoveTurnNo = Game.TurnNo;
                    pieceRook.NoOfMoves++;
                    Board.GetSquare(3, square.Rank).Piece = pieceRook;
                    pieceRook.Square = Board.GetSquare(3, square.Rank);
                    Board.HashCodeA ^= pieceRook.HashCodeA;
                    Board.HashCodeB ^= pieceRook.HashCodeB;
                    this.Player.HasCastled = true;
                    break;

                case Model.Move.MoveNames.PawnPromotionQueen:
                    this.Promote(PieceNames.Queen);
                    break;

                case Model.Move.MoveNames.PawnPromotionRook:
                    this.Promote(PieceNames.Rook);
                    break;

                case Model.Move.MoveNames.PawnPromotionBishop:
                    this.Promote(PieceNames.Bishop);
                    break;

                case Model.Move.MoveNames.PawnPromotionKnight:
                    this.Promote(PieceNames.Knight);
                    break;

                case Model.Move.MoveNames.EnPassent:
                    Board.HashCodeA ^= Board.GetPiece(this.Square.Ordinal - this.Player.PawnForwardOffset).HashCodeA;
                    Board.HashCodeB ^= Board.GetPiece(this.Square.Ordinal - this.Player.PawnForwardOffset).HashCodeB;
                    Board.PawnHashCodeA ^= Board.GetPiece(this.Square.Ordinal - this.Player.PawnForwardOffset).HashCodeA;
                    Board.PawnHashCodeB ^= Board.GetPiece(this.Square.Ordinal - this.Player.PawnForwardOffset).HashCodeB;
                    Board.GetPiece(this.Square.Ordinal - this.Player.PawnForwardOffset).Capture();
                        
                        // Take enemy pawn that is now behind us
                    break;
            }

            Board.HashCodeA ^= this.HashCodeA; // XOR piece into new piece position
            Board.HashCodeB ^= this.HashCodeB; // XOR piece into new piece position
            if (this.Name == PieceNames.Pawn)
            {
                Board.PawnHashCodeA ^= this.HashCodeA;
                Board.PawnHashCodeB ^= this.HashCodeB;
            }

            move.IsInCheck = move.Piece.Player.IsInCheck;
            move.IsEnemyInCheck = move.Piece.Player.OpposingPlayer.IsInCheck;

            move.HashCodeA = Board.HashCodeA;
            move.HashCodeB = Board.HashCodeB;

            Game.MoveHistory.Add(move);

            if (move.Piece.Player.CanClaimThreeMoveRepetitionDraw)
            {
                Board.HashCodeA ^= 31;
                Board.HashCodeB ^= 29;
                move.HashCodeA = Board.HashCodeA;
                move.HashCodeB = Board.HashCodeB;
                move.IsThreeMoveRepetition = true;
            }

            return move;
        }

        /// <summary>
        /// Promote a pawn (change its top to a queen or knight).
        /// </summary>
        /// <param name="name">
        /// Name of piece to change the pawn into.
        /// </param>
        /// <exception cref="ApplicationException">
        /// Indicates failure to promote the piece.
        /// </exception>
        public void Promote(PieceNames name)
        {
            if (this.HasBeenPromoted)
            {
                throw new ApplicationException("Piece has already been promoted!");
            }

            if (this.Name != PieceNames.Pawn)
            {
                throw new ApplicationException("Attempt to promote piece that is not a pawn");
            }

            switch (name)
            {
                case PieceNames.Bishop:
                    this.Top = new PieceBishop(this);
                    break;

                case PieceNames.Knight:
                    this.Top = new PieceKnight(this);
                    break;

                case PieceNames.Rook:
                    this.Top = new PieceRook(this);
                    break;

                case PieceNames.Queen:
                    this.Top = new PieceQueen(this);
                    break;

                default:
                    throw new ApplicationException("Can only promote pawn to either Bishop, Knight, Rook or Queen");
            }

            this.Player.DecreasePawnCount();
            this.Player.IncreaseMaterialCount();
            this.HasBeenPromoted = true;
        }

        /// <summary>
        /// Calculates a penalty positional score based upon the distance to the enemy king - allowing only horizonal or vertical movement (like a rook).
        /// </summary>
        /// <returns>
        /// The taxi cab distance to enemy king penalty.
        /// </returns>
        public int TaxiCabDistanceToEnemyKingPenalty()
        {
            return Math.Abs(this.Square.Rank - this.Player.OpposingPlayer.King.Square.Rank)
                   + Math.Abs(this.Square.File - this.Player.OpposingPlayer.King.Square.File);
        }

        /// <summary>
        /// Undo the capture of a piece. Placing it back on the correct board square is handled in the Move.Undo method.
        /// </summary>
        /// <param name="ordinal">
        /// The ordinal.
        /// </param>
        public void Uncapture(int ordinal)
        {
            this.Player.Pieces.Insert(ordinal, this);
            this.Player.OpposingPlayer.CapturedEnemyPieces.Remove(this);
            this.IsInPlay = true;
            if (this.Name == PieceNames.Pawn)
            {
                this.Player.IncreasePawnCount();
            }
            else
            {
                this.Player.IncreaseMaterialCount();
            }
        }

        #endregion
    }
}