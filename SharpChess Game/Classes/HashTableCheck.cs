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
                hashTableEntries[intIndex].IsInCheck = false;
            }
        }

        /// <summary>
        /// The initialise.
        /// </summary>
        public static void Initialise()
        {
            hashTableSize = Game.AvailableMegaBytes * 4000;
            hashTableEntries = new HashEntry[hashTableSize];
            Clear();
        }

        /// <summary>
        /// Is player in check.
        /// </summary>
        /// <param name="player">
        /// The player.
        /// </param>
        /// <returns>
        /// Returns whether the player in check.
        /// </returns>
        public static unsafe bool IsPlayerInCheck(Player player)
        {
            fixed (HashEntry* phashBase = &hashTableEntries[0])
            {
                ulong hashCodeA = Board.HashCodeA;
                ulong hashCodeB = Board.HashCodeB;

                if (player.Colour == Player.enmColour.Black)
                {
                    hashCodeA |= 0x1;
                    hashCodeB |= 0x1;
                }
                else
                {
                    hashCodeA &= 0xFFFFFFFFFFFFFFFE;
                    hashCodeB &= 0xFFFFFFFFFFFFFFFE;
                }

                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % hashTableSize);

                if (phashEntry->HashCodeA != hashCodeA || phashEntry->HashCodeB != hashCodeB)
                {
                    phashEntry->HashCodeA = hashCodeA;
                    phashEntry->HashCodeB = hashCodeB;
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
            /// The is in check.
            /// </summary>
            public bool IsInCheck;

            #endregion
        }
    }
}