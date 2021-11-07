using Autofac;
using SQLite.Service.Repository;
using SQLite.Service.Service;

namespace Database.Service.Chart.Meta;
public class BootStrapper
{

    public static ContainerBuilder Register(ContainerBuilder containerBuilder)
    {

        foreach (var type in new[]
        {
                typeof(ColumnSeriesModel),
                typeof(ColumnSeriesPairModel),
                typeof(ChartSeriesService),
                typeof(ColumnModel),
                typeof(SeriesRepository),
                typeof(SeriesPairRepository),
                typeof(ColumnDataFactory),
            })
        {
            containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
        }

        return containerBuilder;
    }
}
