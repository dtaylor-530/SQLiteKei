using SQLite.Utility.Factory;
using System.Reactive.Linq;
using Utility.Database.Common;
using Utility.Database.SQLite.Common.Abstract;

namespace Database.Service.Model;

public class TableGeneralModel : ITableGeneralModel
{
    private readonly IHandlerService tableHandlerFactory;
    private readonly Dictionary<ITableKey, TableGeneralInformation> dictionary = new();

    public TableGeneralModel(IHandlerService tableHandlerFactory)
    {
        this.tableHandlerFactory = tableHandlerFactory;

    }

    public IObservable<TableGeneralInformation> Get(ITableKey tableKey)
    {
        if (dictionary.ContainsKey(tableKey))
        {
            return Observable.Return(dictionary[tableKey]);
        }

        return tableHandlerFactory.Table(tableKey, handler =>
        {
            return dictionary[tableKey] = handler.General;
        });
    }

    //using (var dbHandler = new TableHandler(key.DatabasePath, TableName))
    //    {
    //        (string a, long b, Column[] c, int d) = dbHandler.General;
    //        TableCreateStatement = a;
    //        RowCount = b;
    //        ColumnData = c;
    //        ColumnCount = d;
    //    }
}
