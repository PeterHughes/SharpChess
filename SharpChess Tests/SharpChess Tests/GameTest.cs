using SharpChess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SharpChess_Tests
{
    
    
    /// <summary>
    ///This is a test class for GameTest and is intended
    ///to contain all GameTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GameTest
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
        ///A test for MOve Ordering
        ///</summary>
        [TestMethod()]
        [DeploymentItem("SharpChess2.exe")]
        public void MoveOrdering()
        {
            string strFEN = "r2qk2r/ppp2ppp/2b5/4N3/1b1Pp3/8/PPP1QPPP/R1B2RK1 b k - 1 11";
            Game_Accessor.New_Internal(strFEN);
            Game_Accessor.MaximumSearchDepth = 5;
            Game_Accessor.ClockFixedTimePerMove = new TimeSpan(0, 10, 0); // 10 minute max
            Game_Accessor.PlayerToPlay.Think();
            int positions = Game_Accessor.PlayerToPlay.PositionsSearched;

            TimeSpan elpased = Game_Accessor.PlayerToPlay.ThinkingTimeElpased;

            Assert.IsTrue(positions <= 52931);
            
        }
    }
}
