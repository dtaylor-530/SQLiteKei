using ReactiveUI;
using SQLite.Service.Service;
using System.Text.Json;
using Utility;

namespace SQLite.Service.Repository;

public class IsSelectedRepository
{
    readonly Lazy<Dictionary<string, bool>> dictionary = new();
    readonly JsonSerializerOptions settings = new JsonSerializerOptions { WriteIndented = true };

    public IsSelectedRepository()
    {
        settings.Converters.Add(new AbstractClassConverter<Key>());

        dictionary = new(() =>
        {
            Dictionary<string, bool>? dictionary = null;
            if (File.Exists(GetSaveLocationPath()))
            {
                using var stream = File.OpenRead(GetSaveLocationPath());
                if (stream.Length == 0)
                    dictionary = new();
                else
                    dictionary = JsonSerializer.Deserialize<Dictionary<string, bool>>(stream, settings);
            }
            return dictionary ?? new();
        });
    }

    public void Save(Key key, IsSelected pairs)
    {
        var serialisedKey = JsonSerializer.Serialize(key, key.GetType(), settings);
        dictionary.Value[serialisedKey] = pairs.Value;
    }

    public IsSelected Load(Key key)
    {
        if (key == null)
        {
            return new IsSelected(false);
        }
        var serialisedKey = JsonSerializer.Serialize(key, key.GetType(), settings);
        var ret = dictionary.Value.GetValueOrDefault(serialisedKey);
        return new IsSelected(ret);
    }

    public void PersistAll()
    {
        string json = JsonSerializer.Serialize(dictionary.Value, settings);
        File.WriteAllText(GetSaveLocationPath(), json);
    }

    private static string GetSaveLocationPath()
    {
        var directory = Directory.CreateDirectory("../../../Data");
        return Path.Combine(directory.FullName, "IsSelected.json");
    }
}
