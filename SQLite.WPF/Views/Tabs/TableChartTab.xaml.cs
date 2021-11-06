using System.Windows.Controls;
using Utility.WPF.Chart;

namespace Database.WPF.Views.Tabs
{
    /// <summary>
    /// Interaction logic for TableChartTab.xaml
    /// </summary>
    public partial class TableChartTab : UserControl
    {
        public TableChartTab()
        {
            InitializeComponent();
            //this.ColumnSelectionsGrid.ColumnsSelectionChanged += ColumnSelectionsGrid_ColumnsSelectionChanged;
        }

        private void ColumnSelectionsGrid_ColumnsSelectionChanged(object sender, ColumnsSelectionsChangedEventArgs e)
        {
        }
    }
}