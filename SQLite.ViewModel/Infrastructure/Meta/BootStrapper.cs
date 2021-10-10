using Autofac;
using SQLite.ViewModel.Infrastructure.Common;
using SQLite.ViewModel.Infrastructure.Factory;
using SQLite.ViewModel.Infrastructure.Service;

namespace SQLite.ViewModel.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MainWindowViewModel>().SingleInstance().AsImplementedInterfaces();

            foreach (var type in new[] {
                typeof(MainWindowViewModel),
                typeof(MainMenuViewModel),
                typeof(MenuPanelViewModel),
                typeof(TabPanelViewModel),
                typeof(TreeViewModel),
                typeof(AboutViewModel),

                typeof(TabsFactory),
                typeof(ViewModelFactory),

                typeof(TreeRepository),

                typeof(DatabaseService),
                typeof(TableService),
                typeof(StatusService),
                typeof(TabsService),
                typeof(TreeService),
                typeof(ViewService),
            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            foreach (var type in new[] {
                typeof(TableGeneralViewModel),
                typeof(TableRecordsViewModel),
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

//containerBuilder.RegisterType<TableGeneralViewModel>().AsImplementedInterfaces().AsSelf();
//containerBuilder.RegisterType<TableRecordsViewModel>().AsImplementedInterfaces().AsSelf();
//containerBuilder.RegisterType<DatabaseGeneralViewModel>().AsImplementedInterfaces().AsSelf();
//containerBuilder.RegisterType<TableCreatorViewModel>().AsImplementedInterfaces().AsSelf();
