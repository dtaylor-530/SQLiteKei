using System.ComponentModel;
using Utility;

namespace SQLite.Common;

//public interface IKey : IEquatable<IKey>
//{
//}

public interface IKey : IName
{
    public Key Key { get; }
}
public interface IName
{
    string Name { get; }
}

public interface IIsSelected
{
    public bool IsSelected { get; set; }
}
public interface IIsLoaded
{
    public bool IsLoaded { get; set; }
}

public interface IViewModel : IKey, IName, IIsSelected, INotifyPropertyChanged, IIsLoaded
{

}
