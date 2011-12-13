using System;
using System.Collections;

namespace SharpChess
{
	public class Pieces: IEnumerable
	{
		private Player m_player;
		private ArrayList m_colPieces = new ArrayList();

		public Pieces(Player player)
		{
			m_player = player;
		}

		public Player Player
		{
			get { return m_player; }
		}

		public IEnumerator GetEnumerator()
		{
			return m_colPieces.GetEnumerator();
		}

		public Piece Item(int intIndex)
		{
			return (Piece)m_colPieces[intIndex];
		}

		public int Count
		{
			get { return m_colPieces.Count; }
		}

		public void Add(Piece piece)
		{
			m_colPieces.Add(piece);
		}

		public void Insert(int Ordinal, Piece piece)
		{
			m_colPieces.Insert(Ordinal, piece);
		}

		public int IndexOf(Piece piece)
		{
			return m_colPieces.IndexOf(piece);
		}

		public void Remove(Piece piece)
		{
			m_colPieces.Remove(piece);
		}

		public void SortByScore()
		{
			m_colPieces.Sort();
		}

		public object Clone()
		{
			return m_colPieces.Clone();
		}

	}
}
