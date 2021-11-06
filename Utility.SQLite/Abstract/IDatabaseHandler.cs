using System.Data;
using Utility.Database;
using Utility.Database.SQLite.Common.Models;
using Utility.SQLite.Models;

namespace Utility.SQLite.Database
{
    public interface IDatabaseHandler : IDisposable
    {
        string Name { get; }
        DatabasePath Path { get; }

        IEnumerable<Models.Index> Indexes { get; }

        IEnumerable<Table> Tables { get; }
        IEnumerable<Trigger> Triggers { get; }
        IEnumerable<View> Views { get; }

        DataTable ExecuteAndLoadDataTable(string sql);
        IReadOnlyCollection<dynamic> ExecuteDynamicQuery(string sql);
        int ExecuteNonQuery(string sql);
        IReadOnlyCollection<T> ExecuteQuery<T>(string sql);
        //IReadOnlyCollection<TableInformation> TablesInformation();
    }
}