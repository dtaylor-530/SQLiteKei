using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Utility.Common.Base;
using Utility.Database;

namespace Database.Entity
{
    /// <summary>
    /// The base class for tree view items.
    /// </summary>
    public abstract class DatabaseTreeItem : TreeItem, IDatabaseViewModel
    {
        [JsonConstructor]
        protected DatabaseTreeItem(DatabaseKey key, string name) : base(key, name)
        {
            Key = key;
            Name = name;
        }

        public override DatabaseKey Key { get; }

        public override string Name { get; }
    }

    public abstract class DatabaseLeafItem : DatabaseTreeItem
    {
        protected DatabaseLeafItem(DatabaseKey key, string name) : base(key, name)
        {
            Key = key;
        }

        public override DatabaseKey Key { get; }
    }

    public class DatabaseBranchItem : DatabaseTreeItem
    {
        public DatabaseBranchItem(DatabaseKey key, string name, ObservableCollection<DatabaseTreeItem> items) : base(key, name)
        {
            Key = key;
            Items = items;
        }

        public override DatabaseKey Key { get; }
        public ObservableCollection<DatabaseTreeItem> Items { get; }
    }
}