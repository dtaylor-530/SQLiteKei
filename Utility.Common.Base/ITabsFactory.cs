using Utility.Common.Base;
using Utility.Entity;

namespace SQLite.Common.Contracts
{
    public interface ITabsFactory
    {
        IViewModel TabFor(IKey key);
        IEnumerable<IViewModel> TabsFor(IKey treeItem);
        IEnumerable<IViewModel> TabsFor(IReadOnlyCollection<IKey> keys);
    }
}