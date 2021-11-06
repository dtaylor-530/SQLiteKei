using Database.Entity;
using SQLite.Utility.Factory;
using System.Data;
using Utility.Common.Base;

namespace SQLite.Service
{
    public class SelectedDatabaseService : ISelectedDatabaseService
    {
        private SelectedTreeItem selectedTreeItem;
        private readonly IHandlerService databaseHandlerFactory;

        // private DatabasePath CurrentDatabasePath => ((selectedTreeItem.TreeItem.Key as DatabaseKey) ?? throw new Exception("Evfsd dsfd")).DatabasePath;
        private DatabaseKey DatabaseKey => (selectedTreeItem.TreeItem.Key as DatabaseKey) ?? throw new Exception("Evfsd dsfd");

        public SelectedDatabaseService(ITreeItemChanges treeItemChanges, IHandlerService databaseHandlerFactory)
        {
            treeItemChanges.Subscribe(a =>
            {
                selectedTreeItem = a;
            });
            this.databaseHandlerFactory = databaseHandlerFactory;
        }

        public DataTable SelectToDataTable(string selectQuery)
        {
            return databaseHandlerFactory.Database(DatabaseKey, handler => handler.ExecuteAndLoadDataTable(selectQuery));
        }

        public IReadOnlyCollection<dynamic> SelectAsRows(string selectQuery)
        {
            return databaseHandlerFactory.Database(DatabaseKey, handler => handler.ExecuteDynamicQuery(selectQuery));
        }
        public IReadOnlyCollection<T> SelectAsRows<T>(string selectQuery)
        {
            return databaseHandlerFactory.Database(DatabaseKey, handler => handler.ExecuteQuery<T>(selectQuery));
        }
    }

    //public static class ConnectionPathHelper
    //{
    //    public static DataTable SelectToDataTable(this DatabasePath path, string selectQuery)
    //    {
    //        return new DatabaseHandler(path).ExecuteAndLoadDataTable(selectQuery);
    //    }

    //    public static IReadOnlyCollection<dynamic> SelectAsRows(this DatabasePath path, string selectQuery)
    //    {
    //        return new DatabaseHandler(path).ExecuteDynamicQuery(selectQuery);
    //    }
    //    public static IReadOnlyCollection<T> SelectAsRows<T>(this DatabasePath path, string selectQuery)
    //    {
    //        return new DatabaseHandler(path).ExecuteQuery<T>(selectQuery);
    //    }
    //}
}
