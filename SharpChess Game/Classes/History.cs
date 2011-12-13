using System;

namespace SharpChess
{
	public class History
	{
		static int[,] aHistoryEntryWhite = new int[Board.SQUARE_COUNT,Board.SQUARE_COUNT];
		static int[,] aHistoryEntryBlack = new int[Board.SQUARE_COUNT,Board.SQUARE_COUNT];

		static public void Clear()
		{
			for (int i=0; i<Board.SQUARE_COUNT; i++)
			{
				for (int j=0; j<Board.SQUARE_COUNT; j++)
				{
					aHistoryEntryWhite[i,j] = 0;
					aHistoryEntryBlack[i,j] = 0;
				}
			}
		}

		static public void Record(Player.enmColour colour, int OrdinalFrom, int OrdinalTo, int Value)
		{
			if (colour==Player.enmColour.White)
			{
				aHistoryEntryWhite[OrdinalFrom, OrdinalTo] += Value;
			}
			else 
			{
				aHistoryEntryBlack[OrdinalFrom, OrdinalTo] += Value;
			}
		}

		static public int Retrieve(Player.enmColour colour, int OrdinalFrom, int OrdinalTo)
		{
			return colour==Player.enmColour.White ? aHistoryEntryWhite[OrdinalFrom, OrdinalTo] : aHistoryEntryBlack[OrdinalFrom, OrdinalTo];
		}
	}
}
