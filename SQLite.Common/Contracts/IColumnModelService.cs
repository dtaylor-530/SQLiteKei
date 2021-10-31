using SQLite.Common.Model;
using Utility.Database;

namespace SQLite.Service.Service
{
    public interface IColumnModelService
    {
        IReadOnlyCollection<ColumnModel> GetCollection(ITableKey tableKey);
    }
}