﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;
using TicTacToeMatch.Interfaces;

namespace TicTacToeMatch.Internal
{
    internal class AiMove : IAiMove
    {
        public Random Rand { get; set; }

        public List<NextMove> ScoreList { get; set; }

        public PointIndex BestPointMove { get; set; }

        public Int32 BestPointIndex { get; set; }

        public Int32 FunctionCalls { get; set; }

        public Int32 AiRandom(List<Button> buttonList)
        {
            this.Rand = new Random();
            return this.Rand.Next(buttonList.Count);
        }

        #region AiMediumMode

        public Boolean IsMoveLeft(IMatrixAlgorithm ticTacToeMatrix)
        {
            for (Int32 x = 0; x < ticTacToeMatrix.BoardSize; x++)
            {
                for (Int32 y = 0; y < ticTacToeMatrix.BoardSize; y++)
                {
                    if (ticTacToeMatrix.Board[x, y] == PlayerType.Unassigned)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Int32 MiniMax(IMatrixAlgorithm ticTacToeMatrix, Int32 depth, Boolean isMax)
        {
            Int32 score = this.Evaluate(ticTacToeMatrix.Board) - depth;

            if (score == 10)
            {
                return score - depth;
            }

            if (score == -10)
            {
                return depth - score;
            }

            if (this.IsMoveLeft(ticTacToeMatrix))
            {
                return score;
            }

            depth += 1;
            if (isMax)
            {
                Int32 best = Int32.MinValue;

                for (Int32 x = 0; x < ticTacToeMatrix.BoardSize; x++)
                {
                    for (Int32 y = 0; y < ticTacToeMatrix.BoardSize; y++)
                    {
                        if (ticTacToeMatrix.Board[x, y] == PlayerType.Unassigned)
                        {
                            ticTacToeMatrix.Board[x, y] = PlayerType.X;

                            best = Math.Max(best, this.MiniMax(ticTacToeMatrix, depth, false));

                            ticTacToeMatrix.Board[x, y] = PlayerType.Unassigned;
                        }
                    }
                }
                return best;
            }
            else
            {
                Int32 best = Int32.MaxValue;

                for (Int32 x = 0; x < ticTacToeMatrix.BoardSize; x++)
                {
                    for (Int32 y = 0; y < ticTacToeMatrix.BoardSize; y++)
                    {
                        if (ticTacToeMatrix.Board[x, y] == PlayerType.Unassigned)
                        {
                            ticTacToeMatrix.Board[x, y] = PlayerType.O;

                            best = Math.Min(best, this.MiniMax(ticTacToeMatrix, depth, true));

                            ticTacToeMatrix.Board[x, y] = PlayerType.Unassigned;
                        }
                    }
                }
                return best;
            }
        }

        public Move FindBestMove(IMatrixAlgorithm ticTacToeMatrix)
        {
            Move bestMove = new Move
            {
                Row = -1,
                Col = -1
            };
            Int32 bestScore = Int32.MinValue;

            for (Int32 x = 0; x < ticTacToeMatrix.BoardSize; x++)
            {
                for (Int32 y = 0; y < ticTacToeMatrix.BoardSize; y++)
                {
                    if (ticTacToeMatrix.Board[x, y] == PlayerType.Unassigned)
                    {
                        ticTacToeMatrix.Board[x, y] = PlayerType.X;

                        Int32 score = this.MiniMax(ticTacToeMatrix, 0, false);

                        ticTacToeMatrix.Board[x, y] = PlayerType.Unassigned;

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove.Row = x;
                            bestMove.Col = y;
                        }
                    }
                }
            }

            return bestMove;
        }

        public PointIndex AiMediumMode(IMatrixAlgorithm ticTacToeMatrix)
        {
            Move bestMove = this.FindBestMove(ticTacToeMatrix);

            PointIndex point = new PointIndex
            {
                X = bestMove.Row,
                Y = bestMove.Col
            };

            return point;
        }

        public Int32 Evaluate(PlayerType[,] state)
        {
            for (Int32 row = 0; row < 3; row++)
            {
                if (state[row, 0] == state[row, 1] &&
                    state[row, 1] == state[row, 2])
                {
                    if (state[row, 0] == PlayerType.X)
                    {
                        return +10;
                    }

                    if (state[row, 0] == PlayerType.O)
                    {
                        return -10;
                    }
                }
            }

            for (Int32 col = 0; col < 3; col++)
            {
                if (state[0, col] == state[1, col] &&
                    state[1, col] == state[2, col])
                {
                    if (state[0, col] == PlayerType.X)
                    {
                        return +10;
                    }

                    if (state[0, col] == PlayerType.O)
                    {
                        return -10;
                    }
                }
            }

            if (state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2])
            {
                if (state[0, 0] == PlayerType.X)
                {
                    return +10;
                }

                if (state[0, 0] == PlayerType.O)
                {
                    return -10;
                }
            }

            if (state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0])
            {
                if (state[0, 2] == PlayerType.X)
                {
                    return +10;
                }

                if (state[0, 2] == PlayerType.O)
                {
                    return -10;
                }
            }

            return 0;
        }

