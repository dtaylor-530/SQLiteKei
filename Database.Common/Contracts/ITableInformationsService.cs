using Database.Entity;
using Utility.Database;

namespace SQLite.Common.Contracts
{
    public interface ITableInformationsService
    {
        IObservable<IObservable<TableInformation>> Get(DatabaseKey key);
    }
}