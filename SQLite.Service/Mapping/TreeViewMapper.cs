using SQLite.Common.Model;
using System.Collections.ObjectModel;
using Utility;
using Utility.Common.Base;
using Utility.Database;
using Utility.SQLite.Database;
using static Utility.Common.Base.Log;

namespace SQLite.Service.Mapping;

/// <summary>
/// A mapping class that opens a connection to the provided database and builds a hierarchical ViewModel structure.
/// </summary>
public class TreeViewMapper : ITreeViewMapper
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
    public TreeItem Map(IKey key)
    {
        if (key is not DatabaseKey { DatabasePath: { } path } dkey)
        {
            throw new Exception("sdfopopopofsd");
        }

        using var databaseHandler = new DatabaseHandler(path);
        var databaseItem = new DatabaseBranchItem(
            dkey,
            Path.GetFileNameWithoutExtension(databaseHandler.Path.Path),
            new ObservableCollection<DatabaseTreeItem>(FolderItems(localiser, databaseHandler, dkey)));

        Info("Loaded database " + databaseItem.Name + ".");

        return databaseItem;

        static FolderBranchItem[] FolderItems(ILocaliser localiser, DatabaseHandler dbHandler, DatabaseKey databaseKey) => new FolderBranchItem[]
        {
             new TableFolderItem(localiser["TreeItem_Tables"],databaseKey, new ObservableCollection<DatabaseTreeItem>(MapTables(dbHandler))){
              },
            new FolderBranchItem(localiser["TreeItem_Views"],databaseKey, new ObservableCollection<DatabaseTreeItem>(MapViews(dbHandler))){
               },
            new FolderBranchItem(localiser["TreeItem_Indexes"],databaseKey, new ObservableCollection<DatabaseTreeItem>(MapIndexes(dbHandler)) ){
                },
          new FolderBranchItem(localiser["TreeItem_Triggers"], databaseKey, new ObservableCollection<DatabaseTreeItem>(MapTriggers(dbHandler))){
            }
        };

        static IEnumerable<TableLeafItem> MapTables(DatabaseHandler dbHandler) => from table in dbHandler.Tables
                                                                                  select new TableLeafItem(table.Name, new TableKey<TableLeafItem>(dbHandler.Path, table.Name));

        static IEnumerable<IndexLeafItem> MapIndexes(DatabaseHandler dbHandler) => from indexName in dbHandler.Indexes.Select(x => x.Name)
                                                                                   select new IndexLeafItem(indexName, new TableKey<TableLeafItem>(dbHandler.Path, indexName));

        static IEnumerable<TriggerLeafItem> MapTriggers(DatabaseHandler dbHandler) => from triggerName in dbHandler.Triggers.Select(x => x.Name)
                                                                                      select new TriggerLeafItem(triggerName, new TableKey<TableLeafItem>(dbHandler.Path, triggerName));

        static IEnumerable<ViewLeafItem> MapViews(DatabaseHandler dbHandler) => from viewName in dbHandler.Views.Select(x => x.Name)
                                                                                select new ViewLeafItem(viewName, new TableKey<ViewLeafItem>(dbHandler.Path, viewName));
    }

}
