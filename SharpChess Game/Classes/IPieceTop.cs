using System;

namespace SharpChess
{
	public interface IPieceTop
	{
		Piece Base
		{
			get;
		}

		string Abbreviation
		{
			get;
		}

		Piece.enmName Name
		{
			get;
		}
	
		int BasicValue
		{
			get;
		}

		int Value
		{
			get;
		}

		int PositionalPoints
		{
			get;
		}

		int ImageIndex
		{
			get;
		}
	
		bool IsCapturable
		{
			get;
		}

		void GenerateLazyMoves(Moves moves, Moves.enmMovesType movesType);
	}
}
