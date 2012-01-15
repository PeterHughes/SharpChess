// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameTest.cs" company="SharpChess.com">
//   SharpChess.com
// </copyright>
// <summary>
//   This is a test class for GameTest and is intended
//   to contain all GameTest Unit Tests
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

    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SharpChess.Model;

    #endregion

    /// <summary>
    /// This is a test class for GameTest and is intended
    ///  to contain all GameTest Unit Tests
    /// </summary>
    [TestClass]
    public class GameXTest
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
        /// A test for Move Ordering - Mid game
        /// </summary>
        [TestMethod]
        public void MoveOrdering_MidGame()
        {
            int positions = this.NodeCountTest("r2qk2r/ppp2ppp/2b5/4N3/1b1Pp3/8/PPP1QPPP/R1B2RK1 b k - 1 11", 5);

            // Assert.IsTrue(positions == 52931); Before finding pawn king hash score b-u-g.
            // Assert.IsTrue(positions == 94138); Before all captures in quiesence.
            // Assert.IsTrue(positions == 89310); Before reinstating extensions/reductions
            // Assert.IsTrue(positions == 58090); Dont reduce PV node.
            // Assert.IsTrue(positions == 58090); Before MVV/LVA if SEE returns zero.
            // Assert.IsTrue(positions == 54573); Before history * 100
            // Assert.AreEqual(49641, positions); Less nodes without PVS, but more time WTF!
            // Assert.AreEqual(53728, positions); Before losing capture ignored in quiescense.
            // Assert.AreEqual(50205, positions); Clear history and killer moves at the start of each iteration.
            // Assert.AreEqual(48483, positions); Add LMR, and feature enabling
            // Assert.IsTrue(positions == 33033 || positions == 33055); Moved reduction into own method.
            Assert.IsTrue(positions == 33080 || positions == 33102);
        }

        /// <summary>
        /// A test for Move Ordering - at the start of a game - no moves played.
        /// </summary>
        [TestMethod]
        public void MoveOrdering_Opening()
        {
            int positions = this.NodeCountTest(string.Empty, 5);
            Assert.AreEqual(11226, positions);
        }


        /// <summary>
        /// A test for Move Ordering - in the end game with a posible promotion
        /// </summary>
        [TestMethod]
        public void MoveOrdering_EndGameWithPromotion()
        {
            int positions = this.NodeCountTest("8/2R2pk1/2P5/2r5/1p6/1P2Pq2/8/2K1B3 w - - 5 44", 5);
            Assert.AreEqual(34579, positions);
        }
        
        /// <summary>
        /// A test to confirm that the eval (score) function hasn't unexpectedly changed.
        /// </summary>
        [TestMethod]
        public void ScoreEvalHasntChanged()
        {
            const string Fen = "r2qk2r/ppp2ppp/2b5/4N3/1b1Pp3/8/PPP1QPPP/R1B2RK1 b k - 1 11";
            Game_Accessor.NewInternal(Fen);
            Game_Accessor.MaximumSearchDepth = 3;
            Game_Accessor.ClockFixedTimePerMove = new TimeSpan(0, 10, 0); // 10 minute max
            Game_Accessor.UseRandomOpeningMoves = false;
            Game_Accessor.PlayerToPlay.Brain.Think();

            Assert.AreEqual(-141, Game.PlayerToPlay.Score);
        }
        #endregion

        private int NodeCountTest(string fen, int depth)
        {
            Game_Accessor.NewInternal(fen);
            Game_Accessor.MaximumSearchDepth = depth;
            Game_Accessor.ClockFixedTimePerMove = new TimeSpan(0, 10, 0); // 10 minute max
            Game_Accessor.UseRandomOpeningMoves = false;
            Game_Accessor.PlayerToPlay.Brain.Think();
            // TimeSpan elpased = Game_Accessor.PlayerToPlay.Brain.ThinkingTimeElpased;
            return Game_Accessor.PlayerToPlay.Brain.Search.PositionsSearchedThisTurn;
        }
    }
}