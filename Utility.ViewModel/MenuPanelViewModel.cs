using Utility.Common;
using Utility.Common.Contracts;
using Utility.ViewModel.Base;

namespace Utility.ViewModel
{

    public class MenuPanelViewModel : BaseViewModel<IMenuPanelViewModel>, IMenuPanelViewModel
    {
        private readonly IMenuPanelService menuPanelService;

        public MenuPanelViewModel(MenuPanelViewModelKey key, IMenuPanelService menuPanelService) : base(key)
        {
            this.menuPanelService = menuPanelService;
        }

        public IReadOnlyCollection<PanelObject> Collection => menuPanelService.Collection;

        public override string Name { get; }
    }
}
