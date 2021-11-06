using Utility.Chart;
using Utility.Database.Common;
using Utility.Entity;

namespace SQLite.Common.Contracts
{

    public record ChartSeries(IKey Key, IReadOnlyCollection<Series> Collection);

    public record TableSeriesPairs(ITableKey Key, IReadOnlyCollection<SeriesPair> Collection);

    public interface IChartSeriesService : IObservable<ChartSeries> { }

    public interface IColumnSeriesPairModel : IObservable<TableSeriesPairs>
    {
        IReadOnlyCollection<SeriesPair> Get(ITableKey key);
        void Set(ITableKey key, IReadOnlyCollection<SeriesPair> pairs);
    }

    public interface ISeriesPairChanges : IObservable<TableSeriesPairs> { }

}
