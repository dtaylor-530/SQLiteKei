using Utility.Entity;

namespace Utility.Database.Common;

public interface IDatabaseKey : IKey, IDatabasePath
{

}
public interface IDatabaseKey<T> : IKey<T>, IDatabasePath
{

}

public interface ITableKey : ITableName, IDatabaseKey { }
