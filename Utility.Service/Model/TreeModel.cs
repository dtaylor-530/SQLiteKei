using DynamicData;
using ReactiveUI;
using SQLite.Service.Mapping;
using System.Collections.ObjectModel;
using System.Data;
using System.Reactive.Subjects;
using Utility.Common.Base;
using Utility.Entity;
using static Utility.Common.Base.Log;

namespace Utility.Service
{
    public class TreeModel : ReactiveObject, ITreeModel
    {
        private readonly ITreeRepository treeRepository;
        private readonly ITreeViewMapper treeViewMapper;
        private readonly ReplaySubject<SelectedTreeItem> subject = new(1);
        private readonly ReplaySubject<TreeItemChangeRequest> replaySubject = new(1);
        private Lazy<ObservableCollection<TreeItem>> lazyTreeViewItems;
        private ObservableCollection<TreeItem> treeViewItems => lazyTreeViewItems.Value;
        private TreeItem selectedItem;

        public TreeModel(ITreeRepository treeRepository, ITreeViewMapper treeViewMapper)
        {
            this.treeRepository = treeRepository;
            this.treeViewMapper = treeViewMapper;
            lazyTreeViewItems = new(() => new ObservableCollection<TreeItem>(treeRepository.Load()));

            replaySubject
                .Subscribe(a =>
                {
                    if (a.Adds != null)
                        foreach (var add in a.Adds)
                        {
                            treeViewItems.Add(add);
                        }
                    if (a.Removes != null)
                        foreach (var remove in a.Removes)
                        {
                            Remove(remove);
                        }
                    if (a.Refresh != null)
                    {
                        Refresh();
                    }
                });
        }

        public IReadOnlyCollection<TreeItem> TreeViewItems => treeViewItems;

        public TreeItem SelectedItem
        {
            get => selectedItem;
            set
            {
                subject.OnNext(new(selectedItem = value));
            }
        }

        private void Refresh()
        {
            Info("Refreshing the database tree.");
            treeViewItems.Clear();
            treeViewItems.AddRange(TreeViewItems.Select(a => RecreateItems(a.Key)));

            TreeItem RecreateItems(IKey key)
            {
                return treeViewMapper.Map(key);

            }
        }

        private void Remove(IKey key)
        {
            RemoveItemFromHierarchy(treeViewItems, key);

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

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(TreeItemChangeRequest value)
        {
            replaySubject.OnNext(value);
        }
    }
}
