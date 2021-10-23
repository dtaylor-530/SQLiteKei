using DynamicData;
using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.Service.Mapping;
using SQLite.Service.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Reactive.Subjects;
using Utility.Database;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.Service.Service
{
    public static class TreeServiceHelper
    {
        public static DatabasePath CurrentDatabasePath(this DatabaseService treeService)
        {
            return new DatabasePath(treeService.CurrentDatabasePath.FullName);
        }

        public static DataTable SelectCurrentToDataTable(this DatabaseService treeService, string selectQuery)
        {
            return ConnectionPathHelper.SelectToDataTable(treeService.CurrentDatabasePath(), selectQuery);
        }

        public static IReadOnlyCollection<dynamic> SelectCurrentAsRows(this DatabaseService treeService, string selectQuery)
        {
            return ConnectionPathHelper.SelectAsRows(treeService.CurrentDatabasePath(), selectQuery);
        }
        public static IReadOnlyCollection<T> SelectCurrentAsRows<T>(this DatabaseService treeService, string selectQuery)
        {
            return ConnectionPathHelper.SelectAsRows<T>(treeService.CurrentDatabasePath(), selectQuery);
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

    public interface ITreeItemChanges : IObservable<DatabaseTreeItem>
    {
    }

    public class TreeService : ReactiveObject, ITreeItemChanges
    {
        private readonly ITreeRepository treeRepository;
        private readonly ILocaliser localiser;
        private readonly TreeViewMapper treeViewMapper;
        private ObservableCollection<DatabaseTreeItem> treeViewItems;
        private DatabaseTreeItem selectedItem;
        readonly ReplaySubject<DatabaseTreeItem> subject = new(1);

        public TreeService(ITreeRepository treeSaveHelper, ILocaliser localiser, TreeViewMapper treeViewMapper)
        {
            this.treeRepository = treeSaveHelper;
            this.localiser = localiser;

            this.treeViewMapper = treeViewMapper;
        }

        public ObservableCollection<DatabaseTreeItem> TreeViewItems
        {
            get { return treeViewItems ??= new ObservableCollection<DatabaseTreeItem>(treeRepository.Load()); }
        }

        public DatabaseTreeItem SelectedItem
        {
            get => selectedItem;
            set
            {
                subject.OnNext(selectedItem = value);
            }
        }

        public void RefreshTree()
        {
            Info("Refreshing the database tree.");
            var databasePaths = TreeViewItems.OfType<DatabaseBranchItem>().Select(x => new DatabaseBranchItem(x.Name, x.Key, null)).ToArray();
            TreeViewItems.Clear();
            TreeViewItems.AddRange(TreeViewItems.Select(a => RecreateItems(a.Key)));

            DatabaseBranchItem RecreateItems(DatabaseKey key)
            {

                return treeViewMapper.Map(key.DatabasePath);

            }
        }

        public void RemoveItemFromTree(DatabaseTreeItem treeItem)
        {
            RemoveItemFromHierarchy(TreeViewItems, treeItem);

            static void RemoveItemFromHierarchy(ICollection<DatabaseTreeItem> treeItems, DatabaseTreeItem treeItem)
            {
                foreach (var item in treeItems)
                {
                    if (item == treeItem)
                    {
                        treeItems.Remove(item);
                        Info(string.Format("Removed item of type {0} from tree hierarchy.", item.GetType()));
                        break;
                    }

                    if (item is BranchItem directory && directory.Items.Any())
                    {
                        RemoveItemFromHierarchy(directory.Items, treeItem);
                    }
                }
            }
        }

        public void SaveTree()
        {
            treeRepository.Save(TreeViewItems);
        }

        public IDisposable Subscribe(IObserver<DatabaseTreeItem> observer)
        {
            return subject.Subscribe(observer);
        }
    }
}
