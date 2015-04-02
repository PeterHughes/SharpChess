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
        public void SquareAttackPerfTest()
        {
            string fen = "3k4/8/8/8/8/3K4/8/8";
            Game_Accessor.NewInternal(fen);
            Square s;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {

                s = Board_Accessor.GetSquare("c2");
                s.PlayerCanAttackSquare(Game_Accessor.PlayerWhite);
            }
            stopwatch.Stop();
            s = Board_Accessor.GetSquare("c2");
            Assert.IsTrue(s.PlayerCanAttackSquare(Game_Accessor.PlayerWhite));
            Debug.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
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
        public void AttackByKnightTest()
        {
            // just test that a knight in center of board can attack squares
            string fen = "8/8/8/8/3N4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "b3", "b5", "b5", "c2", "c6", "e2", "e6", "f3", "f5" };

            Piece knight = Game_Accessor.PlayerWhite.Pieces.Item(0);
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
        public void AttackByKingTest()
        {
            string fen = "8/8/8/8/3K4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "c3", "c4", "c5", "d3", "d5", "e3", "e4", "e5" };

            Piece king = Game_Accessor.PlayerWhite.Pieces.Item(0);
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
        public void AttackByBishopTest()
        {
            string fen = "8/8/8/8/3B4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "a1", "b2", "c3", "e5", "f6", "g7", "h8",
                                    "a7", "b6", "c5", "e3", "f2", "g1"};

            Piece bishop = Game_Accessor.PlayerWhite.Pieces.Item(0);
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
        public void AttackbyRookTest()
        {
            string fen = "8/8/8/8/3R4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "a4", "b4", "c4", "e4", "f4", "g4", "h4",
                                    "d1", "d2", "d3", "d5", "d6", "d7", "d8"};

            Piece rook = Game_Accessor.PlayerWhite.Pieces.Item(0);
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
        public void AttackbyQueenTest()
        {
            string fen = "8/8/8/8/3Q4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "a4", "b4", "c4", "e4", "f4", "g4", "h4",
                                    "d1", "d2", "d3", "d5", "d6", "d7", "d8",
                                    "a1", "b2", "c3", "e5", "f6", "g7", "h8",
                                    "a7", "b6", "c5", "e3", "f2", "g1"
                                    };

            Piece queen = Game_Accessor.PlayerWhite.Pieces.Item(0);
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

        [TestMethod]
        public void AttackbyPawnTest()
        {
            string fen = "8/8/8/8/3P4/8/8/8 w - - 0 1";
            Game_Accessor.NewInternal(fen);
            string[] good_squares = { "c5", "e5"};

            Piece pawn = Game_Accessor.PlayerWhite.Pieces.Item(0);            
            foreach (string s in good_squares)
            {
                bool canAttack = pawn.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsTrue(canAttack);
            }

            string[] bad_squares = { "b3", "b5", "a8", "c2", "c6", "d4", "e6", "f3", "f5" };
            foreach (string s in bad_squares)
            {
                bool canAttack = pawn.CanAttackSquare(Board_Accessor.GetSquare(s));
                Assert.IsFalse(canAttack);
            }
        }

        [TestMethod]
        public void CheapestPieceDefendingThisSquareTest()
        {
            string fen = "1b1k3r/p1q3r1/npp1pp1n/2p5/8/3K4/8/8";
            Game_Accessor.NewInternal(fen);
            Square s = Board_Accessor.GetSquare("g4");
            Piece p = s.CheapestPieceDefendingThisSquare(Game_Accessor.PlayerBlack);
            Assert.IsTrue(p.Top.Name == Piece.PieceNames.Knight);

            // time this
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                p = s.CheapestPieceDefendingThisSquare(Game_Accessor.PlayerBlack);
            }
            stopwatch.Stop();
            Debug.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }

        [TestMethod]
        public void CanPieceTypeAttackSquareTest()
        {
            string fen = "1b1k3r/p1q3r1/npp1pp1n/2p5/8/3K4/8/8";
            Game_Accessor.NewInternal(fen);
            Square s = Board_Accessor.GetSquare("g4");
            Assert.IsTrue(Piece.CanPlayerPieceNameAttackSquare(s,Game_Accessor.PlayerBlack,Piece.PieceNames.Rook));
            Assert.IsTrue(Piece.CanPlayerPieceNameAttackSquare(s,Game_Accessor.PlayerBlack,Piece.PieceNames.Knight));
            Assert.IsFalse(Piece.CanPlayerPieceNameAttackSquare(s,Game_Accessor.PlayerBlack,Piece.PieceNames.Queen));
            Assert.IsFalse(Piece.CanPlayerPieceNameAttackSquare(s,Game_Accessor.PlayerBlack,Piece.PieceNames.Bishop));
            Assert.IsFalse(Piece.CanPlayerPieceNameAttackSquare(s,Game_Accessor.PlayerBlack,Piece.PieceNames.Pawn));
            Assert.IsFalse(Piece.CanPlayerPieceNameAttackSquare(s,Game_Accessor.PlayerBlack,Piece.PieceNames.King));


            s = Board_Accessor.GetSquare("d4");
            Assert.IsTrue(Piece.CanPlayerPieceNameAttackSquare(s, Game_Accessor.PlayerWhite, Piece.PieceNames.King));
            Assert.IsTrue(Piece.CanPlayerPieceNameAttackSquare(s, Game_Accessor.PlayerBlack, Piece.PieceNames.Pawn));
        }
    }
}