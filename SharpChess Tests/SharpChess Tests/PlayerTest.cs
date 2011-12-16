// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerTest.cs" company="SharpChess.com">
//   Peter Hughes
// </copyright>
// <summary>
//   This is a test class for PlayerTest and is intended
//   to contain all PlayerTest Unit Tests
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

namespace SharpChess_Tests
{
    #region Using

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SharpChess;

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
            int ply = 10;

            Move move1 = new Move(0, 0, Move.MoveNames.Standard, null, null, null, null, 0, 20);
            KillerMoves.RecordPossibleKillerMove(ply, move1);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move1);
            Assert.IsNull(KillerMoves.RetrieveB(ply));

            Move move2 = new Move(0, 0, Move.MoveNames.Standard, null, null, null, null, 0, 10);
            KillerMoves.RecordPossibleKillerMove(ply, move2);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move1);
            Assert.IsTrue(KillerMoves.RetrieveB(ply) == move2);

            Move move3 = new Move(0, 0, Move.MoveNames.Standard, null, null, null, null, 0, 15);
            KillerMoves.RecordPossibleKillerMove(ply, move3);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move1);
            Assert.IsTrue(KillerMoves.RetrieveB(ply) == move3);

            Move move4 = new Move(0, 0, Move.MoveNames.Standard, null, null, null, null, 0, 30);
            KillerMoves.RecordPossibleKillerMove(ply, move4);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move4);
            Assert.IsTrue(KillerMoves.RetrieveB(ply) == move1);

            // Start again
            KillerMoves.Clear();

            Move move5 = new Move(0, 0, Move.MoveNames.Standard, null, null, null, null, 0, 200);
            KillerMoves.RecordPossibleKillerMove(ply, move5);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move5);
            Assert.IsNull(KillerMoves.RetrieveB(ply));

            Move move6 = new Move(0, 0, Move.MoveNames.Standard, null, null, null, null, 0, 300);
            KillerMoves.RecordPossibleKillerMove(ply, move6);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move6);
            Assert.IsTrue(KillerMoves.RetrieveB(ply) == move5);
        }

        #endregion
    }
}