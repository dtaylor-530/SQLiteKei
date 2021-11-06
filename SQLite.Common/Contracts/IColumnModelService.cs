using SQLite.Common.Model;
using Utility.Database.Common;

namespace SQLite.Service.Service
{
    public interface IColumnModelService
    {
        IReadOnlyCollection<ColumnModel> GetCollection(ITableKey tableKey);
    }
}