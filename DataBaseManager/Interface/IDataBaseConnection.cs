using DataBaseManager.Internal;
using System;
using System.Data;

namespace DataBaseManager.Interface
{
    public interface IDataBaseConnection
    {
        void ConOpen();

        void ConClose();

        DataTable GetData(String query);

        void ModifyData(String query);
    }
}