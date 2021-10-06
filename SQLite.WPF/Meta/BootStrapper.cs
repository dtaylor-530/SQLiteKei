using Autofac;
using SQLiteKei.ViewModels.MainWindow;

namespace SQLite.WPF.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MainWindowViewModel>().SingleInstance();
            return containerBuilder;
        }

    }
}
