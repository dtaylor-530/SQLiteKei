using Autofac;
using Splat;
using Splat.Autofac;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Utility.Common;
using Utility.Common.Base;
using Utility.WPF.Demo.Infrastructure;
using Utility.WPF.Service;
using static Utility.Common.Base.Log;

using four = Utility.WPF.Service.Meta.BootStrapper;
using one = Utility.Service.BootStrapper;
using three = Utility.WPF.Meta.BootStrapper;
using two = Utility.ViewModel.Meta.BootStrapper;

namespace Utility.WPF.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ThemeSource>().AsImplementedInterfaces();
            builder.RegisterType<MainMenuModel>().AsImplementedInterfaces();
            builder.RegisterType<MenuPanelService>().AsImplementedInterfaces();
            builder.RegisterType<TreeViewMapper>().AsImplementedInterfaces();
            builder.RegisterType<TabsModel>().AsImplementedInterfaces();
            Register(builder).UseAutofacDependencyResolver();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Fatal("An unhandled exception was thrown and a message is shown to the user.", e.Exception);
            new UnhandledExceptionWindow().ShowDialog();
            var dc = Locator.Current.GetService<IViewModelFactory>().Build(new MainWindowViewModelKey());
            e.Handled = true;
            Current.Shutdown();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SQLite.Data.Settings.Default.UILanguage);
            base.OnStartup(e);

            // registeration can't take place in the constructor
            three.RegisterViews();
            //Database.WPF.Meta.BootStrapper.RegisterViews();

            Information();

            Locator.Current.GetService<ThemeService>()?.ApplyCurrentUserTheme();

            var dc = Locator.Current.GetService<IViewModelFactory>().Build(new MainWindowViewModelKey());

            new MainWindow
            {
                DataContext = dc
            }.Show();
        }

        private static void Information()
        {
            string assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();

            Info("================= GENERAL INFO =================");
            Info("SQLite Kei " + assemblyVersion);
            Info("Running on " + Environment.OSVersion);
            Info("Application language is " + Thread.CurrentThread.CurrentUICulture);
            Info("================================================");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Info("============= APPLICATION SHUTDOWN =============");
        }

        private ContainerBuilder Register(ContainerBuilder builder)
        {
            //SQLite.WPF.Meta.BootStrapper.Register(builder);
            //BootStrapper.Register(builder);
            //SQLite.ViewModel.Meta.BootStrapper.Register(builder);
            //SQLite.Data.Meta.BootStrapper.Register(builder);
            //SQLite.Utility.Meta.BootStrapper.Register(builder);

            one.Register(builder);
            two.Register(builder);
            four.Register(builder);
            return builder;
        }
    }

    class ThemeSource : IThemeSource
    {
        public Theme Theme { get; set; }
        public Theme[] AvailableThemes { get; }
    }
}
