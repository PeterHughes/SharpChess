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
        public const int UNKNOWN = int.MinValue;

        /// <summary>
        /// The m_ hash table size.
        /// </summary>
        public static uint m_HashTableSize;

        /// <summary>
        /// The m_arr hash entry.
        /// </summary>
        private static HashEntry[] m_arrHashEntry;

        /// <summary>
        /// The m_int collisions.
        /// </summary>
        private static int m_intCollisions;

        /// <summary>
        /// The m_int hits.
        /// </summary>
        private static int m_intHits;

        /// <summary>
        /// The m_int overwrites.
        /// </summary>
        private static int m_intOverwrites;

        /// <summary>
        /// The m_int probes.
        /// </summary>
        private static int m_intProbes;

        /// <summary>
        /// The m_int writes.
        /// </summary>
        private static int m_intWrites;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Collisions.
        /// </summary>
        public static int Collisions
        {
            get
            {
                return m_intCollisions;
            }
        }

        /// <summary>
        /// Gets Hits.
        /// </summary>
        public static int Hits
        {
            get
            {
                return m_intHits;
            }
        }

        /// <summary>
        /// Gets Overwrites.
        /// </summary>
        public static int Overwrites
        {
            get
            {
                return m_intOverwrites;
            }
        }

        /// <summary>
        /// Gets Probes.
        /// </summary>
        public static int Probes
        {
            get
            {
                return m_intProbes;
            }
        }

        /// <summary>
        /// Gets SlotsUsed.
        /// </summary>
        public static int SlotsUsed
        {
            get
            {
                int intCounter = 0;

                for (uint intIndex = 0; intIndex < m_HashTableSize; intIndex++)
                {
                    if (m_arrHashEntry[intIndex].HashCodeA != 0)
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
        public static int Writes
        {
            get
            {
                return m_intWrites;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The clear.
        /// </summary>
        public static void Clear()
        {
            ResetStats();
            for (uint intIndex = 0; intIndex < m_HashTableSize; intIndex++)
            {
                m_arrHashEntry[intIndex].HashCodeA = 0;
                m_arrHashEntry[intIndex].HashCodeB = 0;
                m_arrHashEntry[intIndex].Points = UNKNOWN;
            }
        }

        /// <summary>
        /// The initialise.
        /// </summary>
        public static void Initialise()
        {
            m_HashTableSize = Game.AvailableMegaBytes * 3000;
            m_arrHashEntry = new HashEntry[m_HashTableSize];
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
            ulong HashCodeA = Board.HashCodeA;
            ulong HashCodeB = Board.HashCodeB;

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

            m_intProbes++;

            fixed (HashEntry* phashBase = &m_arrHashEntry[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(HashCodeA % m_HashTableSize);

                if (phashEntry->HashCodeA == HashCodeA && phashEntry->HashCodeB == HashCodeB)
                {
                    m_intHits++;
                    return phashEntry->Points;
                }
            }

            return UNKNOWN;
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
            ulong HashCodeA = Board.HashCodeA;
            ulong HashCodeB = Board.HashCodeB;

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

            fixed (HashEntry* phashBase = &m_arrHashEntry[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(HashCodeA % m_HashTableSize);
                phashEntry->HashCodeA = HashCodeA;
                phashEntry->HashCodeB = HashCodeB;
                phashEntry->Points = val;
            }

            m_intWrites++;
        }

        /// <summary>
        /// The reset stats.
        /// </summary>
        public static void ResetStats()
        {
            m_intProbes = 0;
            m_intHits = 0;
            m_intWrites = 0;
            m_intCollisions = 0;
            m_intOverwrites = 0;
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