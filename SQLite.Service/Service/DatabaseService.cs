using SQLite.Common;
using SQLite.Common.Contracts;
using SQLite.Service.Mapping;
using SQLite.Service.Model;
using Utility.Database;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.Service.Service
{
    public class DatabaseService
    {
        private readonly TreeService treeService;
        private readonly TreeViewMapper mapper;
        private readonly ILocaliser localiser;
        private readonly IMessageBoxService messageBoxService;
        private readonly IFileDialogService dialogService;

        Dictionary<string, string> dictionaryDatabasePaths = new();

        public DatabaseService(
            TreeService treeService,
            TreeViewMapper mapper,
            ILocaliser localiser,
            IMessageBoxService messageBoxService,
            IFileDialogService dialogService)
        {
            this.treeService = treeService;
            this.mapper = mapper;
            this.localiser = localiser;
            this.messageBoxService = messageBoxService;
            this.dialogService = dialogService;
        }

        public void OpenDatabase(DatabasePath databasePath)
        {
            var key = new DatabaseKey(new DatabasePath(databasePath.Name));
            if (treeService.TreeViewItems.Any(x => x.Key.Equals(key)))
                throw new Exception("d__434344fs7nbdfsd");

            DatabaseBranchItem databaseItem = mapper.Map(databasePath);
            treeService.TreeViewItems.Add(databaseItem);
            Info("Opened database '" + databaseItem.Name + "'.");

        }

        public void CloseDatabase(DatabasePath databasePath)
        {
            var key = new DatabaseKey(new DatabasePath(databasePath.Name));

            if (treeService.TreeViewItems.SingleOrDefault(x => x.Key.Equals(key)) is { } db)
            {
                treeService.TreeViewItems.Remove(db);
                Info("Closed database '" + db.Name + "'.");
            }
            else
                throw new Exception("ggdf333335");
        }

        public void CreateNewDatabase()
        {

            if (dialogService.Show(new("SQLite (*.sqlite)|*.sqlite", DialogType.Save)) is { FilePath: { } path, Success: true })
            {
                Utility.SQLite.Helpers.ConnectionFactory.CreateDatabase(path);
                OpenDatabase(new DatabasePath(path));
            }

        }

        public void CloseDatabase()
        {
            var selectedItem = treeService.SelectedItem.Key.DatabasePath;

            if (selectedItem != null)
                CloseDatabase(selectedItem);
        }

        public void OpenDatabase()
        {
            if (dialogService.Show(new("Database Files (*.sqlite, *.db)|*.sqlite; *db; |All Files |*.*", DialogType.Open)) is { FilePath: { } path, Success: true })
            {
                OpenDatabase(new DatabasePath(path));
            }
        }

        public void DeleteDatabase()
        {
            if (treeService.SelectedItem is not DatabaseBranchItem { Name: { } name, Key: { DatabasePath: { } dbName } })
            {
                return;
            }

            if (messageBoxService.ShowMessage(new(
                localiser["MessageBox_DatabaseDeleteWarning", name],
                localiser["MessageBoxTitle_DatabaseDeletion"],
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning)) == true)
            {
                var filePath = dictionaryDatabasePaths[dbName];
                if (!System.IO.File.Exists(filePath))
                    throw new FileNotFoundException("Database file could not be found.");

                System.IO.File.Delete(filePath);
                CloseDatabase(new DatabasePath(filePath));
            }
        }

        public bool CreateTable(string sqlStatement)
        {
            //StatusInfo = string.Empty;

            //if (SelectedDatabase == null)
            //{
            //    StatusInfo = localiser["TableCreator_NoDatabaseSelected"];
            //    return;
            //}

            if (!string.IsNullOrEmpty(sqlStatement))
            {
                //var database = SelectedDatabase as DatabaseSelectItem;
                var dbHandler = new DatabaseHandler(new DatabasePath(CurrentDatabasePath.FullName));

                try
                {
                    if (sqlStatement.StartsWith("CREATE TABLE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        dbHandler.ExecuteNonQuery(sqlStatement);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Error("An error occured when the user tried to create a table from the TableCreator.", ex);
                }
            }
            return false;
        }

        public FileInfo CurrentDatabasePath => treeService.SelectedItem.Key.DatabasePath.AsFileInfo;
    }
}
