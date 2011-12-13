using System;

namespace SharpChess
{
	/// <summary>
	/// Summary description for KillerMoves.
	/// </summary>
	public class KillerMoves
	{
		private static Move[] m_arrmoveA = new Move[64];
		private static Move[] m_arrmoveB = new Move[64];

		static KillerMoves()
		{
			Clear();
		}

		public static void Clear()
		{
			for (int intIndex=0; intIndex<64; intIndex++)
			{
				m_arrmoveA[intIndex] = null;
				m_arrmoveB[intIndex] = null;
			}
		}

		public static void AssignA(int depth, Move move)
		{
			m_arrmoveA[depth+32] = move;
		}

		public static Move RetrieveA(int depth)
		{
			return m_arrmoveA[depth+32];
		}

		public static void AssignB(int depth, Move move)
		{
			m_arrmoveB[depth+32] = move;
		}

		public static Move RetrieveB(int depth)
		{
			return m_arrmoveB[depth+32];
		}

        /// <summary>
        /// Adds the move made to the appropriate killer move slot, if it's better than the currnet killer moves
        /// </summary>
        /// <param name="ply">Search depth</param>
        /// <param name="moveMade">Move to be added</param>
        /// <param name="moveKillerA">Killer Move Slot A</param>
        /// <param name="moveKillerB">Killer Move Slot B</param>
        static public void RecordPossibleKillerMove(int ply, Move moveMade)
        {
            Move moveKillerA;
            Move moveKillerB;
            bool blnAssignedA = false; // Have we assign Slot A?

            moveKillerA = KillerMoves.RetrieveA(ply); // Get slot A move
            if (moveKillerA == null)
            {
                // Slot A is blank, so put anything in it.
                KillerMoves.AssignA(ply, moveMade);
                blnAssignedA = true;
            }
            else if (moveMade.Score > moveKillerA.Score)
            {
                // Score is better than Slot A, so
                // transfer move in Slot A to Slot B...
                KillerMoves.AssignB(ply, moveKillerA);
                // record move is Slot A
                KillerMoves.AssignA(ply, moveMade);
                blnAssignedA = true;
            }

            // If the move wasn't assigned to Slot A, then see if it is good enough to go in Slot B
            if (!blnAssignedA)
            {
                moveKillerB = KillerMoves.RetrieveB(ply);
                // Slot B is empty, so put anything in!
                if (moveKillerB == null)
                {
                    KillerMoves.AssignB(ply, moveMade);
                }
                else if (moveMade.Score > moveKillerB.Score)
                {
                    // Score is better than Slot B, so
                    // record move is Slot B
                    KillerMoves.AssignB(ply, moveMade);
                }

            }
        }

	}
}
