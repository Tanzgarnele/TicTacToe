using System.Collections.Generic;
using System.Windows.Forms;
using TicTacToeMatch.Definitions;

namespace TicTacToeUi.Interfaces
{
    public interface IGamePanel
    {
        List<Button> ButtonList { get; set; }

        Difficulty Difficulty { get; set; }
    }
}