using OxyPlot;
using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Factory;
using SQLite.ViewModel.Infrastructure.Service;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Utility.Chart;
using Utility.Database;
using Utility.SQLite.Database;
using Utility.SQLite.Models;

namespace SQLite.ViewModel
{
    public class TableChartConfiguration
    {
        public TableChartConfiguration(string tableName, ConnectionPath connectionPath)
        {
            TableName = tableName;
            ConnectionPath = connectionPath;
        }
        public string TableName { get; }

        public ConnectionPath ConnectionPath { get; }

    }

    public record Row(double X, double Y);
    public record RowInt64(Int64 X, Int64 Y);

    public class TableChartViewModel : ReactiveObject
    {

        private readonly TableRecordsConfiguration configuration;
        private readonly ILocaliser localiser;
        private readonly IListCollectionService listCollectionService;
        private readonly IWindowService windowService;
        private readonly ViewModelFactory viewModelFactory;
        private readonly StatusService statusService;
        private readonly TreeService treeService;
        private IReadOnlyCollection<dynamic> rows;
        private IReadOnlyCollection<SeriesPair> columnSelections;
        private IEnumerable series;

        public TableChartViewModel(
            TableRecordsConfiguration configuration,
            ILocaliser localiser,
            IListCollectionService listCollectionService,
            IWindowService windowService,
            ViewModelFactory viewModelFactory,
            StatusService statusService,
            TreeService treeService)
        {

            this.configuration = configuration;
            this.localiser = localiser;
            this.listCollectionService = listCollectionService;
            this.windowService = windowService;
            this.viewModelFactory = viewModelFactory;
            this.statusService = statusService;
            this.treeService = treeService;

            this.WhenAnyValue(a => a.ColumnSelections)
                .WhereNotNull()
             .Subscribe(a => Execute());

            using (var dbHandler = new TableHandler(configuration.ConnectionPath, configuration.TableName))
            {
                ColumnData = dbHandler.Columns;
            }
        }

        //public IEnumerable Collection => listCollectionService.Collection;

        public IReadOnlyCollection<Column> ColumnData { get; }

        public IEnumerable ColumnSelections
        {
            get => columnSelections;
            set => this.RaiseAndSetIfChanged(ref columnSelections, value as IReadOnlyCollection<SeriesPair>);
        }

        void Execute()
        {
            //Info("Executing select query from SelectQuery window.\n" + selectQuery);
            //try
            //{
            //    var resultTable = treeService.SelectCurrentToDataTable(selectQuery);
            //    listCollectionService.SetSource(resultTable);
            //    statusService.OnNext(string.Format("Rows returned: {0}", resultTable.Rows.Count));
            //}
            //catch (Exception ex)
            //{
            //    Error("Select query failed.", ex);
            //    statusService.OnNext(ErrorMessage(ex));
            //}

            List<Utility.Chart.Series> series = new();
            foreach (var xx in columnSelections)
            {

                var rows = treeService.SelectCurrentAsRows($"Select {xx.ColumnX}, {xx.ColumnY} from {configuration.TableName}");
                var lineSeries = new Utility.Chart.Series(xx.ColumnX, xx.ColumnY, rows
                    .Cast<IDictionary<string, object>>()
                    .Select(r =>
                new DataPoint(System.Convert.ToDouble(r[xx.ColumnX]), System.Convert.ToDouble(r[xx.ColumnY]))).ToArray());
                series.Add(lineSeries);
            }

            Series = series;

            static string ErrorMessage(Exception ex)
            {
                var oneLineMessage = Regex.Replace(ex.Message, @"\n", " ");
                oneLineMessage = Regex.Replace(oneLineMessage, @"\t|\r", "");
                oneLineMessage = oneLineMessage.Replace("SQL logic error or missing database ", "SQL Error - ");
                return oneLineMessage;
            }

        }

        public ICommand SelectCommand { get; }

        public System.Collections.IEnumerable Series { get => series; private set => this.RaiseAndSetIfChanged(ref series, value); }

    }
}
