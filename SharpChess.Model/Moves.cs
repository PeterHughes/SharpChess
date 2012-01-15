// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Moves.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Holds a list of moves.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region License

// SharpChess
// Copyright (C) 2012 SharpChess.com
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

namespace SharpChess.Model
{
    #region Using

    using System;
    using System.Collections;

    #endregion

    /// <summary>
    /// Holds a list of moves.
    /// </summary>
    public class Moves : IEnumerable
    {
        #region Constants and Fields

        /// <summary>
        /// The m_col moves.
        /// </summary>
        private readonly ArrayList moves = new ArrayList(64);

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
            this.Parent = pieceParent;
        }

        #endregion

        #region Enums

        /// <summary>
        /// Indicates how the move list was generated.
        /// </summary>
        public enum MoveListNames
        {
            /// <summary>
            /// All moves.
            /// </summary>
            All, 

            /// <summary>
            /// Recaptures only.
            /// </summary>
            Recaptures,

            /// <summary>
            /// Captures and promotions.
            /// </summary>
            CapturesPromotions,

            /// <summary>
            /// Captures, checks and promotions.
            /// </summary>
            CapturesChecksPromotions
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of moves contained in the move list.
        /// </summary>
        public int Count
        {
            get
            {
                return this.moves.Count;
            }
        }

        /// <summary>
        /// Gets the Last move.
        /// </summary>
        public Move Last
        {
            get
            {
                return this.moves.Count > 0 ? (Move)this.moves[this.moves.Count - 1] : null;
            }
        }

        /// <summary>
        /// Gets Parent object that is holding this move list.
        /// </summary>
        public Piece Parent { get; private set; }

        /// <summary>
        /// Gets Penultimate move in this list.
        /// </summary>
        public Move Penultimate
        {
            get
            {
                return this.moves.Count > 1 ? (Move)this.moves[this.moves.Count - 2] : null;
            }
        }

        /// <summary>
        /// Gets penultimate move in this list For the same side.
        /// </summary>
        public Move PenultimateForSameSide
        {
            get
            {
                return this.moves.Count > 2 ? (Move)this.moves[this.moves.Count - 3] : null;
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// Returns the move specified by the index.
        /// </summary>
        /// <param name="intIndex">
        /// The index value.
        /// </param>
        /// <returns>
        /// The move at the specified index position.
        /// </returns>
        public Move this[int intIndex]
        {
            get
            {
                return (Move)this.moves[intIndex];
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.moves[intIndex] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="turnNo">
        /// The turn no.
        /// </param>
        /// <param name="lastMoveTurnNo">
        /// The last move turn no.
        /// </param>
        /// <param name="moveName">
        /// The move name.
        /// </param>
        /// <param name="piece">
        /// The piece moving.
        /// </param>
        /// <param name="from">
        /// The square the peice is moving from.
        /// </param>
        /// <param name="to">
        /// The square the peice is moving to.
        /// </param>
        /// <param name="pieceCaptured">
        /// The piece being captured.
        /// </param>
        /// <param name="pieceCapturedOrdinal">
        /// Ordinal position of the piece being captured.
        /// </param>
        /// <param name="score">
        /// The positional score.
        /// </param>
        public void Add(int turnNo, int lastMoveTurnNo, Move.MoveNames moveName, Piece piece, Square from, Square to, Piece pieceCaptured, int pieceCapturedOrdinal, int score)
        {
            this.moves.Add(new Move(turnNo, lastMoveTurnNo, moveName, piece, from, to, pieceCaptured, pieceCapturedOrdinal, score));
        }

        /// <summary>
        /// Add a new move to this list.
        /// </summary>
        /// <param name="move">
        /// The move.
        /// </param>
        public void Add(Move move)
        {
            this.moves.Add(move);
        }

        /// <summary>
        /// Clear all moves in the list.
        /// </summary>
        public void Clear()
        {
            this.moves.Clear();
        }

        /// <summary>
        /// Gest the enumerator for this list.
        /// </summary>
        /// <returns>
        /// The enumerator for this list.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return this.moves.GetEnumerator();
        }

        /// <summary>
        /// Insert a move into this list at the specified index position.
        /// </summary>
        /// <param name="intIndex">
        /// The index position.
        /// </param>
        /// <param name="move">
        /// The move to insert.
        /// </param>
        public void Insert(int intIndex, Move move)
        {
            this.moves.Insert(intIndex, move);
        }

        /// <summary>
        /// Remove a move from this list.
        /// </summary>
        /// <param name="move">
        /// The move to remove.
        /// </param>
        public void Remove(Move move)
        {
            this.moves.Remove(move);
        }

        /// <summary>
        /// Remove the move at the specified index from this list position.
        /// </summary>
        /// <param name="index">
        /// The index position.
        /// </param>
        public void RemoveAt(int index)
        {
            this.moves.RemoveAt(index);
        }

        /// <summary>
        /// The remove last move from this list.
        /// </summary>
        public void RemoveLast()
        {
            this.moves.RemoveAt(this.moves.Count - 1);
        }

        /// <summary>
        /// Replace the move at ths specified index position with the supplied move.
        /// </summary>
        /// <param name="intIndex">
        /// The index position to replace.
        /// </param>
        /// <param name="moveNew">
        /// The new move.
        /// </param>
        public void Replace(int intIndex, Move moveNew)
        {
            this.moves[intIndex] = moveNew;
        }

        /// <summary>
        /// Sort this list by score.
        /// </summary>
        public void SortByScore()
        {
            // m_colMoves.Sort();
            QuickSort(this.moves, 0, this.moves.Count - 1);
        }

        #endregion

        // QuickSort implementation

        // QuickSort partition implementation
        #region Methods

        /// <summary>
        /// Partition method of QuickSort function.
        /// </summary>
        /// <param name="moveArray">
        /// The move array.
        /// </param>
        /// <param name="lower">
        /// The n lower.
        /// </param>
        /// <param name="upper">
        /// The n upper.
        /// </param>
        /// <returns>
        /// The partition.
        /// </returns>
        private static int Partition(ArrayList moveArray, int lower, int upper)
        {
            // Pivot with first element
            int left = lower + 1;
            int pivot = ((Move)moveArray[lower]).Score;
            int right = upper;

            // Partition array elements
            Move moveSwap;
            while (left <= right)
            {
                // Find item out of place
                while (left <= right && ((Move)moveArray[left]).Score >= pivot)
                {
                    left = left + 1;
                }

                while (left <= right && ((Move)moveArray[right]).Score < pivot)
                {
                    right = right - 1;
                }

                // Swap values if necessary
                if (left < right)
                {
                    moveSwap = (Move)moveArray[left];
                    moveArray[left] = moveArray[right];
                    moveArray[right] = moveSwap;
                    left = left + 1;
                    right = right - 1;
                }
            }

            // Move pivot element
            moveSwap = (Move)moveArray[lower];
            moveArray[lower] = moveArray[right];
            moveArray[right] = moveSwap;
            return right;
        }

        /// <summary>
        /// Quicksort an array .
        /// </summary>
        /// <param name="moveArray">
        /// Array of moves.
        /// </param>
        /// <param name="lower">
        /// Lower bound.
        /// </param>
        /// <param name="upper">
        /// Upper bound
        /// </param>
        private static void QuickSort(ArrayList moveArray, int lower, int upper)
        {
            // Check for non-base case
            if (lower < upper)
            {
                // Split and sort partitions
                int split = Partition(moveArray, lower, upper);
                QuickSort(moveArray, lower, split - 1);
                QuickSort(moveArray, split + 1, upper);
            }
        }

        #endregion
    }
}