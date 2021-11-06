using SQLite.Common;
using SQLite.Common.Contracts;
using Utility.Common;
using Utility.Common.Base;

namespace SQLite.Service.Service;

public class ViewService : IViewService
{
    private readonly IViewModelFactory viewModelFactory;
    private readonly ILocaliser localiser;
    private readonly IWindowService windowService;
    private readonly ITreeModel treeService;

    public ViewService(IViewModelFactory viewModelFactory, ILocaliser localiser, IWindowService windowService, ITreeModel treeService)
    {
        this.viewModelFactory = viewModelFactory;
        this.localiser = localiser;
        this.windowService = windowService;
        this.treeService = treeService;
    }

    public void OpenQueryEditor()
    {
        var preferences = viewModelFactory.Build(new PreferencesViewModelKey());
        windowService.ShowWindow(new(localiser["WindowTitle_Preferences"], preferences, ResizeMode.NoResize, Show.ShowDialog));
    }
    public void OpenTableCreator()
    {
        var tableCreatorViewModel = viewModelFactory.Build(new TableCreatorViewModelKey());
        windowService.ShowWindow(new(localiser["WindowTitle_Preferences"], tableCreatorViewModel, ResizeMode.NoResize, Show.ShowDialog));

        treeService.OnNext(new TreeItemChangeRequest(Refresh: new object()));
    }
}
