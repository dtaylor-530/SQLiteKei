using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.Service.Service;
using System.Collections.ObjectModel;

namespace SQLite.ViewModel
{
    public class TreeViewModel : ReactiveObject
    {
        private readonly TreeService treeService;

        public TreeViewModel(TreeService treeService)
        {
            this.treeService = treeService;

            treeService.Subscribe(a => this.RaisePropertyChanged(nameof(SelectedItem)));
        }

        public ObservableCollection<DatabaseTreeItem> TreeViewItems => treeService.TreeViewItems;

        public DatabaseTreeItem SelectedItem
        {
            get => treeService.SelectedItem;
            set => treeService.SelectedItem = value;
        }
    }
}
