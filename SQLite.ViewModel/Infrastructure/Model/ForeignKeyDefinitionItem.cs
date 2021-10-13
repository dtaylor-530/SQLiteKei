using ReactiveUI;
using System.Collections.ObjectModel;
using Utility.Database;
using Utility.SQLite.Database;
using Utility.SQLite.Helpers;

namespace SQLite.ViewModel
{
    /// <summary>
    /// ViewModel item that defines a foreign key.
    /// </summary>
    public class ForeignKeyDefinitionItem : ReactiveObject
    {
        private ConnectionPath selectedDatabasePath;
        private string selectedColumn;
        private string selectedTable;
        private string selectedReferencedColumn;

        private ObservableCollection<string> availableTables = new ObservableCollection<string>();
        private ObservableCollection<string> availableColumns = new ObservableCollection<string>();
        private ObservableCollection<string> referencableColumns = new ObservableCollection<string>();

        public ForeignKeyDefinitionItem(ConnectionPath selectedDatabasePath)
        {
            this.selectedDatabasePath = selectedDatabasePath;
            UpdateReferencableTables();
        }

        public ConnectionPath SelectedDatabasePath
        {
            get { return selectedDatabasePath; }
            set { selectedDatabasePath = value; UpdateReferencableTables(); }
        }

        public ObservableCollection<string> AvailableColumns
        {
            get { return availableColumns; }
            set { this.RaiseAndSetIfChanged(ref availableColumns, value); }
        }

        private void UpdateReferencableColumns()
        {
            if (string.IsNullOrEmpty(SelectedDatabasePath.Path)) return;

            using (var tableHandler = new TableHandler(SelectedDatabasePath, selectedTable))
            {
                var columns = tableHandler.DataTable.Columns();
                {
                    ReferencableColumns.Clear();

                    foreach (var column in columns)
                    {
                        ReferencableColumns.Add(column.Name);
                    }
                }
            }
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

        public string SelectedTable
        {
            get { return selectedTable; }
            set { selectedTable = value; UpdateReferencableColumns(); }
        }
        public string SelectedColumn
        {
            get { return selectedColumn; }
            set { this.RaiseAndSetIfChanged(ref selectedColumn, value); }
        }

        public void UpdateReferencableTables()
        {
            if (string.IsNullOrEmpty(SelectedDatabasePath.Path)) return;

            using (var dbHandler = new DatabaseHandler(SelectedDatabasePath))
            {
                var tables = dbHandler.Tables;
                ReferencableTables.Clear();

                foreach (var table in tables)
                {
                    ReferencableTables.Add(table.Name);
                }
            }
        }

    }
}