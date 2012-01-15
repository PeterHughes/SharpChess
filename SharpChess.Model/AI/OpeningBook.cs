// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpeningBook.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Tournament standard opening book where the best possible move is always selected for the current board position.
//   XML opening book files are created from PGN files using the <see cref = "PGNtoXML" /> class.
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

namespace SharpChess.Model.AI
{
    #region Using

    using System;
    using System.Xml;

    using SharpChess.Model;

    #endregion

    /// <summary>
    /// Tournament standard opening book when the best possible move is always selected for the current board position.
    ///  XML opening book files are created from PGN files using the <see cref = "PGNtoXML" /> class.
    /// </summary>
    public class OpeningBook
    {
        #region Constants and Fields

        /// <summary>
        /// The has h_ tabl e_ size.
        /// </summary>
        public const int HashTableSize = 1000777;

        /// <summary>
        /// The unknown.
        /// </summary>
        public const Move NotFoundInHashTable = null;

        /// <summary>
        ///   Pointer to the HashTable
        /// </summary>
        private static readonly HashEntry[] HashTableEntries = new HashEntry[HashTableSize];

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
        /// Load an opening book into memory.
        /// </summary>
        /// <param name="player">
        /// The player to play.
        /// </param>
        public static void LoadOpeningBook(Player player)
        {
            XmlDocument xmldoc = new XmlDocument();

            // xmldoc.Load(@"d:\ob6.xml");
            xmldoc.Load(@"d:\OpeningBook.xml");

            // xmldoc.Load(@"d:\OpeningBook_16plys_146027.xml");
            int intScanMove = ScanPly(player, (XmlElement)xmldoc.SelectSingleNode("OpeningBook"));
            if (intScanMove != 0)
            {
                RecordHash(Board.HashCodeA, Board.HashCodeB, (byte)(intScanMove >> 8 & 0xff), (byte)(intScanMove & 0xff), Move.MoveNames.Standard, player.Colour);
            }
        }

        /// <summary>
        /// Clear opening book (hash table)
        /// </summary>
        public static void Clear()
        {
            for (uint intIndex = 0; intIndex < HashTableSize; intIndex++)
            {
                HashTableEntries[intIndex].HashCodeA = 0;
                HashTableEntries[intIndex].HashCodeB = 0;
                HashTableEntries[intIndex].From = 0xff;
                HashTableEntries[intIndex].To = 0xff;
                HashTableEntries[intIndex].MoveName = Move.MoveNames.NullMove;
            }
        }

