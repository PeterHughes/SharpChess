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

        [TestMethod]
        public void SquareAttackByBishopTest()
        {
            string fen = "8/5k2/1p3P2/8/3B4/8/8/K7 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            Square square;

            string[] good_squares = { "b6", "c5", "e5", "f6", "g7", "e7", "b1", "b2", "a2" };
            foreach (string s in good_squares)
            {
                square = Board_Accessor.GetSquare(s);
                Assert.IsTrue(square.PlayerCanAttackSquare(Game_Accessor.PlayerWhite));
            }

            string[] bad_squares = { "a6", "c4", "e2", "e1", "f8", "h8"};
            foreach (string s in good_squares)
            {
                square = Board_Accessor.GetSquare(s);
                Assert.IsTrue(square.PlayerCanAttackSquare(Game_Accessor.PlayerWhite));
            }

        }

        [TestMethod]
        public void KnightAttackTest()
        {
            // just test that a knight in center of board can attack squares
            string fen = "8/8/8/8/3N4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "b3", "b5", "b5", "c2", "c6", "e2", "e6", "f3", "f5" };

            PieceKnight knight = (PieceKnight)Game_Accessor.PlayerWhite.Pieces.Item(0).Top;
            foreach (string s in good_squares)
            {
                bool canAttack = knight.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsTrue(canAttack);
            }

            string[] bad_squares = { "a3", "b6", "b7", "c1", "c5", "e1", "e8", "f4", "h5" };
            foreach (string s in bad_squares)
            {
                bool canAttack = knight.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsFalse(canAttack);
            }

        }

        [TestMethod]
        public void KingAttackTest()
        {
            // just test that a king in center of board can attack squares
            string fen = "8/8/8/8/3K4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "c3", "c4", "c5", "d3", "d5", "e3", "e4", "e5" };

            PieceKing king = (PieceKing)Game_Accessor.PlayerWhite.Pieces.Item(0).Top;
            foreach (string s in good_squares)
            {
                bool canAttack = king.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsTrue(canAttack);
            }

            string[] bad_squares = { "b3", "b5", "a8", "c2", "c6", "d4", "e6", "f3", "f5" };
            foreach (string s in bad_squares)
            {
                bool canAttack = king.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsFalse(canAttack);
            }
        }


        [TestMethod]
        public void BishopAttackTest()
        {
            // just test that a king in center of board can attack squares
            string fen = "8/8/8/8/3B4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "a1", "b2", "c3", "e5", "f6", "g7", "h8",
                                    "a7", "b6", "c5", "e3", "f2", "g1"};

            PieceBishop bishop = (PieceBishop)Game_Accessor.PlayerWhite.Pieces.Item(0).Top;
            foreach (string s in good_squares)
            {
                bool canAttack = bishop.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsTrue(canAttack);
            }

            string[] bad_squares = { "b3", "b5", "a8", "c2", "c6", "d4", "e6", "f3", "f5" };
            foreach (string s in bad_squares)
            {
                bool canAttack = bishop.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsFalse(canAttack);
            }
        }

        [TestMethod]
        public void RookAttackTest()
        {
            // just test that a king in center of board can attack squares
            string fen = "8/8/8/8/3R4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "a4", "b4", "c4", "e4", "f4", "g4", "h4",
                                    "d1", "d2", "d3", "d5", "d6", "d7", "d8"};

            PieceRook rook = (PieceRook)Game_Accessor.PlayerWhite.Pieces.Item(0).Top;
            foreach (string s in good_squares)
            {
                bool canAttack = rook.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsTrue(canAttack);
            }

            string[] bad_squares = { "b3", "b5", "a8", "c2", "c6", "d4", "e6", "f3", "f5" };
            foreach (string s in bad_squares)
            {
                bool canAttack = rook.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsFalse(canAttack);
            }
        }

        [TestMethod]
        public void QueenAttackTest()
        {
            // just test that a king in center of board can attack squares
            string fen = "8/8/8/8/3Q4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "a4", "b4", "c4", "e4", "f4", "g4", "h4",
                                    "d1", "d2", "d3", "d5", "d6", "d7", "d8",
                                    "a1", "b2", "c3", "e5", "f6", "g7", "h8",
                                    "a7", "b6", "c5", "e3", "f2", "g1"
                                    };

            PieceQueen queen = (PieceQueen)Game_Accessor.PlayerWhite.Pieces.Item(0).Top;
            foreach (string s in good_squares)
            {
                bool canAttack = queen.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsTrue(canAttack);
            }

            string[] bad_squares = { "b3", "b5", "a8", "c2", "c6", "d4", "e6", "f3", "f5"};
            foreach (string s in bad_squares)
            {
                bool canAttack = queen.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsFalse(canAttack);
            }
        }

    }
}