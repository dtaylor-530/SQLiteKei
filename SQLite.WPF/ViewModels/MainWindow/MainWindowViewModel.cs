using SQLiteKei.DataAccess.Database;
using SQLiteKei.Helpers;
using SQLiteKei.Helpers.Interfaces;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.DBTreeView.Mapping;

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using log = SQLiteKei.Helpers.Log;
using System.Linq;
using System.Windows.Controls;
using ReactiveUI;
using SQLite.WPF;
using System.Collections.Specialized;

namespace SQLiteKei.ViewModels.MainWindow
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly ITreeSaveHelper treeSaveHelper;
        private string statusBarInfo;
        private int selectedTabIndex;
        private ObservableCollection<TreeItem> treeViewItems;
        private object selectedItem;

        public MainWindowViewModel()
        {
            this.treeSaveHelper = new TreeSaveHelper();
            TreeViewItems = treeSaveHelper.Load();
            TabItems = new ObservableCollection<TabItem>();
        }

 

        public object SelectedItem { 
            get => selectedItem;             
            set {
                DBTreeView_SelectedItemChanged(value);
                selectedItem = value;                
            } 
        }

        public ObservableCollection<TabItem> TabItems { get; } 

        public ObservableCollection<TreeItem> TreeViewItems
        {
            get { return treeViewItems; }
            set { treeViewItems = value; }
        }

        public string StatusBarInfo
        {
            get { return statusBarInfo; }
            set { this.RaiseAndSetIfChanged(ref statusBarInfo, value); }
        }
        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set { this.RaiseAndSetIfChanged(ref selectedTabIndex, value); }
        }


        public void OpenDatabase(string databasePath)
        {
            if (TreeViewItems.Any(x => x.DatabasePath.Equals(databasePath)))
                return;

            var schemaMapper = new SchemaToViewModelMapper();
            DatabaseItem databaseItem = schemaMapper.MapSchemaToViewModel(databasePath);

            TreeViewItems.Add(databaseItem);

            log.Logger.Info("Opened database '" + databaseItem.DisplayName + "'.");
        }

        public void CloseDatabase(string databasePath)
        {
            var db = TreeViewItems.SingleOrDefault(x => x.DatabasePath.Equals(databasePath));
            TreeViewItems.Remove(db);

            log.Logger.Info("Closed database '" + db.DisplayName + "'.");
        }

        public void RemoveItemFromTree(TreeItem treeItem)
        {
            RemoveItemFromHierarchy(TreeViewItems, treeItem);
        }

        private void RemoveItemFromHierarchy(ICollection<TreeItem> treeItems, TreeItem treeItem)
        {
            foreach (var item in treeItems)
            {
                if (item == treeItem)
                {
                    treeItems.Remove(item);
                    log.Logger.Info(string.Format("Removed item of type {0} from tree hierarchy.", item.GetType()));
                    break;
                }

                var directory = item as DirectoryItem;

                if (directory != null && directory.Items.Any())
                {
                    RemoveItemFromHierarchy(directory.Items, treeItem);
                }
            }
        }

        public void RefreshTree()
        {
            log.Logger.Info("Refreshing the database tree.");
            var databasePaths = TreeViewItems.Select(x => x.DatabasePath).ToList();
            TreeViewItems.Clear();

            var schemaMapper = new SchemaToViewModelMapper();
            foreach (var path in databasePaths)
            {
                TreeViewItems.Add(schemaMapper.MapSchemaToViewModel(path));
            }
        }

        public void SaveTree()
        {
            treeSaveHelper.Save(TreeViewItems);
        }

        public void EmptyTable(string tableName)
        {
            var message = LocalisationHelper.GetString("MessageBox_EmptyTable", tableName);
            var messageTitle = LocalisationHelper.GetString("MessageBoxTitle_EmptyTable");
            var result = MessageBox.Show(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            using (var tableHandler = new TableHandler(SQLite.WPF.Settings.Default.CurrentDatabase))
            {
                try
                {
                    tableHandler.EmptyTable(tableName);
                }
                catch (Exception ex)
                {
                    log.Logger.Error("Failed to empty table" + tableName, ex);
                    StatusBarInfo = ex.Message;
                }
            }
        }

        public void DeleteTable(TableItem tableItem)
        {
            var message = LocalisationHelper.GetString("MessageBox_TableDeleteWarning", tableItem.DisplayName);
            var result = MessageBox.Show(message, LocalisationHelper.GetString("MessageBoxTitle_TableDeletion"), MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                using (var tableHandler = new TableHandler(SQLite.WPF.Settings.Default.CurrentDatabase))
                {
                    tableHandler.DropTable(tableItem.DisplayName);
                    RemoveItemFromTree(tableItem);
                }
            }
            catch (Exception ex)
            {
                log.Logger.Error("Failed to delete table '" + tableItem.DisplayName + "'.", ex);
                var statusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                StatusBarInfo = statusInfo;
            }
        }

        private void DBTreeView_SelectedItemChanged(object selectedItem)
        {
            SetGlobalDatabaseString();
            ResetTabControl();


            var tabs = DatabaseTabGenerator.GenerateTabsFor(selectedItem as TreeItem);

            foreach (TabItem tab in tabs)
                TabItems.Add(tab);


            void SetGlobalDatabaseString()
            {
                if (selectedItem != null)
                {
                    var currentSelection = (TreeItem)selectedItem;
                    Settings.Default.CurrentDatabase = currentSelection.DatabasePath;
                }
            }

            void ResetTabControl()
            {
                var openTabs = TabItems.Count;

                for (var i = openTabs - 1; i >= 0; i--)
                    TabItems.RemoveAt(i);

                var defaultTabs = DatabaseTabGenerator.DefaultTabs;

                foreach (TabItem tab in defaultTabs)
                    TabItems.Add(tab);

                SelectedTabIndex = 0;
            }

        }
    }
}
