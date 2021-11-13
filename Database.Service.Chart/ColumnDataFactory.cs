using Database.Entity;
using Utility.Common.Base;
using Utility.Database.Common;
using Utility.Database.SQLite.Common.Abstract;

namespace Database.Service.Chart;

public class ColumnDataFactory
{
    private readonly IHandlerService tableHandlerFactory;
    private readonly IMap map;

    public ColumnDataFactory(IHandlerService tableHandlerFactory, IMap map)
    {
        this.tableHandlerFactory = tableHandlerFactory;
        this.map = map;
    }

    public IObservable<ColumnModel[]> Create(ITableKey configuration)
    {
        return tableHandlerFactory.Table(configuration, dbHandler =>
        {
            return dbHandler.Columns.Select(map.Map<ColumnModel>).ToArray();
        });
    }
}
