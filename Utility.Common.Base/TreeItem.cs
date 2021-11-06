using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Utility.Entity;

namespace Utility.Common.Base
{
    public abstract class TreeItem : IViewModel, IEquatable<TreeItem?>
    {
        private bool isSelected;

        [JsonConstructor]
        protected TreeItem(Key key, string name)
        {
            Key = key;
            Name = name;
        }

        public virtual Key Key { get; }

        public virtual string Name { get; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public bool IsLoaded { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public override bool Equals(object? obj)
        {
            return Equals(obj as TreeItem);
        }

        public bool Equals(TreeItem? other)
        {
            return other != null &&
                   EqualityComparer<IKey>.Default.Equals(Key, other.Key) &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Name);
        }

        public static bool operator ==(TreeItem? left, TreeItem? right)
        {
            return EqualityComparer<TreeItem>.Default.Equals(left, right);
        }

        public static bool operator !=(TreeItem? left, TreeItem? right)
        {
            return !(left == right);
        }

        public void Deconstruct(out IKey key, out string name)
        {
            name = this.Name;
            key = this.Key;
        }
    }

    public abstract class LeafItem : TreeItem
    {
        protected LeafItem(Key key, string name) : base(key, name)
        {

        }
    }

    public abstract class BranchItem : TreeItem
    {
        protected BranchItem(Key key, string name, ObservableCollection<TreeItem> items) : base(key, name)
        {
            Items = items;
        }

        public virtual ICollection<TreeItem> Items { get; init; }
    }

}