using SQLite.Common.Contracts;
using SQLite.Service.Mapping;
using SQLite.Service.Model;
using System.Text.Json;
using Utility.Database;
using static SQLite.Common.Log;

namespace SQLite.Service.Repository
{

    public class TreeRepository : ITreeRepository
    {

        private readonly TreeViewMapper treeViewMapper;

        public TreeRepository(TreeViewMapper treeViewMapper)
        {
            this.treeViewMapper = treeViewMapper;
        }

        public void Save(IReadOnlyCollection<DatabaseTreeItem> tree)
        {
            var targetPath = GetSaveLocationPath();
            var rootItemDatabasePaths = tree.Select(x => x.Key).ToList();

            try
            {
                //JsonSerializer.Serialize(streamWriter, rootItemDatabasePaths, typeof(List<ConnectionPath>));

                string json = JsonSerializer.Serialize(rootItemDatabasePaths);
                System.IO.File.WriteAllText(targetPath, json);
                //xmlSerializer.Serialize(streamWriter, rootItemDatabasePaths);
                Info("Successfully saved database tree.");
            }
            catch (Exception ex)
            {
                Error("Unable to save database tree.", ex);
            }

        }

        public IReadOnlyCollection<DatabaseTreeItem> Load()
        {
            var targetPath = GetSaveLocationPath();

            if (!System.IO.File.Exists(targetPath))
            {
                return Array.Empty<DatabaseTreeItem>();
            }

            var resultCollection = new List<DatabaseTreeItem>();

            using (var streamReader = new StreamReader(targetPath))
                try
                {
                    resultCollection.AddRange(DatabaseItems(treeViewMapper, streamReader));
                }
                catch (Exception ex)
                {
                    Error("Could not load database tree.", ex);
                }

            return resultCollection;

            static IEnumerable<DatabaseBranchItem> DatabaseItems(TreeViewMapper mapper, StreamReader streamReader)
            {
                Info("Restoring recently used databases...");

                //var databasePaths = new XmlSerializer(typeof(List<string>)).Deserialize(streamReader) as List<string>;
                var json = streamReader.ReadToEnd();
                List<DatabaseKey> databaseKeys = JsonSerializer.Deserialize<List<DatabaseKey>>(json);

                foreach (var key in databaseKeys)
                {
                    DatabaseBranchItem? item = null;
                    try
                    {
                        var path = key.DatabasePath;
                        item = mapper.Map(key.DatabasePath);
                    }
                    catch
                    {
                        Info("Could not restore database file at " + key + "\nThe file might have been moved or deleted outside of SQLK.");
                    }

                    if (item != null)
                        yield return item;
                }
            }
        }

        private static string GetSaveLocationPath()
        {
            //var roamingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //return Path.Combine(roamingDirectory, "SQLiteKei", "TreeView.xml");

            var directory = Directory.CreateDirectory("../../../Data");
            return Path.Combine(directory.FullName, "TreeView.json");
        }
    }
}