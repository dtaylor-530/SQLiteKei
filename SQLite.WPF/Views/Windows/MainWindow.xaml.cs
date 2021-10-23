using Splat;
using SQLite.Service.Service;
using System;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;

namespace SQLite.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {
            KeyDown += new KeyEventHandler(Window_KeyDown);

            InitializeComponent();
        }

        protected override async void OnClosed(EventArgs e)
        {
            Locator.Current.GetService<ExitService>().Exit();
            base.OnClosed(e);
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
