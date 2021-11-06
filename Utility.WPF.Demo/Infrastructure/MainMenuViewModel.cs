using SQLite.Service.Mapping;
using SQLite.Service.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Utility.Common.Base;
using Utility.Common.Contracts;
using Utility.Entity;

namespace Utility.WPF.Demo.Infrastructure
{
    class MainMenuModel : IMainMenuModel
    {
        public IReadOnlyCollection<MenuItem> Collection { get; } = Array.Empty<MenuItem>();
    }

    public class MenuPanelService : IMenuPanelService
    {
        public IReadOnlyCollection<PanelObject> Collection { get; } = Array.Empty<PanelObject>();
    }

    public class TreeViewMapper : ITreeViewMapper
    {
        public TreeItem Map(Key map)
        {
            return new UtilityLeafItem(map, "Main");
        }
    }

    public class TabsModel : ITabsModel
    {
        public ObservableCollection<IViewModel> TabItems { get; } = new();

        public IDisposable Subscribe(IObserver<int> observer)
        {
            return Disposable.Empty;
        }
    }

    class UtilityLeafItem : LeafItem
    {
        public UtilityLeafItem(Key key, string name) : base(key, name)
        {
        }
    }
}
