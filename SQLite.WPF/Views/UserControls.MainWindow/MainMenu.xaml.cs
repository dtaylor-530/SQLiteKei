using Microsoft.Win32;
using Splat;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.MainWindow;
using SQLiteKei.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using log = SQLiteKei.Helpers.Log;

namespace SQLite.WPF.Views.UserControls.MainWindow
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu 
    {
        private readonly MainWindowViewModel? viewModel;

        public MainMenu()
        {
            InitializeComponent();
            this.viewModel = Locator.Current.GetService<MainWindowViewModel>();
        }


        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void OpenPreferences(object sender, RoutedEventArgs e)
        {
            new Preferences().ShowDialog();
        }

        private void OpenQueryEditor(object sender, RoutedEventArgs e)
        {
            new QueryEditor(viewModel.TreeViewItems).ShowDialog();
        }

 
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenDocumentation(object sender, RoutedEventArgs e)
        {
            OpenDocumentation();
        }
        private void OpenDocumentation()
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Documentation.pdf");
            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
{
                log.Logger.Error("Failed to open documentation.", ex);
            }
        }

        private void OpenFileDirectory(object sender, RoutedEventArgs e)
        {
            var database = (DatabaseItem)viewModel.SelectedItem;
            var targetDirectory = System.IO.Path.GetDirectoryName(database.DatabasePath);

            Process.Start(targetDirectory);
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




    }
}
