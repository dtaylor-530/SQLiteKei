using Utility.Common.Contracts;

namespace Utility.Service
{
    public class VersionModel : IVersionModel
    {
        public Version Version => VersionHelper.GetVersion();

        class VersionHelper
        {
            public static Version GetVersion()
            {
                return System.Reflection.Assembly.GetExecutingAssembly()
                                               .GetName()
                                               .Version;
            }
        }
    }

}
