using Autofac;
using SQLite.Service.Factory;
using SQLite.Service.Mapping;
using SQLite.Service.Repository;
using SQLite.Service.Service;

namespace SQLite.Service.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {

            foreach (var type in new[]
            {
                typeof(ColumnDataFactory),
                typeof(TreeViewMapper),
                typeof(TabsRepository),
                typeof(TabsFactory),
                typeof(ViewModelNameService),

                typeof(DatabaseService),
                typeof(ExitService),
                typeof(MainMenuService),
                typeof(MenuPanelService),
                typeof(SelectedDatabaseService),
                typeof(TableInformationsService),
                typeof(TableNameService),
                typeof(TableService),
                typeof(TabsService),
                typeof(ViewService),
            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            return containerBuilder.RegisterTableChart();
        }

        public static ContainerBuilder RegisterTableChart(this ContainerBuilder containerBuilder)
        {

            foreach (var type in new[]
            {
                typeof(ColumnSeriesService),
                typeof(ColumnSeriesPairService),
                typeof(ChartSeriesService),
                typeof(ColumnModelService),
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