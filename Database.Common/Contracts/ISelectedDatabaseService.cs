using System.Data;

namespace Database.Common.Contracts
{
    public interface ISelectedDatabaseService
    {
        IObservable<IReadOnlyCollection<dynamic>> SelectAsRows(string selectQuery);
        IObservable<IReadOnlyCollection<T>> SelectAsRows<T>(string selectQuery);
        IObservable<DataTable> SelectToDataTable(string selectQuery);
    }
}