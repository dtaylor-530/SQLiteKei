using System.Text.Json.Serialization;

namespace Utility.Database;

public record FilePath(string Path)
{
    public static implicit operator string(FilePath fileName)
    {
        return fileName.Path;
    }

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string BaseName => System.IO.Path.GetFileNameWithoutExtension(Path);

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string? Parent => new FileInfo(Path).DirectoryName;

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string Name => new FileInfo(Path).Name;

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public FileInfo AsFileInfo => new(Path);

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string Size => GetSize(AsFileInfo.Length);

    private static string GetSize(long value)
    {
        string[] SizeSuffixes = { "Bytes", "KB", "MB", "GB", "TB" };
        int suffix = 0;
        decimal decimalValue = value;

        while (Math.Round(decimalValue / 1024) >= 1)
        {
            decimalValue /= 1024;
            suffix++;
        }

        return string.Format("{0} {1}", Math.Round(decimalValue, 2), SizeSuffixes[suffix]);
    }
}

public record DatabasePath(string Path) : FilePath(Path)
{
    public static implicit operator string(DatabasePath path)
    {
        return path.Path;
    }

}

public record TableName(string Name)
{
    public static implicit operator string(TableName fileName)
    {
        return fileName.Name;
    }
}

public class TableTabKey : TableKey
{
    public TableTabKey(DatabasePath databasePath, TableName tableName) : base(databasePath, tableName)
    {

    }

    //public string TypeName { get; }

    //public static TableTabKey From(TableKey key, string typeName)
    //{
    //    return new TableTabKey(key.DatabaseName, key.TableName);
    //}
}

public class DatabaseTabKey : DatabaseKey
{
    public DatabaseTabKey(DatabasePath databaseName) : base(databaseName)
    {

    }

}

public class TableKey : DatabaseKey
{

    public TableKey(DatabasePath databaseName, TableName tableName) : base(databaseName)
    {

        TableName = tableName;
    }

    public TableName TableName { get; }

    public override bool Equals(Key? other)
    {
        if (other is TableKey dKey)
            return dKey.TableName.Equals(TableName) && base.Equals(other);
        else if (other == null)
            return false;
        throw new Exception("Key types dont match");
    }
}

public class DatabaseKey : Key
{
    public DatabaseKey(DatabasePath databasePath)
    {
        DatabasePath = databasePath;
    }

    public DatabasePath DatabasePath { get; }

    public override bool Equals(Key? other)
    {
        if (other is DatabaseKey dKey)
            return dKey.DatabasePath.Equals(DatabasePath) && base.Equals(other);
        throw new Exception("Key types dont match");
    }

    public override int GetHashCode()
    {
        return DatabasePath.GetHashCode();
    }
}
