using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;

namespace TicTacToeMatch.Interfaces
{
    public interface IAiMove
    {
        Random Rand { get; set; }

        PointIndex BestPointMove { get; set; }

        Int32 BestPointIndex { get; set; }

        List<NextMove> ScoreList { get; set; }

        Int32 FunctionCalls { get; set; }

        Int32 AiRandom(List<Button> buttonList);

        Boolean IsMoveLeft(IMatrixAlgorithm ticTacToeMatrix);

        Int32 MiniMax(IMatrixAlgorithm ticTacToeMatrix, Int32 depth, Boolean isMax);

        Move FindBestMove(IMatrixAlgorithm ticTacToeMatrix);

        PointIndex AiMediumMode(IMatrixAlgorithm ticTacToeMatrix);

        Int32 Evaluate(PlayerType[,] state);

        Boolean IsMoveLeftHard(IMatrixAlgorithm ticTacToeMatrix, PlayerType[,] newBoard);

        Int32 MiniMaxHard(PlayerType[,] newBoard, PlayerType state, IMatrixAlgorithm ticTacToeMatrix);

        void AiHardMaxValue();

        void AiHardMinValue();

        PointIndex AiHardMode(IMatrixAlgorithm ticTacToeMatrix);

        Boolean EvaluateHard(PlayerType[,] state, PlayerType player);
    }
}