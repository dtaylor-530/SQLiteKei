using Autofac;
using Splat;
using Splat.Autofac;
using SQLite.Views;
using SQLite.WPF.Infrastructure;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Utility.Common;
using Utility.Common.Base;
using static Utility.Common.Base.Log;

namespace SQLite.WPF.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Register(new ContainerBuilder()).UseAutofacDependencyResolver();
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
            Utility.WPF.Meta.BootStrapper.RegisterViews();
            SQLite.WPF.Meta.BootStrapper.RegisterViews();

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
            SQLite.Service.Meta.BootStrapper.Register(builder);
            SQLite.ViewModel.Meta.BootStrapper.Register(builder);
            SQLite.Data.Meta.BootStrapper.Register(builder);

            Utility.Service.BootStrapper.Register(builder);
            Utility.ViewModel.Meta.BootStrapper.Register(builder);
            Utility.WPF.Service.Meta.BootStrapper.Register(builder);
            return builder;
        }
    }
}
