using Utility.Database.Common;
using Utility.SQLite.Database;

namespace Utility.Database.SQLite.Common.Abstract
{
    public interface IHandlerService
    {
        IObservable<T> Database<T>(IDatabaseKey databaseKey, Func<IDatabaseHandler, T> action);
        IObservable<T> Table<T>(ITableKey tableKey, Func<ITableHandler, T> action);
    }
}