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
                .RegisterTableChart()
                .RegisterProfiles();
        }

        static ContainerBuilder RegisterServices(this ContainerBuilder containerBuilder)
        {
            foreach (var type in new[]
           {
                typeof(ColumnDataFactory),
                typeof(TreeViewMapper),
                typeof(TabsRepository),
                typeof(TabsFactory),
                typeof(ViewModelNameService),
                typeof(MenuWindowService),
                typeof(DatabaseService),
                typeof(ExitService),
                typeof(MainMenuModel),
                typeof(MenuPanelModel),
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

        static ContainerBuilder RegisterTableChart(this ContainerBuilder containerBuilder)
        {

            foreach (var type in new[]
            {
                typeof(ColumnSeriesModel),
                typeof(ColumnSeriesPairModel),
                typeof(ChartSeriesService),
                typeof(ColumnModelModel),
                typeof(SeriesRepository),
                typeof(SeriesPairRepository),
                typeof(ColumnDataFactory),
            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            //containerBuilder.RegisterType<SeriesPairChangesService>().SingleInstance().AsImplementedInterfaces().AsSelf().AutoActivate();

            return containerBuilder;

        }
    }
}