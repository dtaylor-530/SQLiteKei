using Utility.Database;

namespace SQLite.Common
{
    public interface IDatabaseViewModel : IName
    {
        DatabaseKey Key { get; }
    }
}