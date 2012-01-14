// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovesTest.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   Summary description for UnitTest1
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

    #endregion

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MovesTest
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
        // public static void MyClassInitialize(TestContext testContext) { }
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        #region Public Methods

        /// <summary>
        /// Test that moves are sorted by score
        /// </summary>
        [TestMethod]
        public void CanSortByScore()
        {
        }

        /// <summary>
        /// A test for SortByScore. Tests that moves are sorted in descending order.
        /// </summary>
        [TestMethod]
        public void SortByScoreTest()
        {
            Moves moves = new Moves();
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 0));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 3));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 1));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 3));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 4));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 0));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 6));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 2));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 3));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 8));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 5));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 6));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 7));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 8));
            moves.Add(new Move(0, 0, Move.MoveNames.NullMove, null, null, null, null, 0, 0));

            moves.SortByScore();

            for (int i = 0; i < moves.Count - 1; i++)
            {
                Assert.IsTrue(moves[i].Score >= moves[i + 1].Score);
            }
        }

        #endregion
    }
}