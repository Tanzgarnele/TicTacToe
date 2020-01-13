using DataBaseManager.Entities;

namespace DataBaseManager.Interfaces
{
    public interface IDataBaseWriter
    {
        void WriteDatabaseFile(HistoryData historyData);
    }
}