using System;
using System.Data;
using System.Threading.Tasks;

namespace DataBaseManager.Interface
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