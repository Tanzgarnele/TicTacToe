using System;
using System.Data;

namespace DataBaseManager.Interface
{
    public interface IDataBaseConnection
    {
        void DatabaseOpenConnection();

        void DatabaseCloseConnection();

        DataTable GetData(String query);

        void DataModifier(String query);
    }
}