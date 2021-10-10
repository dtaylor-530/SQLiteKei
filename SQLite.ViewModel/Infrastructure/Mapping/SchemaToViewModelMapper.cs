using SQLite.Common.Contracts;
using SQLite.Data;

using System.Collections.ObjectModel;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.ViewModel;

/// <summary>
/// A mapping class that opens a connection to the provided database and builds a hierarchical ViewModel structure.
/// </summary>
internal class SchemaToViewModelMapper
{
    /// <summary>
    /// Maps the provided database to a hierarchical ViewModel structure with a DatabaseItem as its root.
    /// </summary>
    /// <param name="databasePath">The database path.</param>
    /// <returns></returns>
    public static DatabaseItem MapSchemaToViewModel(ILocaliser localiser, DatabaseHandler dbHandler)
    {
        //log.Logger.Info("Trying to load database file at " + databasePath);
        //this.databasePath = databasePath;
        //dbHandler = new DatabaseHandler(databasePath);

        var databaseItem = new DatabaseItem(Path.GetFileNameWithoutExtension(dbHandler.Path.Path), dbHandler.Path)
        {
            Items = new ObservableCollection<TreeItem>(FolderItems(localiser, dbHandler))
        };

        Info("Loaded database " + databaseItem.DisplayName + ".");

        return databaseItem;
    }

    private static FolderItem[] FolderItems(ILocaliser localiser, DatabaseHandler dbHandler) => new FolderItem[]
    {
             new TableFolderItem(localiser["TreeItem_Tables"],dbHandler.Path){
                Items = new ObservableCollection<TreeItem>(MapTables(dbHandler)) },
            new FolderItem(localiser["TreeItem_Views"],dbHandler.Path){
                Items = new ObservableCollection<TreeItem>(MapViews(dbHandler)) },
            new FolderItem(localiser["TreeItem_Indexes"],dbHandler.Path){
                Items = new ObservableCollection<TreeItem>(MapIndexes(dbHandler)) },
          new FolderItem(localiser["TreeItem_Triggers"],dbHandler.Path){
                Items = new ObservableCollection<TreeItem>(MapTriggers(dbHandler)) }
    };


    private static IEnumerable<TableItem> MapTables(DatabaseHandler dbHandler) => from table in dbHandler.Tables
                                                                                  select new TableItem(table.Name, dbHandler.Path);


    private static IEnumerable<IndexItem> MapIndexes(DatabaseHandler dbHandler) => from string indexName in dbHandler.Indexes.Select(x => x.Name)
                                                                                   select new IndexItem(indexName, dbHandler.Path);

    private static IEnumerable<TriggerItem> MapTriggers(DatabaseHandler dbHandler) => from string triggerName in dbHandler.Triggers.Select(x => x.Name)
                                                                                      select new TriggerItem(triggerName, dbHandler.Path);


    private static IEnumerable<ViewItem> MapViews(DatabaseHandler dbHandler) => from string viewName in dbHandler.Views.Select(x => x.Name)
                                                                                select new ViewItem(viewName, dbHandler.Path);

}
