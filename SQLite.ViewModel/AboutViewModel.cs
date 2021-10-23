using ReactiveUI;
using SQLite.Common.Contracts;
using System.Diagnostics;
using System.Windows.Input;
using Utility;

namespace SQLite.ViewModel
{

    public class AboutViewModelKey : Key
    {
        public override bool Equals(Key? other)
        {
            return other is AboutViewModelKey;
        }
    }

    public class AboutViewModel
    {
        private readonly ILocaliser localiser;

        public AboutViewModel(ILocaliser localiser)
        {
            this.localiser = localiser;
        }

        public string GithubName => localiser["About_CheckOutOnGithub"];

        public string Version => VersionString();

        public string Title => string.Empty;

        public ICommand NavigateCommand { get; } = ReactiveCommand.Create<Uri, Uri>(a => CheckoutOnGithub(a));

        static string VersionString()
        {
            string assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();

            return string.Format("Version {0}", assemblyVersion);
        }

        static Uri CheckoutOnGithub(Uri e)
        {
            Process.Start(new ProcessStartInfo(e.AbsoluteUri));
            return e;
        }
    }
}
