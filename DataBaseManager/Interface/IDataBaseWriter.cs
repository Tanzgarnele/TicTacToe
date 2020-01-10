using DataBaseManager.Entities;

namespace DataBaseManager.Interface
{
    public interface IDataBaseWriter
    {
        void DataBaseFileWriter(HistoryData historyData);
    }
}