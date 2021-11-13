using System.Text.Json;
using Utility;
using Utility.Common.Base;
using Utility.Entity;

namespace SQLite.Service.Repository
{
    public class TabsRepository : IRepository
    {
        readonly Lazy<Dictionary<string, List<Key>>> dictionary;
        readonly JsonSerializerOptions settings = new() { WriteIndented = true };
        public TabsRepository()
        {
            settings.Converters.Add(new AbstractClassConverter<Key>());

            dictionary = new(() =>
            {
                Dictionary<string, List<Key>>? dictionary = null;
                if (File.Exists(GetSaveLocationPath()))
                {
                    using var stream = File.OpenRead(GetSaveLocationPath());
                    if (stream.Length == 0)
                        dictionary = new();
                    else
                        dictionary = JsonSerializer.Deserialize<Dictionary<string, List<Key>>>(stream, settings);
                }
                return dictionary ?? new();
            });

        }

        public IReadOnlyCollection<Key>? Load(TreeItem? treeItem)
        {
            if (treeItem == null)
            {
                return Array.Empty<Key>();
            }

            return dictionary.Value.GetValueOrDefault(JsonSerializer.Serialize(treeItem, settings));
        }

        public void Save(TreeItem treeItem, List<Key> content)
        {
            dictionary.Value[JsonSerializer.Serialize(treeItem, settings)] = content;
        }

        public void PersistAll()
        {

            string json = JsonSerializer.Serialize(dictionary.Value, settings);
            File.WriteAllText(GetSaveLocationPath(), json);
        }

        private static string GetSaveLocationPath()
        {
            var directory = Directory.CreateDirectory("../../../Data");
            return Path.Combine(directory.FullName, "Tabs.json");
        }
    }
}
