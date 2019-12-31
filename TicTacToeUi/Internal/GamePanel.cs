using System.Collections.Generic;
using System.Windows.Forms;
using TicTacToeMatch.Definitions;
using TicTacToeUi.Interfaces;

namespace TicTacToeUi.Internal
{
    internal class GamePanel : IGamePanel
    {
        public List<Button> ButtonList { get; set; }

        public Difficulty Difficulty { get; set; }
    }
}