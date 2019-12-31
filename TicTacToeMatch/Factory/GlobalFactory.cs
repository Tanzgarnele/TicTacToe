using System;
using TicTacToeMatch.Interfaces;
using TicTacToeMatch.Internal;

namespace TicTacToeMatch.Factory
{
    public static class GlobalFactory
    {
        public static Object Create(Type type)
        {
            if (type == typeof(IMatrixAlgorithm))
            {
                return new MatrixAlgorithm();
            }

            if (type == typeof(IAiMove))
            {
                return new AiMove(); 
            }

            throw new NotSupportedException();
        }
    }
}