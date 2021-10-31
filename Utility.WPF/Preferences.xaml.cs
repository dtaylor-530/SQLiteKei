using System.Windows;
using System.Windows.Controls;

namespace SQLite.Views
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : UserControl
    {
        public Preferences()
        {
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            //Close();
            (this.Parent as Window).Close();
        }
    }
}
