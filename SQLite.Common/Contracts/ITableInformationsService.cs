using Database.Entity;
using Utility.Database;

namespace SQLite.Common.Contracts
{
    public interface ITableInformationsService
    {
        IReadOnlyCollection<TableInformation> Get(DatabaseKey key);
    }
}