        #endregion AiMediumMode

        #region AiHardMode

        public Boolean IsMoveLeftHard(IMatrixAlgorithm ticTacToeMatrix, PlayerType[,] newBoard)
        {
            for (Int32 x = 0; x < ticTacToeMatrix.BoardSize; x++)
            {
                for (Int32 y = 0; y < ticTacToeMatrix.BoardSize; y++)
                {
                    if (newBoard[x, y] == PlayerType.Unassigned)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Int32 MiniMaxHard(PlayerType[,] newBoard, PlayerType state, IMatrixAlgorithm ticTacToeMatrix)
        {
            this.FunctionCalls++;

            if (this.EvaluateHard(newBoard, PlayerType.X))
            {
                return -10;
            }
            else if (this.EvaluateHard(newBoard, PlayerType.O))
            {
                return 10;
            }
            else if (this.IsMoveLeftHard(ticTacToeMatrix, newBoard))
            {
                return 0;
            }
            this.ScoreList = new List<NextMove>();

            for (Int32 x = 0; x < ticTacToeMatrix.BoardSize; x++)
            {
                for (Int32 y = 0; y < ticTacToeMatrix.BoardSize; y++)
                {
                    NextMove nextMove = new NextMove();
                    if (newBoard[x, y] == PlayerType.Unassigned)
                    {
                        nextMove.PointIndex = new PointIndex { X = x, Y = y };
                        newBoard[x, y] = state;

                        if (state == PlayerType.X)
                        {
                            Int32 result = this.MiniMaxHard(newBoard, PlayerType.O, ticTacToeMatrix);
                            nextMove.Score = result;
                        }
                        else
                        {
                            Int32 result = this.MiniMaxHard(newBoard, PlayerType.X, ticTacToeMatrix);
                            nextMove.Score = result;
                        }

                        newBoard[x, y] = PlayerType.Unassigned;

                        this.ScoreList.Add(nextMove);
                    }
                }
            }

            switch (state)
            {
                case PlayerType.X:
                    this.AiHardMinValue();
                    break;

                case PlayerType.O:
                    this.AiHardMaxValue();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }

            return this.BestPointIndex;
        }

        public void AiHardMaxValue()
        {
            Int32 bestScore = Int32.MaxValue;

            for (Int32 i = 0; i < this.ScoreList.Count; i++)
            {
                if (this.ScoreList[i].Score < bestScore)
                {
                    bestScore = this.ScoreList[i].Score;
                    this.BestPointMove = this.ScoreList[i].PointIndex;
                    this.BestPointIndex = i;
                }
            }
        }

        public void AiHardMinValue()
        {
            Int32 bestScore = Int32.MinValue;

            for (Int32 i = 0; i < this.ScoreList.Count; i++)
            {
                if (this.ScoreList[i].Score > bestScore)
                {
                    bestScore = this.ScoreList[i].Score;
                    this.BestPointMove = this.ScoreList[i].PointIndex;
                    this.BestPointIndex = i;
                }
            }
        }

        public PointIndex AiHardMode(IMatrixAlgorithm ticTacToeMatrix)
        {
            this.MiniMaxHard(ticTacToeMatrix.Board, PlayerType.O, ticTacToeMatrix);
            return this.BestPointMove;
        }

        public Boolean EvaluateHard(PlayerType[,] state, PlayerType player)
        {
            for (Int32 row = 0; row < 3; row++)
            {
                if (state[row, 0] == state[row, 1] &&
                    state[row, 1] == state[row, 2] && state[row, 0] == player)
                {
                    return true;
                }
            }

            for (Int32 col = 0; col < 3; col++)
            {
                if (state[0, col] == state[1, col] &&
                    state[1, col] == state[2, col] && state[0, col] == player)
                {
                    return true;
                }
            }

            if (state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2] && state[0, 0] == player)
            {
                return true;
            }

            if (state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0] && state[0, 2] == player)
            {
                return true;
            }
            return false;
        }

        #endregion AiHardMode
    }
}
