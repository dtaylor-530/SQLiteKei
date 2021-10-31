using System.Data;

namespace SQLite.Service
{
    public interface ISelectedDatabaseService
    {
        IReadOnlyCollection<dynamic> SelectAsRows(string selectQuery);
        IReadOnlyCollection<T> SelectAsRows<T>(string selectQuery);
        DataTable SelectToDataTable(string selectQuery);
    }
}