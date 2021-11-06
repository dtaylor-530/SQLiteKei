using ReactiveUI;
using SQLite.Common;
using SQLite.Common.Contracts;
using SQLite.Common.Model;
using SQLite.Service.Service;
using SQLite.ViewModel;
using System.Reactive;
using System.Reactive.Subjects;
using Utility.Chart;
using Utility.Common.Contracts;

namespace Database.ViewModel
{
    public class TableChartViewModel : TableViewModel<ITableChartViewModel>, ITableChartViewModel
    {
        private readonly TableChartViewModelTabKey key;
        private readonly IViewModelNameService nameService;
        private readonly IColumnModelService columnModelService;
        private readonly IColumnSeriesService columnSeriesService;
        private readonly IColumnSeriesPairModel seriesPairService;
        private readonly Subject<Unit> subject = new();
        private IReadOnlyCollection<ColumnModel> columnData;

        public TableChartViewModel(
            TableChartViewModelTabKey key,
            IViewModelNameService nameService,
            IColumnModelService columnModelService,
            IColumnSeriesPairModel seriesPairService,
            IColumnSeriesService columnSeriesService) : base(key)
        {
            this.key = key;
            this.nameService = nameService;
            this.columnModelService = columnModelService;
            this.seriesPairService = seriesPairService;
            this.columnSeriesService = columnSeriesService;

            subject.Subscribe(a =>
            {
                this.RaisePropertyChanged(nameof(Series));
            });
        }

        //public override TableChartViewModelTabKey Key { get; }

        public override string Name => nameService.Get(key);

        public IReadOnlyCollection<ColumnModel> ColumnData
        {
            get
            {
                if (columnData == null)
                    columnModelService
                        .GetCollection(key)
                        .Subscribe(a =>
                    {
                        columnData = a;
                        this.RaisePropertyChanged(nameof(ColumnData));
                    });

                return columnData ?? Array.Empty<ColumnModel>();
            }
        }

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
