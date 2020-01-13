using System;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;
using TicTacToeMatch.Events;
using TicTacToeMatch.Interfaces;

namespace TicTacToeMatch.Internals
{
    internal class MatrixAlgorithm : IMatrixAlgorithm
    {
        public event EventHandler<WinnerMessageEventArgs> GameEnd;

        public Int32 CurrentTurnCount { get; set; } = 0;

        public Int32 BoardSize { get; set; }

        public PlayerType CurrentTurn { get; set; } = PlayerType.X;

        public PlayerType[,] Board { get; set; }

        public Boolean WinnerState { get; set; } = false;

        public Boolean CheckWinner(PointIndex point, PlayerType state)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            for (Int32 x = 0; x < this.BoardSize; x++)
            {
                if (this.Board[x, point.Y] != state)
                {
                    break;
                }

                if (x == this.BoardSize - 1)
                {
                    this.GetWinnerMessage(state);
                    this.WinnerState = true;
                    return true;
                }
            }

            for (Int32 y = 0; y < this.BoardSize; y++)
            {
                if (this.Board[point.X, y] != state)
                {
                    break;
                }

                if (y == this.BoardSize - 1)
                {
                    this.GetWinnerMessage(state);
                    this.WinnerState = true;
                    return true;
                }
            }

            for (Int32 x = 0, y = 0; x < this.BoardSize && y < this.BoardSize; x++, y++)
            {
                if (this.Board[x, y] != state)
                {
                    break;
                }

                if (x == this.BoardSize - 1 || y == this.BoardSize - 1)
                {
                    this.GetWinnerMessage(state);
                    this.WinnerState = true;
                    return true;
                }
            }

            for (Int32 x = this.BoardSize - 1, y = 0; x >= 0 && y < this.BoardSize; x--, y++)
            {
                if (this.Board[x, y] != state)
                {
                    break;
                }

                if (x == 0 || y == this.BoardSize - 1)
                {
                    this.GetWinnerMessage(state);
                    this.WinnerState = true;
                    return true;
                }
            }

            this.CurrentTurnCount++;
            if (this.CurrentTurnCount == this.BoardSize * this.BoardSize)
            {
                this.GetWinnerMessage(state);
                this.WinnerState = true;
                return true;
            }

            this.CurrentTurn = this.CurrentTurn == PlayerType.X ? PlayerType.O : PlayerType.X;
            return false;
        }

        public void InitializeBoard(Int32 dimension)
        {
            if (dimension < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(dimension));
            }

            this.Board = new PlayerType[dimension, dimension];
            this.BoardSize = dimension;
        }

        #region WinnerMessage

        internal void GetWinnerMessage(PlayerType check)
        {
            if (this.CurrentTurnCount == this.BoardSize * this.BoardSize)
            {
                this.GameEnd?.Invoke(this, new WinnerMessageEventArgs("Draw", "Draw"));
            }
            else
            {
                this.GameEnd?.Invoke(this, new WinnerMessageEventArgs((check.ToString() + " Wins"), this.CurrentTurn.ToString()));
            }
        }

        #endregion WinnerMessage
    }
}