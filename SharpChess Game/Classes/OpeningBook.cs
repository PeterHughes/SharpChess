// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpeningBook.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The opening book.
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
    using System.Xml;

    #endregion

    /// <summary>
    /// The opening book.
    /// </summary>
    public class OpeningBook
    {
        #region Constants and Fields

        /// <summary>
        /// The has h_ tabl e_ size.
        /// </summary>
        public const int HASH_TABLE_SIZE = 1000777;

        /// <summary>
        /// The unknown.
        /// </summary>
        public const Move UNKNOWN = null;

        /// <summary>
        /// The m_arr hash entry.
        /// </summary>
        private static readonly HashEntry[] m_arrHashEntry = new HashEntry[HASH_TABLE_SIZE];

        /// <summary>
        /// The collisions.
        /// </summary>
        private static int Collisions;

        /// <summary>
        /// The entries.
        /// </summary>
        private static int Entries;

        /// <summary>
        /// The hits.
        /// </summary>
        private static int Hits;

        /// <summary>
        /// The probes.
        /// </summary>
        private static int Probes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="OpeningBook"/> class.
        /// </summary>
        static OpeningBook()
        {
            Clear();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The book convert.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        public static void BookConvert(Player player)
        {
            XmlDocument xmldoc = new XmlDocument();

            // xmldoc.Load(@"d:\ob6.xml");
            xmldoc.Load(@"d:\OpeningBook.xml");

            // xmldoc.Load(@"d:\OpeningBook_16plys_146027.xml");
            int intScanMove = ScanPly(player, (XmlElement)xmldoc.SelectSingleNode("OpeningBook"));
            if (intScanMove != 0)
            {
                RecordHash(Board.HashCodeA, Board.HashCodeB, (byte)(intScanMove >> 8 & 0xff), (byte)(intScanMove & 0xff), Move.enmName.Standard, player.Colour);
            }
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public static void Clear()
        {
            ResetStats();
            for (uint intIndex = 0; intIndex < HASH_TABLE_SIZE; intIndex++)
            {
                m_arrHashEntry[intIndex].HashCodeA = 0;
                m_arrHashEntry[intIndex].HashCodeB = 0;
                m_arrHashEntry[intIndex].From = 0xff;
                m_arrHashEntry[intIndex].To = 0xff;
                m_arrHashEntry[intIndex].MoveName = Move.enmName.NullMove;
            }
        }

        /// <summary>
        /// The reset stats.
        /// </summary>
        public static void ResetStats()
        {
            Entries = 0;
            Collisions = 0;
            Probes = 0;
            Hits = 0;
        }

        /// <summary>
        /// The search for good move.
        /// </summary>
        /// <param name="BoardHashCodeA">
        /// The board hash code a.
        /// </param>
        /// <param name="BoardHashCodeB">
        /// The board hash code b.
        /// </param>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <returns>
        /// </returns>
        public static Move SearchForGoodMove(ulong BoardHashCodeA, ulong BoardHashCodeB, Player.enmColour colour)
        {
            return ProbeForBestMove(BoardHashCodeA, BoardHashCodeB, colour);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The probe for best move.
        /// </summary>
        /// <param name="HashCodeA">
        /// The hash code a.
        /// </param>
        /// <param name="HashCodeB">
        /// The hash code b.
        /// </param>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <returns>
        /// </returns>
        private static unsafe Move ProbeForBestMove(ulong HashCodeA, ulong HashCodeB, Player.enmColour colour)
        {
            if (colour == Player.enmColour.Black)
            {
                HashCodeA |= 0x1;
                HashCodeB |= 0x1;
            }
            else
            {
                HashCodeA &= 0xFFFFFFFFFFFFFFFE;
                HashCodeB &= 0xFFFFFFFFFFFFFFFE;
            }

            Probes++;

            fixed (HashEntry* phashBase = &m_arrHashEntry[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(HashCodeA % HASH_TABLE_SIZE);

                if (phashEntry->HashCodeA == HashCodeA && phashEntry->HashCodeB == HashCodeB)
                {
                    Hits++;
                    return new Move(0, 0, phashEntry->MoveName, Board.GetPiece(phashEntry->From), Board.GetSquare(phashEntry->From), Board.GetSquare(phashEntry->To), null, 0, 0);
                }
            }

            return UNKNOWN;
        }

        /// <summary>
        /// The record hash.
        /// </summary>
        /// <param name="HashCodeA">
        /// The hash code a.
        /// </param>
        /// <param name="HashCodeB">
        /// The hash code b.
        /// </param>
        /// <param name="From">
        /// The from.
        /// </param>
        /// <param name="To">
        /// The to.
        /// </param>
        /// <param name="MoveName">
        /// The move name.
        /// </param>
        /// <param name="colour">
        /// The colour.
        /// </param>
        private static unsafe void RecordHash(ulong HashCodeA, ulong HashCodeB, byte From, byte To, Move.enmName MoveName, Player.enmColour colour)
        {
            if (colour == Player.enmColour.Black)
            {
                HashCodeA |= 0x1;
                HashCodeB |= 0x1;
            }
            else
            {
                HashCodeA &= 0xFFFFFFFFFFFFFFFE;
                HashCodeB &= 0xFFFFFFFFFFFFFFFE;
            }

            Entries++;

            fixed (HashEntry* phashBase = &m_arrHashEntry[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(HashCodeA % HASH_TABLE_SIZE);
                if (phashEntry->HashCodeA != 0 && phashEntry->HashCodeA != HashCodeA && phashEntry->HashCodeB != HashCodeB)
                {
                    Collisions++;
                }

                phashEntry->HashCodeA = HashCodeA;
                phashEntry->HashCodeB = HashCodeB;
                phashEntry->From = From;
                phashEntry->To = To;
                phashEntry->MoveName = MoveName;
            }
        }

        /// <summary>
        /// The scan ply.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="xmlnodeParent">
        /// The xmlnode parent.
        /// </param>
        /// <returns>
        /// The scan ply.
        /// </returns>
        private static int ScanPly(Player player, XmlElement xmlnodeParent)
        {
            Move moveUndo;
            int intReturnScore = 0;
            int intReturnMove = 0;
            int intScanMove;
            int intScore;

            foreach (XmlElement xmlnodeMove in xmlnodeParent.ChildNodes)
            {
                Move.enmName movename = xmlnodeMove.GetAttribute("N") == null ? Move.enmName.Standard : Move.MoveNameFromString(xmlnodeMove.GetAttribute("N"));
                Square from = Board.GetSquare(xmlnodeMove.GetAttribute("F"));
                Square to = Board.GetSquare(xmlnodeMove.GetAttribute("T"));
                Piece piece = from.Piece;

                intScore = Convert.ToInt32(xmlnodeMove.GetAttribute(player.Colour == Player.enmColour.White ? "W" : "B"));
                if (intScore > intReturnScore)
                {
                    intReturnScore = intScore;
                    intReturnMove = from.Ordinal << 8 | to.Ordinal;
                }

                moveUndo = piece.Move(movename, to);

                intScanMove = ScanPly(player.OtherPlayer, xmlnodeMove);
                if (intScanMove != 0)
                {
                    RecordHash(Board.HashCodeA, Board.HashCodeB, (byte)(intScanMove >> 8 & 0xff), (byte)(intScanMove & 0xff), movename, player.OtherPlayer.Colour);
                }

                Move.Undo(moveUndo);
            }

            return intReturnMove;
        }

        #endregion

        /// <summary>
        /// The hash entry.
        /// </summary>
        private struct HashEntry
        {
            #region Constants and Fields

            /// <summary>
            /// The from.
            /// </summary>
            public byte From;

            /// <summary>
            /// The hash code a.
            /// </summary>
            public ulong HashCodeA;

            /// <summary>
            /// The hash code b.
            /// </summary>
            public ulong HashCodeB;

            /// <summary>
            /// The move name.
            /// </summary>
            public Move.enmName MoveName;

            /// <summary>
            /// The to.
            /// </summary>
            public byte To;

            #endregion
        }
    }
}