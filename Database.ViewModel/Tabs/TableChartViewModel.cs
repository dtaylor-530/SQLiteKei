using Database.Common.Contracts;
using DynamicData;
using ReactiveUI;
using SQLite.Common;
using SQLite.Service.Service;
using SQLite.ViewModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Utility.Chart.Common;
using Utility.Chart.Entity;
using Utility.Common.Contracts;
using Utility.Service;
using Utility.ViewModel.Base;

namespace Database.ViewModel
{
    public class TableChartViewModel : DatabaseTabViewModel<ITableChartViewModel>, ITableChartViewModel
    {
        private readonly TableChartViewModelTabKey key;
        private readonly IViewModelNameService nameService;

        public TableChartViewModel(
            TableChartViewModelTabKey key,
            IViewModelNameService nameService,
            TableColumnsViewModel tableColumnsViewModel,
            SeriesViewModel columnSelectionViewModel,
            IIsSelectedService isSelectedService) : base(key, isSelectedService)
        {
            this.key = key;
            this.nameService = nameService;
            TableColumnsViewModel = tableColumnsViewModel;
            SeriesViewModel = columnSelectionViewModel;
        }

        public override string Name => nameService.Get(key);

        public TableColumnsViewModel TableColumnsViewModel { get; }

        public SeriesViewModel SeriesViewModel { get; }
    }

    public class TableColumnsViewModel : BaseViewModel
    {
        private readonly TableChartViewModelTabKey key;
        private readonly IColumnModel columnModel;
        private readonly IColumnSeriesPairModel seriesPairModel;
        private readonly Subject<Unit> subject = new();
        private IReadOnlyCollection<Column> columns;

        public TableColumnsViewModel(
            TableChartViewModelTabKey key,
            IColumnModel columnModel,
            IColumnSeriesPairModel seriesPairModel) : base(key)
        {
            this.key = key;
            this.columnModel = columnModel;
            this.seriesPairModel = seriesPairModel;

            subject.Subscribe(a =>
            {
                this.RaisePropertyChanged(nameof(Series));
            });
        }

        public override string Name { get; } = nameof(TableColumnsViewModel);

        public IReadOnlyCollection<Column> Columns
        {
            get
            {
                if (columns == null)
                {
                    var obs = columnModel
                         .GetCollection(key)
                         .Select(a =>
                         {
                             columns = a.Select(a => new Column(a.Name, a.TableName, a.IsEnabled)).ToArray();
                             this.RaisePropertyChanged(nameof(Columns));
                             return columns;
                         });

                    obs
                        .Select(ServiceHelper.Update)
                        .Switch()
                        .Select(ServiceHelper.SeriesPairs)
                        .WhereNotNull()
                        .Subscribe(value =>
                        {
                            seriesPairModel.Set(key, value);
                        });

                }
                return columns ?? Array.Empty<Column>();
            }
        }

        class ServiceHelper
        {
            public static SeriesPair[]? SeriesPairs(IEnumerable<Column> columns)
            {
                if (columns.Count(a => a.X) > 1)
                    return null;
                if (columns.SingleOrDefault(a => a.X) is not { } singleX)
                    return null;

                return columns
                    .Where(c => c.Y)
                    .Select(column => new SeriesPair(singleX.Name, column.Name, column.TableName))
                    .ToArray();
            }

            public static IObservable<IReadOnlyCollection<Column>> Update(IEnumerable<Column> columns)
            {
                Subject<Column> subject = new();

                var obs = subject
                            .ToObservableChangeSet(a => a.Name)
                            .ToCollection()
                            .Where(a => a.Count > 0);

                foreach (var col in columns)
                {
                    col
                        .WhenAny((a) => a.X, a => a.Y, (a, b) => a.Sender)
                        .Subscribe(subject);
                }

                return obs;
            }
        }
    }

    public class SeriesViewModel : BaseViewModel
    {
        private readonly TableChartViewModelTabKey key;
        private readonly IColumnSeriesModel columnSeriesModel;
        private readonly Subject<Unit> subject = new();

        public SeriesViewModel(TableChartViewModelTabKey key, IColumnSeriesModel columnSeriesModel, IChartSeriesService chartSeriesService) : base(key)
        {
            this.columnSeriesModel = columnSeriesModel;
            chartSeriesService.Subscribe(a =>
            {
                Series = a.Collection;
                this.RaisePropertyChanged(nameof(Series));
            });
        }

        public IReadOnlyCollection<Series>? Series { get; private set; }

        public override string Name { get; } = nameof(SeriesViewModel);
    }
}
