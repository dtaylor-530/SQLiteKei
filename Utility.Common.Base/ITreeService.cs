namespace Utility.Common.Base
{

    public record SelectedTreeItem(TreeItem TreeItem);

    public interface ITreeItemChanges : IObservable<SelectedTreeItem>
    {
    }

    public interface ITreeService : ITreeItemChanges
    {
        TreeItem SelectedItem { get; set; }
        System.Collections.ObjectModel.ObservableCollection<TreeItem> TreeViewItems { get; }

        void RefreshTree();
        void RemoveItemFromTree(IKey key);
        void SaveTree();
    }
}
