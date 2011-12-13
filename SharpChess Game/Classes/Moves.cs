using System;
using System.Collections;

namespace SharpChess
{
	public class Moves: IEnumerable
	{
		private Piece m_pieceParent = null;
		private ArrayList m_colMoves = new ArrayList(48);

		public enum enmMovesType
		{
			All
				,	Recaptures
						,	CapturesChecksPromotions
		}

		public Moves()
		{
		}

		public Moves(Piece pieceParent)
		{
			m_pieceParent = pieceParent;
		}

		public IEnumerator GetEnumerator()
		{
			return m_colMoves.GetEnumerator();
		}

		public Piece Parent
		{
			get { return m_pieceParent; }
		}

		public int Count
		{
			get { return m_colMoves.Count; }
		}

		public Move this [int intIndex]
		{
			get
			{
				return (Move)m_colMoves[intIndex];
			}
			set
			{
				m_colMoves[intIndex] = value;
			}
		}

		public Move Last
		{
			get { return m_colMoves.Count>0 ? (Move)m_colMoves[m_colMoves.Count-1] : null; }
		}

		public Move PenultimateForSameSide
		{
			get { return m_colMoves.Count>2 ? (Move)m_colMoves[m_colMoves.Count-3] : null;  }
		}

		public Move Penultimate
		{
			get { return m_colMoves.Count>1 ? (Move)m_colMoves[m_colMoves.Count-2] : null;  }
		}

		public void Add(int TurnNo, int LastMoveTurnNo, Move.enmName Name, Piece Piece, Square From, Square To, Piece pieceCaptured, int pieceCapturedOrdinal, int Score)
		{
			m_colMoves.Add(new Move(TurnNo, LastMoveTurnNo, Name, Piece, From, To, pieceCaptured, pieceCapturedOrdinal, Score));
		}

		public void Add(Move move)
		{
			m_colMoves.Add(move);
		}

		public void Insert(int intIndex, Move move)
		{
			m_colMoves.Insert(intIndex, move);
		}

		public void Remove(Move move)
		{
			m_colMoves.Remove(move);
		}

        public void RemoveAt(int Index)
        {
            m_colMoves.RemoveAt(Index);
        }

        public void RemoveLast()
		{
			m_colMoves.RemoveAt(m_colMoves.Count-1);
		}

        public void Clear()
		{
			m_colMoves.Clear();
		}

		public void Replace(int intIndex, Move moveNew )
		{
			m_colMoves[intIndex] = moveNew;
		}

		public void SortByScore()
		{
//			m_colMoves.Sort();
			QuickSort(m_colMoves, 0, m_colMoves.Count - 1);
		}

		// QuickSort implementation
		private static void QuickSort(ArrayList moveArray, int nLower, int nUpper)
		{
			// Check for non-base case
			if (nLower < nUpper)
			{
				// Split and sort partitions
				int nSplit = Partition (moveArray, nLower, nUpper);
				QuickSort (moveArray, nLower, nSplit - 1);
				QuickSort (moveArray, nSplit + 1, nUpper);
			}
		}
		// QuickSort partition implementation
		private static int Partition (ArrayList moveArray, int nLower, int nUpper)
		{
			// Pivot with first element
			int nLeft = nLower + 1;
			int intPivot = ((Move) moveArray[nLower]).Score;
			int nRight = nUpper;
			// Partition array elements
			Move moveSwap;
			while (nLeft <= nRight)
			{
				// Find item out of place
				while (nLeft <= nRight && ((Move)moveArray[nLeft]).Score >= intPivot )
					nLeft = nLeft + 1;
				while (nLeft <= nRight && ((Move)moveArray[nRight]).Score < intPivot )
					nRight = nRight - 1;
				// Swap values if necessary
				if (nLeft < nRight)
				{
					moveSwap = (Move) moveArray[nLeft];
					moveArray[nLeft] = moveArray[nRight];
					moveArray[nRight] = moveSwap;
					nLeft = nLeft + 1;
					nRight = nRight - 1;
				}
			}
			// Move pivot element
			moveSwap = (Move) moveArray[nLower];
			moveArray[nLower] = moveArray[nRight];
			moveArray[nRight] = moveSwap;
			return nRight;
		}

	}
}
