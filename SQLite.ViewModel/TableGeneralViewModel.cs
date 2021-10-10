using ReactiveUI;
using SQLite.Common;
using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Service;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.ViewModels.MainTabControl.Tables;
using System.Windows.Input;
using Utility.Database;
using Utility.SQLite.Database;
using Utility.SQLite.Helpers;
using static SQLite.Common.Log;

namespace SQLite.ViewModel
{

    public class TableGeneralConfiguration
    {
        public TableGeneralConfiguration(string tableName, ConnectionPath connectionPath)
        {
            TableName = tableName;
            ConnectionPath = connectionPath;
        }

        public string TableName { get; }
        public ConnectionPath ConnectionPath { get; }

    }

    public class TableGeneralViewModel : ReactiveObject
    {
        private long rowCount;
        private string tableName;
        private readonly ConnectionPath connectionPath;
        private readonly ILocaliser localiser;
        private readonly IMessageBoxService messageBoxService;
        private readonly StatusService statusService;
        private readonly List<ColumnDataItem> columnData = new();

        public TableGeneralViewModel(
            TableGeneralConfiguration tableGeneralConfiguration,
            ILocaliser localiser,
            IMessageBoxService messageBoxService,
            StatusService statusService)
        {
            this.tableName = tableGeneralConfiguration.TableName;
            this.connectionPath = tableGeneralConfiguration.ConnectionPath;
            this.localiser = localiser;
            this.messageBoxService = messageBoxService;
            this.statusService = statusService;
            EmptyTableCommand = ReactiveCommand.Create(EmptyTable);
            ReindexTableCommand = ReactiveCommand.Create(ReindexTable);

            Initialize(tableGeneralConfiguration.ConnectionPath);

            void Initialize(ConnectionPath connectionPath)
            {
                using (var dbHandler = new TableHandler(connectionPath))
                {
                    TableCreateStatement = dbHandler.CreateStatement(TableName);
                    RowCount = dbHandler.RowCount(TableName);
                    var columns = dbHandler.DataTable(TableName).Columns().ToArray();
                    ColumnCount = columns.Length;

                    foreach (var column in columns)
                    {
                        columnData.Add(Converter.MapToColumnData(column));
                    }
                }
            }
        }

        public ICommand EmptyTableCommand { get; }
        public ICommand ReindexTableCommand { get; }

        public long RowCount
        {
            get => rowCount;
            set => this.RaiseAndSetIfChanged(ref rowCount, value);
        }

        public int ColumnCount { get; private set; }

        public string TableCreateStatement { get; private set; }

        public IReadOnlyCollection<ColumnDataItem> ColumnData => columnData;

        public string TableName => tableName;

        private string NewMethod(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            try
            {
                using (var tableHandler = new TableHandler(connectionPath))
                {
                    tableHandler.RenameTable(tableName, value);
                }
            }
            catch
            {
                //TODO decide what should happen in this case
            }
            return value;
        }

        class Converter
        {
            public static ColumnDataItem MapToColumnData(Column column)
            {
                return new ColumnDataItem
                {
                    Name = column.Name,
                    DataType = column.DataType,
                    IsNotNullable = column.IsNotNullable,
                    IsPrimary = column.IsPrimary,
                    DefaultValue = column.DefaultValue
                };
            }
        }
        public void EmptyTable()
        {

            void EmptyTable(string tableName)
            {
                var message = localiser["MessageBox_EmptyTable", tableName];
                var messageTitle = localiser["MessageBoxTitle_EmptyTable"];
                var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning));

                if (result != true)
                    return;

                using (var tableHandler = new TableHandler(connectionPath))
                {
                    try
                    {
                        tableHandler.EmptyTable(tableName);
                    }
                    catch (Exception ex)
                    {
                        Error("Failed to empty table" + tableName, ex);
                        statusService.OnNext(ex.Message);
                    }

                }
            }
        }

        public void ReindexTable()
        {
            var message = localiser["MessageBox_ReindexTable", tableName];
            var messageTitle = localiser["MessageBoxTitle_ReindexTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question));

            if (result != true) return;

            using (var tableHandler = new TableHandler(connectionPath))
            {
                try
                {
                    tableHandler.ReindexTable(tableName);
                }
                catch (Exception ex)
                {
                    Error("Failed to empty table" + tableName, ex);
                    statusService.OnNext(ex.Message);
                }
            }
        }

        public string AboutKey => localiser["TabContent_GroupBoxHeader_About"];
        public string TableNameKey => localiser["TableGeneralTab_TableName"];
        public string ColumnsKey => localiser["TableCreator_TabHeader_Columns"];
        public string RecordsKey => localiser["TableGeneralTab_Records"];
        public string TableCreateStatementKey => localiser["TableGeneralTab_TableCreateStatement"];
        public string EmptyKey => localiser["TableGeneralTab_ButtonText_Empty"];
        public string ReindexKey => localiser["TableGeneralTab_ButtonText_Reindex"];
        public string GroupBoxHeaderColumnsKey => localiser["TabContent_GroupBoxHeader_Columns"];
        public string NoColumnsFoundKey => localiser["TableGeneralTab_NoColumnsFound"];
    }
}
