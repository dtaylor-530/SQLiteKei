using Database.Common.Contracts;
using Database.Service.Model;
using OxyPlot;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Utility.Chart.Common;
using Utility.Chart.Entity;

namespace Database.Service.Chart
{

    public class ChartSeriesService : IChartSeriesService
    {

        private readonly ReplaySubject<TableSeriesPairs> subject = new(1);
        private readonly ReplaySubject<ChartSeries> chartSubject = new(1);

        public ChartSeriesService(IColumnSeriesPairModel columnSeriesPairModel, ISelectedDatabaseService selectedItemService, ITableGeneralModel generalModel)
        {
            columnSeriesPairModel
                .Select(a =>
                {
                    return generalModel
                    .Get(a.Key)
                    .Select(ad =>
                    {
                        if (a.Collection.Any() == false)
                            return Observable.Return(new ChartSeries(a.Key, null));

                        return LineSeriesConverter
                        .Execute(a.Collection, selectedItemService, a.Key.TableName, ad)
                        .ToObservable()
                        .Switch()
                        .Scan(new List<Series>(), (xx, b) =>
                         {
                             xx.Add(b);
                             return xx;
                         })
                         .Select(ad =>
                         {
                             return new ChartSeries(a.Key, ad);
                         });
                    });
                })
                .Switch()
                .Switch()
                .Subscribe(chartSubject);

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
        public static IEnumerable<IObservable<Series>> Execute(IReadOnlyCollection<SeriesPair> columnSelections, ISelectedDatabaseService treeService, string tableName, Utility.Database.SQLite.Common.Abstract.TableGeneralInformation ad)
        {
            foreach (var xy in columnSelections)
            {

                var xwe = ad.Columns.Single(a => a.Name == xy.ColumnX).DataType == "bigint";
                var ywe = ad.Columns.Single(a => a.Name == xy.ColumnY).DataType == "bigint";

                yield return treeService
                    .SelectAsRows($"Select {xy.ColumnX}, {xy.ColumnY} from {tableName}")
                     .Select(rows =>
                     {
                         var da = rows
                         .Cast<IDictionary<string, object>>()
                         .Select(r => new DataPoint(Convert.ToDouble(r[xy.ColumnX]), Convert.ToDouble(r[xy.ColumnY])))
                         .ToArray();

                         //var da2 = rows
                         //.Cast<IDictionary<string, object>>()
                         //.Select(r => new LiveChartsCore.Defaults.ObservablePoint(Convert.ToDouble(r[xy.ColumnX]), Convert.ToDouble(r[xy.ColumnY])))
                         //.ToArray();

                         return new Series(xy.ColumnX, xy.ColumnY, tableName, da, xwe, ywe);
                     });
            }
        }

    }

}
