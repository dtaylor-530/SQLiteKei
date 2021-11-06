using Database.Common.Contracts;
using Database.Service.Model;
using ReactiveUI;
using SQLite.Common;
using SQLite.ViewModel;
using System.Windows.Input;
using Utility.Common.Base;
using Utility.Common.Contracts;
using Utility.Database.SQLite.Common.Abstract;
using Utility.Service;
using Utility.SQLite.Models;

namespace Database.ViewModel
{
    public class TableGeneralViewModel : DatabaseTabViewModel<ITableGeneralViewModel>, ITableGeneralViewModel
    {
        private TableGeneralViewModelTabKey key;
        private string tableName;
        private TableGeneralInformation? tableInformation;
        private readonly ILocaliser localiser;
        private readonly IViewModelNameService nameService;
        private readonly ITableService tableService;
        private readonly ITableGeneralModel generalModel;

        public TableGeneralViewModel(
            TableGeneralViewModelTabKey key,
            ILocaliser localiser,
            IViewModelNameService nameService,
            IIsSelectedService selectedService,
            ITableService tableService,
            ITableGeneralModel generalModel) : base(key, selectedService)
        {
            this.key = key;
            tableName = key.TableName;
            this.localiser = localiser;
            this.nameService = nameService;
            this.tableService = tableService;
            this.generalModel = generalModel;
            EmptyTableCommand = ReactiveCommand.Create(EmptyTable);
            ReindexTableCommand = ReactiveCommand.Create(ReindexTable);

        }

        public override string Name => nameService.Get(Key);
        public override TableGeneralViewModelTabKey Key => key;

        public ICommand EmptyTableCommand { get; }
        public ICommand ReindexTableCommand { get; }

        public long RowCount => TableInformation.RowCount;

        public int ColumnCount => TableInformation.ColumnCount;

        public string TableCreateStatement => TableInformation.CreateStatement;

        public IReadOnlyCollection<Column> ColumnData => TableInformation.Columns;

        private TableGeneralInformation TableInformation
        {
            get
            {
                if (tableInformation.HasValue == false)
                {
                    generalModel.Get(key)
                        .Subscribe(a =>
                        {
                            tableInformation = a;
                            this.RaisePropertyChanged(null);
                        });
                    return default;
                }
                else
                    return tableInformation.Value;
            }
        }

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
