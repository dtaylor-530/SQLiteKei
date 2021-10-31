using Utility;
using Utility.Common.Base;

namespace SQLite.Common.Contracts
{
    public interface ITabsFactory
    {
        IViewModel TabFor(IKey key);
        IEnumerable<IViewModel> TabsFor(IKey treeItem);
        IEnumerable<IViewModel> TabsFor(IReadOnlyCollection<IKey> keys);
    }
}