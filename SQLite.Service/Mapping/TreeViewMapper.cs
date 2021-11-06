using Database.Entity;
using SQLite.Utility.Factory;
using System.Collections.ObjectModel;
using Utility.Common.Base;
using Utility.Entity;
using Utility.SQLite.Database;
using static Utility.Common.Base.Log;

namespace SQLite.Service.Mapping;

/// <summary>
/// A mapping class that opens a connection to the provided database and builds a hierarchical ViewModel structure.
/// </summary>
public class TreeViewMapper : ITreeViewMapper
{
    private readonly ILocaliser localiser;
    private readonly IHandlerService handlerFactory;

    public TreeViewMapper(ILocaliser localiser, IHandlerService handlerFactory)
    {
        this.localiser = localiser;
        this.handlerFactory = handlerFactory;
    }

    /// <summary>
    /// Maps the provided database to a hierarchical ViewModel structure with a DatabaseItem as its root.
    /// </summary>
    /// <param name="databasePath">The database path.</param>
    /// <returns></returns>
    public TreeItem Map(Key key)
    {
        if (key is not DatabaseKey { DatabasePath: { } path } dkey)
        {
            throw new Exception("sdfopopopofsd");
        }

        var databaseItem = handlerFactory.Database(dkey, handler =>
        {
            var databaseItem = new DatabaseBranchItem(
                dkey,
                Path.GetFileNameWithoutExtension(handler.Path.Path),
                new ObservableCollection<DatabaseTreeItem>(FolderItems(localiser, handler, dkey)));
            return databaseItem;
        });

        Info("Loaded database " + databaseItem.Name + ".");

        return databaseItem;

        static FolderBranchItem[] FolderItems(ILocaliser localiser, IDatabaseHandler dbHandler, DatabaseKey databaseKey) => new FolderBranchItem[]
        {
             new TableFolderItem(localiser["TreeItem_Tables"],databaseKey, new ObservableCollection<DatabaseTreeItem>(MapTables(dbHandler))){
              },
             new FolderBranchItem(localiser["TreeItem_Views"],databaseKey, new ObservableCollection<DatabaseTreeItem>(MapViews(dbHandler))){
              },
            new FolderBranchItem(localiser["TreeItem_Indexes"],databaseKey, new ObservableCollection<DatabaseTreeItem>(MapIndexes(dbHandler))){
              },
            new FolderBranchItem(localiser["TreeItem_Triggers"], databaseKey, new ObservableCollection<DatabaseTreeItem>(MapTriggers(dbHandler))){
              }
        };

        static IEnumerable<TableLeafItem> MapTables(IDatabaseHandler dbHandler) => from table in dbHandler.Tables
                                                                                   select new TableLeafItem(table.Name, new TableKey<TableLeafItem>(dbHandler.Path, table.Name));

        static IEnumerable<IndexLeafItem> MapIndexes(IDatabaseHandler dbHandler) => from indexName in dbHandler.Indexes.Select(x => x.Name)
                                                                                    select new IndexLeafItem(indexName, new TableKey<TableLeafItem>(dbHandler.Path, indexName));

        static IEnumerable<TriggerLeafItem> MapTriggers(IDatabaseHandler dbHandler) => from triggerName in dbHandler.Triggers.Select(x => x.Name)
                                                                                       select new TriggerLeafItem(triggerName, new TableKey<TableLeafItem>(dbHandler.Path, triggerName));

        static IEnumerable<ViewLeafItem> MapViews(IDatabaseHandler dbHandler) => from viewName in dbHandler.Views.Select(x => x.Name)
                                                                                 select new ViewLeafItem(viewName, new TableKey<ViewLeafItem>(dbHandler.Path, viewName));
    }

}
