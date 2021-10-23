using Utility.Database;

namespace SQLite.Common.Contracts
{
    public interface IConnector
    {
        DatabasePath ConnectionPath { get; set; }
    }
}
