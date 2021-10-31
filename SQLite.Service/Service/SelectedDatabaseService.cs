using System.Data;
using Utility.Common.Base;
using Utility.Database;
using Utility.SQLite.Database;

namespace SQLite.Service
{
    public class SelectedDatabaseService : ISelectedDatabaseService
    {
        private SelectedTreeItem selectedTreeItem;

        private DatabasePath CurrentDatabasePath => ((selectedTreeItem.TreeItem.Key as DatabaseKey) ?? throw new Exception("Evfsd dsfd")).DatabasePath;

        public SelectedDatabaseService(ITreeItemChanges treeItemChanges)
        {
            treeItemChanges.Subscribe(a =>
            {
                selectedTreeItem = a;
            });
        }

        public DataTable SelectToDataTable(string selectQuery)
        {
            return new DatabaseHandler(CurrentDatabasePath).ExecuteAndLoadDataTable(selectQuery);
        }

        public IReadOnlyCollection<dynamic> SelectAsRows(string selectQuery)
        {
            return new DatabaseHandler(CurrentDatabasePath).ExecuteDynamicQuery(selectQuery);
        }
        public IReadOnlyCollection<T> SelectAsRows<T>(string selectQuery)
        {
            return new DatabaseHandler(CurrentDatabasePath).ExecuteQuery<T>(selectQuery);
        }
    }

    public static class ConnectionPathHelper
    {
        public static DataTable SelectToDataTable(this DatabasePath path, string selectQuery)
        {
            return new DatabaseHandler(path).ExecuteAndLoadDataTable(selectQuery);
        }

        public static IReadOnlyCollection<dynamic> SelectAsRows(this DatabasePath path, string selectQuery)
        {
            return new DatabaseHandler(path).ExecuteDynamicQuery(selectQuery);
        }
        public static IReadOnlyCollection<T> SelectAsRows<T>(this DatabasePath path, string selectQuery)
        {
            return new DatabaseHandler(path).ExecuteQuery<T>(selectQuery);
        }
    }
}
