using Database.Entity;
using SQLite.Common.Contracts;
using System.Reactive.Linq;
using Utility.Common.Base;
using Utility.Database;
using Utility.Database.SQLite.Common.Abstract;

namespace SQLite.Service.Service;

public class TableInformationsService : ITableInformationsService
{
    private readonly IMap map;
    private readonly IHandlerService handlerService;

    public TableInformationsService(IMap map, IHandlerService handlerService)
    {
        this.map = map;
        this.handlerService = handlerService;
    }

    public IObservable<IObservable<TableInformation>> Get(DatabaseKey key)
    {
        return handlerService.Database(key, handler =>
        {
            return handler.Tables.Select(a =>
            {
                return handlerService.Table(new TableKey(key.DatabasePath, a.Name, null), handler =>
                {
                    return map.Map<TableInformation>(handler);
                });
            });
        }).Select(a =>
            {
                return a.ToObservable().SelectMany(a => a);
            });
    }
}
