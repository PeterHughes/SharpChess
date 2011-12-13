using System;

namespace SharpChess
{
	public class HashTablePawnKing
	{
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

		private struct HashEntry
		{
			public ulong	HashCodeA;
			public ulong	HashCodeB;
			public int		Points;
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

		public const int UNKNOWN = int.MinValue;
		public static uint m_HashTableSize;
		static HashEntry[] m_arrHashEntry = null;

		public static void Initialise()
		{
			m_HashTableSize = Game.AvailableMegaBytes*3000;
			m_arrHashEntry = new HashEntry[m_HashTableSize];
			Clear();
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
				m_arrHashEntry[intIndex].Points = UNKNOWN;
			}
		}

		public unsafe static int ProbeHash(Player.enmColour colour)
		{
			ulong HashCodeA = Board.HashCodeA;
			ulong HashCodeB = Board.HashCodeB;

			if (colour==Player.enmColour.Black)
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
				phashEntry += ((uint)(HashCodeA % m_HashTableSize));
				
				if (phashEntry->HashCodeA == HashCodeA && phashEntry->HashCodeB == HashCodeB)
				{
					m_intHits++;
					return phashEntry->Points;
				}
			}
			return UNKNOWN;
		}
		
		public unsafe static void RecordHash(int val, Player.enmColour colour)
		{
			ulong HashCodeA = Board.HashCodeA;
			ulong HashCodeB = Board.HashCodeB;

			if (colour==Player.enmColour.Black)
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
				phashEntry += ((uint)(HashCodeA % m_HashTableSize));
				phashEntry->HashCodeA = HashCodeA;
				phashEntry->HashCodeB = HashCodeB;
				phashEntry->Points = val;
			}
			m_intWrites++;
		}

	}
}
