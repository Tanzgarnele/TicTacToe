using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;
using TicTacToeMatch.Events;
using TicTacToeMatch.Interfaces;

namespace TicTacToeMatch.Internal
{
    internal class MatrixAlgorithm : IMatrixAlgorithm
    {
        public event EventHandler<WinnerMessageEventArgs> EndGame;

        public Int32 CurrentTurnCount { get; set; } = 0;

        public Int32 BoardSize { get; set; }

        public PlayerType CurrentTurn { get; set; } = PlayerType.X;

        [XmlIgnore]
        public PlayerType[,] Board { get; set; }

        public Boolean WinnerState { get; set; } = false;

        public Boolean CheckWinner(PointIndex point, PlayerType state)
        {
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
            this.Board = new PlayerType[dimension, dimension];
            this.BoardSize = dimension;
        }

        #region WinnerMessage

        internal void GetWinnerMessage(PlayerType check)
        {
            if (this.CurrentTurnCount == this.BoardSize * this.BoardSize)
            {
                this.EndGame?.Invoke(this, new WinnerMessageEventArgs("Draw", "Draw"));
            }
            else
            {
                this.EndGame?.Invoke(this, new WinnerMessageEventArgs((check.ToString() + " Wins"), this.CurrentTurn.ToString()));
            }
        }

        #endregion WinnerMessage
    }
}