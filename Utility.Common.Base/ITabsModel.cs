using System.Collections.ObjectModel;
using Utility.Common.Base;

namespace SQLite.Service.Service
{
    public interface ITabsModel : IObservable<int>
    {
        ObservableCollection<IViewModel> TabItems { get; }

    }
}