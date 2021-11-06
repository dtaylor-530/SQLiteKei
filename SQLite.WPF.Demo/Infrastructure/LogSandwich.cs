using System;
using System.Threading;
using static Utility.Common.Base.Log;

namespace SQLite.WPF.Demo.Infrastructure
{
    class LogSandwich
    {

        public static void Start()
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

        public static void End()
        {
            Info("============= APPLICATION SHUTDOWN =============");
        }
    }
}
