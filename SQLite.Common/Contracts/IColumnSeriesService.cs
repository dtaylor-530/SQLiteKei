using System.Reactive;
using Utility.Chart;
using Utility.Entity;

namespace SQLite.Service.Service
{
    public interface IColumnSeriesService
    {
        IReadOnlyCollection<Series> Get(IKey key, IObserver<Unit> observer);
    }
}