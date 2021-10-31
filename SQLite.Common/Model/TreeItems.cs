using ReactiveUI;
using Splat;
using SQLite.Common.Contracts;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json.Serialization;
using System.Windows.Input;
using Utility.Common.Base;
using Utility.Database;

namespace SQLite.Common.Model
{

    /// <summary>
    /// a database.
    /// </summary>
    //public class DatabaseBranchItem : BranchItem
    //{
    //    public DatabaseBranchItem(string displayName, DatabaseKey key, ObservableCollection<DatabaseTreeItem> items) : base(key, displayName, items)
    //    {
    //        Key = key;
    //    }

    //    public override DatabaseKey Key { get; }
    //}

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
    public class FolderBranchItem : DatabaseBranchItem
    {
        public FolderBranchItem(string displayName, DatabaseKey key, ObservableCollection<DatabaseTreeItem> items) : base(key, displayName, items)
        {
            Key = key;
        }

        public override DatabaseKey Key { get; }
    }

    public class IndexLeafItem : DatabaseLeafItem
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
    public class TableLeafItem : DatabaseLeafItem
    {
        private readonly ILocaliser? localiser;
        private readonly ITableService? tableService;
        private ReactiveCommand<Unit, Unit>? deleteCommand;

        public TableLeafItem(string displayName, TableKey key) : base(key, displayName)
        {
            localiser = Locator.Current.GetService<ILocaliser>();
            tableService = Locator.Current.GetService<ITableService>(); ;
            Key = key;
        }

        [JsonIgnore]
        public ICommand DeleteCommand => deleteCommand ??= ReactiveCommand.Create(Delete);
        [JsonIgnore]
        public string? DeleteKey => localiser?["ContextMenuItem_DeleteTable"];

        private void Delete()
        {
            tableService?.DeleteTable(Key);
        }

        public override TableKey Key { get; }
    }

    /// <summary>
    ///  database trigger.
    /// </summary>
    public class TriggerLeafItem : DatabaseLeafItem
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
    public class ViewLeafItem : DatabaseLeafItem
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