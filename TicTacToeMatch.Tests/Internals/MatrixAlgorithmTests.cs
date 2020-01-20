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
    public class MatrixAlgorithmTests
    {
        [Test]
        public void CheckWinner_PointIndexIsNull_ThrowArgumentNullException()
        {
            Assert.That(() => this.CreateInstance().CheckWinner(null, PlayerType.O), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void InitializeBoard_DimensionIndexIsLowerThanThree_ThrowArgumentOutOfRangeExceotion()
        {
            Assert.That(() => this.CreateInstance().InitializeBoard(1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }





        [Test]

        public void CheckWinner_NoWinner_ReturnFalse()
        {
            PointIndex test = new PointIndex
            {
                X = 0,
                Y = 0
            };

            MatrixAlgorithm matrixAlgorithm = this.CreateInstance();
            matrixAlgorithm.Board = new PlayerType[3,3]
                {
                {PlayerType.X,PlayerType.O,PlayerType.X},
                {PlayerType.X,PlayerType.O,PlayerType.X},
                {PlayerType.O,PlayerType.X,PlayerType.O}
            };

            Assert.That(() => matrixAlgorithm.CheckWinner(test, PlayerType.Unassigned), Is.False);
        }

        [Test]

        public void CheckWinner_PlayerWinsWithRows_ReturnTrue()
        {
            PointIndex test = new PointIndex
            {
                X = 0,
                Y = 0
            };

            MatrixAlgorithm matrixAlgorithm = this.CreateInstance();
            matrixAlgorithm.Board = new PlayerType[3, 3]
                {
                {PlayerType.X,PlayerType.X,PlayerType.X},
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.Unassigned}
            };

            Assert.That(() => matrixAlgorithm.CheckWinner(test, PlayerType.X), Is.True);
        }

        [Test]
        public void CheckWinner_PlayerWinsWithColumns_ReturnTrue()
        {
            PointIndex test = new PointIndex
            {
                X = 0,
                Y = 0
            };

            MatrixAlgorithm matrixAlgorithm = this.CreateInstance();
            matrixAlgorithm.Board = new PlayerType[3, 3]
                {
                {PlayerType.X,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.X,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.X,PlayerType.Unassigned,PlayerType.Unassigned}
            };

            Assert.That(() => matrixAlgorithm.CheckWinner(test, PlayerType.X), Is.True);
        }

        [Test]
        public void CheckWinner_PlayerWinsWithDiagonalsFromLeftToRight_ReturnTrue()
        {
            PointIndex test = new PointIndex
            {
                X = 0,
                Y = 0
            };

            MatrixAlgorithm matrixAlgorithm = this.CreateInstance();
            matrixAlgorithm.Board = new PlayerType[3, 3]
                {
                {PlayerType.X,PlayerType.Unassigned,PlayerType.Unassigned},
                {PlayerType.Unassigned,PlayerType.X,PlayerType.Unassigned},
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.X}
            };

            Assert.That(() => matrixAlgorithm.CheckWinner(test, PlayerType.X), Is.True);
        }

        [Test]
        public void CheckWinner_PlayerWinsWithDiagonalsFromRightToLeft_ReturnTrue()
        {
            PointIndex test = new PointIndex
            {
                X = 0,
                Y = 0
            };

            MatrixAlgorithm matrixAlgorithm = this.CreateInstance();
            matrixAlgorithm.Board = new PlayerType[3, 3]
                {
                {PlayerType.Unassigned,PlayerType.Unassigned,PlayerType.X},
                {PlayerType.Unassigned,PlayerType.X,PlayerType.Unassigned},
                {PlayerType.X,PlayerType.Unassigned,PlayerType.Unassigned}
            };

            Assert.That(() => matrixAlgorithm.CheckWinner(test, PlayerType.X), Is.True);
        }

        [Test]
        public void CheckWinner_GameEndsWithDraw_ReturnTrue()
        {
            PointIndex test = new PointIndex
            {
                X = 0,
                Y = 0
            };

            MatrixAlgorithm matrixAlgorithm = this.CreateInstance();
            matrixAlgorithm.Board = new PlayerType[3, 3]
                {
                {PlayerType.X,PlayerType.X,PlayerType.O},
                {PlayerType.O,PlayerType.O,PlayerType.X},
                {PlayerType.X,PlayerType.O,PlayerType.X}
            };
            matrixAlgorithm.CurrentTurnCount = 8;



            Assert.That(() => matrixAlgorithm.CheckWinner(test, PlayerType.X), Is.True);
        }

        private MatrixAlgorithm CreateInstance()
        {
            MatrixAlgorithm matrixAlgorithm = new MatrixAlgorithm();
            matrixAlgorithm.InitializeBoard(3);
            return matrixAlgorithm;
        }
    }
}