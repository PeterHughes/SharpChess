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
    public class HashTable
    {
        #region Constants and Fields

        /// <summary>
        /// The has h_ tabl e_ slo t_ depth.
        /// </summary>
        public const int HASH_TABLE_SLOT_DEPTH = 3;

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

        #region Enums

        /// <summary>
        /// The enm hash type.
        /// </summary>
        public enum enmHashType
        {
            /// <summary>
            /// The exact.
            /// </summary>
            Exact, 

            /// <summary>
            /// The alpha.
            /// </summary>
            Alpha, 

            /// <summary>
            /// The beta.
            /// </summary>
            Beta
        }

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
                m_arrHashEntry[intIndex].Depth = sbyte.MinValue;
                m_arrHashEntry[intIndex].WhiteFrom = -1;
                m_arrHashEntry[intIndex].BlackFrom = -1;
            }
        }

        /// <summary>
        /// The initialise.
        /// </summary>
        public static void Initialise()
        {
            m_HashTableSize = Game.AvailableMegaBytes * 8000;
            m_arrHashEntry = new HashEntry[m_HashTableSize];
            Clear();
        }

        /// <summary>
        /// The probe for best move.
        /// </summary>
        /// <param name="colour">
        /// The colour.
        /// </param>
        /// <returns>
        /// </returns>
        public static unsafe Move ProbeForBestMove(Player.enmColour colour)
        {
            fixed (HashEntry* phashBase = &m_arrHashEntry[0])
            {
                ulong HashCodeA = Board.HashCodeA;
                ulong HashCodeB = Board.HashCodeB;

                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(HashCodeA % m_HashTableSize);

                int intAttempt = 0;
                while (phashEntry >= phashBase && (phashEntry->HashCodeA != HashCodeA || phashEntry->HashCodeB != HashCodeB))
                {
                    phashEntry--;
                    intAttempt++;
                    if (intAttempt == HASH_TABLE_SLOT_DEPTH)
                    {
                        break;
                    }
                }

                if (phashEntry < phashBase)
                {
                    phashEntry = phashBase;
                }

                if (phashEntry->HashCodeA == HashCodeA && phashEntry->HashCodeB == HashCodeB)
                {
                    if (colour == Player.enmColour.White)
                    {
                        if (phashEntry->WhiteFrom >= 0)
                        {
                            return new Move(0, 0, phashEntry->WhiteMoveName, Board.GetPiece(phashEntry->WhiteFrom), Board.GetSquare(phashEntry->WhiteFrom), Board.GetSquare(phashEntry->WhiteTo), Board.GetSquare(phashEntry->WhiteTo).Piece, 0, phashEntry->Result);
                        }
                    }
                    else
                    {
                        if (phashEntry->BlackFrom >= 0)
                        {
                            return new Move(0, 0, phashEntry->BlackMoveName, Board.GetPiece(phashEntry->BlackFrom), Board.GetSquare(phashEntry->BlackFrom), Board.GetSquare(phashEntry->BlackTo), Board.GetSquare(phashEntry->BlackTo).Piece, 0, phashEntry->Result);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The probe hash.
        /// </summary>
        /// <param name="HashCodeA">
        /// The hash code a.
        /// </param>
        /// <param name="HashCodeB">
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
        /// The probe hash.
        /// </returns>
        public static unsafe int ProbeHash(ulong HashCodeA, ulong HashCodeB, int depth, int alpha, int beta, Player.enmColour colour)
        {
            m_intProbes++;

            fixed (HashEntry* phashBase = &m_arrHashEntry[0])
            {
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(HashCodeA % m_HashTableSize);

                int intAttempt = 0;
                while (phashEntry >= phashBase && (phashEntry->HashCodeA != HashCodeA || phashEntry->HashCodeB != HashCodeB || phashEntry->Depth < depth))
                {
                    phashEntry--;
                    intAttempt++;
                    if (intAttempt == HASH_TABLE_SLOT_DEPTH)
                    {
                        break;
                    }
                }

                if (phashEntry < phashBase)
                {
                    phashEntry = phashBase;
                }

                if (phashEntry->HashCodeA == HashCodeA && phashEntry->HashCodeB == HashCodeB && phashEntry->Depth >= depth)
                {
                    if (phashEntry->Colour == colour)
                    {
                        if (phashEntry->Type == enmHashType.Exact)
                        {
                            m_intHits++;
                            return phashEntry->Result;
                        }

                        if ((phashEntry->Type == enmHashType.Alpha) && (phashEntry->Result <= alpha))
                        {
                            m_intHits++;
                            return alpha;
                        }

                        if ((phashEntry->Type == enmHashType.Beta) && (phashEntry->Result >= beta))
                        {
                            m_intHits++;
                            return beta;
                        }
                    }
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
        /// <param name="depth">
        /// The depth.
        /// </param>
        /// <param name="val">
        /// The val.
        /// </param>
        /// <param name="type">
        /// The type.
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
        public static unsafe void RecordHash(ulong HashCodeA, ulong HashCodeB, int depth, int val, enmHashType type, int From, int To, Move.enmName MoveName, Player.enmColour colour)
        {
            m_intWrites++;
            fixed (HashEntry* phashBase = &m_arrHashEntry[0])
            {
                int intAttempt;
                HashEntry* phashEntry = phashBase;
                phashEntry += (uint)(HashCodeA % m_HashTableSize);
                HashEntry* phashFirst = phashEntry;

                intAttempt = 0;
                while (phashEntry >= phashBase && phashEntry->HashCodeA != 0 && phashEntry->Depth > depth)
                {
                    phashEntry--;
                    intAttempt++;
                    if (intAttempt == HASH_TABLE_SLOT_DEPTH)
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
                    m_intCollisions++;
                    if (phashEntry->HashCodeA != HashCodeA || phashEntry->HashCodeB != HashCodeB)
                    {
                        m_intOverwrites++;
                        phashEntry->WhiteFrom = -1;
                        phashEntry->BlackFrom = -1;
                    }
                }

                phashEntry->HashCodeA = HashCodeA;
                phashEntry->HashCodeB = HashCodeB;
                phashEntry->Result = val;
                phashEntry->Type = type;
                phashEntry->Depth = (sbyte)depth;
                phashEntry->Colour = colour;
                if (From > -1)
                {
                    if (colour == Player.enmColour.White)
                    {
                        phashEntry->WhiteMoveName = MoveName;
                        phashEntry->WhiteFrom = (sbyte)From;
                        phashEntry->WhiteTo = (sbyte)To;
                    }
                    else
                    {
                        phashEntry->BlackMoveName = MoveName;
                        phashEntry->BlackFrom = (sbyte)From;
                        phashEntry->BlackTo = (sbyte)To;
                    }
                }
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
            /// The black from.
            /// </summary>
            public sbyte BlackFrom;

            /// <summary>
            /// The black move name.
            /// </summary>
            public Move.enmName BlackMoveName;

            /// <summary>
            /// The black to.
            /// </summary>
            public sbyte BlackTo;

            /// <summary>
            /// The colour.
            /// </summary>
            public Player.enmColour Colour;

            /// <summary>
            /// The depth.
            /// </summary>
            public sbyte Depth;

            /// <summary>
            /// The hash code a.
            /// </summary>
            public ulong HashCodeA;

            /// <summary>
            /// The hash code b.
            /// </summary>
            public ulong HashCodeB;

            /// <summary>
            /// The result.
            /// </summary>
            public int Result;

            /// <summary>
            /// The type.
            /// </summary>
            public enmHashType Type;

            /// <summary>
            /// The white from.
            /// </summary>
            public sbyte WhiteFrom;

            /// <summary>
            /// The white move name.
            /// </summary>
            public Move.enmName WhiteMoveName;

            /// <summary>
            /// The white to.
            /// </summary>
            public sbyte WhiteTo;

            #endregion
        }
    }
}