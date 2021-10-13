﻿using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Service;
using System.Windows.Input;
using Utility.Database;
using Utility.SQLite.Database;
using Utility.SQLite.Models;

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
        private string tableName;
        private readonly ConnectionPath connectionPath;
        private readonly ILocaliser localiser;
        private readonly TableService tableService;

        public TableGeneralViewModel(
            TableGeneralConfiguration tableGeneralConfiguration,
            ILocaliser localiser,
            TableService tableService)
        {
            this.tableName = tableGeneralConfiguration.TableName;
            this.connectionPath = tableGeneralConfiguration.ConnectionPath;
            this.localiser = localiser;
            this.tableService = tableService;
            EmptyTableCommand = ReactiveCommand.Create(EmptyTable);
            ReindexTableCommand = ReactiveCommand.Create(ReindexTable);

            using (var dbHandler = new TableHandler(tableGeneralConfiguration.ConnectionPath, TableName))
            {
                (string a, long b, Column[] c, int d) = dbHandler.General;
                TableCreateStatement = a;
                RowCount = b;
                ColumnData = c;
                ColumnCount = d;
            }
        }

        public ICommand EmptyTableCommand { get; }
        public ICommand ReindexTableCommand { get; }

        public long RowCount { get; }

        public int ColumnCount { get; }

        public string TableCreateStatement { get; }

        public IReadOnlyCollection<Column> ColumnData { get; }

        public string TableName
        {
            get => tableName;
            set => tableService.RenameTable(new TableItem(tableName, connectionPath), tableName = value);
        }

        public void EmptyTable() => tableService.EmptyTable(new TableItem(tableName, connectionPath));

        public void ReindexTable() => ReindexTable(tableService);

        public void ReindexTable(TableService tableService) => tableService.ReindexTable(new TableItem(tableName, connectionPath));

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
