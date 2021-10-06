using log4net;

using System.Runtime.CompilerServices;

namespace SQLiteKei.Helpers
{
    public static class Log
    {
        public static ILog Logger { get; } = LogManager.GetLogger("MyLogger");

    }
}
