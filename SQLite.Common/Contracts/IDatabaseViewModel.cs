using Utility.Database;

namespace SQLite.Common
{
    public interface IDatabaseKey
    {
        DatabaseKey Key { get; }
    }

    public interface IDatabaseViewModel : IViewModel, IDatabaseKey
    {

    }
}