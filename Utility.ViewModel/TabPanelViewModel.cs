﻿using SQLite.Service.Service;
using System.Collections.ObjectModel;
using Utility.Common;
using Utility.Common.Base;
using Utility.ViewModel.Base;

namespace Utility.ViewModel;

public class TabPanelViewModel : BaseViewModel<ITabPanelViewModel>, ITabPanelViewModel
{
    private readonly ITabsService tabsService;

    public TabPanelViewModel(TabPanelViewModelKey key, ITabsService tabsService) : base(key)
    {
        this.tabsService = tabsService;
        tabsService
            .Subscribe(a =>
        {
            //SelectedTabIndex = a;
            //this.RaisePropertyChanged(nameof(SelectedTabIndex));
        });
    }

    public ObservableCollection<IViewModel> TabItems => tabsService.TabItems;

    public int SelectedTabIndex { get; private set; }
    public override string? Name { get; }
}
