using DynamicData;
using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.Service.Mapping;
using System.Collections.ObjectModel;
using System.Data;
using System.Reactive.Subjects;
using Utility.Common.Base;
using static Utility.Common.Base.Log;

namespace Utility.Service
{
    public class TreeService : ReactiveObject, ITreeService
    {
        private readonly ITreeRepository treeRepository;
        private readonly ITreeViewMapper treeViewMapper;
        private readonly ReplaySubject<SelectedTreeItem> subject = new(1);

        private ObservableCollection<TreeItem> treeViewItems;
        private TreeItem selectedItem;

        public TreeService(ITreeRepository treeRepository, ITreeViewMapper treeViewMapper)
        {
            this.treeRepository = treeRepository;
            this.treeViewMapper = treeViewMapper;
        }

        public ObservableCollection<TreeItem> TreeViewItems
        {
            get { return treeViewItems ??= new ObservableCollection<TreeItem>(treeRepository.Load()); }
        }

        public TreeItem SelectedItem
        {
            get => selectedItem;
            set
            {
                subject.OnNext(new(selectedItem = value));
            }
        }

        public void RefreshTree()
        {
            Info("Refreshing the database tree.");
            TreeViewItems.Clear();
            TreeViewItems.AddRange(TreeViewItems.Select(a => RecreateItems(a.Key)));

            TreeItem RecreateItems(IKey key)
            {
                return treeViewMapper.Map(key);

            }
        }

        public void RemoveItemFromTree(IKey key)
        {
            RemoveItemFromHierarchy(TreeViewItems, key);

            static void RemoveItemFromHierarchy(ICollection<TreeItem> treeItems, IKey key)
            {
                foreach (var item in treeItems)
                {
                    if (item.Key == key)
                    {
                        treeItems.Remove(item);
                        Info(string.Format("Removed item of type {0} from tree hierarchy.", item.GetType()));
                        break;
                    }

                    if (item is BranchItem directory && directory.Items.Any())
                    {
                        RemoveItemFromHierarchy(directory.Items, key);
                    }
                }
            }
        }

        public void SaveTree()
        {
            treeRepository.Save(TreeViewItems);
        }

        public IDisposable Subscribe(IObserver<SelectedTreeItem> observer)
        {
            return subject.Subscribe(observer);
        }
    }
}
