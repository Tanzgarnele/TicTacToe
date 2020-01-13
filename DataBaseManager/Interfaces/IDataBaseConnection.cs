using System;
using System.Data;

namespace DataBaseManager.Interfaces
{
    public interface IDataBaseConnection
    {
        void OpenDataBaseConnection();

        void CloseDataBaseConnection();

        DataTable GetData(String query);

        void ModifyData(String query);

        void CreateDataBase();
    }
}