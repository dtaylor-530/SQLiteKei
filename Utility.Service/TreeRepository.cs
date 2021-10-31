using SQLite.Common.Contracts;
using SQLite.Service.Mapping;
using System.Text.Json;
using Utility.Common.Base;
using static Utility.Common.Base.Log;

namespace Utility.Service
{

    public class TreeRepository : ITreeRepository
    {

        private readonly ITreeViewMapper treeViewMapper;

        public TreeRepository(ITreeViewMapper treeViewMapper)
        {
            this.treeViewMapper = treeViewMapper;
        }

        public void Save(IReadOnlyCollection<TreeItem> tree)
        {
            var targetPath = GetSaveLocationPath();
            var rootItemDatabasePaths = tree.Select(x => x.Key).ToList();

            try
            {
                //JsonSerializer.Serialize(streamWriter, rootItemDatabasePaths, typeof(List<ConnectionPath>));

                string json = JsonSerializer.Serialize(rootItemDatabasePaths);
                File.WriteAllText(targetPath, json);
                //xmlSerializer.Serialize(streamWriter, rootItemDatabasePaths);
                Info("Successfully saved database tree.");
            }
            catch (Exception ex)
            {
                Error("Unable to save database tree.", ex);
            }

        }

        public IReadOnlyCollection<TreeItem> Load()
        {
            var targetPath = GetSaveLocationPath();

            if (!File.Exists(targetPath))
            {
                return Array.Empty<TreeItem>();
            }

            var resultCollection = new List<TreeItem>();

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

            static IEnumerable<TreeItem> DatabaseItems(ITreeViewMapper mapper, StreamReader streamReader)
            {
                Info("Restoring recently used databases...");

                //var databasePaths = new XmlSerializer(typeof(List<string>)).Deserialize(streamReader) as List<string>;
                var json = streamReader.ReadToEnd();
                List<Key> databaseKeys = JsonSerializer.Deserialize<List<Key>>(json);

                foreach (var key in databaseKeys)
                {
                    TreeItem? item = null;
                    try
                    {

                        item = mapper.Map(key);
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