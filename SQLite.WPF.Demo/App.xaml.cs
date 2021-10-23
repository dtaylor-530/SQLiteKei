using Autofac;
using Splat;
using Splat.Autofac;
using SQLite.Service.Meta;
using SQLite.ViewModel;
using SQLite.Views;
using SQLite.WPF.Infrastructure;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using static SQLite.Common.Log;

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
            e.Handled = true;
            Current.Shutdown();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SQLite.Data.Settings.Default.UILanguage);
            base.OnStartup(e);

            // registeration can't take place in the constructor
            WPF.Meta.BootStrapper.RegisterViews();

            Information();

            Locator.Current.GetService<ThemeService>()?.ApplyCurrentUserTheme();

            var dc = Locator.Current.GetService<MainWindowViewModel>();

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
            SQLite.WPF.Meta.BootStrapper.Register(builder);
            SQLite.Service.Meta.BootStrapper.Register(builder);
            SQLite.ViewModel.Meta.BootStrapper.Register(builder);
            BootStrapper.Register(builder);
            SQLite.Data.Meta.BootStrapper.Register(builder);
            return builder;
        }
    }
}
