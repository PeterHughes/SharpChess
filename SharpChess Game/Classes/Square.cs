// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Square.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The square.
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
    /// The square.
    /// </summary>
    public class Square
    {
        #region Constants and Fields

        /// <summary>
        /// The m_aint square values.
        /// </summary>
        private static readonly int[] m_aintSquareValues =
        {
            1, 1, 1, 1, 1, 1, 1, 1,    0,0,0,0,0,0,0,0,
            1,10,10,10,10,10,10, 1,    0,0,0,0,0,0,0,0,
            1,10,25,25,25,25,10, 1,    0,0,0,0,0,0,0,0,
            1,10,25,50,50,25,10, 1,    0,0,0,0,0,0,0,0,
            1,10,25,50,50,25,10, 1,    0,0,0,0,0,0,0,0,
            1,10,25,25,25,25,10, 1,    0,0,0,0,0,0,0,0,
            1,10,10,10,10,10,10, 1 ,   0,0,0,0,0,0,0,0,
            1, 1, 1, 1, 1, 1, 1, 1 ,   0,0,0,0,0,0,0,0
        };

        /// <summary>
        /// The m_colour.
        /// </summary>
        private readonly enmColour m_colour = enmColour.White;

        /// <summary>
        /// The m_int file.
        /// </summary>
        private readonly int m_intFile;

        /// <summary>
        /// The m_int ordinal.
        /// </summary>
        private readonly int m_intOrdinal;

        /// <summary>
        /// The m_int rank.
        /// </summary>
        private readonly int m_intRank;

        /// <summary>
        /// The m_aint king attackers.
        /// </summary>
        private static char[] m_aintKingAttackers = 
        {
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','K',
            'K','K','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','K',
            '.',
            'K','.','.','.','.','.','.','.',   '.','.','.','.','.','.','K','K',
            'K','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.',   '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','.',
            '.','.','.','.','.','.','.','.' ,  '.','.','.','.','.','.','.','.'
        };

        /// <summary>
        /// The m_aint minor attackers.
        /// </summary>
        private static char[] m_aintMinorAttackers = 
        {
            '.','.','.','.','.','.','.','.',   'B','B','.','.','.','.','.','.',
            'R','.','.','.','.','.','.','B',   '.','.','B','.','.','.','.','.',
            'R','.','.','.','.','.','B','.',   '.','.','.','B','.','.','.','.',
            'R','.','.','.','.','B','.','.',   '.','.','.','.','B','.','.','.',
            'R','.','.','.','B','.','.','.',   '.','.','.','.','.','B','.','.',
            'R','.','.','B','.','.','.','.',   '.','.','.','.','.','.','B','N',
            'R','N','B','.','.','.','.','.' ,  '.','.','.','.','.','.','N','B',
            'R','B','N','.','.','.','.','.' ,  '.','R','R','R','R','R','R','R',
            '.',
            'R','R','R','R','R','R','R','.',   '.','.','.','.','.','N','B','R',
            'B','N','.','.','.','.','.','.',   '.','.','.','.','.','B','N','R',
            'N','B','.','.','.','.','.','.',   '.','.','.','.','B','.','.','R',
            '.','.','B','.','.','.','.','.',   '.','.','.','B','.','.','.','R',
            '.','.','.','B','.','.','.','.',   '.','.','B','.','.','.','.','R',
            '.','.','.','.','B','.','.','.',   '.','B','.','.','.','.','.','R',
            '.','.','.','.','.','B','.','.' ,  'B','.','.','.','.','.','.','R',
            '.','.','.','.','.','.','B','B' ,  '.','.','.','.','.','.','.','.'
        };

        /// <summary>
        /// The m_aint queen attackers.
        /// </summary>
        private static char[] m_aintQueenAttackers =
        {
            '.','.','.','.','.','.','.','.',   'Q','Q','.','.','.','.','.','.',
            'Q','.','.','.','.','.','.','Q',   '.','.','Q','.','.','.','.','.',
            'Q','.','.','.','.','.','Q','.',   '.','.','.','Q','.','.','.','.',
            'Q','.','.','.','.','Q','.','.',   '.','.','.','.','Q','.','.','.',
            'Q','.','.','.','Q','.','.','.',   '.','.','.','.','.','Q','.','.',
            'Q','.','.','Q','.','.','.','.',   '.','.','.','.','.','.','Q','.',
            'Q','.','Q','.','.','.','.','.' ,  '.','.','.','.','.','.','.','Q',
            'Q','Q','.','.','.','.','.','.' ,  '.','Q','Q','Q','Q','Q','Q','Q',
            '.',
            'Q','Q','Q','Q','Q','Q','Q','.',   '.','.','.','.','.','.','Q','Q',
            'Q','.','.','.','.','.','.','.',   '.','.','.','.','.','Q','.','Q',
            '.','Q','.','.','.','.','.','.',   '.','.','.','.','Q','.','.','Q',
            '.','.','Q','.','.','.','.','.',   '.','.','.','Q','.','.','.','Q',
            '.','.','.','Q','.','.','.','.',   '.','.','Q','.','.','.','.','Q',
            '.','.','.','.','Q','.','.','.',   '.','Q','.','.','.','.','.','Q',
            '.','.','.','.','.','Q','.','.' ,  'Q','.','.','.','.','.','.','Q',
            '.','.','.','.','.','.','Q','Q' ,  '.','.','.','.','.','.','.','.'
        };


        /// <summary>
        /// The m_aint vectors.
        /// </summary>
        private static int[] m_aintVectors = 
        {
            0,  0,  0,  0,  0,  0,  0,  0,   -15,-17,  0,  0,  0,  0,  0,  0,
            -16,  0,  0,  0,  0,  0,  0,-15,     0,  0,-17,  0,  0,  0,  0,  0,
            -16,  0,  0,  0,  0,  0,-15,  0,     0,  0,  0,-17,  0,  0,  0,  0,
            -16,  0,  0,  0,  0,-15,  0,  0,     0,  0,  0,  0,-17,  0,  0,  0,
            -16,  0,  0,  0,-15,  0,  0,  0,     0,  0,  0,  0,  0,-17,  0,  0,
            -16,  0,  0,-15,  0,  0,  0,  0,     0,  0,  0,  0,  0,  0,-17,100,
            -16,100,-15,  0,  0,  0,  0,  0 ,    0,  0,  0,  0,  0,  0,100,-17,
            -16,-15,100,  0,  0,  0,  0,  0 ,    0, -1, -1, -1, -1, -1, -1, -1,
            0,
            1,  1,  1,  1,  1,  1,  1,  0,     0,  0,  0,  0,  0,100, 15, 16,
            17,100,  0,  0,  0,  0,  0,  0,     0,  0,  0,  0,  0, 15,100, 16,
            100, 17,  0,  0,  0,  0,  0,  0,     0,  0,  0,  0, 15,  0,  0, 16,
            0,  0, 17,  0,  0,  0,  0,  0,     0,  0,  0, 15,  0,  0,  0, 16,
            0,  0,  0, 17,  0,  0,  0,  0,     0,  0, 15,  0,  0,  0,  0, 16,
            0,  0,  0,  0, 17,  0,  0,  0,     0, 15,  0,  0,  0,  0,  0, 16,
            0,  0,  0,  0,  0, 17,  0,  0 ,   15,  0,  0,  0,  0,  0,  0, 16,
            0,  0,  0,  0,  0,  0, 17, 15 ,    0,  0,  0,  0,  0,  0,  0,  0
        };
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Square"/> class.
        /// </summary>
        /// <param name="Ordinal">
        /// The ordinal.
        /// </param>
        public Square(int Ordinal)
        {
            this.m_intOrdinal = Ordinal;
            this.m_intFile = Ordinal % Board.MatrixWidth;
            this.m_intRank = Ordinal / Board.MatrixWidth;

            if (this.m_intFile == 0 || this.m_intFile == 2 || this.m_intFile == 4 || this.m_intFile == 6)
            {
                if (this.m_intRank == 0 || this.m_intRank == 2 || this.m_intRank == 4 || this.m_intRank == 6)
                {
                    this.m_colour = enmColour.Black;
                }
                else
                {
                    this.m_colour = enmColour.White;
                }
            }
            else
            {
                if (this.m_intRank == 0 || this.m_intRank == 2 || this.m_intRank == 4 || this.m_intRank == 6)
                {
                    this.m_colour = enmColour.White;
                }
                else
                {
                    this.m_colour = enmColour.Black;
                }
            }
        }

        #endregion

        #region Enums

        /// <summary>
        /// The enm colour.
        /// </summary>
        public enum enmColour
        {
            /// <summary>
            /// The white.
            /// </summary>
            White, 

            /// <summary>
            /// The black.
            /// </summary>
            Black
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Colour.
        /// </summary>
        public enmColour Colour
        {
            get
            {
                return this.m_colour;
            }
        }

        /// <summary>
        /// Gets File.
        /// </summary>
        public int File
        {
            get
            {
                return this.m_intFile;
            }
        }

        /// <summary>
        /// Gets FileName.
        /// </summary>
        public string FileName
        {
            get
            {
                string[] FileNames = { "a", "b", "c", "d", "e", "f", "g", "h" };
                return FileNames[this.m_intFile];
            }
        }

        /// <summary>
        /// Gets HashCodeA.
        /// </summary>
        public ulong HashCodeA
        {
            get
            {
                return this.Piece == null ? 0UL : this.Piece.HashCodeAForSquareOrdinal(this.Ordinal);
            }
        }

        /// <summary>
        /// Gets HashCodeB.
        /// </summary>
        public ulong HashCodeB
        {
            get
            {
                return this.Piece == null ? 0UL : this.Piece.HashCodeBForSquareOrdinal(this.Ordinal);
            }
        }

        /// <summary>
        /// Gets Name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.FileName + this.RankName;
            }
        }

        /// <summary>
        /// Gets Ordinal.
        /// </summary>
        public int Ordinal
        {
            get
            {
                return this.m_intOrdinal;
            }
        }

        /// <summary>
        /// Gets or sets Piece.
        /// </summary>
        public Piece Piece { get; set; }

        /// <summary>
        /// Gets Rank.
        /// </summary>
        public int Rank
        {
            get
            {
                return this.m_intRank;
            }
        }

        /// <summary>
        /// Gets RankName.
        /// </summary>
        public string RankName
        {
            get
            {
                return (this.m_intRank + 1).ToString();
            }
        }

        /// <summary>
        /// Gets Value.
        /// </summary>
        public int Value
        {
            get
            {
                return m_aintSquareValues[this.Ordinal];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Appends a list of moves of all the pieces that are attacking this square.
        /// </summary>
        /// <param name="moves">
        /// Moves of pieces that are attacking this square.
        /// </param>
        /// <param name="player">
        /// player whose turn it is
        /// </param>
        public void AttackersMoveList(Moves moves, Player player)
        {
            Piece piece;

            // Pawn
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackLeftOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, Board.GetPiece(m_intOrdinal - player.PawnAttackLeftOffset), Board.GetSquare(m_intOrdinal - player.PawnAttackLeftOffset), this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackRightOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, Board.GetPiece(m_intOrdinal - player.PawnAttackRightOffset), Board.GetSquare(m_intOrdinal - player.PawnAttackRightOffset), this, this.Piece, 0, 0);

            // Knight
            piece = Board.GetPiece(m_intOrdinal + 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal + 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal - 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal - 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal - 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal - 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal + 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal + 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);

            // Bishop & Queen
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 15)) != null) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 17)) != null) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -15)) != null) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -17)) != null) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);

            // Rook & Queen
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 1)) != null) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -1)) != null) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 16)) != null) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -16)) != null) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);

            // King!
            piece = Board.GetPiece(m_intOrdinal + 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal + 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal + 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal - 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal - 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal - 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal - 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
            piece = Board.GetPiece(m_intOrdinal + 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, this, this.Piece, 0, 0);
        }

        /// <summary>
        /// The attackers piece list.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <returns>
        /// </returns>
        public Pieces AttackersPieceList(Player player)
        {
            Piece piece;
            Pieces pieces = new Pieces();

            // Pawn
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackLeftOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackRightOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) pieces.Add(piece);

            // Knight
            piece = Board.GetPiece(m_intOrdinal + 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal + 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal - 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal - 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal - 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal - 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal + 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal + 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) pieces.Add(piece);

            // Bishop & Queen
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 15)) != null) pieces.Add(piece);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 17)) != null) pieces.Add(piece);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -15)) != null) pieces.Add(piece);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -17)) != null) pieces.Add(piece);

            // Rook & Queen
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 1)) != null) pieces.Add(piece);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -1)) != null) pieces.Add(piece);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 16)) != null) pieces.Add(piece);
            if ((piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -16)) != null) pieces.Add(piece);

            // King!
            piece = Board.GetPiece(m_intOrdinal + 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal + 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal + 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal - 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal - 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal - 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal - 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) pieces.Add(piece);
            piece = Board.GetPiece(m_intOrdinal + 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) pieces.Add(piece);

            return pieces;
        }

        /// <summary>
        /// The can be moved to by.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <returns>
        /// The can be moved to by.
        /// </returns>
        public bool CanBeMovedToBy(Player player)
        {
            Piece piece;

            // Pawn
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackLeftOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackRightOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) return true;

            // Knight
            piece = Board.GetPiece(m_intOrdinal + 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal + 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal - 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal - 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal - 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal - 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal + 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal + 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return true;

            // Bishop & Queen
            if (Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 15) != null) return true;
            if (Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 17) != null) return true;
            if (Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -15) != null) return true;
            if (Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -17) != null) return true;

            // Rook & Queen
            if (Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 1) != null) return true;
            if (Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -1) != null) return true;
            if (Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 16) != null) return true;
            if (Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -16) != null) return true;

            // King!
            piece = Board.GetPiece(m_intOrdinal + 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal + 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal + 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal - 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal - 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal - 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal - 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return true;
            piece = Board.GetPiece(m_intOrdinal + 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return true;

            return false;
        }

        /// <summary>
        /// The can slide to here from.
        /// </summary>
        /// <param name="squareStart">
        /// The square start.
        /// </param>
        /// <param name="Offset">
        /// The offset.
        /// </param>
        /// <returns>
        /// The can slide to here from.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// An exception indicting that the alogrithm has hit the edge of the board.
        /// </exception>
        public bool CanSlideToHereFrom(Square squareStart, int Offset)
        {
            int intOrdinal = squareStart.Ordinal;
            Square square;

            intOrdinal += Offset;
            while ((square = Board.GetSquare(intOrdinal)) != null)
            {
                if (square == this)
                {
                    return true;
                }

                if (square.Piece != null)
                {
                    return false;
                }

                intOrdinal += Offset;
            }

            throw new ApplicationException("CanSlideToHereFrom: Hit edge of board!");
        }

        /// <summary>
        /// The defence points for.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <returns>
        /// The defence points for.
        /// </returns>
        public int DefencePointsFor(Player player)
        {
            Piece piece;
            int Value = 0;
            int BestValue = 0;

            // Pawn
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackLeftOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackRightOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) return piece.Value;

            // Knight
            piece = Board.GetPiece(m_intOrdinal + 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal + 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal - 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal - 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal - 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal - 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal + 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal + 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece.Value;

            // Bishop & Queen
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 15); Value = piece != null ? piece.Value : 0; if (Value > 0 && Value < 9000) return Value; if (Value > 0) BestValue = Value;
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 17); Value = piece != null ? piece.Value : 0; if (Value > 0 && Value < 9000) return Value; if (Value > 0) BestValue = Value;
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -15); Value = piece != null ? piece.Value : 0; if (Value > 0 && Value < 9000) return Value; if (Value > 0) BestValue = Value;
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -17); Value = piece != null ? piece.Value : 0; if (Value > 0 && Value < 9000) return Value; if (Value > 0) BestValue = Value;

            // Rook & Queen
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 1); Value = piece != null ? piece.Value : 0; if (Value > 0 && Value < 9000) return Value; if (Value > 0) BestValue = Value;
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -1); Value = piece != null ? piece.Value : 0; if (Value > 0 && Value < 9000) return Value; if (Value > 0) BestValue = Value;
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 16); Value = piece != null ? piece.Value : 0; if (Value > 0 && Value < 9000) return Value; if (Value > 0) BestValue = Value;
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -16); Value = piece != null ? piece.Value : 0; if (Value > 0 && Value < 9000) return Value; if (Value > 0) BestValue = Value;

            if (BestValue > 0) return BestValue; // This means a queen was found, but not a Bishop or Rook

            // King!
            piece = Board.GetPiece(m_intOrdinal + 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal + 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal + 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal - 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal - 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal - 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal - 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece.Value;
            piece = Board.GetPiece(m_intOrdinal + 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece.Value;

            return 15000;
        }

        /// <summary>
        /// The defended by.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <returns>
        /// </returns>
        public Piece DefendedBy(Player player)
        {
            Piece piece;
            Piece pieceBest = null;

            // Pawn
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackLeftOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal - player.PawnAttackRightOffset); if (piece != null && piece.Name == Piece.PieceNames.Pawn && piece.Player.Colour == player.Colour) return piece;

            // Knight
            piece = Board.GetPiece(m_intOrdinal + 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal + 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal - 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal - 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal - 33); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal - 18); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal + 14); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal + 31); if (piece != null && piece.Name == Piece.PieceNames.Knight && piece.Player.Colour == player.Colour) return piece;

            // Bishop & Queen
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 15); if (piece != null) { switch (piece.Name) { case Piece.PieceNames.Bishop: return piece; case Piece.PieceNames.Queen: pieceBest = piece; break; } }
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, 17); if (piece != null) { switch (piece.Name) { case Piece.PieceNames.Bishop: return piece; case Piece.PieceNames.Queen: pieceBest = piece; break; } }
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -15); if (piece != null) { switch (piece.Name) { case Piece.PieceNames.Bishop: return piece; case Piece.PieceNames.Queen: pieceBest = piece; break; } }
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Bishop, this, -17); if (piece != null) { switch (piece.Name) { case Piece.PieceNames.Bishop: return piece; case Piece.PieceNames.Queen: pieceBest = piece; break; } }

            // Rook & Queen
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 1); if (piece != null) { switch (piece.Name) { case Piece.PieceNames.Rook: return piece; case Piece.PieceNames.Queen: pieceBest = piece; break; } }
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -1); if (piece != null) { switch (piece.Name) { case Piece.PieceNames.Rook: return piece; case Piece.PieceNames.Queen: pieceBest = piece; break; } }
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, 16); if (piece != null) { switch (piece.Name) { case Piece.PieceNames.Rook: return piece; case Piece.PieceNames.Queen: pieceBest = piece; break; } }
            piece = Board.LinesFirstPiece(player.Colour, Piece.PieceNames.Rook, this, -16); if (piece != null) { switch (piece.Name) { case Piece.PieceNames.Rook: return piece; case Piece.PieceNames.Queen: pieceBest = piece; break; } }

            if (pieceBest != null) return pieceBest; // This means a queen was found, but not a Bishop or Rook

            // King!
            piece = Board.GetPiece(m_intOrdinal + 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal + 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal + 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal - 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal - 16); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal - 17); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal - 1); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece;
            piece = Board.GetPiece(m_intOrdinal + 15); if (piece != null && piece.Name == Piece.PieceNames.King && piece.Player.Colour == player.Colour) return piece;

            return null;
        }

        #endregion
    }
}