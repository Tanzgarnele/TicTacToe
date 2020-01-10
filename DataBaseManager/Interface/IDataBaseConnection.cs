using System;
using System.Data;

namespace DataBaseManager.Interface
{
    public interface IDataBaseConnection
    {
        void OpenDataBaseConnection();

        void CloseDataBaseConnection();

        DataTable GetData(String query);

        void ModifyData(String query);
    }
}