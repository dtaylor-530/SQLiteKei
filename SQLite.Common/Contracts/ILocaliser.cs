namespace SQLite.Common.Contracts;

public interface ILocaliser
{
    string this[string key] { get; }

    string this[string key, params object[] args] { get; }

    static ILocaliser Instance { get; } = new DefaultLocaliser();
    string[] AvailableLanguages { get; }
    string Language { get; }
}


public class DefaultLocaliser : ILocaliser
{
    public string this[string key] => throw new Exception("╚═(°ʖ°)═╝");

    public string this[string key, params object[] args] => throw new Exception("(°ʖ°)");

    public string[] AvailableLanguages => throw new Exception("(°ʖ°)");
    public string Language => throw new Exception("(°ʖ°)");

    public static DefaultLocaliser Instance { get; } = new DefaultLocaliser();

}
