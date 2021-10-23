using Utility.Database;
using Utility.SQLite.Helpers;

namespace SQLite.Service.Service;

public class TableInformationsService
{

    public TableInformationsService()
    {

    }

    public IReadOnlyCollection<TableInformation> Get(DatabaseKey key)
    {
        return ConnectionHelper.TablesInformation(key.DatabasePath);
    }
}
