using Utility.Database;

namespace SQLite.Common.Contracts
{
    public interface ITabsFactory
    {
        IDatabaseViewModel TabFor(DatabaseKey key);
        IEnumerable<IDatabaseViewModel> TabsFor(DatabaseTreeItem? treeItem);
        IEnumerable<IDatabaseViewModel> TabsFor(IReadOnlyCollection<DatabaseKey> keys);
    }
}