using ReactiveUI;
using Utility.Common;
using Utility.Common.Contracts;
using Utility.ViewModel.Base;

namespace Utility.ViewModel
{

    public class StatusViewModel : BaseViewModel<IStatusViewModel>, IStatusViewModel
    {
        private readonly IStatusModel statusModel;

        public StatusViewModel(StatusViewModelKey key, IStatusModel statusModel) : base(key)
        {
            statusModel
                .Subscribe(a =>
            {
                this.RaisePropertyChanged(nameof(StatusInfo));
            });
            this.statusModel = statusModel;
        }

        public string StatusInfo => statusModel.Value;

        public override string Name { get; }
    }
}