        /// <summary>
        /// The search for best move in opening book.
        /// </summary>
        /// <param name="boardHashCodeA">
        /// The board hash code a.
        /// </param>
        /// <param name="boardHashCodeB">
        /// The board hash code b.
        /// </param>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <returns>
        /// The best move from the opening book.
        /// </returns>
        public static Move SearchForGoodMove(ulong boardHashCodeA, ulong boardHashCodeB, Player.PlayerColourNames colour)
        {
            return ProbeForBestMove(boardHashCodeA, boardHashCodeB, colour);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The probe opening book (hash table) for best move for the specied board position (hash code).
        /// </summary>
        /// <param name="hashCodeA">
        /// The hash code for board position A.
        /// </param>
        /// <param name="hashCodeB">
        /// The hash code for board position B.
        /// </param>
        /// <param name="colour">
        /// The player colour.
        /// </param>
        /// <returns>
        /// The best move in the opening book (hash table) or null if there is no opening book entry for the specified board position.
        /// </returns>
        private static unsafe Move ProbeForBestMove(ulong hashCodeA, ulong hashCodeB, Player.PlayerColourNames colour)
        {
            if (colour == Player.PlayerColourNames.Black)
            {
                hashCodeA |= 0x1;
                hashCodeB |= 0x1;
            }
            else
            {
                hashCodeA &= 0xFFFFFFFFFFFFFFFE;
                hashCodeB &= 0xFFFFFFFFFFFFFFFE;
            }

            fixed (HashEntry* phashBase = &HashTableEntries[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % HashTableSize);

                if (phashEntry->HashCodeA == hashCodeA && phashEntry->HashCodeB == hashCodeB)
                {
                    return new Move(0, 0, phashEntry->MoveName, Board.GetPiece(phashEntry->From), Board.GetSquare(phashEntry->From), Board.GetSquare(phashEntry->To), null, 0, 0);
                }
            }

            return NotFoundInHashTable;
        }

        /// <summary>
        /// Record a hash new hash entry in the hash table.
        /// </summary>
        /// <param name="hashCodeA">
        /// Hash Code for Board position A
        /// </param>
        /// <param name="hashCodeB">
        /// Hash Code for Board position B
        /// </param>
        /// <param name="from">
        /// From square ordinal.
        /// </param>
        /// <param name="to">
        /// To square ordinal.
        /// </param>
        /// <param name="moveName">
        /// The move name.
        /// </param>
        /// <param name="colour">
        /// The player colour.
        /// </param>
        private static unsafe void RecordHash(ulong hashCodeA, ulong hashCodeB, byte from, byte to, Move.MoveNames moveName, Player.PlayerColourNames colour)
        {
            if (colour == Player.PlayerColourNames.Black)
            {
                hashCodeA |= 0x1;
                hashCodeB |= 0x1;
            }
            else
            {
                hashCodeA &= 0xFFFFFFFFFFFFFFFE;
                hashCodeB &= 0xFFFFFFFFFFFFFFFE;
            }

            fixed (HashEntry* phashBase = &HashTableEntries[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % HashTableSize);

                phashEntry->HashCodeA = hashCodeA;
                phashEntry->HashCodeB = hashCodeB;
                phashEntry->From = from;
                phashEntry->To = to;
                phashEntry->MoveName = moveName;
            }
        }

        /// <summary>
        /// Load opening book (hash table )
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <param name="xmlnodeParent">
        /// The xmlnode parent.
        /// </param>
        /// <returns>
        /// The move score.
        /// </returns>
        private static int ScanPly(Player player, XmlElement xmlnodeParent)
        {
            int intReturnScore = 0;
            int intReturnMove = 0;

            foreach (XmlElement xmlnodeMove in xmlnodeParent.ChildNodes)
            {
                Move.MoveNames movename = xmlnodeMove.GetAttribute("N") == null ? Move.MoveNames.Standard : Move.MoveNameFromString(xmlnodeMove.GetAttribute("N"));
                Square from = Board.GetSquare(xmlnodeMove.GetAttribute("F"));
                Square to = Board.GetSquare(xmlnodeMove.GetAttribute("T"));
                Piece piece = from.Piece;

                int intScore = Convert.ToInt32(xmlnodeMove.GetAttribute(player.Colour == Player.PlayerColourNames.White ? "W" : "B"));
                if (intScore > intReturnScore)
                {
                    intReturnScore = intScore;
                    intReturnMove = from.Ordinal << 8 | to.Ordinal;
                }

                Move moveUndo = piece.Move(movename, to);

                int intScanMove = ScanPly(player.OpposingPlayer, xmlnodeMove);
                if (intScanMove != 0)
                {
                    RecordHash(Board.HashCodeA, Board.HashCodeB, (byte)(intScanMove >> 8 & 0xff), (byte)(intScanMove & 0xff), movename, player.OpposingPlayer.Colour);
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
            /// The from square ordinal.
            /// </summary>
            public byte From;

            /// <summary>
            /// The board position hash code a.
            /// </summary>
            public ulong HashCodeA;

            /// <summary>
            /// The board position hash code b.
            /// </summary>
            public ulong HashCodeB;

            /// <summary>
            /// The move name.
            /// </summary>
            public Move.MoveNames MoveName;

            /// <summary>
            /// The to square ordinal.
            /// </summary>
            public byte To;

            #endregion
        }
    }
}