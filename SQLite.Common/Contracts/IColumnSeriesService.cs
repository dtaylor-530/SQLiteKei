using System.Reactive;
using Utility;
using Utility.Chart;

namespace SQLite.Service.Service
{
    public interface IColumnSeriesService
    {
        IReadOnlyCollection<Series> Get(IKey key, IObserver<Unit> observer);
    }
}