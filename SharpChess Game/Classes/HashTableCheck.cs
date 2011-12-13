// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashTableCheck.cs" company="SharpChess">
//   Peter Hughes
// </copyright>
// <summary>
//   The hash table check.
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
    /// The hash table check.
    /// </summary>
    public class HashTableCheck
    {
        #region Constants and Fields

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
                m_arrHashEntry[intIndex].IsInCheck = false;
            }
        }

        /// <summary>
        /// The initialise.
        /// </summary>
        public static void Initialise()
        {
            m_HashTableSize = Game.AvailableMegaBytes * 4000;
            m_arrHashEntry = new HashEntry[m_HashTableSize];
            Clear();
        }

        /// <summary>
        /// The is player in check.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <returns>
        /// The is player in check.
        /// </returns>
        public static unsafe bool IsPlayerInCheck(Player player)
        {
            fixed (HashEntry* phashBase = &m_arrHashEntry[0])
            {
                ulong HashCodeA = Board.HashCodeA;
                ulong HashCodeB = Board.HashCodeB;

                if (player.Colour == Player.enmColour.Black)
                {
                    HashCodeA |= 0x1;
                    HashCodeB |= 0x1;
                }
                else
                {
                    HashCodeA &= 0xFFFFFFFFFFFFFFFE;
                    HashCodeB &= 0xFFFFFFFFFFFFFFFE;
                }

                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(HashCodeA % m_HashTableSize);

                if (phashEntry->HashCodeA != HashCodeA || phashEntry->HashCodeB != HashCodeB)
                {
                    phashEntry->HashCodeA = HashCodeA;
                    phashEntry->HashCodeB = HashCodeB;
                    phashEntry->IsInCheck = player.DetermineCheckStatus();
                }

                return phashEntry->IsInCheck;
            }
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
            /// The is in check.
            /// </summary>
            public bool IsInCheck;

            #endregion
        }
    }
}