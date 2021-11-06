using System.ComponentModel;
using Utility.Entity;

namespace Utility.Common.Base;

//public interface IKey : IEquatable<IKey>
//{
//}

public interface IKeyGet : IName
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

public interface IViewModel : IKeyGet, IName, INotifyPropertyChanged, IIsLoaded
{

}
