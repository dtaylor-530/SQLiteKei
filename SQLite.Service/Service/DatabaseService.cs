using Database.Entity;
using SQLite.Common.Contracts;
using SQLite.Service.Mapping;
using System.Reactive.Linq;
using Utility.Common.Base;
using Utility.Database;
using Utility.Database.SQLite.Common.Abstract;
using Utility.Entity;
using static Utility.Common.Base.Log;

namespace SQLite.Service.Service
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMap map;
        private readonly ITreeModel treeModel;
        private readonly TreeViewMapper mapper;
        private readonly ILocaliser localiser;
        private readonly IMessageBoxService messageBoxService;
        private readonly IFileDialogService dialogService;
        private readonly IHandlerService databaseHandlerFactory;

        public DatabaseService(
            IMap map,
            ITreeModel treeModel,
            TreeViewMapper treeViewMapper,
            ILocaliser localiser,
            IMessageBoxService messageBoxService,
            IFileDialogService dialogService,
            IHandlerService databaseHandlerFactory)
        {
            this.map = map;
            this.treeModel = treeModel;
            this.mapper = treeViewMapper;
            this.localiser = localiser;
            this.messageBoxService = messageBoxService;
            this.dialogService = dialogService;
            this.databaseHandlerFactory = databaseHandlerFactory;
        }

        public void OpenDatabase(Key key)
        {
            if (treeModel.TreeViewItems.Any(x => x.Key.Equals(key)))
                throw new Exception("d__434344fs7nbdfsd");

            mapper.Map(key)
                .Subscribe(databaseItem =>
                {
                    treeModel.OnNext(new(Adds: new[] { databaseItem }));
                    Info("Opened database '" + databaseItem.Name + "'.");
                });
        }

        public void CloseDatabase(IKey key)
        {
            if (treeModel.TreeViewItems.SingleOrDefault(x => x.Key.Equals(key)) is { } db)
            {
                //treeModel.TreeViewItems.Remove(db);
                treeModel.OnNext(new(Removes: new[] { db.Key }));
                Info("Closed database '" + db.Name + "'.");
            }
            else
                throw new Exception("ggdf333335");
        }

        public void CreateNewDatabase()
        {
            if (dialogService.Show(new("SQLite (*.sqlite)|*.sqlite", DialogType.Save)) is { FilePath: { } path, Success: true })
            {
                var connection = map.Map<DatabasePath, ConnectionResult>(new DatabasePath(path));
                //OpenDatabase(new DatabaseKey(new DatabasePath(path), this.GetType()));
            }
        }

        public void CloseDatabase()
        {
            var selectedItem = treeModel.SelectedItem.Key;

            if (selectedItem != null)
                CloseDatabase(selectedItem);
        }

        public void OpenDatabase()
        {
            if (dialogService.Show(new("Database Files (*.sqlite, *.db)|*.sqlite; *db; |All Files |*.*", DialogType.Open)) is { FilePath: { } path, Success: true })
            {
                OpenDatabase(new DatabaseKey(new DatabasePath(path), this.GetType()));
            }
        }

        public void DeleteDatabase()
        {
            if (treeModel.SelectedItem is not DatabaseBranchItem { Name: { } name, Key: { DatabasePath: { } dbName } key })
            {
                return;
            }

            if (messageBoxService.ShowMessage(new(
                localiser["MessageBox_DatabaseDeleteWarning", name],
                localiser["MessageBoxTitle_DatabaseDeletion"],
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning)) == true)
            {
                //var filePath = dictionaryDatabasePaths[dbName];
                //if (!System.IO.File.Exists(filePath))
                //    throw new FileNotFoundException("Database file could not be found.");

                //System.IO.File.Delete(filePath);
                CloseDatabase(key);
            }
        }

        public IObservable<bool> CreateTable(string sqlStatement)
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
                return databaseHandlerFactory.Database(CurrentDatabaseKey, dbHandler =>
                {

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
                    return false;
                });
            }
            return Observable.Return(false);
        }

        DatabaseKey CurrentDatabaseKey => (treeModel.SelectedItem.Key as DatabaseKey) ?? throw new Exception("Evfsd dsfd");
    }
}
