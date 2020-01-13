using DataBaseManager.Interfaces;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace DataBaseManager.Internals
{
    internal class DataBaseConnection : IDataBaseConnection
    {
        private static readonly String directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\settings\\autosave.db";
        private readonly SQLiteConnection connection = new SQLiteConnection($"data source={directory}; Version=3;");
        private readonly SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter();

        public void CreateDataBase()
        {
            SQLiteConnection.CreateFile(directory);

            SQLiteConnection newConnection = new SQLiteConnection($"Data source={directory}; Version=3;");
            newConnection.Open();

            String createTable = "CREATE TABLE History (Round int, Winner varchar(10))";

            SQLiteCommand createTableCommand = new SQLiteCommand(createTable, newConnection);
            createTableCommand.ExecuteNonQuery();
            createTableCommand.Dispose();

            newConnection.Close();
        }

        public void OpenDataBaseConnection()
        {
            try
            {
                this.connection.Open();
                Console.WriteLine("Verbindung aufgebaut");
            }
            catch (Exception ex)
            {
                String error = ex.ToString();
                Console.WriteLine(error);
            }
        }

        public void CloseDataBaseConnection()
        {
            this.connection.Close();
        }

        public DataTable GetData(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentOutOfRangeException(nameof(query), "query may not be null, empty or whitespace");
            }

            DataTable dt = new DataTable();
            SQLiteCommand cmd = new SQLiteCommand(query, this.connection)
            {
                CommandText = query,
                Connection = connection
            };

            this.dataAdapter.SelectCommand = cmd;
            this.dataAdapter.Fill(dt);
            return dt;
        }

        public void ModifyData(String query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentOutOfRangeException(nameof(query), "query may not be null, empty or whitespace");
            }

            SQLiteCommand cmd = new SQLiteCommand
            {
                Connection = connection,
                CommandText = query
            };
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}