using Autofac;
using SQLite.ViewModel;

namespace Utility.ViewModel.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {
            foreach (var type in new[] {

                typeof(AboutViewModel),
                typeof(MainMenuViewModel),
                typeof(MainWindowViewModel),
                typeof(MenuPanelViewModel),
                typeof(PreferencesViewModel),
                typeof(StatusViewModel),
                typeof(TabPanelViewModel),
                typeof(TreeViewModel),
                typeof(UnhandledExceptionViewModel),
            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            return containerBuilder;
        }
    }
}
