using System;
using System.Collections.Generic;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Entities;

namespace Serializer.Enteties
{
    public class Data
    {
        public Int32 Round { get; set; }
        public SavedGameState CurrentGame { get; set; }
        public List<History> HistoryList { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
