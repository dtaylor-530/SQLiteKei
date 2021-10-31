using ReactiveUI;
using System.Collections.ObjectModel;
using Utility.Common;
using Utility.Common.Base;
using Utility.ViewModel.Base;

namespace Utility.ViewModel;

public class TreeViewModel : BaseViewModel<ITreeViewModel>, ITreeViewModel
{
    private readonly ITreeService treeService;

    public TreeViewModel(TreeViewModelKey treeViewModelKey, ITreeService treeService) : base(treeViewModelKey)
    {
        this.treeService = treeService;
        treeService.Subscribe(a => this.RaisePropertyChanged(nameof(SelectedItem)));
    }

    public ObservableCollection<TreeItem> TreeViewItems => treeService.TreeViewItems;

    public TreeItem SelectedItem
    {
        get => treeService.SelectedItem;
        set => treeService.SelectedItem = value;
    }
    public override string? Name { get; }
}
