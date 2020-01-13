using DataBaseManager.Entities;
using DataBaseManager.Factories;
using DataBaseManager.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DataBaseManager.Internals
{
    internal class DataBaseWriter : IDataBaseWriter
    {
        private readonly IDataBaseConnection dataBaseConnection = (IDataBaseConnection)DataBaseManagerFactory.Create(typeof(IDataBaseConnection));
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

            String qry = "DELETE FROM History WHERE Round > 0";
            this.dataBaseConnection.ModifyData(qry);

            String query = "BEGIN";
            this.dataBaseConnection.ModifyData(query);

            for (Int32 i = 0; i < historyData.HistoryList.Count; i++)
            {
                query = $"INSERT INTO History(Round, Winner) VALUES ({historyData.HistoryList[i].RoundCount}, '{historyData.HistoryList[i].Winner}')";
                this.dataBaseConnection.ModifyData(query);
            }
            query = "END";
            this.dataBaseConnection.ModifyData(query);
            Console.WriteLine("{0} seconds with one transaction", stopWatch.Elapsed.TotalSeconds);
            this.dataBaseConnection.CloseDataBaseConnection();
        }
    }
}