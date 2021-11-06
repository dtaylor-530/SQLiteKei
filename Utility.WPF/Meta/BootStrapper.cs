using Database.WPF.Infrastructure.IKriv.Windows.Mvvm;
using Database.WPF.Views.UserControls.MainWindow;
using SQLite.ViewModel;
using Utility.ViewModel;
using Utility.WPF.UserControls.MainWindow;

namespace Utility.WPF.Meta
{
    public static class BootStrapper
    {
        public static void RegisterViews()
        {

            foreach (var (a, b) in new[]
            {
                (typeof(MainMenuViewModel),typeof(MainMenu)),
                (typeof(MenuPanelViewModel),typeof(MenuPanel)),
                (typeof(StatusViewModel),typeof(StatusView)),
                (typeof(TabPanelViewModel),typeof(TabPanelViewModel)),
                (typeof(TreeViewModel),typeof(TreeView)),
                (typeof(AboutViewModel),typeof(About)),
                (typeof(PreferencesViewModel),typeof(Preferences)),
                (typeof(UnhandledExceptionViewModel),typeof(UnhandledExceptionWindow)),
                (typeof(MainWindowViewModel),typeof(MainWindow)),

            })
            {
                DataTemplateManager.RegisterDataTemplate(a, b);
            }
        }
    }
}
