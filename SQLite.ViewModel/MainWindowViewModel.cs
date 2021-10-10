using ReactiveUI;
using SQLite.ViewModel.Infrastructure.Service;

namespace SQLite.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly TreeService treeService;
        private string statusBarInfo;

        public MainWindowViewModel(
            StatusService statusService,
            TreeService treeService,
            MainMenuViewModel mainMenuViewModel,
            MenuPanelViewModel menuPanelViewModel,
            TabPanelViewModel tabPanelViewModel,
            TreeViewModel treeViewModel)
        {
            this.treeService = treeService;
            MainMenuViewModel = mainMenuViewModel;
            MenuPanelViewModel = menuPanelViewModel;
            TabPanelViewModel = tabPanelViewModel;
            TreeViewModel = treeViewModel;

            statusService.Subscribe(a =>
            {
                StatusInfo = a;
            });
        }

        public MainMenuViewModel MainMenuViewModel { get; }
        public MenuPanelViewModel MenuPanelViewModel { get; }
        public TabPanelViewModel TabPanelViewModel { get; }
        public TreeViewModel TreeViewModel { get; }

        public string StatusInfo
        {
            get { return statusBarInfo; }
            set { this.RaiseAndSetIfChanged(ref statusBarInfo, value); }
        }

        public void Close()
        {
            treeService.SaveTree();
        }

    }
}
