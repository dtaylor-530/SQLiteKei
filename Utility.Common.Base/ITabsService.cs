using System.Collections.ObjectModel;
using Utility.Common.Base;

namespace SQLite.Service.Service
{
    public interface ITabsService : IObservable<int>
    {
        ObservableCollection<IViewModel> TabItems { get; }

        IDisposable Subscribe(IObserver<int> observer);
    }
}