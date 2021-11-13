using Utility.Chart.Entity;
using Utility.Database.Common;

namespace Utility.Chart.Common
{
    public interface IChartSeriesService : IObservable<ChartSeries> { }

    public interface IColumnSeriesPairModel : IObservable<TableSeriesPairs>
    {
        IReadOnlyCollection<SeriesPair> Get(ITableKey key);
        void Set(ITableKey key, IReadOnlyCollection<SeriesPair> pairs);
    }

    //public interface ISeriesPairChanges : IObservable<TableSeriesPairs> { }

    public record TableSeriesPairs(ITableKey Key, IReadOnlyCollection<SeriesPair> Collection);
}
