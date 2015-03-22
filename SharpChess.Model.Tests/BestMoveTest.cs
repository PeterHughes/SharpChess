// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BestMoveTest.cs" company="SharpChess.com">
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

namespace SharpChess.Model.Tests
{
    #region Using

    using System;
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SharpChess;
    using SharpChess.Model;

    #endregion

    /// <summary>
    /// This is a test class for GameTest and is intended
    ///   to contain all GameTest Unit Tests
    /// </summary>
    [TestClass]
    public class BestMoveTests
    {
        #region Constants and Fields

        /// <summary>
        ///   Maximum time that a best move test should run for.
        /// </summary>
        private const int MaximumSecondsPerTest = 30;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
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

        #region The Encyclopedia of Chess Middlegames
        [TestMethod]
        public void Ecm001()
        {
            this.BestMoveTest("2q1r1k1/1ppb4/r2p1Pp1/p4n1p/2P1n3/5NPP/PP3Q1K/2BRRB2 w", "f6", "f7", 2);
            // Nodes: 1251 
        }

        [TestMethod]
        public void Ecm022()
        {
            this.BestMoveTest("r1r3k1/p3qpp1/b1P4p/3p4/3Nn3/4P3/P1Q2PPP/1BR1K2R b K", "e7", "b4", 2);
            // Nodes: 1,269
        }

        /// <summary>
        /// ECM Test
        /// </summary>
        [TestMethod]
        public void Ecm026()
        {
            this.BestMoveTest("r1b5/4k3/p7/3p1n2/3Bp3/2P2r1P/PPBK1P2/4R2R w", "d4", "c5", 2);
            // Nodes: 917 
        }

        /// <summary>
        /// ECM Test
        /// </summary>
        [TestMethod]
        public void Ecm027()
        {
            this.BestMoveTest("r4rk1/1b3Npp/p7/1p3Q2/3P4/1B2q3/P5PP/3n1R1K b", "b7", "g2", 3);
            // Nodes: 11,298
        }


 /*
                [TestMethod]
                public void Ecm007()
                {
                    this.BestMoveTest("3rr1k1/pb3pp1/1p1q1b1p/1P2NQ2/3P4/P1NB4/3K1P1P/2R3R1 w", "g1", "g7", 6);
                    // Nodes: 525,713
                    // 678,000
                }

                [TestMethod]
                public void Ecm009()
                {
                    this.BestMoveTest("3r4/1b2k3/1pq1pp2/p3n1pr/2P5/5PPN/PP1N1QP1/R2R2K1 b", "h5", "h3", 8);
                    // Nodes: 2,332,122
                }

                [TestMethod]
                public void Ecm013()
                {
                    this.BestMoveTest("rnb2rk1/pp2np1p/2p2q1b/8/2BPPN2/2P2Q2/PP4PP/R1B2RK1 w", "f4", "d5", 6);
                    // Nodes: 235,000
                }

                [TestMethod]
                public void Ecm015()
                {
                    this.BestMoveTest("r3kb1r/pp2pppp/3q4/3Pn3/6b1/2N1BN2/PP3PPP/R2QKB1R w KQkq", "f3", "e5", 6);
                    // Nodes: 374,000 
                }

                [TestMethod]
                public void Ecm017()
                {
                    this.BestMoveTest("r1b1k3/5p1p/p1p5/3np3/1b2N3/4B3/PPP1BPrP/2KR3R w q", "d1", "d5", 7);
                    // Nodes: 583,000
                }

                [TestMethod]
                public void Ecm020()
                {
                    this.BestMoveTest("1rr1nbk1/5ppp/3p4/1q1PpN2/np2P3/5Q1P/P1BB1PP1/2R1R1K1 w", "c2", "a4", 6);
                    // Nodes: 
                }

                [TestMethod]
                public void Ecm030()
                {
                    this.BestMoveTest("r2q1rk1/p3b1pp/2p5/1pn5/1n1Bp1b1/1P6/PQ1PPP2/2RNKBNR b K", "g4", "e2", 6);
                    // Nodes: 368,722
                }
                #endregion 

                #region Null Move Test-Positions
                // http://chessprogramming.wikispaces.com/Null+Move+Test-Positions

                [TestMethod]
                public void Zugzwang_001()
                {
                    this.BestMoveTest("8/8/p1p5/1p5p/1P5p/8/PPP2K1p/4R1rk w - - 0 1", "e1", "f1", 8);
                    // Nodes: 145,000
                }*/

