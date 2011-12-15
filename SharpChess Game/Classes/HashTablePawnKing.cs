// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashTablePawnKing.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The hash table pawn king.
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
    /// The hash table pawn king.
    /// </summary>
    public class HashTablePawnKing
    {
        #region Constants and Fields

        /// <summary>
        /// The unknown.
        /// </summary>
        public const int NotFoundInHashTable = int.MinValue;

        /// <summary>
        /// The m_ hash table size.
        /// </summary>
        private static uint hashTableSize;

        /// <summary>
        /// The m_arr hash entry.
        /// </summary>
        private static HashEntry[] hashTableEntries;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Collisions.
        /// </summary>
        public static int Collisions { get; private set; }

        /// <summary>
        /// Gets Hits.
        /// </summary>
        public static int Hits { get; private set; }

        /// <summary>
        /// Gets Overwrites.
        /// </summary>
        public static int Overwrites { get; private set; }

        /// <summary>
        /// Gets Probes.
        /// </summary>
        public static int Probes { get; private set; }

        /// <summary>
        /// Gets SlotsUsed.
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
        /// Gets Writes.
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
                hashTableEntries[intIndex].Points = NotFoundInHashTable;
            }
        }

        /// <summary>
        /// The initialise.
        /// </summary>
        public static void Initialise()
        {
            hashTableSize = Game.AvailableMegaBytes * 3000;
            hashTableEntries = new HashEntry[hashTableSize];
            Clear();
        }

        /// <summary>
        /// The probe hash.
        /// </summary>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <returns>
        /// The probe hash.
        /// </returns>
        public static unsafe int ProbeHash(Player.enmColour colour)
        {
            ulong hashCodeA = Board.HashCodeA;
            ulong hashCodeB = Board.HashCodeB;

            if (colour == Player.enmColour.Black)
            {
                hashCodeA |= 0x1;
                hashCodeB |= 0x1;
            }
            else
            {
                hashCodeA &= 0xFFFFFFFFFFFFFFFE;
                hashCodeB &= 0xFFFFFFFFFFFFFFFE;
            }

            Probes++;

            fixed (HashEntry* phashBase = &hashTableEntries[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % hashTableSize);

                if (phashEntry->HashCodeA == hashCodeA && phashEntry->HashCodeB == hashCodeB)
                {
                    Hits++;
                    return phashEntry->Points;
                }
            }

            return NotFoundInHashTable;
        }

        /// <summary>
        /// The record hash.
        /// </summary>
        /// <param name="val">
        /// The val.
        /// </param>
        /// <param name="colour">
        /// The colour.
        /// </param>
        public static unsafe void RecordHash(int val, Player.enmColour colour)
        {
            ulong hashCodeA = Board.HashCodeA;
            ulong hashCodeB = Board.HashCodeB;

            if (colour == Player.enmColour.Black)
            {
                hashCodeA |= 0x1;
                hashCodeB |= 0x1;
            }
            else
            {
                hashCodeA &= 0xFFFFFFFFFFFFFFFE;
                hashCodeB &= 0xFFFFFFFFFFFFFFFE;
            }

            fixed (HashEntry* phashBase = &hashTableEntries[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % hashTableSize);
                phashEntry->HashCodeA = hashCodeA;
                phashEntry->HashCodeB = hashCodeB;
                phashEntry->Points = val;
            }

            Writes++;
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
            /// The hash code a.
            /// </summary>
            public ulong HashCodeA;

            /// <summary>
            /// The hash code b.
            /// </summary>
            public ulong HashCodeB;

            /// <summary>
            /// The points.
            /// </summary>
            public int Points;

            #endregion
        }
    }
}