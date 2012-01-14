// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerTest.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   This is a test class for PlayerTest and is intended
//   to contain all PlayerTest Unit Tests
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

namespace SharpChess_Tests
{
    #region Using

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SharpChess;
    using SharpChess.Model;
    using SharpChess.Model.AI;

    #endregion

    /// <summary>
    /// This is a test class for PlayerTest and is intended
    ///  to contain all PlayerTest Unit Tests
    /// </summary>
    [TestClass]
    public class PlayerTest
    {
        #region Public Properties

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion

        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext)
        // {
        // }
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup()
        // {
        // }
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize()
        // {
        // }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #region Public Methods

        /// <summary>
        /// A test for RecordPossibleKillerMove
        /// </summary>
        [TestMethod]
        public void RecordPossibleKillerMoveTest()
        {
            int Ply = 10;

            KillerMoves.Clear();

            Piece piece = new Piece(Piece.PieceNames.Bishop, null, 0, 0, Piece.PieceIdentifierCodes.BlackKingsBishop);

            Move move1 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(0), Board.GetSquare(1), null, 0, 20);
            KillerMoves.RecordPossibleKillerMove(Ply, move1);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply) == move1);
            Assert.IsNull(KillerMoves.RetrieveB(Ply));

            Move move2 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(2), Board.GetSquare(3), null, 0, 10);
            KillerMoves.RecordPossibleKillerMove(Ply, move2);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply) == move1);
            Assert.IsTrue(KillerMoves.RetrieveB(Ply) == move2);

            Move move3 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(4), Board.GetSquare(5), null, 0, 15);
            KillerMoves.RecordPossibleKillerMove(Ply, move3);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply) == move1);
            Assert.IsTrue(KillerMoves.RetrieveB(Ply) == move3);

            Move move4 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(6), Board.GetSquare(7), null, 0, 30);
            KillerMoves.RecordPossibleKillerMove(Ply, move4);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply) == move4);
            Assert.IsTrue(KillerMoves.RetrieveB(Ply) == move1);

            // Start again
            KillerMoves.Clear();

            Move move5 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(16), Board.GetSquare(17), null, 0, 200);
            KillerMoves.RecordPossibleKillerMove(Ply, move5);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply) == move5);
            Assert.IsNull(KillerMoves.RetrieveB(Ply));

            Move move6 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(18), Board.GetSquare(19), null, 0, 300);
            KillerMoves.RecordPossibleKillerMove(Ply, move6);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply) == move6);
            Assert.IsTrue(KillerMoves.RetrieveB(Ply) == move5);
        }

        /// <summary>
        /// A test to ensure that if a move that is already a killer move gets a higher score, 
        /// that it updates rather than inserts into killer mover slots.
        /// </summary>
        [TestMethod]
        public void SameKillerMoveWithHigherScoreReplacesSlotEntry()
        {
            const int Ply = 10;

            KillerMoves.Clear();

            Piece piece = new Piece(Piece.PieceNames.Bishop, null, 0, 0, Piece.PieceIdentifierCodes.BlackKingsBishop);
            Move move1 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(0), Board.GetSquare(1), null, 0, 20);

            // Add a move
            KillerMoves.RecordPossibleKillerMove(Ply, move1);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply) == move1);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply).Score == 20);
            Assert.IsNull(KillerMoves.RetrieveB(Ply));

            // Add same move AGAIN, but with higher score. Move should be replaced, using higher score.
            Move move2 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(0), Board.GetSquare(1), null, 0, 30);
            KillerMoves.RecordPossibleKillerMove(Ply, move2);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply) == move2);
            Assert.IsTrue(KillerMoves.RetrieveA(Ply).Score == 30);
            Assert.IsNull(KillerMoves.RetrieveB(Ply));

            // Add same move AGAIN, but with LOWER score. No killer moves should be changed
            Move move3 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(0), Board.GetSquare(1), null, 0, 10);
            KillerMoves.RecordPossibleKillerMove(Ply, move3);
            Assert.IsTrue(Move.MovesMatch(KillerMoves.RetrieveA(Ply), move2));
            Assert.IsTrue(KillerMoves.RetrieveA(Ply).Score == 30);
            Assert.IsNull(KillerMoves.RetrieveB(Ply));

            // Now add a different move, and check it goes in slot B
            Move move4 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(2), Board.GetSquare(3), null, 0, 5);
            KillerMoves.RecordPossibleKillerMove(Ply, move4);
            Assert.IsTrue(Move.MovesMatch(KillerMoves.RetrieveA(Ply), move3));
            Assert.IsTrue(KillerMoves.RetrieveA(Ply).Score == 30);
            Assert.IsTrue(Move.MovesMatch(KillerMoves.RetrieveB(Ply), move4));
            Assert.IsTrue(KillerMoves.RetrieveB(Ply).Score == 5);

            // Now improve score of the move that is in slot B. 
            // Slot B's score should be updated. Slot A should stay the same.
            // Slot's A & B should be SWAPPED.
            Move move5 = new Move(0, 0, Move.MoveNames.Standard, piece, Board.GetSquare(2), Board.GetSquare(3), null, 0, 100);
            KillerMoves.RecordPossibleKillerMove(Ply, move5);
            Assert.IsTrue(Move.MovesMatch(KillerMoves.RetrieveA(Ply), move5));
            Assert.IsTrue(KillerMoves.RetrieveA(Ply).Score == 100);
            Assert.IsTrue(Move.MovesMatch(KillerMoves.RetrieveB(Ply), move3));
            Assert.IsTrue(KillerMoves.RetrieveB(Ply).Score == 30);
        }

        #endregion
    }
}