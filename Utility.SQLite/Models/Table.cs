using Utility.Database;

namespace Utility.SQLite.Models
{
    public class Table
    {
        public DatabasePath DatabasePath { get; init; }

        public TableName Name { get; init; }

        public string CreateStatement { get; init; }
    }
}
