using Utility.Database;

namespace SQLite.Common.Contracts
{
    public interface ITableService
    {
        bool DeleteTable(ITableKey tableItem);
        void EmptyTable(ITableKey tableItem);
        void ReindexTable(ITableKey tableItem);
        void RenameTable(ITableKey tableItem, string newName);
    }
}