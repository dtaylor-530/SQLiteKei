using ReactiveUI;
using Utility.Common;
using Utility.Common.Contracts;
using Utility.ViewModel.Base;

namespace Utility.ViewModel
{

    public class StatusViewModel : BaseViewModel<IStatusViewModel>, IStatusViewModel
    {
        private string statusBarInfo;

        public StatusViewModel(StatusViewModelKey key, IStatusService statusService) : base(key)
        {
            statusService.Subscribe(a =>
            {
                StatusInfo = a;
            });
        }

        public string StatusInfo
        {
            get { return statusBarInfo; }
            set { this.RaiseAndSetIfChanged(ref statusBarInfo, value); }
        }

        public override string Name { get; }
    }
}
