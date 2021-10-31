using Utility.Common;
using Utility.Common.Base;
using Utility.ViewModel.Base;

namespace SQLite.ViewModel
{

    public class UnhandledExceptionViewModel : BaseViewModel<IUnhandledExceptionViewModel>, IUnhandledExceptionViewModel
    {
        private readonly ILocaliser localiser;

        public UnhandledExceptionViewModel(UnhandledExceptionViewModelKey key, ILocaliser localiser) : base(key)
        {
            this.localiser = localiser;
        }

        public string Title => localiser["UnhandledException_Title"];

        public string UnhandledExceptionKey => localiser["UnhandledException"];

        public override string Name => Title;
    }
}
