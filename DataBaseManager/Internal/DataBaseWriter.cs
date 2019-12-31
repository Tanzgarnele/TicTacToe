using DataBaseManager.Entities;
using DataBaseManager.Interface;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DataBaseManager.Internal
{
    public class DataBaseWriter : IDataBaseWriter
    {
        private readonly DataBaseConnection dataBaseConnection = DataBaseConnection.Connection();

        private static readonly String directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\settings\\autosave.db";

        public void WriteToDataBase(HistoryData historyData)
        {
            if (!File.Exists(directory))
            {
                this.dataBaseConnection.CreataDataBase();
            }
            this.dataBaseConnection.ConOpen();
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
            this.dataBaseConnection.ConClose();
        }
    }
}