using SQLite.Service.Mapping;
using System.Reactive.Linq;
using System.Text.Json;
using Utility.Common.Base;
using Utility.Entity;
using static Utility.Common.Base.Log;

namespace Utility.Service
{

    public class TreeRepository : ITreeRepository
    {

        private readonly ITreeViewMapper treeViewMapper;
        private readonly HashSet<TreeItem> items = new();

        public TreeRepository(ITreeViewMapper treeViewMapper)
        {
            this.treeViewMapper = treeViewMapper;
        }

        public void Save(IReadOnlyCollection<TreeItem> tree)
        {
            foreach (var res in tree)
                items.Add(res);
        }

        public IObservable<IReadOnlyCollection<TreeItem>> Load()
        {
            var targetPath = GetSaveLocationPath();

            if (!File.Exists(targetPath))
            {
                return Observable.Return(Array.Empty<TreeItem>());
            }


            using (var streamReader = new StreamReader(targetPath))
                try
                {
                    return DatabaseItems(treeViewMapper, streamReader)
                        .ToObservable()
                        .SelectMany(a => a)
                        .Select(a =>
                        {
                            items.Add(a);
                            return items;
                        });
                }
                catch (Exception ex)
                {
                    Error("Could not load database tree.", ex);
                }


            return Observable.Return(Array.Empty<TreeItem>());

            static IEnumerable<IObservable<TreeItem>> DatabaseItems(ITreeViewMapper mapper, StreamReader streamReader)
            {
                Info("Restoring recently used databases...");

                //var databasePaths = new XmlSerializer(typeof(List<string>)).Deserialize(streamReader) as List<string>;
                var json = streamReader.ReadToEnd();
                List<Key> databaseKeys = JsonSerializer.Deserialize<List<Key>>(json);

                foreach (var key in databaseKeys)
                {


                    yield return mapper.Map(key);

                    //  Info("Could not restore database file at " + key + "\nThe file might have been moved or deleted outside of SQLK.");


                }
            }
        }

        public void PersistAll()
        {
            var targetPath = GetSaveLocationPath();
            var rootItemDatabasePaths = items.Select(x => x.Key).ToList();

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
        private static string GetSaveLocationPath()
        {
            //var roamingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //return Path.Combine(roamingDirectory, "SQLiteKei", "TreeView.xml");

            var directory = Directory.CreateDirectory("../../../Data");
            return Path.Combine(directory.FullName, "TreeView.json");
        }
    }
}