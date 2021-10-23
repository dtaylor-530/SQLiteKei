using SQLite.Common.Contracts;
using SQLite.Service.Model;
using System.Collections.ObjectModel;
using Utility.Database;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.Service.Mapping;

/// <summary>
/// A mapping class that opens a connection to the provided database and builds a hierarchical ViewModel structure.
/// </summary>
public class TreeViewMapper
{
    private readonly ILocaliser localiser;

    public TreeViewMapper(ILocaliser localiser)
    {
        this.localiser = localiser;
    }

    /// <summary>
    /// Maps the provided database to a hierarchical ViewModel structure with a DatabaseItem as its root.
    /// </summary>
    /// <param name="databasePath">The database path.</param>
    /// <returns></returns>
    public DatabaseBranchItem Map(DatabasePath map)
    {

        using var databaseHandler = new DatabaseHandler(map);
        var databaseItem = new DatabaseBranchItem(
            Path.GetFileNameWithoutExtension(databaseHandler.Path.Path),
            new DatabaseKey(databaseHandler.Path),
            new ObservableCollection<DatabaseTreeItem>(FolderItems(localiser, databaseHandler)));

        Info("Loaded database " + databaseItem.Name + ".");

        return databaseItem;

        static FolderBranchItem[] FolderItems(ILocaliser localiser, DatabaseHandler dbHandler) => new FolderBranchItem[]
        {
             new TableFolderItem(localiser["TreeItem_Tables"],new DatabaseKey(dbHandler.Path), new ObservableCollection<DatabaseTreeItem>(MapTables(dbHandler))){
              },
            new FolderBranchItem(localiser["TreeItem_Views"],new DatabaseKey(dbHandler.Path), new ObservableCollection<DatabaseTreeItem>(MapViews(dbHandler))){
               },
            new FolderBranchItem(localiser["TreeItem_Indexes"],new DatabaseKey(dbHandler.Path), new ObservableCollection<DatabaseTreeItem>(MapIndexes(dbHandler)) ){
                },
          new FolderBranchItem(localiser["TreeItem_Triggers"],new DatabaseKey(dbHandler.Path), new ObservableCollection<DatabaseTreeItem>(MapTriggers(dbHandler))){
            }
        };

        static IEnumerable<TableLeafItem> MapTables(DatabaseHandler dbHandler) => from table in dbHandler.Tables
                                                                                  select new TableLeafItem(table.Name, new TableKey(dbHandler.Path, table.Name));

        static IEnumerable<IndexLeafItem> MapIndexes(DatabaseHandler dbHandler) => from indexName in dbHandler.Indexes.Select(x => x.Name)
                                                                                   select new IndexLeafItem(indexName, new TableKey(dbHandler.Path, indexName));

        static IEnumerable<TriggerLeafItem> MapTriggers(DatabaseHandler dbHandler) => from triggerName in dbHandler.Triggers.Select(x => x.Name)
                                                                                      select new TriggerLeafItem(triggerName, new TableKey(dbHandler.Path, triggerName));

        static IEnumerable<ViewLeafItem> MapViews(DatabaseHandler dbHandler) => from viewName in dbHandler.Views.Select(x => x.Name)
                                                                                select new ViewLeafItem(viewName, new TableKey(dbHandler.Path, viewName));
    }

}
