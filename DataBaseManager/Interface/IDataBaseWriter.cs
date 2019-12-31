using DataBaseManager.Entities;
using DataBaseManager.Internal;

namespace DataBaseManager.Interface
{
    public interface IDataBaseWriter
    {
        void WriteToDataBase(HistoryData historyData);
    }
}
