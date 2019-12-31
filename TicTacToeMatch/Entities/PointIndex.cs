using System;

namespace TicTacToeMatch.Entities
{
    public class PointIndex
    {
        public PointIndex()
            : this(0, 0)
        {
        }

        public PointIndex(Int32 x, Int32 y)
        {
            this.X = x;
            this.Y = y;
        }

        public Int32 X { get; set; }

        public Int32 Y { get; set; }
    }
}