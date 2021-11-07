using Utility.Chart;
using Utility.Database.Common;
using Utility.Entity;

namespace SQLite.Common.Contracts
{

    public record ChartSeries(IKey Key, IReadOnlyCollection<Series> Collection);

    public record TableSeriesPairs(ITableKey Key, IReadOnlyCollection<SeriesPair> Collection);

}
