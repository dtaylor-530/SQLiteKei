using SQLite.Common.Contracts;
using System.Collections.ObjectModel;
using System.Text.Json;
using Utility.Database;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.ViewModel.Infrastructure.Common;

public class TreeRepository : ITreeRepository
{
    private readonly ILocaliser localiser;

    public TreeRepository(ILocaliser localiser)
    {
        this.localiser = localiser;
    }

    public void Save(IReadOnlyCollection<TreeItem> tree)
    {
        var targetPath = GetSaveLocationPath();
        var rootItemDatabasePaths = tree.Select(x => x.DatabasePath).ToList();

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
            return new ObservableCollection<TreeItem>();
        }

        var resultCollection = new List<TreeItem>();

        using (var streamReader = new StreamReader(targetPath))
            try
            {
                resultCollection.AddRange(DatabaseItems(localiser, streamReader));
            }
            catch (Exception ex)
            {
                Error("Could not load database tree.", ex);
            }

        return resultCollection;

        static IEnumerable<DatabaseItem> DatabaseItems(ILocaliser localiser, StreamReader streamReader)
        {
            Info("Restoring recently used databases...");

            //var databasePaths = new XmlSerializer(typeof(List<string>)).Deserialize(streamReader) as List<string>;
            var json = streamReader.ReadToEnd();
            List<ConnectionPath> databasePaths = JsonSerializer.Deserialize<List<ConnectionPath>>(json);
            var schemaMapper = new SchemaToViewModelMapper();

            foreach (var path in databasePaths.Select(a => a.Path))
            {
                if (File.Exists(path))
                {
                    using (var databaseHandler = new DatabaseHandler(new ConnectionPath(path)))
                    {
                        var rootItem = SchemaToViewModelMapper.Map(localiser, databaseHandler);
                        yield return rootItem;
                    }
                }
                else
                {
                    Info("Could not restore database file at " + path + "\nThe file might have been moved or deleted outside of SQLK.");
                }
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
