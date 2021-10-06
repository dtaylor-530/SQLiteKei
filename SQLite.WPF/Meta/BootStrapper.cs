using Autofac;
using SQLiteKei.ViewModels.MainWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
