using Autofac;
using Utility.ViewModel;

namespace Utility.Service
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {

            foreach (var type in new[]
            {
                typeof(IsSelectedRepository),
                typeof(IsSelectedModel),

                typeof(StatusModel),
                typeof(TreeRepository),
                typeof(TreeModel),
                typeof(ViewModelFactory),
                typeof(VersionModel),
                typeof(WebNavigationService),

                typeof(MapService),

            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            return containerBuilder;
        }
    }
}
