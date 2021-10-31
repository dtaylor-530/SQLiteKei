using Utility.Database;
using Utility.SQLite.Helpers;

namespace SQLite.Common.Contracts
{
    public interface ITableInformationsService
    {
        IReadOnlyCollection<TableInformation> Get(DatabaseKey key);
    }
}