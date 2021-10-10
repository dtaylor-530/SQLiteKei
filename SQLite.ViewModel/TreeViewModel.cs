using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Service;
using System.Collections.ObjectModel;

namespace SQLite.ViewModel
{
    public class TreeViewModel
    {
        private readonly TreeService treeService;

        public TreeViewModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public ObservableCollection<TreeItem> TreeViewItems => treeService.TreeViewItems;

        public TreeItem SelectedItem
        {
            get => treeService.SelectedItem;
            set
            {
                treeService.SelectedItem = value;
            }
        }
    }
}
