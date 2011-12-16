// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Moves.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   The moves.
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
    #region Using

    using System.Collections;

    #endregion

    /// <summary>
    /// The moves.
    /// </summary>
    public class Moves : IEnumerable
    {
        #region Constants and Fields

        /// <summary>
        /// The m_col moves.
        /// </summary>
        private readonly ArrayList m_colMoves = new ArrayList(48);

        /// <summary>
        /// The m_piece parent.
        /// </summary>
        private readonly Piece m_pieceParent;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Moves"/> class.
        /// </summary>
        public Moves()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Moves"/> class.
        /// </summary>
        /// <param name="pieceParent">
        /// The piece parent.
        /// </param>
        public Moves(Piece pieceParent)
        {
            this.m_pieceParent = pieceParent;
        }

        #endregion

        #region Enums

        /// <summary>
        /// The enm moves type.
        /// </summary>
        public enum enmMovesType
        {
            /// <summary>
            /// The all.
            /// </summary>
            All, 

            /// <summary>
            /// The recaptures.
            /// </summary>
            Recaptures, 

            /// <summary>
            /// The captures checks promotions.
            /// </summary>
            CapturesChecksPromotions
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.m_colMoves.Count;
            }
        }

        /// <summary>
        /// Gets Last.
        /// </summary>
        public Move Last
        {
            get
            {
                return this.m_colMoves.Count > 0 ? (Move)this.m_colMoves[this.m_colMoves.Count - 1] : null;
            }
        }

        /// <summary>
        /// Gets Parent.
        /// </summary>
        public Piece Parent
        {
            get
            {
                return this.m_pieceParent;
            }
        }

        /// <summary>
        /// Gets Penultimate.
        /// </summary>
        public Move Penultimate
        {
            get
            {
                return this.m_colMoves.Count > 1 ? (Move)this.m_colMoves[this.m_colMoves.Count - 2] : null;
            }
        }

        /// <summary>
        /// Gets PenultimateForSameSide.
        /// </summary>
        public Move PenultimateForSameSide
        {
            get
            {
                return this.m_colMoves.Count > 2 ? (Move)this.m_colMoves[this.m_colMoves.Count - 3] : null;
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="intIndex">
        /// The int index.
        /// </param>
        public Move this[int intIndex]
        {
            get
            {
                return (Move)this.m_colMoves[intIndex];
            }

            set
            {
                this.m_colMoves[intIndex] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="TurnNo">
        /// The turn no.
        /// </param>
        /// <param name="LastMoveTurnNo">
        /// The last move turn no.
        /// </param>
        /// <param name="Name">
        /// The name.
        /// </param>
        /// <param name="Piece">
        /// The piece.
        /// </param>
        /// <param name="From">
        /// The from.
        /// </param>
        /// <param name="To">
        /// The to.
        /// </param>
        /// <param name="pieceCaptured">
        /// The piece captured.
        /// </param>
        /// <param name="pieceCapturedOrdinal">
        /// The piece captured ordinal.
        /// </param>
        /// <param name="Score">
        /// The score.
        /// </param>
        public void Add(int TurnNo, int LastMoveTurnNo, Move.MoveNames Name, Piece Piece, Square From, Square To, Piece pieceCaptured, int pieceCapturedOrdinal, int Score)
        {
            this.m_colMoves.Add(new Move(TurnNo, LastMoveTurnNo, Name, Piece, From, To, pieceCaptured, pieceCapturedOrdinal, Score));
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        public void Add(Move move)
        {
            this.m_colMoves.Add(move);
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            this.m_colMoves.Clear();
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return this.m_colMoves.GetEnumerator();
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="intIndex">
        /// The int index.
        /// </param>
        /// <param name="move">
        /// The move.
        /// </param>
        public void Insert(int intIndex, Move move)
        {
            this.m_colMoves.Insert(intIndex, move);
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        public void Remove(Move move)
        {
            this.m_colMoves.Remove(move);
        }

        /// <summary>
        /// The remove at.
        /// </summary>
        /// <param name="Index">
        /// The index.
        /// </param>
        public void RemoveAt(int Index)
        {
            this.m_colMoves.RemoveAt(Index);
        }

        /// <summary>
        /// The remove last.
        /// </summary>
        public void RemoveLast()
        {
            this.m_colMoves.RemoveAt(this.m_colMoves.Count - 1);
        }

        /// <summary>
        /// The replace.
        /// </summary>
        /// <param name="intIndex">
        /// The int index.
        /// </param>
        /// <param name="moveNew">
        /// The move new.
        /// </param>
        public void Replace(int intIndex, Move moveNew)
        {
            this.m_colMoves[intIndex] = moveNew;
        }

        /// <summary>
        /// The sort by score.
        /// </summary>
        public void SortByScore()
        {
            // m_colMoves.Sort();
            QuickSort(this.m_colMoves, 0, this.m_colMoves.Count - 1);
        }

        #endregion

        // QuickSort implementation

        // QuickSort partition implementation
        #region Methods

        /// <summary>
        /// The partition.
        /// </summary>
        /// <param name="moveArray">
        /// The move array.
        /// </param>
        /// <param name="nLower">
        /// The n lower.
        /// </param>
        /// <param name="nUpper">
        /// The n upper.
        /// </param>
        /// <returns>
        /// The partition.
        /// </returns>
        private static int Partition(ArrayList moveArray, int nLower, int nUpper)
        {
            // Pivot with first element
            int nLeft = nLower + 1;
            int intPivot = ((Move)moveArray[nLower]).Score;
            int nRight = nUpper;

            // Partition array elements
            Move moveSwap;
            while (nLeft <= nRight)
            {
                // Find item out of place
                while (nLeft <= nRight && ((Move)moveArray[nLeft]).Score >= intPivot)
                {
                    nLeft = nLeft + 1;
                }

                while (nLeft <= nRight && ((Move)moveArray[nRight]).Score < intPivot)
                {
                    nRight = nRight - 1;
                }

                // Swap values if necessary
                if (nLeft < nRight)
                {
                    moveSwap = (Move)moveArray[nLeft];
                    moveArray[nLeft] = moveArray[nRight];
                    moveArray[nRight] = moveSwap;
                    nLeft = nLeft + 1;
                    nRight = nRight - 1;
                }
            }

            // Move pivot element
            moveSwap = (Move)moveArray[nLower];
            moveArray[nLower] = moveArray[nRight];
            moveArray[nRight] = moveSwap;
            return nRight;
        }

        /// <summary>
        /// The quick sort.
        /// </summary>
        /// <param name="moveArray">
        /// The move array.
        /// </param>
        /// <param name="nLower">
        /// The n lower.
        /// </param>
        /// <param name="nUpper">
        /// The n upper.
        /// </param>
        private static void QuickSort(ArrayList moveArray, int nLower, int nUpper)
        {
            // Check for non-base case
            if (nLower < nUpper)
            {
                // Split and sort partitions
                int nSplit = Partition(moveArray, nLower, nUpper);
                QuickSort(moveArray, nLower, nSplit - 1);
                QuickSort(moveArray, nSplit + 1, nUpper);
            }
        }

        #endregion
    }
}