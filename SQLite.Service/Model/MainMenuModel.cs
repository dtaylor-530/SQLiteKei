using ReactiveUI;
using Utility;
using Utility.Common.Base;
using Utility.Common.Contracts;

namespace SQLite.Service.Service
{
    public class MainMenuModel : IMainMenuModel
    {

        private readonly ILocaliser localiser;
        private readonly DatabaseService databaseService;

        private readonly IMenuWindowService menuWindowService;

        public MainMenuModel(
            ILocaliser localiser,
            DatabaseService databaseService,
            IMenuWindowService menuWindowService)
        {

            this.localiser = localiser;
            this.databaseService = databaseService;
            this.menuWindowService = menuWindowService;
        }

        public IReadOnlyCollection<MenuItem> Collection => new[]
          {
                new MenuItem(localiser["MenuItemHeader_File"])
        {
            Collection = new MenuObject[]{
                        new MenuItem(localiser["MenuItemHeader_NewDatabase"]){ Command = ReactiveCommand.Create(databaseService.CreateNewDatabase) },
                        new MenuItem(localiser["MenuItemHeader_OpenDatabase"]){ Command = ReactiveCommand.Create(databaseService.OpenDatabase) },
                        new Seperator(),
                        new MenuItem(localiser["MenuItemHeader_Exit"]){ Command = ReactiveCommand.Create(MenuItem_Exit_Click) },
                    }
                },
                new MenuItem(localiser["MenuItemHeader_Tools"])
        {
            Collection = new[]{
                        new MenuItem(localiser["MenuItemHeader_QueryEditor"]),
                        new MenuItem(localiser["MenuItemHeader_Preferences"]){ Command = ReactiveCommand.Create(menuWindowService.OpenPreferences) },
                    }
                },
                new MenuItem(localiser["MenuItemHeader_Help"])
        {
            Collection = new[]{
                        new MenuItem(localiser["MenuItemHeader_Documentation"]),
                        new MenuItem(localiser["MenuItemHeader_About"]){ Command = ReactiveCommand.Create(menuWindowService.OpenAbout) },

                    }
                },

            };

        private void MenuItem_Exit_Click()
        {

            throw new NotImplementedException();
            //TODO replace with service
            // System.Windows.Application.Current.Shutdown();
        }

    }
}
