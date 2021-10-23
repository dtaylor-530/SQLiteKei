using ReactiveUI;
using SQLite.Common;
using SQLite.Service.Service;
using System.Collections.ObjectModel;

namespace SQLite.ViewModel;

public class TabPanelViewModel : ReactiveObject
{
    private readonly TabsService tabsService;

    public TabPanelViewModel(TabsService tabsService)
    {
        this.tabsService = tabsService;
        tabsService.Subscribe(a =>
        {
            SelectedTabIndex = a;
            this.RaisePropertyChanged(nameof(SelectedTabIndex));
        });
    }

    public ObservableCollection<IDatabaseViewModel> TabItems => tabsService.TabItems;

    public int SelectedTabIndex
    {
        get; private set;
    }
}
