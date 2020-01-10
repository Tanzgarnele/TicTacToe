using DataBaseManager.Entities;

namespace DataBaseManager.Interface
{
    public interface IDataBaseWriter
    {
        void WriteDatabaseFile(HistoryData historyData);
    }
}