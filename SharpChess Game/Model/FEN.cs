// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FEN.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Converts a Forsyth–Edwards Notation (FEN) string into a SharpChess board position.
//   http://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation
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
    /// Converts a Forsyth–Edwards Notation (FEN) string into a SharpChess board position.
    /// http://chessprogramming.wikispaces.com/Forsyth-Edwards+Notation
    /// </summary>
    public static class Fen
    {
        #region Public Properties

        /// <summary>
        ///   Gets GameStartPosition.
        /// </summary>
        public static string GameStartPosition
        {
            get
            {
                return "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Extraction the current position in FEN: Forsyth-Edwards Notation
        /// </summary>
        /// <returns>
        /// string of the FEN position with 6 fields separated by " "
        ///   <list type="number">
        /// <item>
        /// Piece placement data: <see cref="FenGet1Pieces"/>(strbFen, strbCouldCastlingW, strbCouldCastlingB)
        ///     </item>
        /// <item>
        /// Active color: " w " or " b "
        ///     </item>
        /// <item>
        /// Castling availability: "KQkq" or "-" see <see cref="FenGet3CastlingIsPossible"/>(...)
        ///     </item>
        /// <item>
        /// En passant target square coordonates: <see cref="FenGet4EnPassant"/>()
        ///     </item>
        /// <item>
        /// Number of ply since the last pawn advance or capturing move: <see cref="FenGet5Counter50MoveDraw"/>()
        ///     </item>
        /// <item>
        /// Full move number = Game.TurnNo \ 2 + 1
        ///     </item>
        /// </list>
        /// </returns>
        /// <example>
        /// <list type="bullet">
        /// <item>
        /// "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1" initial position
        ///     </item>
        /// <item>
        /// "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1" after 1. e4
        ///     </item>
        /// <item>
        /// "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e6 0 2" after 1. e4 e5
        ///     </item>
        /// <item>
        /// "rnbqkbnr/pppp1ppp/8/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2" after 2. Nf3
        ///     </item>
        /// <item>
        /// "r1bqkbnr/pppp1ppp/2n5/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R w KQkq - 2 3" after 2. Nf3 Nc6
        ///     </item>
        /// <item>
        /// "r1bqkbnr/pppp1ppp/2n5/1B2p3/4P3/5N2/PPPP1PPP/RNBQK2R b KQkq - 3 3" after 3. Bb5
        ///     </item>
        /// <item>
        /// "r1bqkbnr/1ppp1ppp/p1n5/1B2p3/4P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 0 4" after 3. Bb5 a6
        ///     </item>
        /// </list>
        /// </example>
        public static string GetBoardPosition()
        {
            Game.SuspendPondering();
            var strbFen = new StringBuilder();

            // Field 1: Piece placement data
            FenGet1Pieces(strbFen);

            // Field 2: Active color
            strbFen.Append((Game.PlayerToPlay.Colour == Player.PlayerColourNames.White) ? " w " : " b ");

            // Field 3: Castling availability
            bool whiteCanCastle = FenGet3CastlingIsPossible(Game.PlayerWhite.King, strbFen);
            bool blackCanCastle = FenGet3CastlingIsPossible(Game.PlayerBlack.King, strbFen);
            if (!whiteCanCastle && !blackCanCastle)
            {
                strbFen.Append("-"); // No castling availability for either side
            }

            // Field 4: En passant target square coordonates
            strbFen.Append(FenGet4EnPassant());

            // Field 5: number of Halfmove clock or ply since the last pawn advance or capturing move.
            strbFen.Append(FenGet5Counter50MoveDraw());

            // Field 6: Full move number
            strbFen.Append(((Game.TurnNo >> 1) + 1).ToString()); // Incremented at each move of Blacks

            Game.ResumePondering();
            return strbFen.ToString();
        }

        /// <summary>
        /// The set board position.
        /// </summary>
        /// <param name="fenString">
        /// The str fen.
        /// </param>
        public static void SetBoardPosition(string fenString)
        {
            string strActiveColour = "w";
            string strCastlingRights = string.Empty;
            string strEnPassant = "-";
            string strHalfMoveClock = "0";
            string strFullMoveNumber = "1";

            Game.FenStartPosition = fenString;

            Game.CaptureAllPieces();
            Game.DemoteAllPieces();

            // Break up the string into its various parts
            // rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            fenString += " ";

            // Piece Placement
            int pos = fenString.IndexOf(" ");
            char[] acharPiecePlacement = fenString.ToCharArray(0, pos);
            fenString = fenString.Substring(pos + 1);

            // Active Colour
            pos = fenString.IndexOf(" ");
            if (pos > -1)
            {
                strActiveColour = fenString.Substring(0, pos);
                fenString = fenString.Substring(pos + 1);
            }

            // Castling Rights
            pos = fenString.IndexOf(" ");
            if (pos > -1)
            {
                strCastlingRights = fenString.Substring(0, pos);
                fenString = fenString.Substring(pos + 1);
            }

            // En passant
            pos = fenString.IndexOf(" ");
            if (pos > -1)
            {
                strEnPassant = fenString.Substring(0, pos);
                fenString = fenString.Substring(pos + 1);
            }

            // Half move clock
            pos = fenString.IndexOf(" ");
            if (pos > -1)
            {
                strHalfMoveClock = fenString.Substring(0, pos);
                fenString = fenString.Substring(pos + 1);
            }

            // Full move number
            pos = fenString.IndexOf(" ");
            if (pos > -1)
            {
                strFullMoveNumber = fenString.Substring(0, pos);
            }

            // Match FEN pieces against actual pieces, and move them onto the board

            // Pass 1: Match piece name and location exactly
            SetPiecePlacement(ref acharPiecePlacement, false, false);

            // Pass 2: Match piece name
            SetPiecePlacement(ref acharPiecePlacement, true, false);

            // Pass 3: For non-pawns and not the king, allow pawns to be promoted to the named piece
            SetPiecePlacement(ref acharPiecePlacement, true, true);

            // Set player to play
            Game.PlayerToPlay = strActiveColour == "b" ? Game.PlayerBlack : Game.PlayerWhite;

            // Set castling rights
            Piece pieceRook;

            // White King's Rook
            if ((pieceRook = Board.GetPiece(7, 0)) != null && pieceRook.Name == Piece.PieceNames.Rook
                && pieceRook.Player.Colour == Player.PlayerColourNames.White)
            {
                pieceRook.NoOfMoves = strCastlingRights.LastIndexOf("K") >= 0 ? 0 : 1;
            }

            // Black King's Rook
            if ((pieceRook = Board.GetPiece(7, 7)) != null && pieceRook.Name == Piece.PieceNames.Rook
                && pieceRook.Player.Colour == Player.PlayerColourNames.Black)
            {
                pieceRook.NoOfMoves = strCastlingRights.LastIndexOf("k") >= 0 ? 0 : 1;
            }

            // White Queen's Rook
            if ((pieceRook = Board.GetPiece(0, 0)) != null && pieceRook.Name == Piece.PieceNames.Rook
                && pieceRook.Player.Colour == Player.PlayerColourNames.White)
            {
                pieceRook.NoOfMoves = strCastlingRights.LastIndexOf("Q") >= 0 ? 0 : 1;
            }

            // Black Queen's Rook
            if ((pieceRook = Board.GetPiece(0, 7)) != null && pieceRook.Name == Piece.PieceNames.Rook
                && pieceRook.Player.Colour == Player.PlayerColourNames.Black)
            {
                pieceRook.NoOfMoves = strCastlingRights.LastIndexOf("q") >= 0 ? 0 : 1;
            }

            // Half move (50 move draw) clock.
            Game.FiftyMoveDrawBase = int.Parse(strHalfMoveClock);

            // Full move number. Default 1. Must be defined before En Passant.
            Game.TurnNo = (int.Parse(strFullMoveNumber) - 1) << 1;
            if (Game.PlayerToPlay.Colour == Player.PlayerColourNames.Black)
            {
                Game.TurnNo++; // Always odd for the previous White's move 
            }

            // En Passant
            if (strEnPassant[0] != '-')
            {
                int indFile = Board.FileFromName(Convert.ToString(strEnPassant[0]));
                int indRank = int.Parse(Convert.ToString(strEnPassant[1]));
                if (indRank == 6)
                {
                    // if fenString = "e6"
                    indRank = 4; // last move was e7-e5 so indRank = 6 - 2 = 4
                }

                // else if indRank = 3, fenString = "e3" last move was e2-e4 so indRank = 3
                Piece piecePassed = Board.GetPiece(indFile, indRank);
                piecePassed.NoOfMoves = 1;
                piecePassed.LastMoveTurnNo = Game.TurnNo;
            }

            // Recalculate the hashkey for the current position.
            Board.EstablishHashKey();

            VerifyPiecePlacement(ref acharPiecePlacement);
        }

        /// <summary>
        /// Check if the array of strings represents a valid FEN position
        /// </summary>
        /// <param name="fenString">
        /// FEN chess board position string
        /// </param>
        /// <remarks>
        /// <list type="number">
        /// <item>
        /// Field 1: <see cref="FenCheck1PiecePlace"/>(arrStrFen[0]) Piece placement data
        ///     </item>
        /// <item>
        /// Field 2: <see cref="FenCheck2Color"/>(arrStrFen[1]) Active color
        ///     </item>
        /// <item>
        /// Field 3: <see cref="FenCheck3Castle"/>(arrStrFen[2]) Castling availability
        ///     </item>
        /// <item>
        /// Field 4: <see cref="FenCheck4EnPassant"/>(arrStrFen[3]) En passant target square coordonates
        ///     </item>
        /// <item>
        /// Field 5: <see cref="FenCheck5Counter50MoveDraw"/>(arrStrFen[4]) number of ply since the last capture or pawn move
        ///     </item>
        /// <item>
        /// Field 6: <see cref="FenCheck6NbrMove"/>(arrStrFen[5]) Full move number
        ///     </item>
        /// </list>
        /// </remarks>
        public static void Validate(string fenString)
        {
            char[] arrDelimiter = " ".ToCharArray();
            string[] arrStrFen = fenString.Split(arrDelimiter);
            int fieldCount = arrStrFen.Length;

            if (fieldCount < 1 || fieldCount > 6)
            {
                throw new ValidationException(
                    "1000: A FEN string must 1 to 6 fields separated by spaces\n e.g. rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            }

            if (fieldCount >= 2)
            {
                FenCheck2Color(arrStrFen[1]);
            }

            if (fieldCount >= 3)
            {
                FenCheck3Castle(arrStrFen[2]);
            }

            if (fieldCount >= 4)
            {
                FenCheck4EnPassant(arrStrFen[3]);
            }

            if (fieldCount >= 5)
            {
                FenCheck5Counter50MoveDraw(arrStrFen[4]);
            }

            if (fieldCount >= 6)
            {
                FenCheck6NbrMove(arrStrFen[5]);
            }
        }

        #endregion

        // end FenCheckPosition 

        // end FenHlpMsg
        #region Methods

        /// <summary>
        /// Check the squares in the FEN position
        /// </summary>
        /// <param name="fenString">
        /// field 1 of the FEN string: "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR"
        /// </param>
        /// <returns>
        /// true if all squares OK otherwise false
        /// </returns>
        private static bool FenCheck1PiecePlace(string fenString)
        {
            // TODO Investigate why whiteRook and blackRook are assigned values but never used.
            int whiteKing = 0, whiteQueen = 0, whiteRook = 0, whiteBishop = 0, whiteKnight = 0, whitePawn = 0; // White
            int blackKing = 0, blackQueen = 0, blackRook = 0, blackBishop = 0, blackKnight = 0, blackPawn = 0; // Black
            int squareNumber = 0, slashNumber = 0;
            for (int indChar = 0; indChar < fenString.Length; indChar++)
            {
                switch (fenString[indChar])
                {
                    case 'K':
                        whiteKing++;
                        squareNumber++;
                        break;
                    case 'k':
                        blackKing++;
                        squareNumber++;
                        break;
                    case 'Q':
                        whiteQueen++;
                        squareNumber++;
                        break;
                    case 'q':
                        blackQueen++;
                        squareNumber++;
                        break;
                    case 'R':
                        whiteRook++;
                        squareNumber++;
                        break;
                    case 'r':
                        blackRook++;
                        squareNumber++;
                        break;
                    case 'B':
                        whiteBishop++;
                        squareNumber++;
                        break;
                    case 'b':
                        blackBishop++;
                        squareNumber++;
                        break;
                    case 'N':
                        whiteKnight++;
                        squareNumber++;
                        break;
                    case 'n':
                        blackKnight++;
                        squareNumber++;
                        break;
                    case 'P':
                        whitePawn++;
                        squareNumber++;
                        break;
                    case 'p':
                        blackPawn++;
                        squareNumber++;
                        break;
                    case '/':
                        slashNumber++;
                        if (squareNumber != 8)
                        {
                            throw new ValidationException(
                                "1010: " + FenHlpMsg(1)
                                + string.Format("The rank {0} does not contain 8 squares", slashNumber));
                        }

                        squareNumber = 0;
                        break;

                    default:
                        if ((fenString[indChar] < '1') || (fenString[indChar] > '8'))
                        {
                            throw new ValidationException(
                                "1020: " + FenHlpMsg(1) + "Expected character 'KQRBNP/kqrbnp' '1'..'8'");
                        }

                        squareNumber += Convert.ToInt32(fenString[indChar].ToString());

                        break;
                }
            }

            if (slashNumber != 7)
            {
                throw new ValidationException("1030: " + FenHlpMsg(1) + "The string must have 7 slash separators");
            }

            if (squareNumber != 8)
            {
                throw new ValidationException("1040: " + FenHlpMsg(1) + "The last rank does not contain 8 squares");
            }

            if (whiteKing != 1)
            {
                throw new ValidationException("1050: " + FenHlpMsg(1) + "No White King");
            }

            if (blackKing != 1)
            {
                throw new ValidationException("1060: " + FenHlpMsg(1) + "No Black King");
            }

            if ((whiteQueen > 9) || (whiteBishop > 9) || (whiteKnight > 9) || (whitePawn > 8))
            {
                throw new ValidationException("1070: " + FenHlpMsg(1) + "Too many White Q or B or N or P");
            }

            if ((blackQueen > 9) || (blackBishop > 9) || (blackKnight > 9) || (blackPawn > 8))
            {
                throw new ValidationException("1080: " + FenHlpMsg(1) + "Too many Black q or b or n or p");
            }

            return true;
        }

        // end FenCheck1PiecePlace

        /// <summary>
        /// Check the active color in the FEN string
        /// </summary>
        /// <param name="fenString">
        /// field 2 of the FEN string: "w" or "b"
        /// </param>
        private static void FenCheck2Color(string fenString)
        {
            if (fenString == null)
            {
                throw new ArgumentNullException("fenString");
            }

            if ((fenString != "w") && (fenString != "b"))
            {
                throw new ValidationException("1200: " + FenHlpMsg(2) + "The active color must be 'w' or 'b'");
            }

            return;
        }

        // end FenCheck2Color

        /// <summary>
        /// Check the castling availability in the FEN string
        /// </summary>
        /// <param name="fenString">
        /// field 3 of the FEN string: "KQkq", ..., "-"
        /// </param>
        /// <remarks>
        /// If White/Black King could O-O or O-O-O then fenString = "KQkq"
        /// </remarks>
        private static void FenCheck3Castle(string fenString)
        {
            int dash = 0, whiteKing = 0, whiteQueen = 0, blackKing = 0, blackQueen = 0;
            foreach (char t in fenString)
            {
                switch (t)
                {
                    case 'K':
                        whiteKing++;
                        break;
                    case 'k':
                        blackKing++;
                        break;
                    case 'Q':
                        whiteQueen++;
                        break;
                    case 'q':
                        blackQueen++;
                        break;
                    case '-':
                        dash++;
                        break;
                    default:
                        throw new ValidationException("1300: " + FenHlpMsg(3) + "Expected character 'KQkq-'");
                }
            }

            if ((whiteKing > 1) || (whiteQueen > 1) || (blackKing > 1) || (blackQueen > 1) || (dash > 1))
            {
                throw new ValidationException("1310: " + FenHlpMsg(3) + "At least one occurrence of 'KQkq-'");
            }

            if ((dash == 1) && ((whiteKing == 1) || (whiteQueen == 1) || (blackKing == 1) || (blackQueen == 1)))
            {
                throw new ValidationException("1320: " + FenHlpMsg(3) + "'KQkq' or exclusive '-'");
            }

            return;
        }

        // end FenCheck3Castle

        /// <summary>
        /// Check the capture square En Passant in the FEN string
        /// </summary>
        /// <param name="fenString">
        /// field 4 of the FEN string: "e3", "e6", ..., "-"
        /// </param>
        /// <remarks>
        /// If the last move was e2-e4, then fenString = "e3"
        /// </remarks>
        private static void FenCheck4EnPassant(string fenString)
        {
            if (((fenString[0] < 'a') || (fenString[0] > 'h')) && (fenString[0] != '-'))
            {
                throw new ValidationException("1400: " + FenHlpMsg(4) + "Expected character 'abcdefgh-'");
            }

            if (fenString[0] == '-')
            {
                if (fenString.Length > 1)
                {
                    throw new ValidationException("1410: " + FenHlpMsg(4) + "No expected character after '-'");
                }
            }
            else if (((fenString[0] >= 'a') && (fenString[0] <= 'h'))
                     &&
                     (((fenString.Length == 2) && (fenString[1] != '3') && (fenString[1] != '6'))
                      || (fenString.Length > 2)))
            {
                throw new ValidationException(
                    "1420: " + FenHlpMsg(4) + "After the pawn file, expect the rank '3' or '6'");
            }

            return;
        }

        // end FenCheck4EnPassant

        /// <summary>
        /// Check the half move number in the FEN string
        /// </summary>
        /// <param name="fenString">
        /// field 5 of the FEN string: 0..100
        /// </param>
        /// <remarks>
        /// Represent the number of ply after a capture or a pawn move
        /// </remarks>
        private static void FenCheck5Counter50MoveDraw(string fenString)
        {
            if (fenString.Length > 2)
            {
                throw new ValidationException(
                    "1500: " + FenHlpMsg(5) + "1 or 2 digits for the nbr of ply for rule of 50 moves");
            }

            int halfMove;
            try
            {
                halfMove = int.Parse(fenString);
            }
            catch
            {
                throw new ValidationException(
                    "1510: " + FenHlpMsg(5) + "Expect a half move number for the rule of 50 moves");
            }

            if ((halfMove < 0) || (halfMove > 100))
            {
                throw new ValidationException("1520: " + FenHlpMsg(5) + "Expect a non negative half move number <= 100");
            }

            return;
        }

        // end FenCheck5Counter50MoveDraw

        /// <summary>
        /// Check the full move number in the FEN string
        /// </summary>
        /// <param name="fenString">
        /// field 6 of the FEN string: 1..200
        /// </param>
        private static void FenCheck6NbrMove(string fenString)
        {
            int fullMove;
            try
            {
                fullMove = int.Parse(fenString);
            }
            catch
            {
                throw new ValidationException("1600: " + FenHlpMsg(6) + "Expect a full move number");
            }

            if ((fullMove < 1) || (fullMove > 200))
            {
                throw new ValidationException("1610: " + FenHlpMsg(6) + "Expect a positive full move number <= 200");
            }

            return;
        }

        // end FenCheck6NbrMove

        // end FenGetPosition 

        /// <summary>
        /// FEN piece placement string by rank 7..0 and by file 0..7
        /// </summary>
        /// <param name="strbFen">
        /// string builder FEN of the chessboard
        /// </param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// strbFen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR" for the intial position
        ///     </item>
        /// <item>
        /// "r" = Black rook, "n" = Black knight,... "p" = Black pawn
        ///     </item>
        /// <item>
        /// "R" = White rook, "N" = White knight,... "P" = White pawn
        ///     </item>
        /// <item>
        /// "4P3" means 4 empty squares before the White pawn, then 3 empty squares
        ///     </item>
        /// <item>
        /// "8" means empty rank
        ///     </item>
        /// </list>
        /// </remarks>
        private static void FenGet1Pieces(StringBuilder strbFen)
        {
            for (int emptySquare = 0, indRank = Board.RankCount - 1; indRank >= 0; indRank--)
            {
                if (indRank != Board.RankCount - 1)
                {
                    strbFen.Append('/'); // Separator between 2 ranks
                }

                for (int indFile = 0; indFile < Board.FileCount; indFile++)
                {
                    // Browse by column
                    Square squareThis = Board.GetSquare(indFile, indRank);
                    Piece pieceThis = squareThis.Piece;
                    if (pieceThis == null)
                    {
                        emptySquare++;
                    }
                    else
                    {
                        if (emptySquare > 0)
                        {
                            // Nbr of empty squares before the piece
                            strbFen.Append(emptySquare.ToString());
                            emptySquare = 0;
                        }

                        switch (pieceThis.Player.Colour)
                        {
                            case Player.PlayerColourNames.Black:
                                strbFen.Append(pieceThis.Abbreviation.ToLower()); // Blacks in lowercase
                                break;
                            default:
                                strbFen.Append(pieceThis.Abbreviation);
                                break;
                        }
                    }
                }

                if (emptySquare > 0)
                {
                    // Nbr of empty squares after the last piece
                    strbFen.Append(emptySquare.ToString());
                    emptySquare = 0;
                }
            }
        }

        // end FenGet1Pieces

        /// <summary>
        /// FEN notation of castling availability of the King in the future
        /// </summary>
        /// <param name="pieceKing">
        /// the White or Black King
        /// </param>
        /// <param name="fenString">
        /// <list type="bullet">
        /// <item>
        /// append "K" if White castling availability
        ///     </item>
        /// <item>
        /// append "Q" if White castling availability both
        ///     </item>
        /// <item>
        /// append "k" if Black castling availability both Rook-side
        ///     </item>
        /// <item>
        /// append "q" if Black castling availability both Queen-side
        ///     </item>
        /// <item>
        /// append "KQkq" if O-O and O-O-O for White first then Black
        ///     </item>
        /// </list>
        /// </param>
        /// <returns>
        /// The fen get 3 castling future.
        /// </returns>
        private static bool FenGet3CastlingIsPossible(Piece pieceKing, StringBuilder fenString)
        {
            bool canCastle = false;
            if (((PieceKing)pieceKing.Top).CanCastleKingSide)
            {
                // King could castle Rook-side in the future
                fenString.Append((pieceKing.Player.Colour == Player.PlayerColourNames.White) ? "K" : "k");
                canCastle = true;
            }

            if (((PieceKing)pieceKing.Top).CanCastleQueenSide)
            {
                // King could castle Queen-side in the future
                fenString.Append((pieceKing.Player.Colour == Player.PlayerColourNames.White) ? "Q" : "q");
                canCastle = true;
            }

            return canCastle;
        }

        // end FenGet3CastlingIsPossible

        /// <summary>
        /// FEN string indicating the potential square target for a capture en passant
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item>
        /// "e3" if the last move was e2-e4
        ///     </item>
        /// <item>
        /// "e6" if the last move was e7-e5
        ///     </item>
        /// <item>
        /// " - " if the last move was not a pawn move of 2 squares
        ///     </item>
        /// </list>
        /// </returns>
        private static string FenGet4EnPassant()
        {
            if ((Game.MoveHistory.Count > 0) && (Game.MoveHistory.Last.Piece.Name == Piece.PieceNames.Pawn)
                && (Game.MoveHistory.Last.From.File == Game.MoveHistory.Last.To.File)
                &&
                (((Game.MoveHistory.Last.From.Rank == Game.MoveHistory.Last.To.Rank + 2)
                  && (Game.MoveHistory.Last.Piece.Player.Colour == Player.PlayerColourNames.Black))
                 ||
                 ((Game.MoveHistory.Last.From.Rank == Game.MoveHistory.Last.To.Rank - 2)
                  && (Game.MoveHistory.Last.Piece.Player.Colour == Player.PlayerColourNames.White))))
            {
                return " " + Game.MoveHistory.Last.From.FileName
                       + ((Game.MoveHistory.Last.Piece.Player.Colour == Player.PlayerColourNames.White) ? "3 " : "6 ");
                    
                    // The case between From and To
            }

            return " - "; // There is not en passant target square
        }

        // end FenGet4EnPassant

        /// <summary>
        /// FEN string of the number of ply since the last pawn advance or capturing move
        /// </summary>
        /// <returns>
        /// Return 50 move draw
        /// </returns>
        private static string FenGet5Counter50MoveDraw()
        {
            return string.Format(
                "{0} ", Game.MoveHistory.Count > 0 ? Game.MoveHistory.Last.FiftyMoveDrawCounter : Game.FiftyMoveDrawBase);
        }

        /// <summary>
        /// FEN helper naming each field
        /// </summary>
        /// <param name="fieldNumber">
        /// A value between 1 and 6
        /// </param>
        /// <returns>
        /// FEN field {iField}: help message
        /// </returns>
        /// <remarks>
        /// Used in a Warning message
        /// </remarks>
        private static string FenHlpMsg(int fieldNumber)
        {
            switch (fieldNumber)
            {
                case 1:
                    return "FEN field 1: Piece placement data.\n";
                case 2:
                    return "FEN field 2: Active color.\n";
                case 3:
                    return "FEN field 3: Castling availability.\n";
                case 4:
                    return "FEN field 4: En passant target square coordonates.\n";
                case 5:
                    return "FEN field 5: Nbr of half move without capture or pawn move.\n";
                case 6:
                    return "FEN field 6: Full move number.\n";
            }

            return string.Empty;
        }

        /// <summary>
        /// The move piece to fen position.
        /// </summary>
        /// <param name="charToken">
        /// The char token.
        /// </param>
        /// <param name="intFile">
        /// The int file.
        /// </param>
        /// <param name="intRank">
        /// The int rank.
        /// </param>
        /// <param name="blnAnyLocation">
        /// The bln any location.
        /// </param>
        /// <param name="blnAllowPromotion">
        /// The bln allow promotion.
        /// </param>
        private static void MovePieceToFenPosition(
            ref char charToken, int intFile, int intRank, bool blnAnyLocation, bool blnAllowPromotion)
        {
            Piece.PieceNames piecename = Piece.PieceNames.King;
            Player player = charToken.ToString() == charToken.ToString().ToUpper() ? Game.PlayerWhite : Game.PlayerBlack;

            switch (charToken.ToString().ToUpper())
            {
                case "K":
                    piecename = Piece.PieceNames.King;
                    break;
                case "Q":
                    piecename = Piece.PieceNames.Queen;
                    break;
                case "R":
                    piecename = Piece.PieceNames.Rook;
                    break;
                case "B":
                    piecename = Piece.PieceNames.Bishop;
                    break;
                case "N":
                    piecename = Piece.PieceNames.Knight;
                    break;
                case "P":
                    piecename = Piece.PieceNames.Pawn;
                    break;
            }

            // Try to find the required piece in from the available pool of captured 
            // pieces that haven't been placed on the board yet.
            Piece pieceToUse = null;
            foreach (Piece pieceCaptured in player.OpposingPlayer.CapturedEnemyPieces)
            {
                if ((pieceCaptured.Name == piecename || (blnAllowPromotion && pieceCaptured.Name == Piece.PieceNames.Pawn))
                    && (pieceCaptured.StartLocation == Board.GetSquare(intFile, intRank) || blnAnyLocation))
                {
                    pieceToUse = pieceCaptured;
                    break;
                }
            }

            if (pieceToUse != null)
            {
                Square square = Board.GetSquare(intFile, intRank);
                pieceToUse.Uncapture(0);
                square.Piece = pieceToUse;
                pieceToUse.Square = square;
                pieceToUse.NoOfMoves = blnAnyLocation ? 1 : 0;
                if (pieceToUse.Name != piecename)
                {
                    pieceToUse.Promote(piecename);
                }

                // Mark the token in the original FEN string with a * to indicate that the piece has been processed
                charToken = '.';
            }
        }

        /// <summary>
        /// The set piece placement.
        /// </summary>
        /// <param name="acharPiecePlacement">
        /// The achar piece placement.
        /// </param>
        /// <param name="blnAnyLocation">
        /// The bln any location.
        /// </param>
        /// <param name="blnAllowPromotion">
        /// The bln allow promotion.
        /// </param>
        /// <exception cref="ValidationException">
        /// Unknow character in FEN string.
        /// </exception>
        private static void SetPiecePlacement(
            ref char[] acharPiecePlacement, bool blnAnyLocation, bool blnAllowPromotion)
        {
            // Setup piece placement
            int intRank = 7;
            int intFile = 0;
            for (int intIndex = 0; intIndex < acharPiecePlacement.Length; intIndex++)
            {
                switch (acharPiecePlacement[intIndex])
                {
                    case '.':

                        // Indicates a processed piece, so move on
                        intFile++;
                        break;

                    case '/':
                        intFile = 0;
                        intRank--;
                        break;

                    case 'K':
                    case 'Q':
                    case 'R':
                    case 'B':
                    case 'N':
                    case 'P':
                    case 'k':
                    case 'q':
                    case 'r':
                    case 'b':
                    case 'n':
                    case 'p':
                        MovePieceToFenPosition(
                            ref acharPiecePlacement[intIndex], intFile, intRank, blnAnyLocation, blnAllowPromotion);
                        intFile++;
                        break;

                    default:
                        if (char.IsDigit(acharPiecePlacement[intIndex]))
                        {
                            intFile += int.Parse(acharPiecePlacement[intIndex].ToString());
                        }
                        else
                        {
                            throw new ValidationException(
                                "Unknow character in FEN string:" + acharPiecePlacement[intIndex]);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// The verify piece placement.
        /// </summary>
        /// <param name="acharPiecePlacement">
        /// The achar piece placement.
        /// </param>
        /// <exception cref="ValidationException">
        /// Raised when unable to place piece.
        /// </exception>
        private static void VerifyPiecePlacement(ref char[] acharPiecePlacement)
        {
            // Check to see if there were any pieces left that we couldnt place.
            string strPieceName = string.Empty;

            foreach (char t in acharPiecePlacement)
            {
                switch (t)
                {
                    case 'K':
                        strPieceName += " White King";
                        break;
                    case 'Q':
                        strPieceName += " White Queen";
                        break;
                    case 'R':
                        strPieceName += " White Rook";
                        break;
                    case 'B':
                        strPieceName += " White Bishop";
                        break;
                    case 'N':
                        strPieceName += " White Knight";
                        break;
                    case 'P':
                        strPieceName += " White Pawn";
                        break;
                    case 'k':
                        strPieceName += " Black King";
                        break;
                    case 'q':
                        strPieceName += " Black Queen";
                        break;
                    case 'r':
                        strPieceName += " Black Rook";
                        break;
                    case 'b':
                        strPieceName += " Black Bishop";
                        break;
                    case 'n':
                        strPieceName += " Black Knight";
                        break;
                    case 'p':
                        strPieceName += " Black Pawn";
                        break;
                }
            }

            if (strPieceName != string.Empty)
            {
                throw new ValidationException("Unable to place the following pieces: " + strPieceName);
            }
        }

        #endregion

        /// <summary>
        /// The validation exception.
        /// </summary>
        public class ValidationException : ApplicationException
        {
            #region Constants and Fields

            /// <summary>
            ///   Message text.
            /// </summary>
            private readonly string message = string.Empty;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ValidationException"/> class.
            /// </summary>
            /// <param name="strMessage">
            /// The str message.
            /// </param>
            public ValidationException(string strMessage)
            {
                this.message = strMessage;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///   Gets FENMessage.
            /// </summary>
            public string FenMessage
            {
                get
                {
                    return this.message;
                }
            }

            #endregion
        }
    }
}