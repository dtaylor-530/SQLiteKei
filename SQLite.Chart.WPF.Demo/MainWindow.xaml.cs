using OxyPlot;
using System;
using System.Linq;
using System.Windows;
using Utility.Chart.Entity;

namespace SQLite.Chart.WPF.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            //double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            //WpfPlot1.Plot.AddScatter(dataX, dataY);
            //WpfPlot1.Refresh();
            this.DataContext = new ViewModel();
        }
    }

    public class ViewModel
    {
        public ViewModel()
        {
            var now = DateTime.Now;
            Series = new[]
            {
                new Series("A", "B", "Table", Enumerable.Range(2,10).Select(a=> new DataPoint(a, a)).ToArray())

            };
            Columns = new[]
            {
                new Column("A","Table",true),
                new Column("B","Table", true),
                new Column("C","Table"),
            };

        }

        public Series[] Series { get; }

        public Column[] Columns { get; }

    }
}
