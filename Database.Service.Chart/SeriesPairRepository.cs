﻿using System.Text.Json;
using Utility.Chart.Entity;
using Utility.Common.Base;
using Utility.Entity;

namespace SQLite.Service.Repository
{

    public class SeriesPairRepository : IRepository
    {
        readonly Lazy<Dictionary<string, List<SeriesPair>>> dictionary = new();
        private JsonSerializerOptions settings = new JsonSerializerOptions { WriteIndented = true };

        public SeriesPairRepository()
        {
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

        public void Save(IKey configuration, List<SeriesPair> pairs)
        {
            dictionary.Value[JsonSerializer.Serialize(configuration)] = pairs;
        }

        public IReadOnlyCollection<SeriesPair> Load(IKey configuration)
        {

            if (configuration == null)
            {
                return Array.Empty<SeriesPair>();
            }

            return dictionary.Value.GetValueOrDefault(JsonSerializer.Serialize(configuration, settings)) ?? new List<SeriesPair>();
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
