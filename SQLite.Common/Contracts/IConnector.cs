using Utility.Database;

namespace SQLite.Common.Contracts
{
    public interface IConnector
    {
        ConnectionPath ConnectionPath { get; set; }
    }
}
