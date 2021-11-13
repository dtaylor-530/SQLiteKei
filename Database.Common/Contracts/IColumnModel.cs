using Database.Entity;
using Utility.Database.Common;

namespace SQLite.Service.Service
{
    public interface IColumnModel
    {
        IObservable<IReadOnlyCollection<ColumnModel>> GetCollection(ITableKey tableKey);
    }
}