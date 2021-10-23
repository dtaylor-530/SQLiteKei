using ReactiveUI;
//using System.Reactive.Linq;
using System.Text.Json;
using Utility;
using Utility.Chart;
using Utility.Database;

namespace SQLite.Service.Repository
{
    public class SeriesRepository
    {
        readonly Lazy<Dictionary<string, List<Series>>> dictionary;
        readonly JsonSerializerOptions settings;
        public SeriesRepository()
        {
            settings = new JsonSerializerOptions { WriteIndented = true };

            this.dictionary = new(() =>
            {
                Dictionary<string, List<Series>>? dictionary = null;
                if (File.Exists(GetSaveLocationPath()))
                {
                    using var stream = File.OpenRead(GetSaveLocationPath());
                    if (stream.Length == 0)
                        dictionary = new();
                    else
                        dictionary = JsonSerializer.Deserialize<Dictionary<string, List<Series>>>(stream, settings);
                }
                return dictionary ?? new();
            });

        }

        public void Save(TableKey configuration, List<Series> pairs)
        {
            dictionary.Value[JsonSerializer.Serialize(configuration)] = pairs;
        }

        public IReadOnlyCollection<Series> Load(Key configuration)
        {

            if (configuration == null)
            {
                return Array.Empty<Series>();
            }

            return dictionary.Value.GetValueOrDefault(JsonSerializer.Serialize(configuration)) ?? new List<Series>(); ;
        }

        public void PersistAll()
        {
            string json = JsonSerializer.Serialize(dictionary.Value);
            System.IO.File.WriteAllText(GetSaveLocationPath(), json);
        }

        private static string GetSaveLocationPath()
        {
            var directory = Directory.CreateDirectory("../../../Data");
            return Path.Combine(directory.FullName, "Series.json");
        }
    }

}
