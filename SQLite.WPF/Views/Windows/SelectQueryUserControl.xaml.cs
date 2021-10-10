using System.Windows;

namespace SQLite.Views
{
    /// <summary>
    /// Interaction logic for GenerateSelectQueryWindow.xaml
    /// </summary>
    public partial class SelectQueryUserControl
    {
        public SelectQueryUserControl()
        {
            InitializeComponent();
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
            //Close();
        }
    }
}
