using OxyPlot;
using ReactiveUI;
using SQLite.ViewModel.Infrastructure.Service;
using System.Collections;
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
        private readonly TreeService treeService;
        private IReadOnlyCollection<SeriesPair> columnSelections;
        private IEnumerable series;

        public TableChartViewModel(
            TableRecordsConfiguration configuration,
            TreeService treeService)
        {
            this.configuration = configuration;
            this.treeService = treeService;

            this.WhenAnyValue(a => a.ColumnSelections)
                .WhereNotNull()
             .Subscribe(a => Execute());

            using (var dbHandler = new TableHandler(configuration.ConnectionPath, configuration.TableName))
            {
                ColumnData = dbHandler.Columns.Select(a => new ColumnViewModel(a)).ToArray();
            }
        }

        public IReadOnlyCollection<ColumnViewModel> ColumnData { get; }
        public ICommand SelectCommand { get; }
        public IEnumerable Series { get => series; private set => this.RaiseAndSetIfChanged(ref series, value); }

        #region dependency properties
        public IEnumerable ColumnSelections
        {
            get => columnSelections;
            set => this.RaiseAndSetIfChanged(ref columnSelections, value as IReadOnlyCollection<SeriesPair>);
        }
        #endregion dependency properties

        void Execute()
        {

            List<Utility.Chart.Series> series = new();
            foreach (var xx in columnSelections)
            {

                var rows = treeService.SelectCurrentAsRows($"Select {xx.ColumnX}, {xx.ColumnY} from {configuration.TableName}");
                var lineSeries = new Utility.Chart.Series(xx.ColumnX, xx.ColumnY, rows
                    .Cast<IDictionary<string, object>>()
                    .Select(r => new DataPoint(Convert.ToDouble(r[xx.ColumnX]), Convert.ToDouble(r[xx.ColumnY]))).ToArray());
                series.Add(lineSeries);
            }

            Series = series;

        }

        public class ColumnViewModel : Column
        {
            public ColumnViewModel(Column columnData)
            {
                this.Name = columnData.Name;
                this.DataType = columnData.DataType;
                this.DefaultValue = columnData.DefaultValue;
                this.IsNotNullable = columnData.IsNotNullable;
                this.IsPrimary = columnData.IsPrimary;
            }

            public bool IsEnabled => new string[] { "Blob", "Char", "Varchar" }.Any(a => DataType.Equals(a, StringComparison.InvariantCultureIgnoreCase)) == false;

        }

    }
}
