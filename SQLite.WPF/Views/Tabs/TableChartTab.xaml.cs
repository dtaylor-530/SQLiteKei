using System.Collections.Generic;
using System.Windows.Controls;
using Utility.WPF.Chart;

namespace SQLite.WPF.Views.Tabs
{
    /// <summary>
    /// Interaction logic for TableChartTab.xaml
    /// </summary>
    public partial class TableChartTab : UserControl
    {
        public TableChartTab()
        {
            InitializeComponent();
            this.ColumnSelectionsGrid.ColumnsSelectionChanged += ColumnSelectionsGrid_ColumnsSelectionChanged;
            //personFaker = new AutoFaker<Columns>()
            //    .RuleFor(fake => fake.First, fake => fake.Random.Int(-10, 200))
            //    .RuleFor(fake => fake.Second, fake => fake.Random.Odd(10, 20))
            //    .RuleFor(fake => fake.Name, fake => fake.Random.Double(-220, 4200))
            //    .RuleFor(fake => fake.Id, fake => fake.Random.Int(-1, 4));
        }

        private void ColumnSelectionsGrid_ColumnsSelectionChanged(object sender, ColumnsSelectionsChangedEventArgs e)
        {
            List<Utility.Chart.Series> series = new();
            foreach (var xx in e.ColumnSelections.Collection)
            {
                //var lineSeries = new Utility.WPF.Chart.Series(xx.ColumnX, xx.ColumnY, personFaker.Generate(1000).Select(a =>
                //{
                //    double x, y;
                //    //switch (xx.ColumnX)
                //    //{
                //    //    case nameof(Columns.First):
                //    //        x = a.First;
                //    //        break;
                //    //    case nameof(Columns.Id):
                //    //        x = a.Id;
                //    //        break;
                //    //    case nameof(Columns.Name):
                //    //        x = a.Name;
                //    //        break;
                //    //    case nameof(Columns.Second):
                //    //        x = a.Second;
                //    //        break;
                //    //    default:
                //    //        throw new System.Exception("es44222r3323");
                //    //};

                //    //switch (xx.ColumnY)
                //    //{
                //    //    case nameof(Columns.First):
                //    //        y = a.First;
                //    //        break;
                //    //    case nameof(Columns.Id):
                //    //        y = a.Id;
                //    //        break;
                //    //    case nameof(Columns.Name):
                //    //        y = a.Name;
                //    //        break;
                //    //    case nameof(Columns.Second):
                //    //        y = a.Second;
                //    //        break;
                //    //    default:
                //    //        throw new System.Exception("esr3323");
                //    //};

                //    return new DataPoint(x, y);
                //}).ToArray());

                //   series.Add(lineSeries);
            }

            // DynamicChart.Series = series;
        }
    }
}