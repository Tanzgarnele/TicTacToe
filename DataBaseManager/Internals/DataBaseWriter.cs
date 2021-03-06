﻿using DataBaseManager.Entities;
using DataBaseManager.Factories;
using DataBaseManager.Interface;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DataBaseManager.Internal
{
    internal class DataBaseWriter : IDataBaseWriter
    {
        readonly IDataBaseConnection dataBaseConnection = (IDataBaseConnection)DataBaseManagerFactory.Create(typeof(IDataBaseConnection));
        private static readonly String directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\settings\\autosave.db";

        public void WriteDatabaseFile(HistoryData historyData)
        {
            if (historyData is null)
            {
                throw new ArgumentNullException(nameof(historyData));
            }

            if (!File.Exists(directory))
            {
                this.dataBaseConnection.CreateDataBase();
            }
            this.dataBaseConnection.OpenDataBaseConnection();
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            String qry = "Delete From history Where Round > 0";
            this.dataBaseConnection.ModifyData(qry);

            String query = "begin";
            this.dataBaseConnection.ModifyData(query);

            for (Int32 i = 0; i < historyData.HistoryList.Count; i++)
            {
                query = $"Insert into history(Round, Winner) Values ({historyData.HistoryList[i].RoundCount}, '{historyData.HistoryList[i].Winner}')";
                this.dataBaseConnection.ModifyData(query);
            }
            query = "end";
            this.dataBaseConnection.ModifyData(query);
            Console.WriteLine("{0} seconds with one transaction", stopWatch.Elapsed.TotalSeconds);
            this.dataBaseConnection.CloseDataBaseConnection();
        }
    }
}