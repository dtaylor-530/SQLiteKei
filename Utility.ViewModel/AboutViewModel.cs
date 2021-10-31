using ReactiveUI;
using System.Diagnostics;
using System.Windows.Input;
using Utility.Common;
using Utility.Common.Base;
using Utility.Common.Contracts;
using Utility.ViewModel.Base;

namespace Utility.ViewModel
{

    public class AboutViewModel : BaseViewModel, IAboutViewModel
    {
        private readonly ILocaliser localiser;
        private readonly IVersionService versionService;

        public AboutViewModel(AboutViewModelKey key, ILocaliser localiser, IVersionService versionService) : base(key)
        {
            this.localiser = localiser;
            this.versionService = versionService;
        }

        public string GithubName => localiser["About_CheckOutOnGithub"];

        public string Version => string.Format("Version {0}", versionService.Version);

        public string Title => string.Empty;

        public ICommand NavigateCommand { get; } = ReactiveCommand.Create<Uri, Uri>(a => CheckoutOnGithub(a));
        public override string Name => nameof(AboutViewModel);

        static Uri CheckoutOnGithub(Uri e)
        {
            Process.Start(new ProcessStartInfo(e.AbsoluteUri));
            return e;
        }
    }
}
