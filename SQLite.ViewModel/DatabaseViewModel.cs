using Utility.Database;
using Utility.Service;
using Utility.ViewModel.Base;

namespace SQLite.ViewModel
{

    public abstract class DatabaseViewModel : BaseViewModel //, IDatabaseKey
    {
        public DatabaseViewModel(DatabaseKey key) : base(key)
        {
            Key = key;
        }

        public override DatabaseKey Key { get; }
    }

    public abstract class DatabaseViewModel<T> : BaseViewModel<T> //, IDatabaseKey
    {
        public DatabaseViewModel(DatabaseKey<T> key) : base(key)
        {
        }
    }

    public abstract class TableViewModel<T> : BaseViewModel<T> where T : IDatabaseViewModel //, IDatabaseKey
    {
        public TableViewModel(TableKey<T> key) : base(key)
        {
        }
    }

    public abstract class DatabaseTabViewModel<T> : SelectableViewModel, IDatabaseViewModel where T : IDatabaseViewModel
    {
        public DatabaseTabViewModel(DatabaseTabViewModelKey<T> key, IIsSelectedService service) : base(key, service)
        {
            Key = key;
        }

        public override DatabaseKey<T> Key { get; }
    }
}
