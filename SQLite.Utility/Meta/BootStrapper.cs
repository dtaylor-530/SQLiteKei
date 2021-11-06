using Autofac;
using SQLite.Utility.Factory;

namespace SQLite.Utility.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {
            foreach (var type in new[]
     {
                typeof(Profile),

            })
            {
                containerBuilder.RegisterType(type).SingleInstance().As<AutoMapper.Profile>();
            }


            foreach (var type in new[]
            {

                typeof(HandlerService),

            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            return containerBuilder;
        }
    }
}
