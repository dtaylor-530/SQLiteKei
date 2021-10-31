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
                typeof(IsSelectedService),
                typeof(MenuWindowService),
                typeof(StatusService),
                typeof(TreeRepository),
                typeof(TreeService),
                typeof(ViewModelFactory),
                typeof(VersionService),
                typeof(WebNavigationService),

            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            return containerBuilder;
        }
    }
}
