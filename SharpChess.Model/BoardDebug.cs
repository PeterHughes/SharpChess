// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoardDebug.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Helper methods for debuging board positions.
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
    using System.Diagnostics;
    using System.Text;

    #endregion

    /// <summary>
    /// Helper methods for debuging board positions.
    /// </summary>
    public static class BoardDebug
    {
        #region Public Properties

        /// <summary>
        ///   Gets a Debug String representing the currnet board position.
        /// </summary>
        public static string DebugString
        {
            get
            {
                string strOutput = string.Empty;
                int intOrdinal = Board.SquareCount - 1;

                for (int intRank = 0; intRank < Board.RankCount; intRank++)
                {
                    for (int intFile = 0; intFile < Board.FileCount; intFile++)
                    {
                        Square square = Board.GetSquare(intOrdinal);
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Display the chessboard in the Immediate Windows
        /// </summary>
        /// <remarks>
        /// VS.NET menu "Debug" / "Windows" / "Immediate"
        /// </remarks>
        /// <example>
        /// Board. DebugDisplay()
        /// </example>
        public static void DebugDisplay()
        {
            Debug.Write(DebugGetBoard());
            Debug.Write(". ");
        }

        #endregion

        // end DebugDisplay
        #region Methods

        /// <summary>
        /// Display info on the game at the right of the chessboard
        /// </summary>
        /// <param name="indRank">
        /// the rank in the chessboard
        /// </param>
        /// <param name="strbBoard">
        /// output buffer
        /// </param>
        /// <remarks>
        /// Display the captured pieces and the MoveHistory
        /// </remarks>
        private static void DebugGameInfo(int indRank, ref StringBuilder strbBoard)
        {
            strbBoard.Append(":");
            strbBoard.Append(indRank);
            strbBoard.Append(" ");
            switch (indRank)
            {
                case 0:
                case 7:
                    Pieces piecesCaptureList = (indRank == 7)
                                                   ? Game.PlayerWhite.CapturedEnemyPieces
                                                   : Game.PlayerBlack.CapturedEnemyPieces;
                    if (piecesCaptureList.Count > 1)
                    {
                        strbBoard.Append("x ");
                        foreach (Piece pieceCaptured in piecesCaptureList)
                        {
                            strbBoard.Append(
                                (pieceCaptured.Name == Piece.PieceNames.Pawn)
                                    ? string.Empty
                                    : pieceCaptured.Abbreviation + pieceCaptured.Square.Name + " ");
                        }
                    }

                    break;

                case 5:
                    int turnNumberOld = Game.TurnNo; // Backup TurNo
                    Game.TurnNo -= Game.PlayerToPlay.Brain.Search.SearchDepth;
                    for (int indMov = Math.Max(1, Game.MoveHistory.Count - Game.PlayerToPlay.Brain.Search.MaxSearchDepth);
                         indMov < Game.MoveHistory.Count;
                         indMov++)
                    {
                        Move moveThis = Game.MoveHistory[indMov];
                        if (moveThis.Piece.Player.Colour == Player.PlayerColourNames.White)
                        {
                            strbBoard.Append(indMov >> 1);
                            strbBoard.Append(". ");
                        }

                        // moveThis.PgnSanFormat(false); // Contextual to Game.TurNo
                        strbBoard.Append(moveThis.Description + " ");
                        Game.TurnNo++;
                    }

                    Game.TurnNo = turnNumberOld; // Restore TurNo
                    break;
            }

            strbBoard.Append("\n");
        }

        /// <summary>
        /// A string representation of the board position - useful for debugging.
        /// </summary>
        /// <returns>
        /// Board position string.
        /// </returns>
        public static string DebugGetBoard()
        {
            var strbBoard = new StringBuilder(160);
            strbBoard.Append("  0 1 2 3 4 5 6 7 :PlayerToPlay = ");
            strbBoard.Append((Game.PlayerToPlay.Colour == Player.PlayerColourNames.White) ? "White\n" : "Black\n");
            for (int indRank = 7; indRank >= 0; indRank--)
            {
                strbBoard.Append(indRank + 1);
                strbBoard.Append(":");
                for (int indFile = 0; indFile < 8; indFile++)
                {
                    Square square = Board.GetSquare(indFile, indRank);
                    if (square != null)
                    {
                        if (square.Piece == null)
                        {
                            strbBoard.Append(". ");
                        }
                        else
                        {
                            switch (square.Piece.Player.Colour)
                            {
                                case Player.PlayerColourNames.White:
                                    strbBoard.Append(square.Piece.Abbreviation);
                                    break;
                                default:
                                    strbBoard.Append(square.Piece.Abbreviation.ToLower());
                                    break;
                            }

                            strbBoard.Append(" ");
                        }
                    }
                }

                DebugGameInfo(indRank, ref strbBoard);
            }

            strbBoard.Append("  a b c d e f g h :TurnNo = ");
            strbBoard.Append(Game.TurnNo);
            return strbBoard.ToString();
        }

        #endregion
    }
}