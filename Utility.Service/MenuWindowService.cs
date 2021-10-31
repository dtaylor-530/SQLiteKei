using SQLite.Common.Contracts;
using Utility.Common;
using Utility.Common.Base;
using Utility.Common.Contracts;

namespace Utility.Service
{
    public class MenuWindowService : IMenuWindowService
    {
        private readonly IViewModelFactory viewModelFactory;
        private readonly IWindowService windowService;
        private readonly ILocaliser localiser;

        public MenuWindowService(IViewModelFactory viewModelFactory, IWindowService windowService, ILocaliser localiser)
        {
            this.viewModelFactory = viewModelFactory;
            this.windowService = windowService;
            this.localiser = localiser;
        }

        public void OpenAbout()
        {
            var about = viewModelFactory.Build(new AboutViewModelKey());
            windowService.ShowWindow(new(localiser["WindowTitle_About"], about, ResizeMode.NoResize, Show.ShowDialog));

        }

        public void OpenPreferences()
        {
            var preferences = viewModelFactory.Build(new PreferencesViewModelKey());
            windowService.ShowWindow(new(localiser["WindowTitle_Preferences"], preferences, ResizeMode.NoResize, Show.ShowDialog));
        }

    }
}
