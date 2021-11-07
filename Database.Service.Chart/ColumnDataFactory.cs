using SQLite.Common.Model;
using Utility.Database.Common;
using Utility.Database.SQLite.Common.Abstract;

namespace Database.Service.Chart;

public class ColumnDataFactory
{
    private readonly IHandlerService tableHandlerFactory;

    public ColumnDataFactory(IHandlerService tableHandlerFactory)
    {
        this.tableHandlerFactory = tableHandlerFactory;
    }

    public IObservable<ColumnModel[]> Create(ITableKey configuration)
    {
        return tableHandlerFactory.Table(configuration, dbHandler =>
        {
            return dbHandler.Columns.Select(a => new ColumnModel(a)).ToArray();
        });
    }
}
