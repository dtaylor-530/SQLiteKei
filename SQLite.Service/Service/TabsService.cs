using ReactiveUI;
using SQLite.Common;
using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Service;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;

namespace SQLite.Service.Service
{
    public class TabsService : ReactiveObject, IObservable<int>
    {
        readonly ReplaySubject<int> selectedIndex = new(1);
        private readonly TabsRepository tabsRepository;

        public TabsService(TabsRepository tabsRepository, ITreeItemChanges treeService, ITabsFactory tabsFactory)
        {
            this.tabsRepository = tabsRepository;
            treeService
                .Subscribe(a =>
                {
                    ResetTabs(a, tabsRepository, tabsFactory);
                });

            void ResetTabs(DatabaseTreeItem selectedItem, TabsRepository tabsRepository, ITabsFactory tabsFactory)
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
                    IReadOnlyCollection<IDatabaseViewModel>? tabs = null;
                    if (tabsRepository.Load(selectedItem) is { } loadedKeys)
                    {
                        tabs = tabsFactory.TabsFor(loadedKeys).ToArray();
                    }
                    else
                    {
                        tabs = tabsFactory.TabsFor(selectedItem).ToArray();
                        tabsRepository.Save(selectedItem, tabs.Select(a => (a as IDatabaseKey).Key).ToList());
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

        public ObservableCollection<IDatabaseViewModel> TabItems { get; } = new ObservableCollection<IDatabaseViewModel>();

        public IDisposable Subscribe(IObserver<int> observer)
        {
            return selectedIndex.Subscribe(observer);
        }
    }
}
