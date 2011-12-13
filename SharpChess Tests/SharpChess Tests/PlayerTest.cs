using SharpChess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SharpChess_Tests
{
    
    
    /// <summary>
    ///This is a test class for PlayerTest and is intended
    ///to contain all PlayerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PlayerTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for RecordPossibleKillerMove
        ///</summary>
        [TestMethod()]
        public void RecordPossibleKillerMoveTest()
        {
            int ply = 10; 

            Move move1 = new Move(0, 0, Move.enmName.Standard, null, null, null, null, 0, 20);
            KillerMoves.RecordPossibleKillerMove(ply, move1);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move1);
            Assert.IsNull(KillerMoves.RetrieveB(ply));

            Move move2 = new Move(0, 0, Move.enmName.Standard, null, null, null, null, 0, 10);
            KillerMoves.RecordPossibleKillerMove(ply, move2);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move1);
            Assert.IsTrue(KillerMoves.RetrieveB(ply) == move2);

            Move move3 = new Move(0, 0, Move.enmName.Standard, null, null, null, null, 0, 15);
            KillerMoves.RecordPossibleKillerMove(ply, move3);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move1);
            Assert.IsTrue(KillerMoves.RetrieveB(ply) == move3);

            Move move4 = new Move(0, 0, Move.enmName.Standard, null, null, null, null, 0, 30);
            KillerMoves.RecordPossibleKillerMove(ply, move4);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move4);
            Assert.IsTrue(KillerMoves.RetrieveB(ply) == move1);

            // Start again
            KillerMoves.Clear();

            Move move5 = new Move(0, 0, Move.enmName.Standard, null, null, null, null, 0, 200);
            KillerMoves.RecordPossibleKillerMove(ply, move5);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move5);
            Assert.IsNull(KillerMoves.RetrieveB(ply));

            Move move6 = new Move(0, 0, Move.enmName.Standard, null, null, null, null, 0, 300);
            KillerMoves.RecordPossibleKillerMove(ply, move6);
            Assert.IsTrue(KillerMoves.RetrieveA(ply) == move6);
            Assert.IsTrue(KillerMoves.RetrieveB(ply) == move5);
        }
    }
}
