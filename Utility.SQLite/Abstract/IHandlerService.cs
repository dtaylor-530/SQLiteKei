using Utility.Database.Common;
using Utility.Database.SQLite.Common.Abstract;
using Utility.SQLite.Database;

namespace SQLite.Utility.Factory
{
    public interface IHandlerService
    {
        IObservable<T> Database<T>(IDatabaseKey databaseKey, Func<IDatabaseHandler, T> action);
        IObservable<T> Table<T>(ITableKey tableKey, Func<ITableHandler, T> action);
    }
}