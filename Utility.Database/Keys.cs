using Utility.Common.Base;

namespace Utility.Database;

//public interface IDatabaseKey
//{
//    DatabaseKey Key { get; }
//}

public interface IDatabaseViewModel : IViewModel
{
}

public interface ITableName { TableName TableName { get; } }

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

public class TableTabKey<T> : TableKey<T> where T : IDatabaseViewModel
{
    public TableTabKey(DatabasePath databasePath, TableName tableName) : base(databasePath, tableName)
    {
    }
}

public class DatabaseTabKey<T> : DatabaseKey<T> where T : IDatabaseViewModel
{
    public DatabaseTabKey(DatabasePath databaseName) : base(databaseName)
    {
    }
}

public interface ITableKey : ITableName, IDatabaseKey { }

public class TableKey : DatabaseKey, ITableKey
{
    public TableKey(DatabasePath databasePath, TableName tableName, Type type) : base(databasePath, type)
    {
        TableName = tableName;
    }

    public TableName TableName { get; }

    public override bool Equals(IKey? other)
    {
        if (other is TableKey dKey)
            return dKey.TableName.Equals(TableName) && base.Equals(other);
        else if (other == null)
            return false;
        throw new Exception("Key types dont match");
    }
}

public class TableKey<T> : TableKey, ITableKey, IDatabaseKey<T> where T : IDatabaseViewModel
{
    public TableKey(DatabasePath databaseName, TableName tableName) : base(databaseName, tableName, typeof(T))
    {
    }

    public static implicit operator Key<T>(TableKey<T> dkey)
    {
        return new Key<T>();
    }
}

public interface IDatabasePath
{
    public DatabasePath DatabasePath { get; }
}

public interface IDatabaseKey : IKey, IDatabasePath
{

}
public interface IDatabaseKey<T> : IKey<T>, IDatabasePath
{

}

public class DatabaseKey : Key, IDatabaseKey
{
    public DatabaseKey(DatabasePath databasePath, Type type) : base(type)
    {
        DatabasePath = databasePath;
    }

    public DatabasePath DatabasePath { get; }

    public override bool Equals(IKey? other)
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

public class DatabaseKey<T> : DatabaseKey, IKey<T>
{
    public DatabaseKey(DatabasePath databasePath) : base(databasePath, typeof(T))
    {
    }

    public static implicit operator Key<T>(DatabaseKey<T> dkey)
    {
        return new Key<T>();
    }
}

public abstract class TableTabViewModelKey<T> : DatabaseTabViewModelKey<T>, ITableKey where T : IDatabaseViewModel
{
    public TableTabViewModelKey(DatabasePath path, TableName tableName) : base(path)
    {
        TableName = tableName;
    }

    public TableName TableName { get; }
}

public abstract class DatabaseTabViewModelKey<T> : DatabaseKey<T> where T : IDatabaseViewModel
{
    public DatabaseTabViewModelKey(DatabasePath path) : base(path) { }
}
