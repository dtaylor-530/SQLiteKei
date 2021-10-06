using Autofac;
using SQLiteKei.Helpers;
using SQLiteKei.Views;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using log = SQLiteKei.Helpers.Log;
using Splat.Autofac;

namespace SQLite.WPF.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 
        public App()
        {
            var builder = new ContainerBuilder();
            Register(builder);
            builder.UseAutofacDependencyResolver();
            new MainWindow().Show();
        }


        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            log.Logger.Fatal("An unhandled exception was thrown and a message is shown to the user.", e.Exception);

            new UnhandledExceptionWindow().ShowDialog();
            e.Handled = true;

            Current.Shutdown();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Settings.Default.UILanguage);
            base.OnStartup(e);

            string assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();

            log.Logger.Info("================= GENERAL INFO =================");
            log.Logger.Info("SQLite Kei " + assemblyVersion);
            log.Logger.Info("Running on " + Environment.OSVersion);
            log.Logger.Info("Application language is " + Thread.CurrentThread.CurrentUICulture);
            log.Logger.Info("================================================");

            ThemeHelper.LoadCurrentUserTheme();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            log.Logger.Info("============= APPLICATION SHUTDOWN =============");
        }


        private void Register(ContainerBuilder builder)
        {
            SQLite.WPF.Meta.BootStrapper.Register(builder);
        }
    }
}
