// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieceTest.cs">
//   SharpChess.com
// </copyright>
// <summary>
//   This is a test class for Pieces, Squares, and movement 
// </summary>
// --------------------------------------------------------------------------------------------------------------------


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
    public class PieceTests
    {

        [TestMethod]
        public void SquareAttackTest()
        {
            string fen = "k7/8/8/8/3N4/8/8/K7 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            Square s;
            // white king is in the corner, check that it can attack everything around it
            s = Board_Accessor.GetSquare("b1");
            Assert.IsTrue(s.PlayerCanAttackSquare(Game_Accessor.PlayerWhite));
            s = Board_Accessor.GetSquare("a2");
            Assert.IsTrue(s.PlayerCanAttackSquare(Game_Accessor.PlayerWhite));
            s = Board_Accessor.GetSquare("b2");
            Assert.IsTrue(s.PlayerCanAttackSquare(Game_Accessor.PlayerWhite));

            // white can't attack far away squares
            s = Board_Accessor.GetSquare("h8");
            Assert.IsFalse(s.PlayerCanAttackSquare(Game_Accessor.PlayerWhite));
            s = Board_Accessor.GetSquare("b8");
            Assert.IsFalse(s.PlayerCanAttackSquare(Game_Accessor.PlayerWhite));

            // white knights can attack b3
            s = Board_Accessor.GetSquare("b3");
            Assert.IsTrue(s.PlayerCanAttackSquare(Game_Accessor.PlayerWhite));

            // black king can attack around it
            s = Board_Accessor.GetSquare("a7");
            Assert.IsTrue(s.PlayerCanAttackSquare(Game_Accessor.PlayerBlack));

            s = Board_Accessor.GetSquare("b7");
            Assert.IsTrue(s.PlayerCanAttackSquare(Game_Accessor.PlayerBlack));

            s = Board_Accessor.GetSquare("b8");
            Assert.IsTrue(s.PlayerCanAttackSquare(Game_Accessor.PlayerBlack));

        }
    }
}