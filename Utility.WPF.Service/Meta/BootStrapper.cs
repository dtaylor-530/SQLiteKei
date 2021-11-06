using Autofac;
using Database.WPF.Infrastructure;

namespace Utility.WPF.Service.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {

            foreach (var type in new[]
            {
                typeof(FileDialogService),
                typeof(ListCollectionService),
                typeof(MessageBoxService),
                typeof(ThemeService),
                typeof(WindowService),

            })
            {
                containerBuilder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
            }

            return containerBuilder;
        }
    }
}
