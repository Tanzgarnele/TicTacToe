using System;

namespace TicTacToeMatch.Events
{
    public class WinnerMessageEventArgs : EventArgs
    {
        public WinnerMessageEventArgs(String message, String winner)
            : base()
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentOutOfRangeException(nameof(message), "message must be used");
            }

            if (String.IsNullOrWhiteSpace(winner))
            {
                throw new ArgumentOutOfRangeException(nameof(winner), "winnwe must be used");
            }

            this.Message = message;
            this.Winner = winner;
        }

        public String Message { get; private set; }

        public String Winner { get; private set; }
    }
}