using SQLite.Common.Contracts;
using SQLite.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace SQLite.WPF.Views.UserControls.MainWindow
{
    /// <summary>
    /// Interaction logic for TreeViewPanel.xaml
    /// </summary>
    public partial class TreeViewPanel
    {

        public TreeViewPanel()
        {
            InitializeComponent();

        }

        private void DBTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is TreeViewModel viewmodel && e.NewValue is TreeItem treeItem)
                viewmodel.SelectedItem = treeItem;
            else
                throw new System.Exception("kktiit");

        }

        private void TreeViewRightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
