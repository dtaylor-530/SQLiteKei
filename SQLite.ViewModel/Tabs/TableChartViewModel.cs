using ReactiveUI;
using SQLite.Common;
using SQLite.Common.Contracts;
using SQLite.Common.Model;
using SQLite.Service.Service;
using System.Reactive;
using System.Reactive.Subjects;
using Utility.Chart;
using Utility.Common.Contracts;

namespace SQLite.ViewModel
{
    public class TableChartViewModel : TableViewModel<ITableChartViewModel>, ITableChartViewModel
    {
        private readonly TableChartViewModelTabKey key;
        private readonly IViewModelNameService nameService;
        private readonly IColumnModelService modelService;
        private readonly IColumnSeriesService columnSeriesService;
        private readonly IColumnSeriesPairService seriesPairService;
        private readonly Subject<Unit> subject = new();

        public TableChartViewModel(
            TableChartViewModelTabKey key,
            IViewModelNameService nameService,
            IColumnModelService modelService,
            IColumnSeriesPairService seriesPairService,
            IColumnSeriesService columnSeriesService) : base(key)
        {
            this.key = key;
            this.nameService = nameService;
            this.modelService = modelService;
            this.seriesPairService = seriesPairService;
            this.columnSeriesService = columnSeriesService;

            subject.Subscribe(a =>
            {
                this.RaisePropertyChanged(nameof(Series));
            });
        }

        //public override TableChartViewModelTabKey Key { get; }

        public override string Name => nameService.Get(key);

        public IReadOnlyCollection<ColumnModel> ColumnData => modelService.GetCollection(key);

        public IReadOnlyCollection<SeriesPair> ColumnSelections
        {
            get => seriesPairService.Get(key);
            set => seriesPairService.Set(key, value);
        }

        public IReadOnlyCollection<Series> Series
        {
            get => columnSeriesService.Get(key, subject);
        }

    }
}
