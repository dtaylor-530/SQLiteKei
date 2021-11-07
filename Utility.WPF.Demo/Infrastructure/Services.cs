using ReactiveUI;
using SQLite.Service.Mapping;
using SQLite.Service.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Utility.Common.Base;
using Utility.Common.Contracts;
using Utility.Entity;
using Utility.ViewModel.Base;

namespace Utility.WPF.Demo.Infrastructure
{
    class MainMenuModel : IMainMenuModel
    {
        public IReadOnlyCollection<MenuItem> Collection { get; } = Array.Empty<MenuItem>();
    }

    public class MainToolBarModel : IMainToolBarModel
    {
        private readonly ITreeModel treeModel;
        private readonly ITreeViewMapper mapper;

        public MainToolBarModel(ITreeModel treeModel, ITreeViewMapper mapper)
        {
            this.treeModel = treeModel;
            this.mapper = mapper;
        }

        public void Open(Key key)
        {
            mapper.Map(key)
                .Subscribe(databaseItem =>
                {
                    treeModel.OnNext(new(Adds: new[] { databaseItem }));
                });
        }

        public IReadOnlyCollection<PanelObject> Collection => new PanelObject[]
    {
                new ImageButton(ReactiveCommand.Create(()=>{ Open(null); }), " ","./Resources/PNG_transparency_demonstration_1.png" ),
             };
    }

    public class TreeViewMapper : ITreeViewMapper
    {
        public IObservable<TreeItem> Map(Key map)
        {
            return Observable.Return(new UtilityLeafItem(map, "Main"));
        }
    }

    public class TabsModel : ITabsModel
    {

        public TabsModel(ITreeItemChanges changes)
        {
            changes
                .Subscribe(a =>
            {
                TabItems.Add(new TabViewModel());
            });
        }

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

    class TabViewModel : BaseViewModel
    {
        public TabViewModel() : base(null)
        {
            Name = DateTime.Now.ToString("T");
        }

        public override string Name { get; }
    }
}
