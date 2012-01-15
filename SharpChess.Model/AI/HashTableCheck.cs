// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashTableCheck.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The hash table (also know as Transposition table) specifically for check positions.
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
    /// <summary>
    /// The hash table (also know as Transposition table) specifically for check positions.
    /// </summary>
    public static class HashTableCheck
    {
        #region Constants and Fields

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
        ///   Gets the number of hash table Hits that have occured.
        /// </summary>
        public static int Hits { get; private set; }

        /// <summary>
        ///   Gets the number of hash table Overwrites that have occured.
        /// </summary>
        public static int Overwrites { get; private set; }

        /// <summary>
        ///   Gets the number of hash table Probes that have occured.
        /// </summary>
        public static int Probes { get; private set; }

        /// <summary>
        ///   Gets the number of hash table Writes that have occured.
        /// </summary>
        public static int Writes { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears all entries in the hash table.
        /// </summary>
        public static void Clear()
        {
            ResetStats();
            for (uint intIndex = 0; intIndex < hashTableSize; intIndex++)
            {
                hashTableEntries[intIndex].HashCodeA = 0;
                hashTableEntries[intIndex].HashCodeB = 0;
                hashTableEntries[intIndex].IsInCheck = false;
            }
        }

        /// <summary>
        /// Initialises the HashTable.
        /// </summary>
        public static void Initialise()
        {
            hashTableSize = Game.AvailableMegaBytes * 4000;
            hashTableEntries = new HashEntry[hashTableSize];
            Clear();
        }

        /// <summary>
        /// Checks if the player is in check for the specified position, and caches the result.
        /// </summary>
        /// <param name="hashCodeA">
        /// Hash Code for Board position A
        /// </param>
        /// <param name="hashCodeB">
        /// Hash Code for Board position B
        /// </param>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <returns>
        /// Returns whether the player in check.
        /// </returns>
        public static unsafe bool QueryandCachePlayerInCheckStatusForPosition(ulong hashCodeA, ulong hashCodeB, Player player)
        {
            fixed (HashEntry* phashBase = &hashTableEntries[0])
            {
                if (player.Colour == Player.PlayerColourNames.Black)
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

                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % hashTableSize);

                if (phashEntry->HashCodeA != hashCodeA || phashEntry->HashCodeB != hashCodeB)
                {
                    if (phashEntry->HashCodeA != 0)
                    {
                        Overwrites++;
                    }

                    phashEntry->HashCodeA = hashCodeA;
                    phashEntry->HashCodeB = hashCodeB;
                    phashEntry->IsInCheck = player.DetermineCheckStatus();
                    Writes++;
                }
                else
                {
                    Hits++;
                }

                return phashEntry->IsInCheck;
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
            Overwrites = 0;
        }

        #endregion

        /// <summary>
        /// Reset hash table stats.
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