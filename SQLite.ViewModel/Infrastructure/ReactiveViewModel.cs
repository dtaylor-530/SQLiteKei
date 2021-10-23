using ReactiveUI;
using SQLite.Common;
using Utility;
using Utility.Database;

namespace SQLite.ViewModel.Infrastructure;

public class ReactiveViewModel : ReactiveObject, IViewModel
{
    public ReactiveViewModel(Key key, string name)
    {
        Key = key;
        Name = name;
    }

    public virtual Key Key { get; }
    public virtual string Name { get; }
}

public class DatabaseViewModel : ReactiveViewModel, IDatabaseViewModel
{
    public DatabaseViewModel(DatabaseKey key, string name) : base(key, name)
    {
        Key = key;
    }

    public override DatabaseKey Key { get; }
}
