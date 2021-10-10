using Autofac;
using SQLite.ViewModel;
using SQLite.Views;
using SQLite.Views.UserControls;
using SQLite.WPF.Infrastructure;
using SQLite.WPF.Infrastructure.IKriv.Windows.Mvvm;

namespace SQLite.WPF.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {

            containerBuilder.RegisterType<FileDialogService>().SingleInstance().AsImplementedInterfaces().AsSelf();
            containerBuilder.RegisterType<MessageBoxService>().SingleInstance().AsImplementedInterfaces().AsSelf();
            containerBuilder.RegisterType<ListCollectionService>().SingleInstance().AsImplementedInterfaces().AsSelf();
            containerBuilder.RegisterType<ThemeService>().SingleInstance().AsImplementedInterfaces().AsSelf();
            containerBuilder.RegisterType<WindowService>().SingleInstance().AsImplementedInterfaces().AsSelf();


            return containerBuilder;
        }

        public static void RegisterViews()
        {

            var manager = new DataTemplateManager();

            manager.RegisterDataTemplate<TableGeneralViewModel, TableGeneralTabContent>();
            manager.RegisterDataTemplate<TableRecordsViewModel, TableRecordsTabContent>();
            manager.RegisterDataTemplate<DatabaseGeneralViewModel, DatabaseGeneralTabContent>();
            manager.RegisterDataTemplate<AboutViewModel, About>();
            manager.RegisterDataTemplate<SelectQueryViewModel, SelectQueryUserControl>();
        }

    }
}
