using Autofac;
using Database.Service.Model;
using Database.Service.Service;
using SQLite.Service;
using SQLite.Service.Factory;
using SQLite.Service.Mapping;
using SQLite.Service.Repository;
using SQLite.Service.Service;
using Utility.Service;

namespace Database.Service.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {
            return containerBuilder
                .RegisterServices()
                .RegisterProfiles();
        }

        static ContainerBuilder RegisterServices(this ContainerBuilder containerBuilder)
        {
            foreach (var type in new[]
           {

                typeof(TreeViewMapper),
                typeof(TabsRepository),
                typeof(TabsFactory),
                typeof(ViewModelNameService),
                typeof(MenuWindowService),
                typeof(DatabaseService),
                typeof(MainMenuModel),
                typeof(MainToolBarModel),
                typeof(SelectedDatabaseService),
                typeof(TableInformationsService),
                typeof(SelectQueryService),
                typeof(TableService),
                typeof(TabsModel),
                typeof(TableGeneralModel),
                typeof(ViewService),
            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }
            return containerBuilder;
        }

        static ContainerBuilder RegisterProfiles(this ContainerBuilder containerBuilder)
        {
            foreach (var type in new[]
         {
                typeof(Profile),

            })
            {
                containerBuilder.RegisterType(type).SingleInstance().As<AutoMapper.Profile>();
            }

            return containerBuilder;
        }

    }
}