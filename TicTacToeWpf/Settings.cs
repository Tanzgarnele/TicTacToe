using System;
using System.Collections.Generic;
using TicTacToeMatch.Entities;
using TicTacToeWPF.Interfaces;

namespace TicTacToeWPF.Internal
{
    internal class Settings : ISettings
    {
        public List<History> HistoryList { get; set; }

        public Int32 RoundCount { get; set; } = 1;

        public Boolean XmlIsUsed { get; set; }
    }
}
