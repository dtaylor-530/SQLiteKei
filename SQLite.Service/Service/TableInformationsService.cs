using SQLite.Common.Contracts;
using Utility.Database;
using Utility.SQLite.Helpers;

namespace SQLite.Service.Service;

public class TableInformationsService : ITableInformationsService
{

    public TableInformationsService()
    {

    }

    public IReadOnlyCollection<TableInformation> Get(DatabaseKey key)
    {
        return ConnectionHelper.TablesInformation(key.DatabasePath);
    }
}
