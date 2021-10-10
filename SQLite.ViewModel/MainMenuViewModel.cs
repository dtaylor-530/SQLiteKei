using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Factory;
using SQLite.ViewModel.Infrastructure.Service;
using System.Diagnostics;
using System.Windows.Input;

namespace SQLite.ViewModel
{

    public class MainMenuViewModel : MenuViewModel
    {

        private readonly ILocaliser localiser;
        private readonly TreeService treeService;
        private readonly ViewModelFactory viewModelFactory;
        private readonly IWindowService windowService;

        public MainMenuViewModel(
            ILocaliser localiser,
            TreeService treeService,
            ViewModelFactory viewModelFactory,
            IWindowService windowService,
            DatabaseService databaseService)
        {
            Collection = new[]
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
                    Collection = new []{
                        new MenuItem(localiser["MenuItemHeader_QueryEditor"]),
                        new MenuItem(localiser["MenuItemHeader_Preferences"]){ Command = ReactiveCommand.Create(OpenPreferences) },
                    }
                },
                new MenuItem(localiser["MenuItemHeader_Help"])
                {
                    Collection = new []{
                        new MenuItem(localiser["MenuItemHeader_Documentation"]),
                        new MenuItem(localiser["MenuItemHeader_About"]){ Command = ReactiveCommand.Create(OpenAbout) },

                    }
                },

            };
            this.localiser = localiser;
            this.treeService = treeService;
            this.viewModelFactory = viewModelFactory;
            this.windowService = windowService;
        }


        private void OpenAbout()
        {
            var about = viewModelFactory.Build<AboutViewModel>();
            windowService.ShowWindow(new(localiser["WindowTitle_About"], about, ResizeMode.NoResize, Show.ShowDialog));

        }

        private void OpenPreferences()
        {
            var preferences = viewModelFactory.Build<PreferencesViewModel>();
            windowService.ShowWindow(new(localiser["WindowTitle_Preferences"], preferences, ResizeMode.NoResize, Show.ShowDialog));
        }



        private void MenuItem_Exit_Click()
        {

            throw new NotImplementedException();
            //TODO replace with service
            // System.Windows.Application.Current.Shutdown();
        }

        //private void OpenDocumentation()
        //{
        //    string path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Documentation.pdf");
        //    try
        //    {
        //        Process.Start(path);
        //    }
        //    catch (Exception ex)
        //    {
        //        Error("Failed to open documentation.", ex);
        //    }
        //}

        private void OpenFileDirectory()
        {
            if (treeService.SelectedItem is DatabaseItem { DatabasePath: { Directory: { } dir } })
            {
                Process.Start(dir);
            }
            else
                throw new Exception("s,,333d32,,,333333");
        }



        //private void CloseDatabase()
        //{

        //    if (selectedItemService.SelectedItem is TreeItem { DatabasePath: { Path: { } path } })
        //    {
        //        var db = treeService.TreeViewItems.SingleOrDefault(x => x.DatabasePath.Equals(path));
        //        treeService.TreeViewItems.Remove(db);
        //        Info("Closed database '" + db.DisplayName + "'.");
        //    }
        //}


    }

    public class MenuViewModel
    {
        public MenuViewModel()
        {
        }

        public IReadOnlyCollection<MenuItem> Collection { get; init; }

    }

    public class Seperator : MenuObject
    {


    }


    public class MenuObject
    {
    }

    public class MenuItem : MenuObject
    {
        public MenuItem(string header)
        {
            Header = header;

        }

        public string Header { get; }
        public ICommand? Command { get; init; }
        public IReadOnlyCollection<MenuObject> Collection { get; init; } = Array.Empty<MenuObject>();
    }
}