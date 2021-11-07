using ReactiveUI;
using Utility.Common;
using Utility.Common.Base;
using Utility.ViewModel.Base;

namespace Utility.ViewModel;

public class TreeViewModel : BaseViewModel<ITreeViewModel>, ITreeViewModel
{
    private readonly ITreeModel treeModel;

    public TreeViewModel(TreeViewModelKey treeViewModelKey, ITreeModel treeModel) : base(treeViewModelKey)
    {
        this.treeModel = treeModel;
        treeModel.Subscribe(a => this.RaisePropertyChanged(nameof(SelectedItem)));
    }

    public IReadOnlyCollection<TreeItem> TreeViewItems => treeModel.TreeViewItems;

    public TreeItem SelectedItem
    {
        get => treeModel.SelectedItem;
        set => treeModel.SelectedItem = value;
    }
    public override string? Name { get; }
}
