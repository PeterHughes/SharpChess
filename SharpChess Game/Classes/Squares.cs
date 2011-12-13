using System;
using System.Collections;

namespace SharpChess
{
	public class Squares: IEnumerable
	{
		private ArrayList m_colSquares = new ArrayList(24);

		public IEnumerator GetEnumerator()
		{
			return m_colSquares.GetEnumerator();
		}

		public Square Item(int intIndex)
		{
			return (Square)m_colSquares[intIndex];
		}

		public int Count
		{
			get { return m_colSquares.Count; }
		}

		public void Add(Square square)
		{
			m_colSquares.Add(square);
		}

		public void Insert(int Ordinal, Square square)
		{
			m_colSquares.Insert(Ordinal, square);
		}

		public int IndexOf(Square square)
		{
			return m_colSquares.IndexOf(square);
		}

		public void Remove(Square square)
		{
			m_colSquares.Remove(square);
		}
	}
}
