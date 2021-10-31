using OxyPlot;
using SQLite.Common.Contracts;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Utility.Chart;

namespace SQLite.Service.Service
{

    public class ChartSeriesService : IChartSeriesService
    {

        private readonly ReplaySubject<TableSeriesPairs> subject = new(1);
        private readonly ReplaySubject<ChartSeries> chartSubject = new(1);

        public ChartSeriesService(ColumnSeriesPairService columnSeriesPairService, ISelectedDatabaseService selectedItemService)
        {
            columnSeriesPairService
                .Select(a =>
                {
                    return new ChartSeries(a.Key, LineSeriesConverter.Execute(a.Collection, selectedItemService, a.Key.TableName).ToArray());

                }).Subscribe(chartSubject);

        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(TableSeriesPairs value)
        {
            subject.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<ChartSeries> observer)
        {
            return chartSubject.Subscribe(observer);
        }
    }

    static class LineSeriesConverter
    {
        public static IEnumerable<Series> Execute(IReadOnlyCollection<SeriesPair> columnSelections, ISelectedDatabaseService treeService, string tableName)
        {
            foreach (var xy in columnSelections)
            {
                var rows = treeService.SelectAsRows($"Select {xy.ColumnX}, {xy.ColumnY} from {tableName}");
                var lineSeries = new Series(xy.ColumnX, xy.ColumnY, rows
                    .Cast<IDictionary<string, object>>()
                    .Select(r => new DataPoint(Convert.ToDouble(r[xy.ColumnX]), Convert.ToDouble(r[xy.ColumnY]))).ToArray());
                yield return lineSeries;
            }
        }

    }

}
