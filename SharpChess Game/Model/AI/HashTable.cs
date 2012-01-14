// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashTable.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   The hash table, also know as Transposition table. Stores information about positions previously considered. Stores scores and "best moves".
//   http://chessprogramming.wikispaces.com/Transposition+Table
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
    // TODO Incorporate side-to-play colour into hash table key.
    // TODO Incorporate 3 move repetition into hash table key.
    /// <summary>
    /// The hash table, also know as Transposition table. Stores information about positions previously considered. Stores scores and "best moves".
    /// http://chessprogramming.wikispaces.com/Transposition+Table
    /// </summary>
    public static class HashTable
    {
        #region Constants and Fields

        /// <summary>
        ///   Indicates that a position was not found in the Hash Table.
        /// </summary>
        public const int NotFoundInHashTable = int.MinValue;

        /// <summary>
        ///   The number of chess positions that may be stored against the same hashtable entry key.
        /// </summary>
        private const int HashTableSlotDepth = 3;

        /// <summary>
        ///   Pointer to the HashTable
        /// </summary>
        private static HashEntry[] hashTableEntries;

        /// <summary>
        ///   Size of the HashTable.
        /// </summary>
        private static uint hashTableSize;

        #endregion

        #region Enums

        /// <summary>
        /// Type of HashTable entry.
        /// </summary>
        public enum HashTypeNames
        {
            /// <summary>
            ///   Exact value.
            /// </summary>
            Exact, 

            /// <summary>
            ///   Alpha value.
            /// </summary>
            Alpha, 

            /// <summary>
            ///   Beta value.
            /// </summary>
            Beta
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the number of hash table Collisions that have occured.
        /// </summary>
        public static int Collisions { get; private set; }

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
        ///   Gets the number of hash table slots used.
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
                hashTableEntries[intIndex].BlackFrom = -1;
                hashTableEntries[intIndex].BlackMoveName = Move.MoveNames.NullMove;
                hashTableEntries[intIndex].BlackTo = -1;
                hashTableEntries[intIndex].Colour = Player.PlayerColourNames.White;
                hashTableEntries[intIndex].Depth = sbyte.MinValue;
                hashTableEntries[intIndex].HashCodeA = 0;
                hashTableEntries[intIndex].HashCodeB = 0;
                hashTableEntries[intIndex].Result = int.MinValue;
                hashTableEntries[intIndex].Type = HashTypeNames.Exact;
                hashTableEntries[intIndex].WhiteFrom = -1;
                hashTableEntries[intIndex].WhiteMoveName = Move.MoveNames.NullMove;
                hashTableEntries[intIndex].WhiteTo = -1;
            }
        }

        /// <summary>
        /// Initialises the HashTable.
        /// </summary>
        public static void Initialise()
        {
            hashTableSize = Game.AvailableMegaBytes * 8000;
            hashTableEntries = new HashEntry[hashTableSize];
            Clear();
        }

        /// <summary>
        /// Search for best move in hash table.
        /// </summary>
        /// <param name="hashCodeA">
        /// Hash Code for Board position A
        /// </param>
        /// <param name="hashCodeB">
        /// Hash Code for Board position B
        /// </param>
        /// <param name="colour">
        /// The player colour.
        /// </param>
        /// <returns>
        /// Best move, or null.
        /// </returns>
        public static unsafe Move ProbeForBestMove(ulong hashCodeA, ulong hashCodeB, Player.PlayerColourNames colour)
        {
            // Disable if this feature when switched off.
            if (!Game.EnableTranspositionTable)
            {
                return null;
            }

            // TODO Unit test Hash Table. What happens when same position stored at different depths in diffenent slots with the same hash?
            fixed (HashEntry* phashBase = &hashTableEntries[0])
            {
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
                    if (colour == Player.PlayerColourNames.White)
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
        /// Hash Code for Board position A
        /// </param>
        /// <param name="hashCodeB">
        /// Hash Code for Board position B
        /// </param>
        /// <param name="depth">
        /// The search depth.
        /// </param>
        /// <param name="alpha">
        /// Apha value.
        /// </param>
        /// <param name="beta">
        /// Beta value.
        /// </param>
        /// <param name="colour">
        /// The player colour.
        /// </param>
        /// <returns>
        /// The positional score.
        /// </returns>
        public static unsafe int ProbeForScore(
            ulong hashCodeA, ulong hashCodeB, int depth, int alpha, int beta, Player.PlayerColourNames colour)
        {
            // Disable if this feature when switched off.
            if (!Game.EnableTranspositionTable)
            {
                return NotFoundInHashTable;
            }

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
        /// Record a hash new hash entry in the hash table.
        /// </summary>
        /// <param name="hashCodeA">
        /// Hash Code for Board position A
        /// </param>
        /// <param name="hashCodeB">
        /// Hash Code for Board position B
        /// </param>
        /// <param name="depth">
        /// The search depth.
        /// </param>
        /// <param name="val">
        /// The score of the position to record.
        /// </param>
        /// <param name="type">
        /// The position type: alpha, beta or exact value.
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
        public static unsafe void RecordHash(
            ulong hashCodeA, 
            ulong hashCodeB, 
            int depth, 
            int val, 
            HashTypeNames type, 
            int from, 
            int to, 
            Move.MoveNames moveName, 
            Player.PlayerColourNames colour)
        {
            Writes++;
            fixed (HashEntry* phashBase = &hashTableEntries[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(hashCodeA % hashTableSize);

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
                    if (colour == Player.PlayerColourNames.White)
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
        /// Reset hash table stats.
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
        /// The hash table entry.
        /// </summary>
        private struct HashEntry
        {
            #region Constants and Fields

            /// <summary>
            ///   Black from square ordinal.
            /// </summary>
            public sbyte BlackFrom;

            /// <summary>
            ///   Black move name.
            /// </summary>
            public Move.MoveNames BlackMoveName;

            /// <summary>
            ///   Black to square ordinal.
            /// </summary>
            public sbyte BlackTo;

            /// <summary>
            ///   Player colour.
            /// </summary>
            public Player.PlayerColourNames Colour;

            /// <summary>
            ///   Search depth.
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
            ///   The result (positional score).
            /// </summary>
            public int Result;

            /// <summary>
            ///   The hash table entry type.
            /// </summary>
            public HashTypeNames Type;

            /// <summary>
            ///   White from square ordinal.
            /// </summary>
            public sbyte WhiteFrom;

            /// <summary>
            ///   White move name.
            /// </summary>
            public Move.MoveNames WhiteMoveName;

            /// <summary>
            ///   White to square ordinal.
            /// </summary>
            public sbyte WhiteTo;

            #endregion
        }
    }
}