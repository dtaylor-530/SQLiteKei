using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using log = SQLiteKei.Helpers.Log;
using System.Windows.Input;
using ReactiveUI;

namespace SQLiteKei.ViewModels.TableCreatorWindow
{
    public class TableCreatorViewModel : NotifyingModel
    {

        private DatabaseSelectItem selectedDatabase;
        private string tableName;
        private string sqlStatement;
        private bool isValidTableDefinition;
        private readonly ICommand addColumnCommand;
        private readonly ICommand createCommand;


        public TableCreatorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();
            ColumnDefinitions = new ObservableCollection<ColumnDefinitionItem>();
            ForeignKeyDefinitions = new ObservableCollection<ForeignKeyDefinitionItem>();

            ColumnDefinitions.CollectionChanged += CollectionContentChanged;
            ForeignKeyDefinitions.CollectionChanged += CollectionContentChanged;

            addColumnCommand = ReactiveCommand.Create(AddColumnDefinition);
            addForeignKeyCommand = ReactiveCommand.Create(AddForeignKeyDefinition);
            createCommand = ReactiveCommand.Create(Create);
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
                    foreignKey.SelectedDatabasePath = selectedDatabase.DatabasePath;
                }
            }
        }

        public List<DatabaseSelectItem> Databases { get; set; }

        public ObservableCollection<ColumnDefinitionItem> ColumnDefinitions { get; set; }

        public ObservableCollection<ForeignKeyDefinitionItem> ForeignKeyDefinitions { get; set; }

        public string TableName
        {
            get { return tableName; }
            set 
            { 
                tableName = value; 
                NotifyPropertyChanged("TableName");
                UpdateSqlStatement();
            }
        }

        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; NotifyPropertyChanged("SqlStatement"); }
        }

        public bool IsValidTableDefinition
        {
            get { return isValidTableDefinition; }
            set { isValidTableDefinition = value; NotifyPropertyChanged("IsValidTableDefinition"); }
        }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }



        private void CollectionContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (NotifyingModel item in e.OldItems)
                {
                    item.PropertyChanged -= CollectionItemPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (NotifyingModel item in e.NewItems)
                {
                    item.PropertyChanged += CollectionItemPropertyChanged;
                }
            }

            var sendingModel = sender as ObservableCollection<ColumnDefinitionItem>;
            if(sendingModel != null)
            {
                UpdateAvailableColumnsForForeignKeys();
            }
        }

        private void CollectionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
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

                foreach(var foreignKey in ForeignKeyDefinitions)
                {
                    if(!string.IsNullOrWhiteSpace(foreignKey.SelectedColumn)
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
            foreach(var foreignKey in ForeignKeyDefinitions)
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


        public ICommand AddColumnCommand => addColumnCommand;

        public void AddForeignKeyDefinition()
        {
            ForeignKeyDefinitionItem foreignKeyDefinition;

            if (selectedDatabase == null)
                foreignKeyDefinition = new ForeignKeyDefinitionItem(string.Empty);
            else
                foreignKeyDefinition = new ForeignKeyDefinitionItem(selectedDatabase.DatabasePath);

            foreach(var column in ColumnDefinitions)
            {
                foreignKeyDefinition.AvailableColumns.Add(column.ColumnName);
            }

            ForeignKeyDefinitions.Add(foreignKeyDefinition);
        }

        private readonly ICommand addForeignKeyCommand;

        public ICommand AddForeignKeyCommand { get { return addForeignKeyCommand; } }

        private void Create()
        {
            StatusInfo = string.Empty;

            if (SelectedDatabase == null)
            {
                StatusInfo = LocalisationHelper.GetString("TableCreator_NoDatabaseSelected");
                return;
            }

            if (!string.IsNullOrEmpty(SqlStatement))
            {
                var database = SelectedDatabase as DatabaseSelectItem;
                var dbHandler = new DatabaseHandler(database.DatabasePath);

                try
                {
                    if (SqlStatement.StartsWith("CREATE TABLE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        dbHandler.ExecuteNonQuery(SqlStatement);
                        StatusInfo = LocalisationHelper.GetString("TableCreator_TableCreateSuccess");
                    }
                }
                catch (Exception ex)
                {
                    log.Logger.Error("An error occured when the user tried to create a table from the TableCreator.", ex);
                    StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                }
            }
        }


    }
}
