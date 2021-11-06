using ReactiveUI;
using Utility.Common;
using Utility.Common.Base;
using Utility.ViewModel.Base;

namespace Utility.ViewModel;

public class TreeViewModel : BaseViewModel<ITreeViewModel>, ITreeViewModel
{
    private readonly ITreeModel treeService;

    public TreeViewModel(TreeViewModelKey treeViewModelKey, ITreeModel treeModel) : base(treeViewModelKey)
    {
        this.treeService = treeModel;
        treeModel.Subscribe(a => this.RaisePropertyChanged(nameof(SelectedItem)));
    }

    public IReadOnlyCollection<TreeItem> TreeViewItems => treeService.TreeViewItems;

    public TreeItem SelectedItem
    {
        get => treeService.SelectedItem;
        set => treeService.SelectedItem = value;
    }
    public override string? Name { get; }
}
