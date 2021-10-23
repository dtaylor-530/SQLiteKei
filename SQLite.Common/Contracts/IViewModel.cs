using Utility;

namespace SQLite.Common;

//public interface IKey : IEquatable<IKey>
//{
//}

public interface IViewModel : IName
{
    Key Key { get; }


}


public interface IName
{
    string Name { get; }

}