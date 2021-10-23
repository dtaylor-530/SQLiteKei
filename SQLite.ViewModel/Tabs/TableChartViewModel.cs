using ReactiveUI;
using SQLite.Service.Model;
using SQLite.Service.Service;
using SQLite.ViewModel.Infrastructure;
using System.Reactive.Linq;
//using System.Reactive.Linq;
using Utility.Chart;
using Utility.Database;

namespace SQLite.ViewModel
{

    public class AutoActivateAttribute : Attribute
    {

    }

    public class TableChartViewModelTabKey : TableTabKey
    {
        public TableChartViewModelTabKey(DatabasePath databasePath, TableName tableName) : base(databasePath, tableName)
        {
        }
    }

    public class TableChartViewModel : DatabaseViewModel
    {
        private readonly ViewModelNameService nameService;
        private readonly ColumnModelService modelService;
        private readonly ColumnSeriesService columnSeriesService;
        private readonly ColumnSeriesPairService seriesPairService;

        public TableChartViewModel(
            TableChartViewModelTabKey key,
            ViewModelNameService nameService,
            ColumnModelService modelService,
            ColumnSeriesPairService seriesPairService,
            ChartSeriesService chartSeriesService,
            ColumnSeriesService columnSeriesService) : base(key, string.Empty)
        {
            Key = key;
            this.nameService = nameService;
            this.modelService = modelService;
            this.seriesPairService = seriesPairService;
            this.columnSeriesService = columnSeriesService;

            chartSeriesService
                .Where(a => a.TableName == Key.TableName)
                .Subscribe(a =>
            {
                columnSeriesService.Set(Key, a.Collection.ToList());
                this.RaisePropertyChanged(nameof(Series));
            });
        }

        public override TableChartViewModelTabKey Key { get; }

        public override string Name => nameService.Get(Key);

        public IReadOnlyCollection<ColumnModel> ColumnData => modelService.GetCollection(Key);

        public IReadOnlyCollection<SeriesPair> ColumnSelections
        {
            get => seriesPairService.Get(Key);
            set => seriesPairService.Set(Key, value);
        }

        public IReadOnlyCollection<Series> Series
        {
            get => columnSeriesService.Get(Key);
            //private set => columnSeriesService.Set(Key, value);
        }

        //void Execute()
        //{

        //    List<Utility.Chart.Series> series = new();
        //    foreach (var xx in columnSelections ?? throw new Exception("!!se544455df2244444)d"))
        //    {

        //        var rows = treeService.SelectCurrentAsRows($"Select {xx.ColumnX}, {xx.ColumnY} from {configuration.TableName}");
        //        var lineSeries = new Utility.Chart.Series(xx.ColumnX, xx.ColumnY, rows
        //            .Cast<IDictionary<string, object>>()
        //            .Select(r => new DataPoint(Convert.ToDouble(r[xx.ColumnX]), Convert.ToDouble(r[xx.ColumnY]))).ToArray());
        //        series.Add(lineSeries);
        //    }

        //    Series = series;

        //}

    }

    //public class TableChartViewModelService
    //{
    //    public TableChartViewModelService()
    //    {

    //    }

    //    public IReadOnlyCollection<ReactiveObject> Collection { get; }
    //}

}
