using Utility.Common;
using Utility.Common.Contracts;
using Utility.ViewModel.Base;

namespace Utility.ViewModel
{

    public class MainMenuViewModel : BaseViewModel<IMainMenuViewModel>, IMainMenuViewModel
    {

        private readonly IMainMenuService mainMenuService;

        public MainMenuViewModel(MainMenuViewModelKey key, IMainMenuService mainMenuService) : base(key)
        {
            this.mainMenuService = mainMenuService;
        }

        public IReadOnlyCollection<MenuItem> Collection => mainMenuService.Collection;

        public override string Name => nameof(MainMenuViewModel);
    }

    //public class MenuViewModel
    //{
    //    public MenuViewModel()
    //    {
    //    }

    //    public IReadOnlyCollection<MenuItem> Collection { get; init; }

    //}

}