using ReactiveUI;
using SQLite.Common;
using SQLite.Service.Service;
using System.Reactive.Linq;
using Utility;
using Utility.Database;

namespace SQLite.ViewModel.Infrastructure;

public abstract class ReactiveViewModel : ReactiveObject, IViewModel
{
    private readonly IsSelectedService isSelectedService;
    private readonly ObservableAsPropertyHelper<bool> prop;
    private bool isLoaded;

    public ReactiveViewModel(Key key, IsSelectedService isSelectedService)
    {
        Key = key;
        this.isSelectedService = isSelectedService;
        prop = isSelectedService.Get(key, this.WhenAnyValue(a => a.IsLoaded)).Select(a => a.Value).ToProperty(this, a => a.IsSelected);
    }

    public virtual Key Key { get; }

    public abstract string Name { get; }

    public bool IsLoaded
    {
        get => isLoaded;
        set => this.RaiseAndSetIfChanged(ref isLoaded, value);
    }

    public bool IsSelected
    {
        get => prop.Value;
        set
        {
            isSelectedService.Set(Key, new IsSelected(value));
        }
    }
}

public abstract class DatabaseViewModel : ReactiveViewModel, IDatabaseViewModel
{
    public DatabaseViewModel(DatabaseKey key, IsSelectedService isSelectedService) : base(key, isSelectedService)
    {
        Key = key;
    }

    public override DatabaseKey Key { get; }
}
