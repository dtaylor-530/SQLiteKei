using SQLite.Common;
using SQLite.Common.Contracts;

namespace SQLite.ViewModel.Infrastructure.Factory;

/// <summary>
/// A class that generates tabs for the currently selected tree item in the main tree.
/// </summary>
public class TabsFactory
{
    private readonly ILocaliser localiser;
    private readonly ViewModelFactory factory;

    public TabsFactory(ILocaliser localiser, ViewModelFactory factory)
    {
        this.localiser = localiser;
        this.factory = factory;
    }

    /// <summary>
    /// Generates the tabs for the specified tree item depending on its type.
    /// </summary>
    /// <param name="treeItem">The tree item.</param>
    /// <returns></returns>
    public IEnumerable<HeaderContent> TabsFor(TreeItem? treeItem)
    {
        return treeItem switch
        {
            DatabaseItem dItem => DatabaseTabs(dItem),
            TableItem tItem => TableTabs(tItem),
            _ => DefaultTabs(),
        };

        IEnumerable<HeaderContent> DatabaseTabs(DatabaseItem databaseItem)
        {
            yield return new HeaderContent(
                databaseItem.DisplayName,
                factory.Build<DatabaseGeneralViewModel>(new DatabaseGeneralConfiguration(databaseItem.DatabasePath)));

        }

        IEnumerable<HeaderContent> TableTabs(TableItem tableItem)
        {
            yield return new HeaderContent(
                localiser["TabHeader_GeneralTable", tableItem.DisplayName],
                factory.Build<TableGeneralViewModel>(new TableGeneralConfiguration(tableItem.DisplayName, tableItem.DatabasePath)));

            yield return new HeaderContent(
                localiser["TabHeader_TableRecords"],
                factory.Build<TableRecordsViewModel>(new TableRecordsConfiguration(tableItem.DisplayName, tableItem.DatabasePath)));

            yield return new HeaderContent(
                "chart",
                factory.Build<TableChartViewModel>(new TableChartConfiguration(tableItem.DisplayName, tableItem.DatabasePath)));
        }

        IEnumerable<HeaderContent> DefaultTabs() => Array.Empty<HeaderContent>();
    }
}
