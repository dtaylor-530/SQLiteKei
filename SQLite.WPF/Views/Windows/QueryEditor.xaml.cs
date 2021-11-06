using Database.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SQLite.Views
{
    /// <summary>
    /// Interaction logic for QueryEditor.xaml
    /// </summary>
    public partial class QueryEditor : UserControl
    {
        public QueryEditor()
        {
            KeyDown += new KeyEventHandler(Window_KeyDown);

            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                (DataContext as QueryEditorViewModel).Execute();
        }
    }
}
