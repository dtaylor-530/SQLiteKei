using System.Windows.Controls;
using Utility.Chart.WPF;

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

            this.Loaded += TableChartTab_Loaded;
            //this.ColumnSelectionsGrid.ColumnsSelectionChanged += ColumnSelectionsGrid_ColumnsSelectionChanged;
        }

        private void TableChartTab_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            //double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            //double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            //WpfPlot1.Plot.AddScatter(dataX, dataY);
            //WpfPlot1.Refresh();
        }

        private void ColumnSelectionsGrid_ColumnsSelectionChanged(object sender, ColumnsSelectionsChangedEventArgs e)
        {
        }

    }
}