using SQLite.Common.Contracts;
using System.Text.Json;
using Utility;
using Utility.Database;

namespace SQLite.ViewModel.Infrastructure.Service
{
    public class TabsRepository
    {
        readonly Lazy<Dictionary<string, List<DatabaseKey>>> dictionary;
        readonly JsonSerializerOptions settings = new() { WriteIndented = true };
        public TabsRepository()
        {
            settings.Converters.Add(new AbstractClassConverter<DatabaseKey>());

            dictionary = new(() =>
            {
                Dictionary<string, List<DatabaseKey>>? dictionary = null;
                if (File.Exists(GetSaveLocationPath()))
                {
                    using var stream = File.OpenRead(GetSaveLocationPath());
                    if (stream.Length == 0)
                        dictionary = new();
                    else
                        dictionary = JsonSerializer.Deserialize<Dictionary<string, List<DatabaseKey>>>(stream, settings);
                }
                return dictionary ?? new();
            });

        }

        public IReadOnlyCollection<DatabaseKey>? Load(DatabaseTreeItem? treeItem)
        {
            if (treeItem == null)
            {
                return Array.Empty<DatabaseKey>();
            }

            return dictionary.Value.GetValueOrDefault(JsonSerializer.Serialize(treeItem));
        }

        public void Save(DatabaseTreeItem treeItem, List<DatabaseKey> content)
        {
            dictionary.Value[JsonSerializer.Serialize(treeItem)] = content;
        }

        public void PersistAll()
        {

            string json = JsonSerializer.Serialize(dictionary.Value, settings);
            System.IO.File.WriteAllText(GetSaveLocationPath(), json);
        }

        private static string GetSaveLocationPath()
        {
            var directory = Directory.CreateDirectory("../../../Data");
            return Path.Combine(directory.FullName, "Tabs.json");
        }
    }
}
