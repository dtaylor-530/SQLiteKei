using Autofac;
using SQLite.Service.Factory;
using SQLite.Service.Service;
using SQLite.ViewModel.Infrastructure;
using SQLite.ViewModel.Infrastructure.Service;

namespace SQLite.ViewModel.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {
            foreach (var type in new[] {

                typeof(TabsFactory),
                typeof(TabsRepository),
                typeof(ViewService),
                typeof(ViewModelNameService),

                typeof(MainWindowViewModel),
                typeof(MainMenuViewModel),
                typeof(MenuPanelViewModel),
                typeof(TabPanelViewModel),
                typeof(TreeViewModel),
                typeof(AboutViewModel),
            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            foreach (var type in new[] {
                typeof(TableGeneralViewModel),
                typeof(TableRecordsViewModel),
                typeof(TableChartViewModel),
                typeof(DatabaseGeneralViewModel),
                typeof(TableCreatorViewModel),
                typeof(SelectQueryViewModel),
            })
            {
                containerBuilder.RegisterType(type).AsImplementedInterfaces().AsSelf();
            }

            return containerBuilder;
        }
    }
}