                [TestMethod]
                public void Zugzwang_002()
                {
                    this.BestMoveTest("1q1k4/2Rr4/8/2Q3K1/8/8/8/8 w - - 0 1", "g5", "h6", 5);
                    // Nodes: 37,500
                }
        /*
                [TestMethod]
                public void Zugzwang_003()
                {
                    this.BestMoveTest("7k/5K2/5P1p/3p4/6P1/3p4/8/8 w - - 0 1", "g4", "g5", 10);
                    // Nodes: 
                }

                [TestMethod]
                public void Zugzwang_004()
                {
                    this.BestMoveTest("8/6B1/p5p1/Pp4kp/1P5r/5P1Q/4q1PK/8 w - - 0 32", "g5", "h6", 5);
                    // Nodes: 
                }

                [TestMethod]
                public void Zugzwang_005()
                {
                    this.BestMoveTest("8/8/1p1r1k2/p1pPN1p1/P3KnP1/1P6/8/3R4 b - - 0 1", "f4", "d5", 9);
                    // Nodes: 2,876,091
                }

                #endregion

                #region Silent but deadly tests
                // http://chessprogramming.wikispaces.com/Silent+but+deadly#cite_note-3
                [TestMethod]
                public void Sbd_001()
                {
                    this.BestMoveTest("1qr3k1/p2nbppp/bp2p3/3p4/3P4/1P2PNP1/P2Q1PBP/1N2R1K1 b", "b8", "c7", 5); // best 5
                    // Nodes: 
                }

        */
                [TestMethod]
                public void Sbd_003()
                {
                    this.BestMoveTest("2b1k2r/2p2ppp/1qp4n/7B/1p2P3/5Q2/PPPr2PP/R2N1R1K b k", "e8", "g8", 4);
                    // Nodes: 15,769
                }
        /*
                [TestMethod]
                public void Sbd_004()
                {
                    this.BestMoveTest("2b5/1p4k1/p2R2P1/4Np2/1P3Pp1/1r6/5K2/8 w", "d6", "d8", 7);
                    // Nodes: 234,270
                }      
        */
        #endregion

        #region Methods

        /// <summary>
        /// Helper method that tests that the nest move is found for a known test position, in the required search depth.
        /// </summary>
        private void BestMoveTest(
            string testPositionFen, string expectedMoveFrom, string expectedMoveTo, int expectedDepth)
        {
            Game_Accessor.NewInternal(testPositionFen);
            Game_Accessor.MaximumSearchDepth = expectedDepth;
            Game_Accessor.ClockFixedTimePerMove = new TimeSpan(0, MaximumSecondsPerTest, 0);
            Game_Accessor.UseRandomOpeningMoves = false;
            Game_Accessor.PlayerToPlay.Brain.Think();

            int positions = Game_Accessor.PlayerToPlay.Brain.Search.PositionsSearchedThisTurn;
            TimeSpan elpased = Game_Accessor.PlayerToPlay.Brain.ThinkingTimeElpased;

            Debug.WriteLine(string.Format("Nodes: {0} ", Game_Accessor.PlayerToPlay.Brain.Search.PositionsSearchedThisTurn));
            Debug.WriteLine(string.Format("Time: {0} ", Game_Accessor.PlayerToPlay.Brain.ThinkingTimeElpased));
            Debug.WriteLine(string.Format("Score: {0} ", Game_Accessor.PlayerToPlay.Brain.PrincipalVariation[0].Score));

            Assert.AreEqual(expectedMoveFrom, Game_Accessor.PlayerToPlay.Brain.PrincipalVariation[0].From.Name , "From move wrong");
            Assert.AreEqual(expectedMoveTo, Game_Accessor.PlayerToPlay.Brain.PrincipalVariation[0].To.Name, "To move wrong");
        }

        #endregion
    }
}