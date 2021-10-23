using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Utility;
using Utility.Database;

namespace SQLite.Common.Contracts
{
    public abstract class Item : IViewModel, IEquatable<Item?>
    {
        [JsonConstructor]
        protected Item(DatabaseKey key, string name)
        {
            Key = key;
            Name = name;
        }

        public virtual Key Key { get; }

        public virtual string Name { get; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Item);
        }

        public bool Equals(Item? other)
        {
            return other != null &&
                   EqualityComparer<Key>.Default.Equals(Key, other.Key) &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Name);
        }

        public static bool operator ==(Item? left, Item? right)
        {
            return EqualityComparer<Item>.Default.Equals(left, right);
        }

        public static bool operator !=(Item? left, Item? right)
        {
            return !(left == right);
        }
    }

    /// <summary>
    /// The base class for tree view items.
    /// </summary>
    public abstract class DatabaseTreeItem : Item
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

    public abstract class LeafItem : DatabaseTreeItem
    {
        protected LeafItem(DatabaseKey key, string name) : base(key, name)
        {

        }
    }

    public abstract class BranchItem : DatabaseTreeItem
    {
        protected BranchItem(DatabaseKey key, string name, ObservableCollection<DatabaseTreeItem> items) : base(key, name)
        {
            Items = items;
        }

        public ICollection<DatabaseTreeItem> Items { get; init; }
    }
}