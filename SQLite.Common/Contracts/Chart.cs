using SQLite.Common.Contracts;
using Utility.Chart;
using Utility.Database.Common;

namespace Database.Common.Contracts
{
    public interface IChartSeriesService : IObservable<ChartSeries> { }

    public interface IColumnSeriesPairModel : IObservable<TableSeriesPairs>
    {
        IReadOnlyCollection<SeriesPair> Get(ITableKey key);
        void Set(ITableKey key, IReadOnlyCollection<SeriesPair> pairs);
    }

    public interface ISeriesPairChanges : IObservable<TableSeriesPairs> { }
}
