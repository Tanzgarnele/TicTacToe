using System;

namespace TicTacToeMatch.Entities
{
    public struct NextMove
    {
        public PointIndex PointIndex { get; set; }

        public Int32 Score { get; set; }
    }
}