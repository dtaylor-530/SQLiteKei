using ReactiveUI;
using Splat;
using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Service;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using Utility.Database;

namespace SQLite.ViewModel;

public abstract class DirectoryItem : TreeItem
{
    protected DirectoryItem(string displayName, ConnectionPath databasePath) : base(displayName, databasePath)
    {
    }

    public ObservableCollection<TreeItem> Items { get; init; }

}

/// <summary>
/// A tree item representing a database.
/// </summary>
public class DatabaseItem : DirectoryItem
{
    public DatabaseItem(string displayName, ConnectionPath databasePath) : base(displayName, databasePath)
    {
    }
}

/// <summary>
/// A directory item that is used to contain tables in the tree view.
/// </summary>
public class TableFolderItem : FolderItem
{
    public TableFolderItem(string displayName, ConnectionPath databasePath) : base(displayName, databasePath)
    {
    }
}

// TODO replace with FolderItems for Triggers, Views and Indexes lateron
[Obsolete("Needs to be replaced with concrete FolderItems for each type so they can have individual context menu actions.")]
public class FolderItem : DirectoryItem
{
    public FolderItem(string displayName, ConnectionPath databasePath) : base(displayName, databasePath)
    {
    }
}

public class IndexItem : TreeItem
{
    public IndexItem(string displayName, ConnectionPath databasePath) : base(displayName, databasePath)
    {
    }
}

/// <summary>
/// A tree item representing a database table.
/// </summary>
public class TableItem : TreeItem
{
    private readonly ILocaliser? localiser;
    private readonly TableService? tableService;
    private ReactiveCommand<Unit, Unit>? deleteCommand;

    public TableItem(string displayName, ConnectionPath databasePath) : base(displayName, databasePath)
    {
        this.localiser = Locator.Current.GetService<ILocaliser>();
        this.tableService = Locator.Current.GetService<TableService>(); ;
    }

    public ICommand DeleteCommand => deleteCommand ??= ReactiveCommand.Create(Delete);

    public string? DeleteKey => localiser?["ContextMenuItem_DeleteTable"];

    private void Delete()
    {
        tableService?.DeleteTable(this);
    }
}

/// <summary>
/// A tree item representing a database trigger.
/// </summary>
public class TriggerItem : TreeItem
{
    public TriggerItem(string displayName, ConnectionPath databasePath) : base(displayName, databasePath)
    {
    }
}

/// <summary>
/// A tree item representing a database view.
/// </summary>
public class ViewItem : TreeItem
{
    public ViewItem(string displayName, ConnectionPath databasePath) : base(displayName, databasePath)
    {
    }
}

/// <summary>
/// A ViewModel that resembles a selectable database which is used in ComboBoxes.
/// </summary>
public record DatabaseSelectItem(string DatabaseName, ConnectionPath DatabasePath);