using DynamicData;
using ReactiveUI;
using SQLite.Common.Contracts;
using System.Collections.ObjectModel;
using System.Data;
using Utility.Database;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.ViewModel.Infrastructure.Service
{
    public static class TreeServiceHelper
    {
        public static ConnectionPath CurrentDatabasePath(this TreeService treeService)
        {
            return treeService.SelectedItem.DatabasePath;
        }

        public static DataTable SelectCurrentToDataTable(this TreeService treeService, string selectQuery)
        {
            return new DatabaseHandler(treeService.CurrentDatabasePath()).ExecuteAndLoadDataTable(selectQuery);
        }

        public static IReadOnlyCollection<dynamic> SelectCurrentAsRows(this TreeService treeService, string selectQuery)
        {
            return new DatabaseHandler(treeService.CurrentDatabasePath()).ExecuteDynamicQuery(selectQuery);
        }
        public static IReadOnlyCollection<T> SelectCurrentAsRows<T>(this TreeService treeService, string selectQuery)
        {
            return new DatabaseHandler(treeService.CurrentDatabasePath()).ExecuteQuery<T>(selectQuery);
        }

    }

    public class TreeService : ReactiveObject
    {
        private readonly ITreeRepository treeSaveHelper;
        private readonly ILocaliser localiser;
        private ObservableCollection<TreeItem> treeViewItems;
        private TreeItem selectedItem;

        public TreeService(ITreeRepository treeSaveHelper, ILocaliser localiser)
        {
            this.treeSaveHelper = treeSaveHelper;
            this.localiser = localiser;

        }

        public ObservableCollection<TreeItem> TreeViewItems
        {
            get { return treeViewItems ??= new ObservableCollection<TreeItem>(treeSaveHelper.Load()); }
        }

        public TreeItem SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        public void RefreshTree()
        {
            Info("Refreshing the database tree.");
            var databasePaths = TreeViewItems.Select(x => x.DatabasePath).ToArray();
            TreeViewItems.Clear();
            TreeViewItems.AddRange(RecrateItems(databasePaths));
        }

        private IEnumerable<DatabaseItem> RecrateItems(ConnectionPath[] databasePaths)
        {
            foreach (var path in databasePaths)
            {
                using var databaseHandler = new DatabaseHandler(path);
                yield return SchemaToViewModelMapper.Map(localiser, databaseHandler);
            }
        }

        public void RemoveItemFromTree(TreeItem treeItem)
        {
            RemoveItemFromHierarchy(TreeViewItems, treeItem);

            static void RemoveItemFromHierarchy(ICollection<TreeItem> treeItems, TreeItem treeItem)
            {
                foreach (var item in treeItems)
                {
                    if (item == treeItem)
                    {
                        treeItems.Remove(item);
                        Info(string.Format("Removed item of type {0} from tree hierarchy.", item.GetType()));
                        break;
                    }

                    if (item is DirectoryItem directory && directory.Items.Any())
                    {
                        RemoveItemFromHierarchy(directory.Items, treeItem);
                    }
                }
            }
        }

        public void SaveTree()
        {
            treeSaveHelper.Save(TreeViewItems);
        }

    }
}
