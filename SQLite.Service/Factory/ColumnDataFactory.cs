using SQLite.Common.Model;
using SQLite.Utility.Factory;
using System.Reactive.Linq;
using Utility.Database.Common;

namespace SQLite.Service.Factory;

public class ColumnDataFactory
{
    private readonly IHandlerService tableHandlerFactory;

    public ColumnDataFactory(IHandlerService tableHandlerFactory)
    {
        this.tableHandlerFactory = tableHandlerFactory;
    }

    public ColumnModel[] Create(ITableKey configuration)
    {
        return tableHandlerFactory.Table(configuration, dbHandler =>
        {
            return dbHandler.Columns.Select(a => new ColumnModel(a)).ToArray();
        });
    }
}
