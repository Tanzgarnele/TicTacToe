﻿using DataBaseManager.Factory;
using DataBaseManager.Interface;
using Serializer.Factory;
using Serializer.Interface;
using System;
using System.Windows.Forms;
using TicTacToeMatch.Factory;
using TicTacToeMatch.Interfaces;

namespace TicTacToeUi
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            IMatrixAlgorithm ticTacToeMatrix = (IMatrixAlgorithm)GlobalFactory.Create(typeof(IMatrixAlgorithm));
            IAiMove aiMoves = (IAiMove)GlobalFactory.Create(typeof(IAiMove));
            ISerializeData serialize = (ISerializeData)SerDesFactory.Create(typeof(ISerializeData));
            IDeSerializeData deserialize = (IDeSerializeData)SerDesFactory.Create(typeof(IDeSerializeData));
            IIniParseData iniParser = (IIniParseData)SerDesFactory.Create(typeof(IIniParseData));
            IDataBaseWriter dataBaseWriter = (IDataBaseWriter)DataBaseManagerFactory.Create(typeof(IDataBaseWriter));
            IDataBaseConnection dataBaseConnection = (IDataBaseConnection)DataBaseManagerFactory.Create(typeof(IDataBaseConnection));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow(ticTacToeMatrix, aiMoves, serialize, deserialize, iniParser, dataBaseConnection, dataBaseWriter));
        }
    }
}