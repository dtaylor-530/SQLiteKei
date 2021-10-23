using SQLite.Common;
using SQLite.Common.Contracts;
using SQLite.Service.Model;
using SQLite.ViewModel;
using SQLite.ViewModel.Infrastructure;
using SQLite.ViewModel.Infrastructure.Factory;
using Utility.Database;

namespace SQLite.Service.Factory;

/// <summary>
/// A class that generates tabs for the currently selected tree item in the main tree.
/// </summary>
public class TabsFactory : ITabsFactory
{
    private readonly ViewModelFactory factory;

    public TabsFactory(ViewModelFactory factory)
    {
        this.factory = factory;
    }

    public IEnumerable<IDatabaseViewModel> TabsFor(IReadOnlyCollection<DatabaseKey> keys)
    {
        foreach (var key in keys)
        {
            yield return TabFor(key);
        }
    }

    public IDatabaseViewModel TabFor(DatabaseKey key)
    {
        return key switch
        {
            TableGeneralViewModelTabKey tItem => factory.Build<TableGeneralViewModel>(tItem),
            TableRecordsViewModelTabKey tItem => factory.Build<TableRecordsViewModel>(tItem),
            TableChartViewModelTabKey tItem => factory.Build<TableChartViewModel>(tItem),
            DatabaseGeneralViewModelTabKey dItem => factory.Build<DatabaseGeneralViewModel>(dItem),
            _ => throw new NotImplementedException("DF33 fdfdd"),
        };
    }

    /// <summary>
    /// Generates the tabs for the specified tree item depending on its type.
    /// </summary>
    /// <param name="treeItem">The tree item.</param>
    /// <returns></returns>
    public IEnumerable<IDatabaseViewModel> TabsFor(DatabaseTreeItem? treeItem)
    {
        return treeItem switch
        {
            DatabaseBranchItem dItem => DatabaseTabs(dItem.Key),
            TableLeafItem tItem => TableTabs(tItem.Key),
            _ => DefaultTabs(),
        };

        IEnumerable<DatabaseViewModel> DatabaseTabs(DatabaseKey key)
        {
            yield return
                factory.Build<DatabaseGeneralViewModel>(new DatabaseGeneralViewModelTabKey(key.DatabasePath));
        }

        IEnumerable<DatabaseViewModel> TableTabs(TableKey key)
        {
            yield return
                factory.Build<TableGeneralViewModel>(new TableGeneralViewModelTabKey(key.DatabasePath, key.TableName));

            yield return
                factory.Build<TableRecordsViewModel>(new TableRecordsViewModelTabKey(key.DatabasePath, key.TableName));

            yield return
                factory.Build<TableChartViewModel>(new TableChartViewModelTabKey(key.DatabasePath, key.TableName));
        }

        IEnumerable<DatabaseViewModel> DefaultTabs() => Array.Empty<DatabaseViewModel>();
    }
}
