using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpChess;

namespace SharpChess_Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MovesTest
    {
        public MovesTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Test that moves are sorted by score
        /// </summary>
        [TestMethod]
        public void CanSortByScore()
        {

        }

        /// <summary>
        ///A test for SortByScore. Tests that moves are sorted in descending order.
        ///</summary>
        [TestMethod()]
        public void SortByScoreTest()
        {
            Moves moves = new Moves();
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 0));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 3));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 1));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 3));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 4));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 0));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 6));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 2));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 3));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 8));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 5));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 6));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 7));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 8));
            moves.Add(new Move(0, 0, Move.enmName.NullMove, null, null, null, null, 0, 0));

            moves.SortByScore();

            for (int i = 0; i < moves.Count - 1; i++)
            {
                Assert.IsTrue(moves[i].Score >= moves[i + 1].Score);
            }
        }
    }
}
