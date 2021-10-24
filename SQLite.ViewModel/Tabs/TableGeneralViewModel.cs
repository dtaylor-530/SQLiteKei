using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.Service.Model;
using SQLite.Service.Service;
using SQLite.ViewModel.Infrastructure;
using System.Windows.Input;
using Utility.Database;
using Utility.SQLite.Database;
using Utility.SQLite.Models;

namespace SQLite.ViewModel
{
    public class TableGeneralViewModelTabKey : TableTabKey
    {
        public TableGeneralViewModelTabKey(DatabasePath databasePath, TableName tableName) : base(databasePath, tableName)
        {
        }
    }

    public class TableGeneralViewModel : DatabaseViewModel
    {
        private string tableName;
        private readonly DatabasePath connectionPath;
        private readonly ILocaliser localiser;
        private readonly ViewModelNameService nameService;
        private readonly TableService tableService;

        public TableGeneralViewModel(
            TableGeneralViewModelTabKey key,
            ILocaliser localiser,
            ViewModelNameService nameService,
            IsSelectedService isSelectedService,
            TableService tableService) : base(key, isSelectedService)
        {
            this.Key = key;
            this.tableName = key.TableName;
            this.connectionPath = key.DatabasePath;
            this.localiser = localiser;
            this.nameService = nameService;
            this.tableService = tableService;
            EmptyTableCommand = ReactiveCommand.Create(EmptyTable);
            ReindexTableCommand = ReactiveCommand.Create(ReindexTable);

            using (var dbHandler = new TableHandler(key.DatabasePath, TableName))
            {
                (string a, long b, Column[] c, int d) = dbHandler.General;
                TableCreateStatement = a;
                RowCount = b;
                ColumnData = c;
                ColumnCount = d;
            }
        }

        public override string Name => nameService.Get(this.Key);
        public override TableGeneralViewModelTabKey Key { get; }

        public ICommand EmptyTableCommand { get; }
        public ICommand ReindexTableCommand { get; }

        public long RowCount { get; }

        public int ColumnCount { get; }

        public string TableCreateStatement { get; }

        public IReadOnlyCollection<Column> ColumnData { get; }

        public string TableName
        {
            get => tableName;
            set => tableService.RenameTable(new TableLeafItem(tableName, new TableKey(connectionPath, Key.TableName)), tableName = value);
        }

        public void EmptyTable() => tableService.EmptyTable(new TableLeafItem(tableName, new TableKey(connectionPath, Key.TableName)));

        public void ReindexTable() => ReindexTable(tableService);

        public void ReindexTable(TableService tableService) => tableService.ReindexTable(new TableLeafItem(tableName, new TableKey(connectionPath, Key.TableName)));

        public string AboutKey => localiser["Tab_GroupBoxHeader_About"];
        public string TableNameKey => localiser["TableGeneralTab_TableName"];
        public string ColumnsKey => localiser["TableCreator_TabHeader_Columns"];
        public string RecordsKey => localiser["TableGeneralTab_Records"];
        public string TableCreateStatementKey => localiser["TableGeneralTab_TableCreateStatement"];
        public string EmptyKey => localiser["TableGeneralTab_ButtonText_Empty"];
        public string ReindexKey => localiser["TableGeneralTab_ButtonText_Reindex"];
        public string GroupBoxHeaderColumnsKey => localiser["Tab_GroupBoxHeader_Columns"];
        public string NoColumnsFoundKey => localiser["TableGeneralTab_NoColumnsFound"];
    }
}
