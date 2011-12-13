using System;

namespace SharpChess
{
	public class HashTable
	{
		public const int HASH_TABLE_SLOT_DEPTH = 3;


		private static int m_intProbes = 0;
		private static int m_intHits = 0;
		private static int m_intWrites = 0;
		private static int m_intCollisions = 0;
		private static int m_intOverwrites = 0;

		public static int Probes
		{
			get {return m_intProbes;}
		}

		public static int Hits
		{
			get {return m_intHits;}
		}

		public static int Writes
		{
			get {return m_intWrites;}
		}

		public static int Collisions
		{
			get { return m_intCollisions; }
		}

		public static int Overwrites
		{
			get { return m_intOverwrites; }
		}

		public enum enmHashType
		{
				Exact
			,	Alpha
			,	Beta
		}

		private struct HashEntry
		{
			public ulong	HashCodeA;
			public ulong	HashCodeB;
			public sbyte	Depth;
			public enmHashType Type;
			public Player.enmColour Colour;
			public int		Result;
			public Move.enmName WhiteMoveName;
			public Move.enmName BlackMoveName;
			public sbyte	WhiteFrom;
			public sbyte	WhiteTo;
			public sbyte	BlackFrom;
			public sbyte	BlackTo;
		}

		public static uint m_HashTableSize;
		public const int UNKNOWN = int.MinValue;
		static HashEntry[] m_arrHashEntry = null;

		public static void Initialise()
		{
			m_HashTableSize = Game.AvailableMegaBytes*8000;
			m_arrHashEntry = new HashEntry[m_HashTableSize];
			Clear();
		}

		public static int SlotsUsed
		{
			get
			{
				int intCounter = 0;

				for (uint intIndex=0; intIndex<m_HashTableSize; intIndex++)
				{
					if (m_arrHashEntry[intIndex].HashCodeA != 0)
					{
						intCounter++;
					}
				}
				return intCounter;
			}
		}

		public static void ResetStats()
		{
			m_intProbes = 0;
			m_intHits = 0;
			m_intWrites = 0;
			m_intCollisions = 0;
			m_intOverwrites = 0;
		}

		public static void Clear()
		{
			ResetStats();
			for (uint intIndex=0; intIndex<m_HashTableSize; intIndex++)
			{
				m_arrHashEntry[intIndex].HashCodeA = 0;
				m_arrHashEntry[intIndex].HashCodeB = 0;
				m_arrHashEntry[intIndex].Depth = sbyte.MinValue;
				m_arrHashEntry[intIndex].WhiteFrom = -1;
				m_arrHashEntry[intIndex].BlackFrom = -1;
			}
		}

		public unsafe static int ProbeHash(ulong HashCodeA, ulong HashCodeB, int depth, int alpha, int beta, Player.enmColour colour)
		{
			m_intProbes++;

			fixed (HashEntry* phashBase = &m_arrHashEntry[0])
			{
				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(HashCodeA % m_HashTableSize));

				int intAttempt = 0;
				while (phashEntry>=phashBase && (phashEntry->HashCodeA!=HashCodeA || phashEntry->HashCodeB!=HashCodeB || phashEntry->Depth < depth) )
				{
					phashEntry--;
					intAttempt++;
					if (intAttempt==HASH_TABLE_SLOT_DEPTH)
					{
						break;
					}
				}

				if (phashEntry<phashBase)
				{
					phashEntry = phashBase;
				}

				if (phashEntry->HashCodeA==HashCodeA && phashEntry->HashCodeB==HashCodeB && phashEntry->Depth >= depth )
				{
					if (phashEntry->Colour==colour)
					{
						if ( phashEntry->Type==enmHashType.Exact )
						{
							m_intHits++;
							return phashEntry->Result;
						}
						if ( (phashEntry->Type==enmHashType.Alpha) && (phashEntry->Result<=alpha))
						{
							m_intHits++;
							return alpha;
						}
						if ( (phashEntry->Type==enmHashType.Beta) && (phashEntry->Result>=beta))
						{
							m_intHits++;
							return beta;
						}
					}
				}
			}
			return UNKNOWN;
		}
		
		public unsafe static void RecordHash(ulong HashCodeA, ulong HashCodeB, int depth, int val, enmHashType type, int From, int To, Move.enmName MoveName, Player.enmColour colour)
		{
			m_intWrites++;
			fixed (HashEntry* phashBase = &m_arrHashEntry[0])
			{
				int intAttempt;
				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(HashCodeA % m_HashTableSize));
				HashEntry* phashFirst = phashEntry;

				intAttempt = 0;
				while (phashEntry>=phashBase && phashEntry->HashCodeA!=0 && phashEntry->Depth > depth)
				{
					phashEntry--;
					intAttempt++;
					if (intAttempt==HASH_TABLE_SLOT_DEPTH)
					{
						break;
					}
				}

				if (phashEntry<phashBase)
				{
					phashEntry = phashBase;
				}

				if (phashEntry->HashCodeA!=0)
				{
					m_intCollisions++;
					if (phashEntry->HashCodeA!=HashCodeA || phashEntry->HashCodeB!=HashCodeB)
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
				if (From>-1)
				{
					if (colour==Player.enmColour.White)
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

		public unsafe static Move ProbeForBestMove(Player.enmColour colour)
		{
			fixed (HashEntry* phashBase = &m_arrHashEntry[0])
			{
				ulong HashCodeA = Board.HashCodeA;
				ulong HashCodeB = Board.HashCodeB;

				HashEntry* phashEntry = phashBase;
				phashEntry += ((uint)(HashCodeA % m_HashTableSize));
				
				int intAttempt = 0;
				while (phashEntry>=phashBase && (phashEntry->HashCodeA!=HashCodeA || phashEntry->HashCodeB!=HashCodeB) )
				{
					phashEntry--;
					intAttempt++;
					if (intAttempt==HASH_TABLE_SLOT_DEPTH)
					{
						break;
					}
				}

				if (phashEntry<phashBase)
				{
					phashEntry = phashBase;
				}

				if (phashEntry->HashCodeA==HashCodeA && phashEntry->HashCodeB==HashCodeB)
				{
					if (colour==Player.enmColour.White)
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
	
	}
}
