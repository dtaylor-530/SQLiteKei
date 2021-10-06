using Splat;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.MainWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLite.WPF.Views.UserControls.MainWindow
{
    /// <summary>
    /// Interaction logic for TreeViewPanel.xaml
    /// </summary>
    public partial class TreeViewPanel 
    {
        private readonly MainWindowViewModel? viewModel;

        public TreeViewPanel()
        {
            InitializeComponent();
            this.viewModel = Locator.Current.GetService<MainWindowViewModel>();

        }

        private void CloseDatabase(object sender, RoutedEventArgs e)
        {

        }

        private void OpenFileDirectory(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteDatabase(object sender, RoutedEventArgs e)
        {

        }

        private void OpenTableCreator(object sender, RoutedEventArgs e)
        {

        }

        private void EmptyTable(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteTable(object sender, RoutedEventArgs e)
        {

        }

        private void DBTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            viewModel.SelectedItem = e.NewValue;

        }


       

        private void TreeViewRightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void OpenDatabase(object sender, RoutedEventArgs e)
        {

        }

        private void CreateNewDatabase(object sender, RoutedEventArgs e)
        {

        }
    }
}
