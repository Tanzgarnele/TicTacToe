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

        Int32 GetAiEasyPointIndex(List<Button> buttonList);

        Boolean GetMovesLeft(IMatrixAlgorithm ticTacToeMatrix);

        Int32 GetMiniMaxMedium(IMatrixAlgorithm ticTacToeMatrix, Int32 depth, Boolean isMax);

        Move GetBestAiMoveMedium(IMatrixAlgorithm ticTacToeMatrix);

        PointIndex GetAiMediumPointIndex(IMatrixAlgorithm ticTacToeMatrix);

        Int32 EvaluateAiMediumMoves(PlayerType[,] state);

        Boolean GetMovesLeftAiHard(IMatrixAlgorithm ticTacToeMatrix, PlayerType[,] newBoard);

        Int32 GetMiniMaxAiHard(PlayerType[,] newBoard, PlayerType state, IMatrixAlgorithm ticTacToeMatrix);

        void CalcMaxValueAiHard();

        void CalcMinValueAiHard();

        PointIndex GetAiHardPointIndex(IMatrixAlgorithm ticTacToeMatrix);

        Boolean EvaluateAiHardMode(PlayerType[,] state, PlayerType player);
    }
}