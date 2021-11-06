using Utility.Entity;

namespace Utility.Common.Base
{
    public record struct SelectedTreeItem(TreeItem TreeItem);

    public record struct TreeItemChangeRequest(IReadOnlyCollection<IKey>? Removes = null, IReadOnlyCollection<TreeItem>? Adds = null, object? Refresh = null);

    public interface ITreeItemChanges : IObservable<SelectedTreeItem>, IObserver<TreeItemChangeRequest>
    {
    }

    public interface ITreeModel : ITreeItemChanges
    {
        TreeItem SelectedItem { get; set; }

        IReadOnlyCollection<TreeItem> TreeViewItems { get; }

        //void RefreshTree();
        //void RemoveItemFromTree(IKey key);
        void SaveTree();
    }
}
