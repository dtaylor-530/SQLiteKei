using SQLite.Common;
using SQLite.Common.Contracts;
using Utility.Database;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.ViewModel.Infrastructure.Service
{
    public class DatabaseService
    {
        private readonly TreeService treeService;
        private readonly ILocaliser localiser;
        private readonly IMessageBoxService messageBoxService;
        private readonly IFileDialogService dialogService;

        public DatabaseService(
            TreeService treeService,
            ILocaliser localiser,
            IMessageBoxService messageBoxService,
            IFileDialogService dialogService)
        {
            this.treeService = treeService;
            this.localiser = localiser;
            this.messageBoxService = messageBoxService;
            this.dialogService = dialogService;
        }


        public void OpenDatabase(string databasePath)
        {
            if (treeService.TreeViewItems.Any(x => x.DatabasePath.Path.Equals(databasePath)))
                return;

            using (DatabaseHandler handler = new DatabaseHandler(new ConnectionPath(databasePath)))
            {
                DatabaseItem databaseItem = SchemaToViewModelMapper.MapSchemaToViewModel(localiser, handler);
                treeService.TreeViewItems.Add(databaseItem);
                Info("Opened database '" + databaseItem.DisplayName + "'.");
            }
        }

        public void CloseDatabase(string databasePath)
        {
            if (treeService.TreeViewItems.SingleOrDefault(x => x.DatabasePath.Equals(databasePath)) is { } db)
            {
                treeService.TreeViewItems.Remove(db);
                Info("Closed database '" + db.DisplayName + "'.");
            }
            else
                throw new Exception("ggdf333335");
        }



        public void CreateNewDatabase()
        {

            if (dialogService.Show(new("SQLite (*.sqlite)|*.sqlite", DialogType.Save)) is { FilePath: { } path, Success: true })
            {
                Utility.SQLite.Helpers.ConnectionFactory.CreateDatabase(path);
                OpenDatabase(path);
            }

        }

        public void CloseDatabase()
        {
            var selectedItem = treeService.SelectedItem;

            if (selectedItem != null)
                CloseDatabase(selectedItem.DatabasePath.Path);
        }

        public void OpenDatabase()
        {

            if (dialogService.Show(new("Database Files (*.sqlite, *.db)|*.sqlite; *db; |All Files |*.*", DialogType.Open)) is { FilePath: { } path, Success: true })
            {
                OpenDatabase(path);
            }
        }

        public void DeleteDatabase()
        {
            if (treeService.SelectedItem is not DatabaseItem { DisplayName: { } name, DatabasePath: { Path: { } path } })
            {
                return;
            }

            if (messageBoxService.ShowMessage(new(
                localiser["MessageBox_DatabaseDeleteWarning", name],
                localiser["MessageBoxTitle_DatabaseDeletion"],
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning)) == true)
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException("Database file could not be found.");

                File.Delete(path);
                CloseDatabase(path);
            }
        }
    }
}
