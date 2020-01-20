using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;
using TicTacToeMatch.Interfaces;
using TicTacToeMatch.Internals;

namespace TicTacToeMatch.Tests.Internals
{
    [ExcludeFromCodeCoverage]
    [Category("UnitTest")]
    public class AiMoveTests
    {
        [Test]
        public void GetAiEasyPointIndex_ButtonListIsNull_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => this.CreateInstance().GetAiEasyPointIndex(null), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetAiEasyPointIndex_ButtonListIsEmpty_ThrowArgumentOutOfRangeException()
        {
            Assert.That(() => this.CreateInstance().GetAiEasyPointIndex(new List<Button>()), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetAiEasyPointIndex_InRangeOfZeroAndThree_IsInRange()
        {
            List<Button> foo = new List<Button>
            {
                new Button(),
                new Button(),
                new Button()
            };

            Int32 actual = this.CreateInstance().GetAiEasyPointIndex(foo);

            Assert.That(() => actual, Is.InRange(0, foo.Count));
        }

        [Test]
        public void GetMovesLeft_TicTacToeMatrixIsNull_ThrowArgumentNullException()
        {
            Assert.That(() => this.CreateInstance().GetMovesLeft(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetMovesLeft_TicTaxToeMatrixBoardIsEqualToPlayerTypeUnassigned_ReturnFalse()
        {
            MatrixAlgorithm matrixAlgorithm = this.CreateMatrixInstance();
            matrixAlgorithm.Board[0, 0] = PlayerType.Unassigned;

            Boolean actual = this.CreateInstance().GetMovesLeft(matrixAlgorithm);

            Assert.That(() => actual, Is.False);
        }

        [Test]
        public void GetMovesLeft_TicTaxToeMatrixBoardIsNotEqualToPlayerTypeUnassigned_ReturnTrue()
        {
            MatrixAlgorithm matrixAlgorithm = this.CreateMatrixInstance();

            for (Int32 x = 0; x < 3; x++)
            {
                for (Int32 y = 0; y < 3; y++)
                {
                    matrixAlgorithm.Board[x, y] = PlayerType.X;
                }
            }

            Boolean actual = this.CreateInstance().GetMovesLeft(matrixAlgorithm);

            Assert.That(() => actual, Is.True);
        }

        [Test]
        public void GetMiniMaxMedium_TicTaxToeMatrixIsNull_ThrowArgumentNullException()
        {
            Assert.That(() => this.CreateInstance().GetMiniMaxMedium(null, 0, true), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetMiniMaxMedium_TicTacToeMatrixBoardIsNull_ThrowArgumentNullException()
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = null;

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Assert.That(() => this.CreateInstance().GetMiniMaxMedium(algorithm.Object, 0, true), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        [TestCase(PlayerType.X)]
        [TestCase(PlayerType.O)]
        public void GetMiniMaxMedium_ScoreIsEqualToEvaluateMediumMoves_ReturnAsExpected(PlayerType player)
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {player,player,player},
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.Unassigned}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(player);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Int32 actual = this.CreateInstance().GetMiniMaxMedium(algorithm.Object, 0, true);
            Int32 expected = this.CreateInstance().EvaluateAiMediumMoves(board);

            Assert.That(() => actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetMiniMaxMedium_GetMovesLeft_ReturnScore()
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.X,PlayerType.O,PlayerType.X},
                {PlayerType.X,PlayerType.O,PlayerType.X},
                {PlayerType.O,PlayerType.X,PlayerType.O}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(PlayerType.O);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Int32 actual = this.CreateInstance().GetMiniMaxMedium(algorithm.Object, 0, true);
            Int32 expected = this.CreateInstance().EvaluateAiMediumMoves(board);

            Assert.That(() => actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, -1)]
        public void GetMiniMaxMedium_IsMaxIsTrue_ReturnBest(Int32 depth, Int32 expected)
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.X,PlayerType.X,PlayerType.O},
                {PlayerType.O,PlayerType.X,PlayerType.Unassigned},
                {PlayerType.X,PlayerType.O,PlayerType.O}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(PlayerType.O);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Int32 actual = this.CreateInstance().GetMiniMaxMedium(algorithm.Object, depth, true);

            Assert.That(() => actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, -11)]
        public void GetMiniMaxMedium_IsMaxIsfalse_ReturnBest(Int32 depth, Int32 expected)
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.X,PlayerType.X,PlayerType.O},
                {PlayerType.O,PlayerType.X,PlayerType.Unassigned},
                {PlayerType.X,PlayerType.O,PlayerType.O}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(PlayerType.O);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Int32 actual = this.CreateInstance().GetMiniMaxMedium(algorithm.Object, depth, false);

            Assert.That(() => actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetBestAiMoveMedium_ScoreGreaterThanBestScpre_ReturnTrue()
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.X,PlayerType.X,PlayerType.O},
                {PlayerType.O,PlayerType.X,PlayerType.Unassigned},
                {PlayerType.X,PlayerType.O,PlayerType.O}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(PlayerType.O);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Move actual = this.CreateInstance().GetBestAiMoveMedium(algorithm.Object);

            Move expected = new Move
            {
                Row = 1,
                Col = 2
            };

            Assert.That(() => actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetAiMediumPointIndex_BestMoveIsPointIndex_ReturnTrue()
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.X,PlayerType.X,PlayerType.O},
                {PlayerType.O,PlayerType.X,PlayerType.Unassigned},
                {PlayerType.X,PlayerType.O,PlayerType.O}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(PlayerType.O);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            PointIndex expected = this.CreateInstance().GetAiMediumPointIndex(algorithm.Object);

            Move bestMove = this.CreateInstance().GetBestAiMoveMedium(algorithm.Object);

            PointIndex actual = new PointIndex
            {
                X = bestMove.Row,
                Y = bestMove.Col
            };

            Assert.That(() => actual, Is.EqualTo(expected));
        }

        [Test]
        public void EvaluateAiMediumMoves_XDimensionIsNotThree_ThrowArgumentOutOfRangeException()
        {
            PlayerType[,] board = new PlayerType[2, 3]
            {
                {PlayerType.X,PlayerType.X,PlayerType.X},
                {PlayerType.X,PlayerType.X,PlayerType.X},
            };

            Assert.That(() => this.CreateInstance().EvaluateAiMediumMoves(board), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void EvaluateAiMediumMoves_YDimensionIsNotThree_ThrowArgumentOutOfRangeException()
        {
            PlayerType[,] board = new PlayerType[3, 2]
            {
                {PlayerType.X,PlayerType.X},
                {PlayerType.X,PlayerType.X},
                {PlayerType.X,PlayerType.X},
            };

            Assert.That(() => this.CreateInstance().EvaluateAiMediumMoves(board), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(PlayerType.O, -10)]
        [TestCase(PlayerType.X, 10)]
        public void EvaluateAiMediumMoves_XWinsWithColumns_ReturnExpected(PlayerType player, Int32 expected)
        {
            PlayerType[,] board = new PlayerType[3, 3]
            {
                {player,PlayerType.X,PlayerType.Unassigned},
                {player,PlayerType.Unassigned,PlayerType.O},
                {player,PlayerType.X,PlayerType.Unassigned}
            };

            Assert.That(() => this.CreateInstance().EvaluateAiMediumMoves(board), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(PlayerType.O, -10)]
        [TestCase(PlayerType.X, 10)]
        public void EvaluateAiMediumMoves_PlayerWinsLeftToRightDiagonals_ReturnExpected(PlayerType player, Int32 expected)
        {
            PlayerType[,] board = new PlayerType[3, 3]
            {
                {player,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.Unassigned,player,PlayerType.Unassigned},
                {PlayerType.Unassigned,PlayerType.Unassigned,player}
            };

            Assert.That(() => this.CreateInstance().EvaluateAiMediumMoves(board), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(PlayerType.O, -10)]
        [TestCase(PlayerType.X, 10)]
        public void EvaluateAiMediumMoves_PlayerWinsRightToLeftDiagonals_ReturnExpected(PlayerType player, Int32 expected)
        {
            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.Unassigned,PlayerType.Unassigned,player},
                {PlayerType.Unassigned,player,PlayerType.Unassigned},
                {player,PlayerType.Unassigned,PlayerType.Unassigned}
            };

            Assert.That(() => this.CreateInstance().EvaluateAiMediumMoves(board), Is.EqualTo(expected));
        }

        [Test]
        public void GetMovesLeftAHard_TicTacToeMatrixIsNull_ThrowArgumentNullException()
        {
            Assert.That(() => this.CreateInstance().GetMovesLeftAiHard(null, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetMovesLeftAHard_HardMoveIsLeftEmptyBoard_ReturnFalse()
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] emptyBoard = new PlayerType[3, 3]
            {
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.Unassigned}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(emptyBoard);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(PlayerType.O);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Boolean actual = this.CreateInstance().GetMovesLeftAiHard(algorithm.Object, emptyBoard);

            Assert.That(() => actual, Is.False);
        }

        [Test]
        public void GetMovesLeftAHard_HardMoveIsLeftFullBoard_ReturnTrue()
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.X,PlayerType.X,PlayerType.O},
                {PlayerType.O,PlayerType.X,PlayerType.X},
                {PlayerType.X,PlayerType.O,PlayerType.O}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(PlayerType.O);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Boolean actual = this.CreateInstance().GetMovesLeftAiHard(algorithm.Object, board);

            Assert.That(() => actual, Is.True);
        }

        [Test]
        public void GetMiniMaxAiHard_TicTacToeMatrixIsNull_ThrowArgumentNullException()
        {
            Assert.That(() => this.CreateInstance().GetMiniMaxAiHard(null, PlayerType.O, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        [TestCase(PlayerType.X, -10)]
        [TestCase(PlayerType.O, 10)]
        public void GetMiniMaxAiHard_EvaluateHardModeForPlayer_ReturnExpected(PlayerType player, Int32 expected)
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {player,player,player},
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.Unassigned}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(player);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Int32 actual = this.CreateInstance().GetMiniMaxAiHard(board, player, algorithm.Object);
            Assert.That(() => actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetMiniMaxAiHard_EvaluateHardModeForX_ReturnZero()
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.X,PlayerType.X,PlayerType.O},
                {PlayerType.O,PlayerType.X,PlayerType.X},
                {PlayerType.X,PlayerType.O,PlayerType.O}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(PlayerType.O);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Int32 actual = this.CreateInstance().GetMiniMaxAiHard(board, PlayerType.O, algorithm.Object);
            Assert.That(() => actual, Is.EqualTo(0));
        }

        [Test]
        [TestCase(PlayerType.X, 0)]
        [TestCase(PlayerType.O, 0)]
        public void GetMiniMaxAiHard_StateIsX_NextMoveScoreIsEqualToResult(PlayerType player, Int32 expected)
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.O,PlayerType.X,PlayerType.O},
                {PlayerType.X,PlayerType.X,PlayerType.Unassigned},
                {PlayerType.X,PlayerType.O,PlayerType.O}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(player);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            Int32 actual = this.CreateInstance().GetMiniMaxAiHard(board, player, algorithm.Object);
            Assert.That(() => actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetAiHardPointIndex_TicTacToeMatrixIsNull_ThrowArgumentNullException()
        {
            Assert.That(() => this.CreateInstance().GetAiHardPointIndex(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GetAiHardPointIndex_MiniMaxToPointIndex_()
        {
            Mock<IMatrixAlgorithm> algorithm = new Mock<IMatrixAlgorithm>();

            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.O,PlayerType.X,PlayerType.O},
                {PlayerType.X,PlayerType.X,PlayerType.Unassigned},
                {PlayerType.X,PlayerType.O,PlayerType.O}
            };

            algorithm
                .Setup(x => x.Board)
                .Returns(board);
            algorithm
                .Setup(x => x.CurrentTurn)
                .Returns(PlayerType.O);
            algorithm
                .Setup(x => x.BoardSize)
                .Returns(3);

            AiMove localInstance = this.CreateInstance();

            PointIndex actual = localInstance.GetAiHardPointIndex(algorithm.Object);

            PointIndex expected = localInstance.BestPointMove;

            Assert.That(() => actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(PlayerType.O, true)]
        [TestCase(PlayerType.X, true)]
        [TestCase(PlayerType.Unassigned, false)]
        public void EvaluateAiHardMode_PlayerWinsLeftToRightDiagonals_ReturnExpected(PlayerType player, Boolean expected)
        {
            PlayerType[,] board = new PlayerType[3, 3]
            {
                {player,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.Unassigned,player,PlayerType.Unassigned},
                {PlayerType.Unassigned,PlayerType.Unassigned,player}
            };

            Assert.That(() => this.CreateInstance().EvaluateAiHardMode(board), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(PlayerType.O, true)]
        [TestCase(PlayerType.X, true)]
        [TestCase(PlayerType.Unassigned, false)]
        public void EvaluateAiHardMode_PlayerWinsRightToLeftDiagonals_ReturnExpected(PlayerType player, Boolean expected)
        {
            PlayerType[,] board = new PlayerType[3, 3]
            {
                {PlayerType.Unassigned,PlayerType.Unassigned,player},
                {PlayerType.Unassigned,player,PlayerType.Unassigned},
                {player,PlayerType.Unassigned,PlayerType.Unassigned}
            };

            Assert.That(() => this.CreateInstance().EvaluateAiHardMode(board), Is.EqualTo(expected));
        }

        private MatrixAlgorithm CreateMatrixInstance()
        {
            MatrixAlgorithm matrixAlgorithm = new MatrixAlgorithm();
            matrixAlgorithm.InitializeBoard(3);
            return matrixAlgorithm;
        }

        private AiMove CreateInstance()
        {
            return new AiMove();
        }
    }
}