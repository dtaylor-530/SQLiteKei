using Splat;
using SQLiteKei.ViewModels.MainWindow;

using System;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel viewModel = Locator.Current.GetService<MainWindowViewModel>();

        public MainWindow()
        {
            DataContext = viewModel;
            KeyDown += new KeyEventHandler(Window_KeyDown);
            this.viewModel.TabItems.CollectionChanged += TabItems_CollectionChanged;
            InitializeComponent();

        }

        private void TabItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (var item in e.OldItems)
                    MainTabControl.Items.Remove(item);

            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    MainTabControl.Items.Add(item);
        }

        protected override void OnClosed(EventArgs e)
        {
            viewModel.SaveTree();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
        }



        #region TreeViewRightClickEvent
        /// <summary>
        /// Method that is used to make sure a tree view element is selected on a right click event before the context menu is opened.
        /// </summary>
        private void TreeViewRightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
        #endregion
    }
}
