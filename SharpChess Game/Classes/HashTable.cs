// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashTable.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The hash table.
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
    /// <summary>
    /// The hash table.
    /// </summary>
    public static class HashTable
    {
        #region Constants and Fields

        /// <summary>
        ///   The unknown.
        /// </summary>
        public const int NotFoundInHashTable = int.MinValue;

        /// <summary>
        ///   The has h_ tabl e_ slo t_ depth.
        /// </summary>
        private const int HashTableSlotDepth = 3;

        /// <summary>
        ///   The m_arr hash entry.
        /// </summary>
        private static HashEntry[] hashTableEntries;

        /// <summary>
        ///   The m_ hash table size.
        /// </summary>
        private static uint hashTableSize;

        #endregion

        #region Enums

        /// <summary>
        /// The enm hash type.
        /// </summary>
        public enum HashTypeNames
        {
            /// <summary>
            ///   The exact.
            /// </summary>
            Exact, 

            /// <summary>
            ///   The alpha.
            /// </summary>
            Alpha, 

            /// <summary>
            ///   The beta.
            /// </summary>
            Beta
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets Collisions.
        /// </summary>
        public static int Collisions { get; private set; }

        /// <summary>
        ///   Gets Hits.
        /// </summary>
        public static int Hits { get; private set; }

        /// <summary>
        ///   Gets Overwrites.
        /// </summary>
        public static int Overwrites { get; private set; }

        /// <summary>
        ///   Gets Probes.
        /// </summary>
        public static int Probes { get; private set; }

        /// <summary>
        ///   Gets SlotsUsed.
        /// </summary>
        public static int SlotsUsed
        {
            get
            {
                int intCounter = 0;

                for (uint intIndex = 0; intIndex < hashTableSize; intIndex++)
                {
                    if (hashTableEntries[intIndex].HashCodeA != 0)
                    {
                        intCounter++;
                    }
                }

                return intCounter;
            }
        }

        /// <summary>
        ///   Gets Writes.
        /// </summary>
        public static int Writes { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The clear.
        /// </summary>
        public static void Clear()
        {
            ResetStats();
            for (uint intIndex = 0; intIndex < hashTableSize; intIndex++)
            {
                hashTableEntries[intIndex].HashCodeA = 0;
                hashTableEntries[intIndex].HashCodeB = 0;
                hashTableEntries[intIndex].Depth = sbyte.MinValue;
                hashTableEntries[intIndex].WhiteFrom = -1;
                hashTableEntries[intIndex].BlackFrom = -1;
            }
        }

        /// <summary>
        /// The initialise.
        /// </summary>
        public static void Initialise()
        {
            hashTableSize = Game.AvailableMegaBytes * 8000;
            hashTableEntries = new HashEntry[hashTableSize];
            Clear();
        }

        /// <summary>
        /// The probe for best move.
        /// </summary>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <returns>
        /// Best move, or null.
        /// </returns>
        public static unsafe Move ProbeForBestMove(Player.enmColour colour)
        {
            fixed (HashEntry* phashBase = &hashTableEntries[0])
            {
                ulong hashCodeA = Board.HashCodeA;
                ulong hashCodeB = Board.HashCodeB;

                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % hashTableSize);

                int intAttempt = 0;
                while (phashEntry >= phashBase
                       && (phashEntry->HashCodeA != hashCodeA || phashEntry->HashCodeB != hashCodeB))
                {
                    phashEntry--;
                    intAttempt++;
                    if (intAttempt == HashTableSlotDepth)
                    {
                        break;
                    }
                }

                if (phashEntry < phashBase)
                {
                    phashEntry = phashBase;
                }

                if (phashEntry->HashCodeA == hashCodeA && phashEntry->HashCodeB == hashCodeB)
                {
                    if (colour == Player.enmColour.White)
                    {
                        if (phashEntry->WhiteFrom >= 0)
                        {
                            return new Move(
                                0, 
                                0, 
                                phashEntry->WhiteMoveName, 
                                Board.GetPiece(phashEntry->WhiteFrom), 
                                Board.GetSquare(phashEntry->WhiteFrom), 
                                Board.GetSquare(phashEntry->WhiteTo), 
                                Board.GetSquare(phashEntry->WhiteTo).Piece, 
                                0, 
                                phashEntry->Result);
                        }
                    }
                    else
                    {
                        if (phashEntry->BlackFrom >= 0)
                        {
                            return new Move(
                                0, 
                                0, 
                                phashEntry->BlackMoveName, 
                                Board.GetPiece(phashEntry->BlackFrom), 
                                Board.GetSquare(phashEntry->BlackFrom), 
                                Board.GetSquare(phashEntry->BlackTo), 
                                Board.GetSquare(phashEntry->BlackTo).Piece, 
                                0, 
                                phashEntry->Result);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Search Hash table for a previously stored score.
        /// </summary>
        /// <param name="hashCodeA">
        /// The hash code a.
        /// </param>
        /// <param name="hashCodeB">
        /// The hash code b.
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
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <returns>
        /// The score
        /// </returns>
        public static unsafe int ProbeHash(
            ulong hashCodeA, ulong hashCodeB, int depth, int alpha, int beta, Player.enmColour colour)
        {
            Probes++;

            fixed (HashEntry* phashBase = &hashTableEntries[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % hashTableSize);

                int intAttempt = 0;
                while (phashEntry >= phashBase
                       &&
                       (phashEntry->HashCodeA != hashCodeA || phashEntry->HashCodeB != hashCodeB
                        || phashEntry->Depth < depth))
                {
                    phashEntry--;
                    intAttempt++;
                    if (intAttempt == HashTableSlotDepth)
                    {
                        break;
                    }
                }

                if (phashEntry < phashBase)
                {
                    phashEntry = phashBase;
                }

                if (phashEntry->HashCodeA == hashCodeA && phashEntry->HashCodeB == hashCodeB
                    && phashEntry->Depth >= depth)
                {
                    if (phashEntry->Colour == colour)
                    {
                        if (phashEntry->Type == HashTypeNames.Exact)
                        {
                            Hits++;
                            return phashEntry->Result;
                        }

                        if ((phashEntry->Type == HashTypeNames.Alpha) && (phashEntry->Result <= alpha))
                        {
                            Hits++;
                            return alpha;
                        }

                        if ((phashEntry->Type == HashTypeNames.Beta) && (phashEntry->Result >= beta))
                        {
                            Hits++;
                            return beta;
                        }
                    }
                }
            }

            return NotFoundInHashTable;
        }

        /// <summary>
        /// The record hash.
        /// </summary>
        /// <param name="hashCodeA">
        /// The hash code a.
        /// </param>
        /// <param name="hashCodeB">
        /// The hash code b.
        /// </param>
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <param name="val">
        /// The val.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="moveName">
        /// The move name.
        /// </param>
        /// <param name="colour">
        /// The colour.
        /// </param>
        public static unsafe void RecordHash(
            ulong hashCodeA, 
            ulong hashCodeB, 
            int depth, 
            int val, 
            HashTypeNames type, 
            int from, 
            int to, 
            Move.enmName moveName, 
            Player.enmColour colour)
        {
            Writes++;
            fixed (HashEntry* phashBase = &hashTableEntries[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % hashTableSize);
                HashEntry* phashFirst = phashEntry;

                int intAttempt = 0;
                while (phashEntry >= phashBase && phashEntry->HashCodeA != 0 && phashEntry->Depth > depth)
                {
                    phashEntry--;
                    intAttempt++;
                    if (intAttempt == HashTableSlotDepth)
                    {
                        break;
                    }
                }

                if (phashEntry < phashBase)
                {
                    phashEntry = phashBase;
                }

                if (phashEntry->HashCodeA != 0)
                {
                    Collisions++;
                    if (phashEntry->HashCodeA != hashCodeA || phashEntry->HashCodeB != hashCodeB)
                    {
                        Overwrites++;
                        phashEntry->WhiteFrom = -1;
                        phashEntry->BlackFrom = -1;
                    }
                }

                phashEntry->HashCodeA = hashCodeA;
                phashEntry->HashCodeB = hashCodeB;
                phashEntry->Result = val;
                phashEntry->Type = type;
                phashEntry->Depth = (sbyte)depth;
                phashEntry->Colour = colour;
                if (from > -1)
                {
                    if (colour == Player.enmColour.White)
                    {
                        phashEntry->WhiteMoveName = moveName;
                        phashEntry->WhiteFrom = (sbyte)from;
                        phashEntry->WhiteTo = (sbyte)to;
                    }
                    else
                    {
                        phashEntry->BlackMoveName = moveName;
                        phashEntry->BlackFrom = (sbyte)from;
                        phashEntry->BlackTo = (sbyte)to;
                    }
                }
            }
        }

        /// <summary>
        /// The reset stats.
        /// </summary>
        public static void ResetStats()
        {
            Probes = 0;
            Hits = 0;
            Writes = 0;
            Collisions = 0;
            Overwrites = 0;
        }

        #endregion

        /// <summary>
        /// The hash entry.
        /// </summary>
        private struct HashEntry
        {
            #region Constants and Fields

            /// <summary>
            ///   The black from.
            /// </summary>
            public sbyte BlackFrom;

            /// <summary>
            ///   The black move name.
            /// </summary>
            public Move.enmName BlackMoveName;

            /// <summary>
            ///   The black to.
            /// </summary>
            public sbyte BlackTo;

            /// <summary>
            ///   The colour.
            /// </summary>
            public Player.enmColour Colour;

            /// <summary>
            ///   The depth.
            /// </summary>
            public sbyte Depth;

            /// <summary>
            ///   The hash code a.
            /// </summary>
            public ulong HashCodeA;

            /// <summary>
            ///   The hash code b.
            /// </summary>
            public ulong HashCodeB;

            /// <summary>
            ///   The result.
            /// </summary>
            public int Result;

            /// <summary>
            ///   The type.
            /// </summary>
            public HashTypeNames Type;

            /// <summary>
            ///   The white from.
            /// </summary>
            public sbyte WhiteFrom;

            /// <summary>
            ///   The white move name.
            /// </summary>
            public Move.enmName WhiteMoveName;

            /// <summary>
            ///   The white to.
            /// </summary>
            public sbyte WhiteTo;

            #endregion
        }
    }
}