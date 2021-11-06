using Database.Common.Contracts;
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

        public ChartSeriesService(ColumnSeriesPairModel columnSeriesPairService, ISelectedDatabaseService selectedItemService)
        {
            columnSeriesPairService
                .SelectMany(a =>
                {
                    return LineSeriesConverter.Execute(a.Collection, selectedItemService, a.Key.TableName).ToObservable()
                     .SelectMany(xx => xx)
                     .Scan(new List<Series>(), (xx, b) =>
                     {
                         xx.Add(b);
                         return xx;
                     })
                     .Select(ad =>
                     {
                         return new ChartSeries(a.Key, ad);
                     });

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
        public static IEnumerable<IObservable<Series>> Execute(IReadOnlyCollection<SeriesPair> columnSelections, ISelectedDatabaseService treeService, string tableName)
        {
            foreach (var xy in columnSelections)
            {
                yield return treeService.SelectAsRows($"Select {xy.ColumnX}, {xy.ColumnY} from {tableName}")
                     .Select(rows =>
                     {
                         var da = rows
                         .Cast<IDictionary<string, object>>()
                         .Select(r => new DataPoint(Convert.ToDouble(r[xy.ColumnX]), Convert.ToDouble(r[xy.ColumnY])))
                         .ToArray();

                         return new Series(xy.ColumnX, xy.ColumnY, da);
                     });
            }
        }

    }

}
