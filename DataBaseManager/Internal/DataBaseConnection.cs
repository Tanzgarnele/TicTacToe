using DataBaseManager.Interface;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace DataBaseManager.Internal
{
    public class DataBaseConnection : IDataBaseConnection
    {
        private static readonly String directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\settings\\autosave.db";
        private static DataBaseConnection _Connection;
        private readonly SQLiteConnection connection = new SQLiteConnection($"data source={directory}; Version=3;");
        private readonly SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter();

        public DataBaseConnection()
        {
        }

        public void DataBaseCreator()
        {
            SQLiteConnection.CreateFile(directory);

            SQLiteConnection newConnection = new SQLiteConnection($"Data source={directory}; Version=3;");
            newConnection.Open();

            String createTable = "Create Table history (Round int, Winner varchar(10))";

            SQLiteCommand createTableCommand = new SQLiteCommand(createTable, newConnection);
            createTableCommand.ExecuteNonQuery();
            createTableCommand.Dispose();

            newConnection.Close();
        }

        public static DataBaseConnection Connection()
        {
            if (_Connection == null)
            {
                _Connection = new DataBaseConnection();
            }

            return _Connection;
        }

        public void DatabaseOpenConnection()
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

        public void DatabaseCloseConnection()
        {
            this.connection.Close();
        }

        public DataTable GetData(String query)
        {
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

        public void DataModifier(String query)
        {
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