using Utility;
using Utility.Chart;
using Utility.Database;

namespace SQLite.Common.Contracts
{

    public record ChartSeries(IKey Key, IReadOnlyCollection<Series> Collection);

    public interface IChartSeriesService : IObservable<ChartSeries> { }

    public interface IColumnSeriesPairService : IObservable<TableSeriesPairs>
    {
        IReadOnlyCollection<SeriesPair> Get(ITableKey key);
        void Set(ITableKey key, IReadOnlyCollection<SeriesPair> pairs);
    }

    public interface ISeriesPairChanges : IObservable<TableSeriesPairs> { }

    public record TableSeriesPairs(ITableKey Key, IReadOnlyCollection<SeriesPair> Collection);

}
