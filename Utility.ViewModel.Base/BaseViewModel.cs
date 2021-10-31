using ReactiveUI;
using Utility.Common.Base;

namespace Utility.ViewModel.Base;

public abstract class BaseViewModel : ReactiveObject, IViewModel
{
    private bool isLoaded;

    public BaseViewModel(Key key)
    {
        Key = key;
    }

    public virtual Key Key { get; }

    public abstract string Name { get; }

    public bool IsLoaded
    {
        get => isLoaded;
        set => this.RaiseAndSetIfChanged(ref isLoaded, value);
    }

}

public abstract class BaseViewModel<T> : BaseViewModel
{
    public BaseViewModel(Key key) : base(key)
    {
        if (typeof(T) != key.Type)
            throw new Exception("dsfg55__gfddfg");
    }

    public BaseViewModel(Key<T> key) : base(key)
    {
    }
}