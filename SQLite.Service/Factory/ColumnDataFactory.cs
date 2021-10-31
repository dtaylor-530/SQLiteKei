using SQLite.Common.Model;
using System.Reactive.Linq;
using Utility.Database;
using Utility.SQLite.Database;

namespace SQLite.Service.Factory;

public class ColumnDataFactory
{
    public ColumnModel[] Create(ITableKey configuration)
    {
        using (var dbHandler = new TableHandler(configuration.DatabasePath, configuration.TableName))
        {
            return dbHandler.Columns.Select(a => new ColumnModel(a)).ToArray();
        }
    }
}
