using System.Reactive;
using Utility.Chart.Entity;
using Utility.Entity;

namespace Database.Common.Contracts
{
    public interface IColumnSeriesModel
    {
        IReadOnlyCollection<Series> Get(IKey key, IObserver<Unit> observer);
    }
}