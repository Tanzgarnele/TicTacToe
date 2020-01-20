using System;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;
using TicTacToeMatch.Events;

namespace TicTacToeMatch.Interfaces
{
    public interface IMatrixAlgorithm
    {
        event EventHandler<WinnerMessageEventArgs> GameEnd;

        Int32 CurrentTurnCount { get; set; }

        Int32 BoardSize { get; set; }

        PlayerType CurrentTurn { get; set; }

        PlayerType[,] Board { get; set; }

        Boolean WinnerState { get; set; }

        Boolean CheckWinner(PointIndex point, PlayerType state);

        void InitializeBoard(Int32 dimension);
    }
}