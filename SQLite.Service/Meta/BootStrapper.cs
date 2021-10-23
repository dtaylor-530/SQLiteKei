using Autofac;
using SQLite.Service.Factory;
using SQLite.Service.Mapping;
using SQLite.Service.Repository;
using SQLite.Service.Service;
using SQLite.ViewModel.Infrastructure.Factory;

namespace SQLite.Service.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {

            foreach (var type in new[] {

                typeof(ViewModelFactory),

                typeof(TreeRepository),

                typeof(DatabaseService),
                typeof(ExitService),
                typeof(TableService),
                typeof(StatusService),
                typeof(TabsService),
                typeof(TreeService),
                typeof(TableInformationsService),

                typeof(TreeViewMapper),
            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            return containerBuilder.RegisterTableChart();
        }

        public static ContainerBuilder RegisterTableChart(this ContainerBuilder containerBuilder)
        {

            foreach (var type in new[] {
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

//containerBuilder.RegisterType<TableGeneralViewModel>().AsImplementedInterfaces().AsSelf();
//containerBuilder.RegisterType<TableRecordsViewModel>().AsImplementedInterfaces().AsSelf();
//containerBuilder.RegisterType<DatabaseGeneralViewModel>().AsImplementedInterfaces().AsSelf();
//containerBuilder.RegisterType<TableCreatorViewModel>().AsImplementedInterfaces().AsSelf();
