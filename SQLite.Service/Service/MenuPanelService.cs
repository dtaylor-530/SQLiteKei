using ReactiveUI;
using SQLite.Common.Contracts;
using Utility;
using Utility.Common.Base;
using Utility.Common.Contracts;

namespace SQLite.Service.Service;

public class MenuPanelService : IMenuPanelService
{
    private readonly DatabaseService databaseService;
    private readonly IViewService viewService;
    private readonly ITreeService treeService;
    private readonly ILocaliser localiser;

    public MenuPanelService(DatabaseService databaseService, IViewService viewService, ITreeService treeService, ILocaliser localiser)
    {
        this.databaseService = databaseService;
        this.viewService = viewService;
        this.treeService = treeService;
        this.localiser = localiser;
    }

    public IReadOnlyCollection<PanelObject> Collection => new PanelObject[]
        {
                //TODO new image resource service
                new ImageButton(ReactiveCommand.Create(databaseService.CreateNewDatabase), localiser["Tooltip_NewDatabase"],"./Resources/Icons/Database_New.png" ),
                new ImageButton(ReactiveCommand.Create(databaseService.OpenDatabase), localiser["Tooltip_OpenDatabase"],"./Resources/Icons/Database_Open.png" ),
                new ImageButton(ReactiveCommand.Create(databaseService.CloseDatabase), localiser["Tooltip_CloseDatabase"],"./Resources/Icons/Database_Close.png" ),
                new ImageButton(ReactiveCommand.Create(databaseService.DeleteDatabase), localiser["Tooltip_DeleteDatabase"],"./Resources/Icons/Database_Delete.png" ),
                new ImageButton(ReactiveCommand.Create(treeService.RefreshTree), "Refresh","./Resources/Icons/Refresh.png" ),
                new SeperatorItem(),
                new ImageButton(ReactiveCommand.Create(viewService.OpenQueryEditor), localiser["Tooltip_WriteSQLStatement"],"./Resources/Icons/SQL_Statement.png" ),
                new ImageButton(ReactiveCommand.Create(viewService.OpenTableCreator), localiser["Tooltip_CreateTable"],"./Resources/Icons/Table_New.png" ),
        };
}
