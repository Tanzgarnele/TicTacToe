using System.Collections.Generic;
using System.Windows.Forms;
using TicTacToeMatch.Definitions;
using TicTacToeUi.Interfaces;

namespace TicTacToeUi.Internals
{
    internal class GamePanel : IGamePanel
    {
        public List<Button> ButtonList { get; set; }

        public Difficulty Difficulty { get; set; }
    }
}