using Database.Entity;
using SQLite.Common;
using SQLite.Common.Contracts;
using Utility.Common.Base;
using Utility.Database.Common;
using Utility.Entity;

namespace SQLite.Service.Factory;

/// <summary>
/// A class that generates tabs for the currently selected tree item in the main tree.
/// </summary>
public class TabsFactory : ITabsFactory
{
    private readonly IViewModelFactory factory;

    public TabsFactory(IViewModelFactory factory)
    {
        this.factory = factory;
    }

    public IEnumerable<IViewModel> TabsFor(IReadOnlyCollection<IKey> keys)
    {
        foreach (var key in keys)
        {
            yield return TabFor(key);
        }
    }

    public IViewModel TabFor(IKey key)
    {
        return factory.Build(key);

    }

    /// <summary>
    /// Generates the tabs for the specified tree item depending on its type.
    /// </summary>
    /// <param name="treeItem">The tree item.</param>
    /// <returns></returns>
    public IEnumerable<IViewModel> TabsFor(IKey? treeItem)
    {
        return treeItem switch

        {
            TableKey tKey => TableTabs(tKey),
            DatabaseKey dKey => DatabaseTabs(dKey),
            _ => DefaultTabs(),
        };

        IEnumerable<IViewModel> DatabaseTabs(IDatabaseKey key)
        {
            yield return
                factory.Build(new DatabaseGeneralViewModelTabKey(key.DatabasePath));
        }

        IEnumerable<IViewModel> TableTabs(ITableKey key)
        {
            yield return
                factory.Build(new TableGeneralViewModelTabKey(key.DatabasePath, key.TableName));

            yield return
                factory.Build(new TableRecordsViewModelTabKey(key.DatabasePath, key.TableName));

            yield return
                factory.Build(new TableChartViewModelTabKey(key.DatabasePath, key.TableName));
        }

        IEnumerable<IViewModel> DefaultTabs() => Array.Empty<IViewModel>();
    }
}
