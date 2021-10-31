using System.Windows;
using System.Windows.Input;
using Utility.Common.Base;
using Utility.ViewModel;

namespace Utility.WPF.UserControls.MainWindow
{
    /// <summary>
    /// Interaction logic for TreeViewPanel.xaml
    /// </summary>
    public partial class TreeView
    {

        public TreeView()
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
