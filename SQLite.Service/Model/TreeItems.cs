using ReactiveUI;
using Splat;
using SQLite.Common.Contracts;
using SQLite.Service.Service;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json.Serialization;
using System.Windows.Input;
using Utility.Database;

namespace SQLite.Service.Model
{

    /// <summary>
    /// a database.
    /// </summary>
    public class DatabaseBranchItem : BranchItem
    {
        public DatabaseBranchItem(string displayName, DatabaseKey key, ObservableCollection<DatabaseTreeItem> items) : base(key, displayName, items)
        {
            Key = key;
        }

        public override DatabaseKey Key { get; }
    }

    /// <summary>
    /// A directory item that is used to contain tables in the tree view.
    /// </summary>
    public class TableFolderItem : FolderBranchItem
    {
        public TableFolderItem(string displayName, DatabaseKey key, ObservableCollection<DatabaseTreeItem> items) : base(displayName, key, items)
        {
            Key = key;
        }

        public override DatabaseKey Key { get; }
    }

    // TODO replace with FolderItems for Triggers, Views and Indexes lateron
    [Obsolete("Needs to be replaced with concrete FolderItems for each type so they can have individual context menu actions.")]
    public class FolderBranchItem : BranchItem
    {
        public FolderBranchItem(string displayName, DatabaseKey key, ObservableCollection<DatabaseTreeItem> items) : base(key, displayName, items)
        {
            Key = key;
        }

        public override DatabaseKey Key { get; }
    }

    public class IndexLeafItem : LeafItem
    {
        public IndexLeafItem(string displayName, TableKey key) : base(key, displayName)
        {
            Key = key;
        }

        public override TableKey Key { get; }
    }

    /// <summary>
    /// database table.
    /// </summary>
    public class TableLeafItem : LeafItem
    {
        private readonly ILocaliser? localiser;
        private readonly TableService? tableService;
        private ReactiveCommand<Unit, Unit>? deleteCommand;

        public TableLeafItem(string displayName, TableKey key) : base(key, displayName)
        {
            this.localiser = Locator.Current.GetService<ILocaliser>();
            this.tableService = Locator.Current.GetService<TableService>(); ;
            Key = key;
        }

        [JsonIgnore]
        public ICommand DeleteCommand => deleteCommand ??= ReactiveCommand.Create(Delete);
        [JsonIgnore]
        public string? DeleteKey => localiser?["ContextMenuItem_DeleteTable"];

        private void Delete()
        {
            tableService?.DeleteTable(this);
        }

        public override TableKey Key { get; }
    }

    /// <summary>
    ///  database trigger.
    /// </summary>
    public class TriggerLeafItem : LeafItem
    {
        public TriggerLeafItem(string displayName, TableKey key) : base(key, displayName)
        {
            Key = key;
        }

        public override TableKey Key { get; }
    }

    /// <summary>
    /// database view.
    /// </summary>
    public class ViewLeafItem : LeafItem
    {
        public ViewLeafItem(string displayName, TableKey key) : base(key, displayName)
        {
        }
        public override TableKey Key { get; }
    }

    /// <summary>
    /// A ViewModel that resembles a selectable database which is used in ComboBoxes.
    /// </summary>
    public record DatabaseSelectItem(DatabasePath Path);
}