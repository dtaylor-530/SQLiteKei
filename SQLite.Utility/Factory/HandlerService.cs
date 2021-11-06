using Utility.Database.Common;
using Utility.Database.SQLite.Common.Abstract;
using Utility.SQLite.Database;

namespace SQLite.Utility.Factory
{
    public class HandlerService : IHandlerService
    {

        public T Table<T>(ITableKey tableKey, Func<ITableHandler, T> action)
        {
            using (var tableHandler = TableHandlerFactory.Build(tableKey))
            {
                return action(tableHandler);
            }
        }

        public T Database<T>(IDatabaseKey databaseKey, Func<IDatabaseHandler, T> action)
        {
            using (var databaseHandler = DatabaseHandlerFactory.Build(databaseKey))
            {
                return action(databaseHandler);
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
}
