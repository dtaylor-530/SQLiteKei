using ReactiveUI;
using SQLite.Common;
using SQLite.Common.Contracts;
using System.Windows.Input;
using Utility.Common.Base;
using Utility.Common.Contracts;
using Utility.Service;
using Utility.SQLite.Database;
using Utility.SQLite.Models;

namespace SQLite.ViewModel
{

    public class TableGeneralViewModel : DatabaseTabViewModel<ITableGeneralViewModel>, ITableGeneralViewModel
    {
        private string tableName;
        private readonly ILocaliser localiser;
        private readonly IViewModelNameService nameService;
        private readonly ITableService tableService;

        public TableGeneralViewModel(
            TableGeneralViewModelTabKey key,
            ILocaliser localiser,
            IViewModelNameService nameService,
            IIsSelectedService selectedService,
            ITableService tableService) : base(key, selectedService)
        {
            this.Key = key;
            this.tableName = key.TableName;
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
            set => tableService.RenameTable(Key, tableName = value);
        }

        public void EmptyTable() => tableService.EmptyTable(Key);

        public void ReindexTable() => tableService.ReindexTable(Key);

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
