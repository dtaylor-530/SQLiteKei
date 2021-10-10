using SQLite.Common.Contracts;
using SQLite.Data.Localisation;

namespace SQLite.Data;

public class LocalisationSource : ILocaliser
{
    public string this[string key] => Resources.ResourceManager.GetString(key) ?? throw new Exception("sfd)()Ed");

    public string this[string key, params object[] args] => string.Format(Resources.ResourceManager.GetString(key), args) ?? throw new Exception("sfd)()Ed");

    public static LocalisationSource Instance { get; } = new LocalisationSource();

    public string[] AvailableLanguages => new string[]
    {
                this["Preferences_Language_German"],
                this["Preferences_Language_English"]
    };

    public string Language { get; }
}
