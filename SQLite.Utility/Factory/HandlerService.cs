using System.Reactive.Threading.Tasks;
using Utility.Database.Common;
using Utility.Database.SQLite.Common.Abstract;
using Utility.SQLite.Database;

namespace SQLite.Utility.Factory
{
    public class HandlerService : IHandlerService
    {

        public IObservable<T> Table<T>(ITableKey tableKey, Func<ITableHandler, T> action)
        {
            var tableHandler = TableHandlerFactory.Build(tableKey);

            return Task.Run(() => action(tableHandler)).ToObservable();
        }

        public IObservable<T> Database<T>(IDatabaseKey databaseKey, Func<IDatabaseHandler, T> action)
        {
            var databaseHandler = DatabaseHandlerFactory.Build(databaseKey);

            return Task.Run(() => action(databaseHandler)).ToObservable();
        }
    }

    class DatabaseHandlerFactory
    {
        public static IDatabaseHandler Build(IDatabaseKey databaseKey)
        {
            return new DatabaseHandler(databaseKey.DatabasePath);
        }
    }

    class TableHandlerFactory
    {
        public static ITableHandler Build(ITableKey tablekey)
        {
            return new TableHandler(tablekey.DatabasePath, tablekey.TableName);
        }

    }
}
