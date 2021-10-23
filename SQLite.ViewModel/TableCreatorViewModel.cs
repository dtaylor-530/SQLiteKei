using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.Service.Model;
using SQLite.Service.Service;
using SQLiteKei.DataAccess.QueryBuilders;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Utility;

namespace SQLite.ViewModel
{
    public class TableCreatorViewModelKey : Key
    {
        public override bool Equals(Key? other)
        {
            return other is TableCreatorViewModelKey;
        }
    }

    public class TableCreatorViewModel : ReactiveObject
    {

        private DatabaseSelectItem selectedDatabase;
        private string tableName;
        private string sqlStatement;
        private bool isValidTableDefinition;
        private readonly ICommand addColumnCommand;
        private readonly ICommand createCommand;
        private string statusInfo;
        private readonly ICommand addForeignKeyCommand;
        private readonly ILocaliser localiser;
        private readonly DatabaseService databaseService;
        //private readonly DatabaseSelectItem[] databases;

        public TableCreatorViewModel(ILocaliser localiser, TreeService treeService, DatabaseService databaseService)
        {
            ColumnDefinitions.CollectionChanged += CollectionContentChanged;
            ForeignKeyDefinitions.CollectionChanged += CollectionContentChanged;

            addColumnCommand = ReactiveCommand.Create(AddColumnDefinition);
            addForeignKeyCommand = ReactiveCommand.Create(AddForeignKeyDefinition);
            createCommand = ReactiveCommand.Create(Create);
            this.localiser = localiser;
            this.databaseService = databaseService;
            //databases = treeService.TreeViewItems.Select(a => new DatabaseSelectItem(a.Key.DatabasePath)).ToArray();
        }

        public ICommand CreateCommand => createCommand;

        public DatabaseSelectItem SelectedDatabase
        {
            get { return selectedDatabase; }
            set
            {
                selectedDatabase = value;

                foreach (var foreignKey in ForeignKeyDefinitions)
                {
                    foreignKey.SelectedDatabasePath = selectedDatabase.Path;
                }
            }
        }

        public IReadOnlyCollection<DatabaseSelectItem> Databases => new[] { SelectedDatabase };

        public ObservableCollection<ColumnDefinitionItem> ColumnDefinitions { get; } = new ObservableCollection<ColumnDefinitionItem>();

        public ObservableCollection<ForeignKeyDefinitionItem> ForeignKeyDefinitions { get; } = new ObservableCollection<ForeignKeyDefinitionItem>();

        public string TableName
        {
            get { return tableName; }
            set
            {
                tableName = value;
                this.RaisePropertyChanged("TableName");
                UpdateSqlStatement();
            }
        }

        public string SqlStatement
        {
            get { return sqlStatement; }
            set { this.RaiseAndSetIfChanged(ref sqlStatement, value); }
        }

        public bool IsValidTableDefinition
        {
            get { return isValidTableDefinition; }
            set { this.RaiseAndSetIfChanged(ref isValidTableDefinition, value); }
        }

        public string StatusInfo
        {
            get { return statusInfo; }
            set { this.RaiseAndSetIfChanged(ref statusInfo, value); }
        }

        public ICommand AddForeignKeyCommand { get { return addForeignKeyCommand; } }

        public ICommand AddColumnCommand => addColumnCommand;

        private void CollectionContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= CollectionItemPropertyChanged;
                }
            }
            else if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += CollectionItemPropertyChanged;
                }
            }

            var sendingModel = sender as ObservableCollection<ColumnDefinitionItem>;
            if (sendingModel != null)
            {
                UpdateAvailableColumnsForForeignKeys();
            }
        }

        private void CollectionItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateSqlStatement();

            if (e.PropertyName == "ColumnName")
                UpdateAvailableColumnsForForeignKeys();
        }

        private void UpdateSqlStatement()
        {
            StatusInfo = string.Empty;

            try
            {
                var builder = QueryBuilder.Create(tableName);

                foreach (var definition in ColumnDefinitions)
                {
                    builder.AddColumn(definition.ColumnName,
                        definition.DataType,
                        definition.IsPrimary,
                        definition.IsNotNull,
                        definition.DefaultValue);
                }

                foreach (var foreignKey in ForeignKeyDefinitions)
                {
                    if (!string.IsNullOrWhiteSpace(foreignKey.SelectedColumn)
                       && !string.IsNullOrWhiteSpace(foreignKey.SelectedTable)
                       && !string.IsNullOrWhiteSpace(foreignKey.SelectedReferencedColumn))
                    {
                        builder.AddForeignKey(foreignKey.SelectedColumn, foreignKey.SelectedTable, foreignKey.SelectedReferencedColumn);
                    }
                }

                SqlStatement = builder.Build();
                IsValidTableDefinition = true;
            }
            catch (Exception ex)
            {
                StatusInfo = ex.Message;
                IsValidTableDefinition = false;
            }
        }

        private void UpdateAvailableColumnsForForeignKeys()
        {
            foreach (var foreignKey in ForeignKeyDefinitions)
            {
                foreignKey.AvailableColumns.Clear();
                foreach (var column in ColumnDefinitions)
                {
                    foreignKey.AvailableColumns.Add(column.ColumnName);
                }
            }
        }

        private void AddColumnDefinition()
        {
            ColumnDefinitions.Add(new ColumnDefinitionItem());
        }

        public void AddForeignKeyDefinition()
        {
            ForeignKeyDefinitionItem foreignKeyDefinition = selectedDatabase == null ?
                new ForeignKeyDefinitionItem(null) :
                new ForeignKeyDefinitionItem(selectedDatabase.Path);

            foreach (var column in ColumnDefinitions)
            {
                foreignKeyDefinition.AvailableColumns.Add(column.ColumnName);
            }

            ForeignKeyDefinitions.Add(foreignKeyDefinition);
        }

        private void Create()
        {
            StatusInfo = string.Empty;

            if (SelectedDatabase == null)
            {
                StatusInfo = localiser["TableCreator_NoDatabaseSelected"];
                return;
            }

            StatusInfo = databaseService.CreateTable(sqlStatement) ?
                localiser["TableCreator_TableCreateSuccess"] :
                "SQL logic error or missing database\r\n";
        }

        public string Title => localiser["WindowTitle_TableCreator"];

        public string DatabaseKey => localiser["TableCreator_Label_Database"];

        public string TableNameKey => localiser["TableCreator_Label_TableName"];

        public string ColumnsKey => localiser["TableCreator_TabHeader_Columns"];

        public string DefaultKey => localiser["TableCreator_Label_Default"];

        public string AddColumnKey => localiser["TableCreator_ButtonText_AddColumn"];

        public string ForeignKeysKey => localiser["TableCreator_TabHeader_ForeignKeys"];

        public string ReferencesKey => localiser["TableCreator_References"];

        public string CreateKey => localiser["ButtonText_Create"];

        public string CancelKey => localiser["ButtonText_Cancel"];
    }
}