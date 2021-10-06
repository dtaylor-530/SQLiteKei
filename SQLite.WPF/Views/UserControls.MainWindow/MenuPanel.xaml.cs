using Microsoft.Win32;
using Splat;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.MainWindow;
using SQLiteKei.Views;
using System.IO;
using System.Windows;

namespace SQLite.WPF.Views.UserControls.MainWindow
{
    /// <summary>
    /// Interaction logic for MenuPanel.xaml
    /// </summary>
    public partial class MenuPanel 
    {
        private readonly MainWindowViewModel? viewModel;

        public MenuPanel()
        {
            InitializeComponent();
            this.viewModel = Locator.Current.GetService<MainWindowViewModel>();
        }


        private void CreateNewDatabase(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            {
                dialog.Filter = "SQLite (*.sqlite)|*.sqlite";
                if (dialog.ShowDialog() == true)
                {
                    DatabaseHandler.CreateDatabase(dialog.FileName);
                    viewModel.OpenDatabase(dialog.FileName);
                }
            }
        }

        private void CloseDatabase(object sender, RoutedEventArgs e)
        {
            var selectedItem = viewModel.SelectedItem as TreeItem;

            if (selectedItem != null)
                viewModel.CloseDatabase(selectedItem.DatabasePath);
        }

        private void OpenDatabase(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            {
                dialog.Filter = "Database Files (*.sqlite, *.db)|*.sqlite; *db; |All Files |*.*";
                if (dialog.ShowDialog() == true)
                {
                    viewModel.OpenDatabase(dialog.FileName);
                }
            }
        }

        private void DeleteDatabase(object sender, RoutedEventArgs e)
        {
            var selectedItem = viewModel.SelectedItem as DatabaseItem;

            if (selectedItem != null)
            {
                var message = LocalisationHelper.GetString("MessageBox_DatabaseDeleteWarning", selectedItem.DisplayName);
                var result = System.Windows.MessageBox.Show(message, LocalisationHelper.GetString("MessageBoxTitle_DatabaseDeletion"), MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes) return;
                if (!File.Exists(selectedItem.DatabasePath))
                    throw new FileNotFoundException("Database file could not be found.");

                File.Delete(selectedItem.DatabasePath);
                viewModel.CloseDatabase(selectedItem.DatabasePath);
            }
        }

        private void RefreshTree(object sender, RoutedEventArgs e)
        {
            viewModel.RefreshTree();
        }


        private void OpenQueryEditor(object sender, RoutedEventArgs e)
        {
            new QueryEditor(viewModel.TreeViewItems).ShowDialog();
        }

        private void OpenTableCreator(object sender, RoutedEventArgs e)
        {
            new TableCreator(viewModel.TreeViewItems).ShowDialog();
            viewModel.RefreshTree();
        }
    }
}
