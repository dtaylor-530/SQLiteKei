using ReactiveUI;
using System.Collections.ObjectModel;
using Utility.Database.Common;

namespace Database.Entity
{
    /// <summary>
    /// ViewModel item that defines a foreign key.
    /// </summary>
    public class ForeignKeyDefinitionItem : ReactiveObject
    {
        private IDatabaseKey? selectedDatabasePath;
        //private readonly IDatabaseHandlerFactory databaseHandlerFactory;
        //private readonly IHandlerService tableHandlerFactory;
        private string selectedColumn;
        private ITableKey selectedTable;
        private string selectedReferencedColumn;

        private ObservableCollection<string> availableTables = new ObservableCollection<string>();
        private ObservableCollection<string> availableColumns = new ObservableCollection<string>();
        private ObservableCollection<string> referencableColumns = new ObservableCollection<string>();

        public ForeignKeyDefinitionItem(/*IHandlerService tableHandlerFactory,*/ IDatabaseKey? selectedDatabasePath = null)
        {
            SelectedDatabasePath = selectedDatabasePath;
            //this.tableHandlerFactory = tableHandlerFactory;
            UpdateReferencableTables();
        }

        public IDatabaseKey SelectedDatabasePath
        {
            get { return selectedDatabasePath; }
            set { selectedDatabasePath = value; UpdateReferencableTables(); }
        }

        public ITableKey SelectedTable
        {
            get { return selectedTable; }
            set { selectedTable = value; UpdateReferencableColumns(); }
        }
        public string SelectedColumn
        {
            get { return selectedColumn; }
            set { this.RaiseAndSetIfChanged(ref selectedColumn, value); }
        }

        public ObservableCollection<string> AvailableColumns
        {
            get { return availableColumns; }
            set { this.RaiseAndSetIfChanged(ref availableColumns, value); }
        }

        public ObservableCollection<string> ReferencableTables
        {
            get { return availableTables; }
            set { this.RaiseAndSetIfChanged(ref availableTables, value); }
        }

        public ObservableCollection<string> ReferencableColumns
        {
            get { return referencableColumns; }
            set { this.RaiseAndSetIfChanged(ref referencableColumns, value); }
        }

        public string SelectedReferencedColumn
        {
            get { return selectedReferencedColumn; }
            set { this.RaiseAndSetIfChanged(ref selectedReferencedColumn, value); }
        }

        public void UpdateReferencableTables()
        {
            //if (selectedDatabasePath == null) return;
            //ReferencableTables.Clear();
            //var tables = tableHandlerFactory.Database(SelectedDatabasePath, dbHandler =>
            // {
            //     var tables = dbHandler.Tables;
            //     return tables.Select(a => a.Name.Name).ToArray();

            // });
            //ReferencableTables.AddRange(tables);
        }

        private void UpdateReferencableColumns()
        {
            //if (selectedDatabasePath == null) return;
            ////if (string.IsNullOrEmpty(SelectedDatabasePath.Path)) return;
            //ReferencableColumns.Clear();
            //var columns = tableHandlerFactory.Table(SelectedTable, dbHandler =>
            //{
            //    var columns = dbHandler.Columns;
            //    return columns.Select(a => a.Name).ToArray();

            //});

            //ReferencableColumns.AddRange(columns);
        }
    }

}
