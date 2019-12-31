using System;
using System.Collections.Generic;
using TicTacToeMatch.Entities;

namespace TicTacToeUi.Interfaces
{
    public interface ISettings
    {
        List<History> HistoryList { get; set; }

        Int32 RoundCount { get; set; }

        Boolean XmlIsUsed { get; set; }
    }
}