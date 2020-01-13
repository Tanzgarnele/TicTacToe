using System;
using TicTacToeMatch.Definitions;

namespace Serializer.Enteties
{
    public class SavedGameState
    {
        public Int32 CurrentTurnCount { get; set; }
        public Int32 BoardSize { get; set; }
        public PlayerType CurrentTurn { get; set; }
        public String BoardData { get; set; }

    }
}