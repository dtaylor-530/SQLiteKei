using ReactiveUI;
using SQLite.Common;
using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Factory;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;

namespace SQLite.ViewModel.Infrastructure.Service
{
    public class TabsService : ReactiveObject, IObservable<int>
    {
        ReplaySubject<int> selectedIndex = new(1);

        public TabsService(TabsFactory tabsFactory, TreeService treeService)
        {
            treeService
                .WhenAnyValue(a => a.SelectedItem)
                .Subscribe(a =>
                {
                    ResetTabs(a, tabsFactory);
                });
        }

        public ObservableCollection<HeaderContent> TabItems { get; } = new ObservableCollection<HeaderContent>();

        public IDisposable Subscribe(IObserver<int> observer)
        {
            return selectedIndex.Subscribe(observer);
        }

        private void ResetTabs(TreeItem selectedItem, TabsFactory tabsFactory)
        {
            ResetTabControl();
            ;
            ReaddTabs();

            void ResetTabControl()
            {
                var openTabs = TabItems.Count;

                for (var i = openTabs - 1; i >= 0; i--)
                    TabItems.RemoveAt(i);

                var defaultTabs = tabsFactory.TabsFor(null);

                foreach (var tab in defaultTabs)
                    TabItems.Add(tab);

                selectedIndex.OnNext(0);
            }

            void ReaddTabs()
            {
                foreach (var tab in tabsFactory.TabsFor(selectedItem))
                    TabItems.Add(tab);
            }

        }
    }
}
