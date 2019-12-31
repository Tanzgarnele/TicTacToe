﻿using System;
using System.Text;
using TicTacToeMatch.Definitions;
using TicTacToeMatch.Interfaces;

namespace Serializer.Mapper
{
    public class Mapper
    {

        public String CurrentBoardToString(IMatrixAlgorithm matrixAlgorithm)
        {
            StringBuilder stringBuilder = new StringBuilder(String.Empty);
            for (Int32 x = 0; x < matrixAlgorithm.Board.GetLength(0); x++)
            {
                stringBuilder.Append(",{");
                for (Int32 y = 0; y < matrixAlgorithm.Board.GetLength(1); y++)
                {
                    stringBuilder.Append($"{matrixAlgorithm.Board[x, y]},");
                }

                stringBuilder.Append("}");
            }
            stringBuilder.Replace(",}", "}").Remove(0, 1);

            return stringBuilder.ToString();
        }

        public PlayerType[,] CurrentStringToBoard(String BoardData, IMatrixAlgorithm matrixAlgorithm)
        {
            for (Int32 board = 0; board < matrixAlgorithm.BoardSize* matrixAlgorithm.BoardSize ; board++)
            {
                BoardData = BoardData.Replace("}", String.Empty);
                BoardData = BoardData.Replace("{", String.Empty);
                String[] boardDataArray = BoardData.Split(',');

                Int32 col = board / matrixAlgorithm.BoardSize;
                Int32 row = board % matrixAlgorithm.BoardSize;

                matrixAlgorithm.Board[col, row] = (PlayerType)Enum.Parse(typeof(PlayerType), boardDataArray[board]);
            }

            return matrixAlgorithm.Board;
        }
    }
}