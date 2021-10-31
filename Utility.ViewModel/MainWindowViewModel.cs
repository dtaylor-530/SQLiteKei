using Utility.Common;
using Utility.Common.Base;
using Utility.ViewModel.Base;

namespace Utility.ViewModel
{

    public class MainWindowViewModel : BaseViewModel<IMainWindowViewModel>, IMainWindowViewModel
    {
        private readonly IViewModelFactory viewModelFactory;

        public MainWindowViewModel(MainWindowViewModelKey key, IViewModelFactory viewModelFactory) : base(key)
        {
            this.viewModelFactory = viewModelFactory;
        }

        public IViewModel StatusViewModel => viewModelFactory.Build(new StatusViewModelKey());
        public IViewModel MainMenuViewModel => viewModelFactory.Build(new MainMenuViewModelKey());
        public IViewModel MenuPanelViewModel => viewModelFactory.Build(new MenuPanelViewModelKey());
        public IViewModel TabPanelViewModel => viewModelFactory.Build(new TabPanelViewModelKey());
        public IViewModel TreeViewModel => viewModelFactory.Build(new TreeViewModelKey());

        public override string? Name { get; }
    }
}
