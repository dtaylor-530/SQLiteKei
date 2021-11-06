using Database.Common.Contracts;
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

        public IObservable<DataTable> SelectToDataTable(string selectQuery)
        {
            return databaseHandlerFactory.Database(DatabaseKey, handler => handler.ExecuteAndLoadDataTable(selectQuery));
        }

        public IObservable<IReadOnlyCollection<dynamic>> SelectAsRows(string selectQuery)
        {
            return databaseHandlerFactory.Database(DatabaseKey, handler => handler.ExecuteDynamicQuery(selectQuery));
        }
        public IObservable<IReadOnlyCollection<T>> SelectAsRows<T>(string selectQuery)
        {
            return databaseHandlerFactory.Database(DatabaseKey, handler => handler.ExecuteQuery<T>(selectQuery));
        }
    }
}
