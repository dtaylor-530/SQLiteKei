using System.Data;

namespace Database.Common.Contracts
{
    public interface ISelectedDatabaseService
    {
        IReadOnlyCollection<dynamic> SelectAsRows(string selectQuery);
        IReadOnlyCollection<T> SelectAsRows<T>(string selectQuery);
        DataTable SelectToDataTable(string selectQuery);
    }
}