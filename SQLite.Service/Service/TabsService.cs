using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.Service.Repository;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using Utility.Common.Base;

namespace SQLite.Service.Service
{
    public class TabsService : ReactiveObject, IObservable<int>, ITabsService
    {
        readonly ReplaySubject<int> selectedIndex = new(1);
        private readonly TabsRepository tabsRepository;

        public TabsService(TabsRepository tabsRepository, ITreeItemChanges treeService, ITabsFactory tabsFactory)
        {
            this.tabsRepository = tabsRepository;
            treeService
                .Subscribe(a =>
                {
                    ResetTabs(a.TreeItem, tabsRepository, tabsFactory);
                });

            void ResetTabs(TreeItem selectedItem, TabsRepository tabsRepository, ITabsFactory tabsFactory)
            {
                ResetTabControl();
                ReaddTabs();

                void ResetTabControl()
                {
                    var openTabs = TabItems.Count;

                    for (var i = openTabs - 1; i >= 0; i--)
                        TabItems.RemoveAt(i);

                    var defaultTabs = tabsRepository.Load(null);

                    foreach (var tab in defaultTabs)
                        TabItems.Add(tabsFactory.TabFor(tab));

                    selectedIndex.OnNext(0);
                }

                void ReaddTabs()
                {
                    IReadOnlyCollection<IViewModel>? tabs = null;
                    if (tabsRepository.Load(selectedItem) is { } loadedKeys)
                    {
                        tabs = tabsFactory.TabsFor(loadedKeys).ToArray();
                    }
                    else
                    {
                        tabs = tabsFactory.TabsFor(selectedItem.Key).ToArray();
                        tabsRepository.Save(selectedItem, tabs.Select(a => a.Key).ToList());
                    }

                    foreach (var tab in tabs)
                    {
                        TabItems.Add(tab);
                    }
                }
            }
        }

        internal void SaveTabs()
        {
            tabsRepository.PersistAll();
        }

        public ObservableCollection<IViewModel> TabItems { get; } = new ObservableCollection<IViewModel>();

        public IDisposable Subscribe(IObserver<int> observer)
        {
            return selectedIndex.Subscribe(observer);
        }
    }
}
