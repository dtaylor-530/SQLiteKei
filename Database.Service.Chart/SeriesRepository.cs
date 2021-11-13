using ReactiveUI;
using System.Text.Json;
using Utility.Chart.Entity;
using Utility.Common.Base;
using Utility.Entity;

namespace SQLite.Service.Repository
{
    public class SeriesRepository : IRepository
    {
        readonly Lazy<Dictionary<string, List<Series>>> dictionary;
        readonly JsonSerializerOptions settings = new() { WriteIndented = true };
        public SeriesRepository()
        {

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

        public void Save(IKey configuration, List<Series> pairs)
        {
            dictionary.Value[JsonSerializer.Serialize(configuration)] = pairs;
        }

        public IReadOnlyCollection<Series> Load(IKey key)
        {

            if (key == null)
            {
                return Array.Empty<Series>();
            }

            return dictionary.Value.GetValueOrDefault(JsonSerializer.Serialize(key)) ?? new List<Series>(); ;
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
