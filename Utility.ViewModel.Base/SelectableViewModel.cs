using ReactiveUI;
using System.Reactive.Linq;
using Utility.Common.Base;
using Utility.Service;

namespace Utility.ViewModel.Base;
public abstract class SelectableViewModel : BaseViewModel, IIsSelected
{
    private readonly IIsSelectedService isSelectedService;
    private readonly ObservableAsPropertyHelper<bool> prop;

    public SelectableViewModel(Key key, IIsSelectedService isSelectedService) : base(key)
    {
        this.isSelectedService = isSelectedService;
        prop = isSelectedService.Get(key, this.WhenAnyValue(a => a.IsLoaded)).Select(a => a.Value).ToProperty(this, a => a.IsSelected);

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
