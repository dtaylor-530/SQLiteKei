using ReactiveUI;
using System.Text.Json;
using Utility.Chart;
using Utility.Database;

namespace SQLite.Service.Repository
{
    public class SeriesPairRepository
    {
        readonly Lazy<Dictionary<string, List<SeriesPair>>> dictionary = new();
        private JsonSerializerOptions settings;

        public SeriesPairRepository()
        {
            settings = new JsonSerializerOptions { WriteIndented = true };

            this.dictionary = new(() =>
            {
                Dictionary<string, List<SeriesPair>>? dictionary = null;
                if (File.Exists(GetSaveLocationPath()))
                {
                    using var stream = File.OpenRead(GetSaveLocationPath());
                    if (stream.Length == 0)
                        dictionary = new();
                    else
                        dictionary = JsonSerializer.Deserialize<Dictionary<string, List<SeriesPair>>>(stream, settings);
                }
                return dictionary ?? new();
            });
        }

        public void Save(TableKey configuration, List<SeriesPair> pairs)
        {
            dictionary.Value[JsonSerializer.Serialize(configuration)] = pairs;
        }

        public IReadOnlyCollection<SeriesPair> Load(TableKey configuration)
        {

            if (configuration == null)
            {
                return Array.Empty<SeriesPair>();
            }

            return dictionary.Value.GetValueOrDefault(JsonSerializer.Serialize(configuration)) ?? new List<SeriesPair>();
        }

        public void PersistAll()
        {
            string json = JsonSerializer.Serialize(dictionary.Value);
            System.IO.File.WriteAllText(GetSaveLocationPath(), json);
        }

        private static string GetSaveLocationPath()
        {
            var directory = Directory.CreateDirectory("../../../Data");
            return Path.Combine(directory.FullName, "SeriesPairs.json");
        }
    }

}
