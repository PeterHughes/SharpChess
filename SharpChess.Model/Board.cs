// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Board.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Represents the chess board using a 0x88 represenation.
//   http://chessprogramming.wikispaces.com/0x88
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
    /// Represents the chess board using a 0x88 represenation.
    /// http://chessprogramming.wikispaces.com/0x88
    /// </summary>
    public static class Board
    {
        #region Constants and Fields

        /// <summary>
        ///   Number of files on the chess board.
        /// </summary>
        public const byte FileCount = 8;

        /// <summary>
        ///   Width of board matrix.
        /// </summary>
        public const byte MatrixWidth = 16;

        /// <summary>
        ///   Number of ranks on the chess board.
        /// </summary>
        public const byte RankCount = 8;

        /// <summary>
        ///   Number of square in the board matrix.
        /// </summary>
        public const byte SquareCount = 128;

        /// <summary>
        ///   List of squares on the board.
        /// </summary>
        private static readonly Square[] Squares = new Square[RankCount * MatrixWidth];

        /// <summary>
        ///   Orientation of the board. Black or White at the bottom.
        /// </summary>
        private static OrientationNames orientation = OrientationNames.White;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "Board" /> class.
        /// </summary>
        static Board()
        {
            for (int intOrdinal = 0; intOrdinal < SquareCount; intOrdinal++)
            {
                Squares[intOrdinal] = new Square(intOrdinal);
            }
        }

        #endregion

        #region Enums

        /// <summary>
        /// Valid values for orientation of the board. Black or White at the bottom.
        /// </summary>
        public enum OrientationNames
        {
            /// <summary>
            ///   White at the bottom.
            /// </summary>
            White, 

            /// <summary>
            ///   Black at the bottom.
            /// </summary>
            Black
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets DebugString.
        /// </summary>
        // ReSharper disable UnusedMember.Global
        public static string DebugString
        {
            // ReSharper restore UnusedMember.Global
            get
            {
                string strOutput = string.Empty;
                int intOrdinal = SquareCount - 1;

                for (int intRank = 0; intRank < RankCount; intRank++)
                {
                    for (int intFile = 0; intFile < FileCount; intFile++)
                    {
                        Square square = GetSquare(intOrdinal);
                        if (square != null)
                        {
                            Piece piece;
                            if ((piece = square.Piece) != null)
                            {
                                strOutput += piece.Abbreviation;
                            }
                            else
                            {
                                strOutput += square.Colour == Square.ColourNames.White ? "." : "#";
                            }
                        }

                        strOutput += Convert.ToChar(13) + Convert.ToChar(10);

                        intOrdinal--;
                    }
                }

                return strOutput;
            }
        }

        /// <summary>
        ///   Gets or sets the hash code a.
        /// </summary>
        public static ulong HashCodeA { get; set; }

        /// <summary>
        ///   Gets or sets the hash code b.
        /// </summary>
        public static ulong HashCodeB { get; set; }

        /// <summary>
        ///   Gets or sets Orientation.
        /// </summary>
        public static OrientationNames Orientation
        {
            get
            {
                return orientation;
            }

            set
            {
                orientation = value;
            }
        }

        /// <summary>
        ///   Gets or sets the pawn hash code a.
        /// </summary>
        public static ulong PawnHashCodeA { get; set; }

        /// <summary>
        ///   Gets or sets the pawn hash code b.
        /// </summary>
        public static ulong PawnHashCodeB { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The append piece path.
        /// </summary>
        /// <param name="moves">
        /// The moves.
        /// </param>
        /// <param name="piece">
        /// The piece.
        /// </param>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="offset">
        /// The offset.
        /// </param>
        /// <param name="movesType">
        /// The moves type.
        /// </param>
        public static void AppendPiecePath(
            Moves moves, Piece piece, Player player, int offset, Moves.MoveListNames movesType)
        {
            int intOrdinal = piece.Square.Ordinal;
            Square square;

            intOrdinal += offset;
            while ((square = GetSquare(intOrdinal)) != null)
            {
                if (square.Piece == null)
                {
                    if (movesType == Moves.MoveListNames.All)
                    {
                        moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, square, null, 0, 0);
                    }
                }
                else if (square.Piece.Player.Colour != player.Colour && square.Piece.IsCapturable)
                {
                    moves.Add(0, 0, Move.MoveNames.Standard, piece, piece.Square, square, square.Piece, 0, 0);
                    break;
                }
                else
                {
                    break;
                }

                intOrdinal += offset;
            }
        }

        /// <summary>
        /// The establish hash key.
        /// </summary>
        public static void EstablishHashKey()
        {
            HashCodeA = 0UL;
            HashCodeB = 0UL;
            PawnHashCodeA = 0UL;
            PawnHashCodeB = 0UL;
            for (int intOrdinal = 0; intOrdinal < SquareCount; intOrdinal++)
            {
                Piece piece = GetPiece(intOrdinal);
                if (piece != null)
                {
                    HashCodeA ^= piece.HashCodeAForSquareOrdinal(intOrdinal);
                    HashCodeB ^= piece.HashCodeBForSquareOrdinal(intOrdinal);
                    if (piece.Name == Piece.PieceNames.Pawn)
                    {
                        PawnHashCodeA ^= piece.HashCodeAForSquareOrdinal(intOrdinal);
                        PawnHashCodeB ^= piece.HashCodeBForSquareOrdinal(intOrdinal);
                    }
                }
            }
        }

        /// <summary>
        /// Board File number from file name.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The file number.
        /// </returns>
        public static int FileFromName(string fileName)
        {
            switch (fileName)
            {
                case "a":
                    return 0;
                case "b":
                    return 1;
                case "c":
                    return 2;
                case "d":
                    return 3;
                case "e":
                    return 4;
                case "f":
                    return 5;
                case "g":
                    return 6;
                case "h":
                    return 7;
            }

            return -1;
        }

        /// <summary>
        /// The flip.
        /// </summary>
        public static void Flip()
        {
            orientation = Orientation == OrientationNames.White ? OrientationNames.Black : OrientationNames.White;
        }

        /// <summary>
        /// The get piece.
        /// </summary>
        /// <param name="ordinal">
        /// The ordinal.
        /// </param>
        /// <returns>
        /// The piece
        /// </returns>
        public static Piece GetPiece(int ordinal)
        {
            return (ordinal & 0x88) == 0 ? Squares[ordinal].Piece : null;
        }

        /// <summary>
        /// The get piece.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="rank">
        /// The rank.
        /// </param>
        /// <returns>
        /// The piece
        /// </returns>
        public static Piece GetPiece(int file, int rank)
        {
            return (OrdinalFromFileRank(file, rank) & 0x88) == 0 ? Squares[OrdinalFromFileRank(file, rank)].Piece : null;
        }

        /// <summary>
        /// The get square.
        /// </summary>
        /// <param name="ordinal">
        /// The ordinal.
        /// </param>
        /// <returns>
        /// The square
        /// </returns>
        public static Square GetSquare(int ordinal)
        {
            return (ordinal & 0x88) == 0 ? Squares[ordinal] : null;
        }

        /// <summary>
        /// The get square.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="rank">
        /// The rank.
        /// </param>
        /// <returns>
        /// The square
        /// </returns>
        public static Square GetSquare(int file, int rank)
        {
            return (OrdinalFromFileRank(file, rank) & 0x88) == 0 ? Squares[OrdinalFromFileRank(file, rank)] : null;
        }

        /// <summary>
        /// The get square.
        /// </summary>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <returns>
        /// Matching Square
        /// </returns>
        public static Square GetSquare(string label)
        {
            return
                Squares[OrdinalFromFileRank(FileFromName(label.Substring(0, 1)), int.Parse(label.Substring(1, 1)) - 1)];
        }

        /// <summary>
        /// The line threatened by.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="squares">
        /// The squares.
        /// </param>
        /// <param name="squareStart">
        /// The square start.
        /// </param>
        /// <param name="offset">
        /// The offset.
        /// </param>
        public static void LineThreatenedBy(Player player, Squares squares, Square squareStart, int offset)
        {
            int intOrdinal = squareStart.Ordinal;
            Square square;

            intOrdinal += offset;
            while ((square = GetSquare(intOrdinal)) != null)
            {
                if (square.Piece == null)
                {
                    squares.Add(square);
                }
                else if (square.Piece.Player.Colour != player.Colour && square.Piece.IsCapturable)
                {
                    squares.Add(square);
                    break;
                }
                else
                {
                    break;
                }

                intOrdinal += offset;
            }
        }

        /// <summary>
        /// Returns the first piece found in a vector from the specified Square.
        /// </summary>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <param name="pieceName">
        /// The piece name.
        /// </param>
        /// <param name="squareStart">
        /// The square start.
        /// </param>
        /// <param name="vectorOffset">
        /// The vector offset.
        /// </param>
        /// <returns>
        /// The first piece on the line, or null.
        /// </returns>
        public static Piece LinesFirstPiece(
            Player.PlayerColourNames colour, Piece.PieceNames pieceName, Square squareStart, int vectorOffset)
        {
            int intOrdinal = squareStart.Ordinal;
            Square square;

            intOrdinal += vectorOffset;
            while ((square = GetSquare(intOrdinal)) != null)
            {
                if (square.Piece == null)
                {
                }
                else if (square.Piece.Player.Colour != colour)
                {
                    return null;
                }
                else if (square.Piece.Name == pieceName)
                {
                    return square.Piece;
                }
                else
                {
                    return null;
                }

                intOrdinal += vectorOffset;
            }

            return null;
        }

        /// <summary>
        /// Calculates a positional penalty score for a single open line to a square (usually the king square), in a specified direction.
        /// </summary>
        /// <param name="colour">
        /// The player's colour.
        /// </param>
        /// <param name="squareStart">
        /// The square piece (king) is on.
        /// </param>
        /// <param name="directionOffset">
        /// The direction offset.
        /// </param>
        /// <returns>
        /// The open line penalty.
        /// </returns>
        public static int OpenLinePenalty(Player.PlayerColourNames colour, Square squareStart, int directionOffset)
        {
            int intOrdinal = squareStart.Ordinal;
            int intSquareCount = 0;
            int intPenalty = 0;
            Square square;

            intOrdinal += directionOffset;

            while (intSquareCount <= 2
                   &&
                   ((square = GetSquare(intOrdinal)) != null
                    &&
                    (square.Piece == null
                     || (square.Piece.Name != Piece.PieceNames.Pawn && square.Piece.Name != Piece.PieceNames.Rook)
                     || square.Piece.Player.Colour != colour)))
            {
                intPenalty += 75;
                intSquareCount++;
                intOrdinal += directionOffset;
            }

            return intPenalty;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get ordinal number of a given square, specified by file and rank.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="rank">
        /// The rank.
        /// </param>
        /// <returns>
        /// Ordinal value from file and rank.
        /// </returns>
        private static int OrdinalFromFileRank(int file, int rank)
        {
            return (rank << 4) | file;
        }

        #endregion
    }
}