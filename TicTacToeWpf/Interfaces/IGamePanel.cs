using System.Collections.Generic;
using System.Windows.Controls;
using TicTacToeMatch.Definitions;

namespace TicTacToeWPF.Interfaces
{
    public interface IGamePanel
    {
        List<Button> ButtonList { get; set; }

        Difficulty Difficulty { get; set; }
    }
}
