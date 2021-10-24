using ReactiveUI;
using SQLite.Service.Model;
using SQLite.Service.Service;
using SQLite.ViewModel.Infrastructure;
using System.Reactive;
using System.Reactive.Subjects;
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
        private readonly Subject<Unit> subject = new();

        public TableChartViewModel(
            TableChartViewModelTabKey key,
            ViewModelNameService nameService,
            ColumnModelService modelService,
            ColumnSeriesPairService seriesPairService,
            IsSelectedService isSelectedService,
            ColumnSeriesService columnSeriesService) : base(key, isSelectedService)
        {
            Key = key;
            this.nameService = nameService;
            this.modelService = modelService;
            this.seriesPairService = seriesPairService;
            this.columnSeriesService = columnSeriesService;

            subject.Subscribe(a =>
            {
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
            get => columnSeriesService.Get(Key, subject);
        }

    }
}
