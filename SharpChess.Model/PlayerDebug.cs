#region License

// SharpChess
// Copyright (C) 2011 Peter Hughes
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
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
    ///   Debug helper methods for the player class.
    /// </summary>
    public static class PlayerDebug
    {
        #region Constants and Fields

        /// <summary>
        ///   Internal buffer to convert the PV to a string
        /// </summary>
        private static readonly StringBuilder m_strbPV = new StringBuilder(50);

        /// <summary>
        ///   Number of iteration of AlphaBeta at the top level
        /// </summary>
        private static int m_iDbgIteration;

        /// <summary>
        ///   Level of depth of the variation
        /// </summary>
        private static int m_iDbgLevel;

        /// <summary>
        ///   Unambiguous descriptive variation after conversion of the PGN variation
        /// </summary>
        private static string m_strDbgLine = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        ///   Does the current position match the specified variation?
        /// </summary>
        /// <param name="strVariation"> the iteration and the variation. Ex: "5 Rb4b5 Pf4f5 Pe5f6" </param>
        /// <param name="iPly"> number positive or 0 of halfmove. Do not confuse with iDepth </param>
        /// <param name="moveThis"> the current move at the beginning of the research </param>
        /// <param name="intSearchDepth">Search depth.</param>
        /// <param name="intMaxSearchDepth">Max search depth.</param>
        /// <returns> true if the variation is recognized otherwise false </returns>
        /// <remarks>
        ///   Must be called after moveThis.DoMove() in AlphaBeta
        /// </remarks>
        private static bool DebugMatchLine(
            string strVariation, int iPly, Move moveThis, int intSearchDepth, int intMaxSearchDepth)
        {
            const int iSAN_LENGTH = 5; // Length of Abbreviation of the piece + From square + To square
            if (m_iDbgLevel == iPly)
            {
                // Is the level of depth of the variation reached?
                if (m_strDbgLine.Length == 0)
                {
                    // Interpret dynamically the variation
                    // In PlayerDebug version, strVariation contains unambiguous descriptive moves
                    int indPos = 0; // Evaluate the number of iteration and parse the variation
                    while (char.IsNumber(strVariation[indPos]))
                    {
                        indPos++;
                    }

                    m_iDbgIteration = Convert.ToInt32(strVariation.Substring(0, indPos));
                    m_strDbgLine = strVariation.Substring(indPos); // Parse the variation
                    m_strDbgLine = m_strDbgLine.Replace(" ", string.Empty); // removing all whitespaces
                    m_strDbgLine = m_strDbgLine.Replace("x", string.Empty); // removing all "x"
                }

                if (intSearchDepth == m_iDbgIteration)
                {
                    // Number of iteration of AlphaBeta at the top level
                    int indPiece = iPly * iSAN_LENGTH; // Index where begins the notation of the move
                    int iLenVar = m_strDbgLine.Length;
                    string strMoveDescr = moveThis.Piece.Abbreviation + moveThis.From.Name + moveThis.To.Name;
                    if ((indPiece <= iLenVar - iSAN_LENGTH)
                        && (strMoveDescr == m_strDbgLine.Substring(indPiece, iSAN_LENGTH)))
                    {
                        if (++m_iDbgLevel == iLenVar / iSAN_LENGTH)
                        {
                            // Number of moves in the variation
                            m_iDbgLevel = intMaxSearchDepth + 1; // Do not recall PlayerDebug utility
                            BoardDebug.DebugDisplay(); // Display the current position in the "Output Window"
                            Debug.WriteLine("\nPosition after: " + strVariation);
                            return true; // The current position matches the wished variation
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///   Break on the variation at the given iteration
        /// </summary>
        /// <param name="iPly"> the positive or null ply of halfmove. Don't confuse with iDepth </param>
        /// <param name="moveThis"> the current move </param>
        /// <param name="intSearchDepth">Search depth.</param>
        /// <param name="intMaxSearchDepth">Max search depth.</param>
        /// <returns> true if the position is reached otherwise false </returns>
        private static bool DebugMatchVariation(int iPly, Move moveThis, int intSearchDepth, int intMaxSearchDepth)
        {
            // Syntax of the string strVariation: <iteration> Move1 Move2 ...
#if !SKIP_MATCH_LINE

            // Add or remove the exclamation mark before SKIP_MATCH_LINE
            return DebugMatchLine(
                "5 Bb3a4 Bc8d7 Ba4xc6 Bd7xc6 Rf1e1 Bf8e7 Bc1d2", iPly, moveThis, intSearchDepth, intMaxSearchDepth);

            // The variation/line you want to debug!
#else
            return false;

            // Do not break on the variation
#endif

            // SKIP_MATCH_LINE
        }

        /// <summary>
        ///   Convert the Principal Variation to a string
        /// </summary>
        /// <param name="moveList"> the list of moves of the variation </param>
        /// <returns> the string of the Principal Variation. Ex: 5 Bb3a4 Bc8d7 Ba4xc6 </returns>
        private static string PvLine(Moves moveList)
        {
            if (moveList != null)
            {
                m_strbPV.Remove(0, m_strbPV.Length);
                for (int intIndex = 0; intIndex < moveList.Count; intIndex++)
                {
                    Move move = moveList[intIndex];
                    if (move != null)
                    {
                        m_strbPV.Append(move.Piece.Abbreviation);
                        m_strbPV.Append(move.From.Name);
                        if (move.PieceCaptured != null)
                        {
                            m_strbPV.Append("x");
                        }

                        m_strbPV.Append(move.To.Name);
                        m_strbPV.Append(" ");
                    }
                }
            }

            return m_strbPV.ToString();
        }

        #endregion
    }
}