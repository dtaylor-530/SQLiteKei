using Autofac;
using Database.Data;

namespace SQLite.Data.Meta;

public static class BootStrapper
{
    public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<LocalisationSource>().SingleInstance().AsImplementedInterfaces().AsSelf();
        containerBuilder.RegisterType<ThemeSource>().SingleInstance().AsImplementedInterfaces().AsSelf();

        return containerBuilder;
    }

}
