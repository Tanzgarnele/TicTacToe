using System.Collections.Generic;
using System.Windows.Controls;
using TicTacToeMatch.Definitions;
using TicTacToeWPF.Interfaces;

namespace TicTacToeWPF.Internal
{
    internal class GamePanel : IGamePanel
    {
        public List<Button> ButtonList { get; set; }

        public Difficulty Difficulty { get; set; }
    }
}
