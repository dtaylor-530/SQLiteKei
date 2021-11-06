namespace Utility.Database;

public record DatabasePath(string Path) : FilePath(Path)
{
    public static implicit operator string(DatabasePath path)
    {
        return path.Path;
    }
}
