using Utility.Entity;

namespace Utility.Chart.Entity
{
    public record ChartSeries(IKey Key, IReadOnlyCollection<Series>? Collection);

}
