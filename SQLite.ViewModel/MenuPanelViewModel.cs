using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Service;
using System.Windows.Input;

namespace SQLite.ViewModel
{
    public class ImageButton : PanelObject
    {
        public ImageButton(ICommand command, string toolTip, string source)
        {
            Command = command;
            ToolTip = toolTip;
            Source = source;
        }

        public ICommand Command { get; }
        public string ToolTip { get; }
        public string Source { get; }
    }


    public class PanelObject
    {

    }
    public class SeperatorItem : PanelObject
    {

    }

    public class MenuPanelViewModel
    {


        public MenuPanelViewModel(
            TreeService treeService,
            ILocaliser localiser,
            DatabaseService databaseService,
            ViewService viewService)
        {
            Collection = new PanelObject[]
            {
                //TODO new image resource service
                new ImageButton(ReactiveCommand.Create(databaseService.CreateNewDatabase), localiser["Tooltip_NewDatabase"],"../../Resources/Icons/Database_New.png" ),
                new ImageButton(ReactiveCommand.Create(databaseService.OpenDatabase), localiser["Tooltip_OpenDatabase"],"../../Resources/Icons/Database_Open.png" ),
                new ImageButton(ReactiveCommand.Create(databaseService.CloseDatabase), localiser["Tooltip_CloseDatabase"],"../../Resources/Icons/Database_Close.png" ),
                new ImageButton(ReactiveCommand.Create(databaseService.DeleteDatabase), localiser["Tooltip_DeleteDatabase"],"../../Resources/Icons/Database_Delete.png" ),
                new ImageButton(ReactiveCommand.Create(treeService.RefreshTree), "Refresh","../../Resources/Icons/Refresh.png" ),
                new SeperatorItem(),
                new ImageButton(ReactiveCommand.Create(viewService.OpenQueryEditor), localiser["Tooltip_WriteSQLStatement"],"../../Resources/Icons/SQL_Statement.png" ),
                new ImageButton(ReactiveCommand.Create(viewService.OpenTableCreator), localiser["Tooltip_CreateTable"],"../../Resources/Icons/Table_New.png" ),
            };
        }


        public IReadOnlyCollection<PanelObject> Collection { get; }
    }
}

