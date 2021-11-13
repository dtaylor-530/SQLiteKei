using AutoBogus;
using Bogus;
using OxyPlot;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Utility.WPF.Chart;
using static Utility.WPF.Chart.ColumnSelectionsGrid;

namespace Sandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Faker<Columns> personFaker;

        public class Columns
        {
            public double First { get; init; }
            public double Second { get; init; }
            public double Name { get; init; }
            public double Id { get; init; }
        }

        public MainWindow()
        {
            InitializeComponent();
            ColumnSelectionsGrid.Columns = new Column[] { new Column("First"), new Column("Second"), new Column("Name"), new Column("Id"), };
            ColumnSelectionsGrid.ColumnPropertyKey = nameof(Column.Name);

            this.ColumnSelectionsGrid.ColumnsSelectionChanged += ColumnSelectionsGrid_ColumnsSelectionChanged;
            personFaker = new AutoFaker<Columns>()
                .RuleFor(fake => fake.First, fake => fake.Random.Int(-10, 200))
                .RuleFor(fake => fake.Second, fake => fake.Random.Odd(10, 20))
                .RuleFor(fake => fake.Name, fake => fake.Random.Double(-220, 4200))
                .RuleFor(fake => fake.Id, fake => fake.Random.Int(-1, 4));
        }

        private void ColumnSelectionsGrid_ColumnsSelectionChanged(object sender, ColumnsSelectionsChangedEventArgs e)
        {
            List<Utility.Chart.Series> series = new();
            foreach (var xx in e.ColumnSelections.Collection)
            {
                var lineSeries = new Utility.Chart.Series(xx.ColumnX, xx.ColumnY, personFaker.Generate(1000).Select(a =>
                 {
                     double x, y;
                     switch (xx.ColumnX)
                     {
                         case nameof(Columns.First):
                             x = a.First;
                             break;
                         case nameof(Columns.Id):
                             x = a.Id;
                             break;
                         case nameof(Columns.Name):
                             x = a.Name;
                             break;
                         case nameof(Columns.Second):
                             x = a.Second;
                             break;
                         default:
                             throw new System.Exception("es44222r3323");
                     };

                     switch (xx.ColumnY)
                     {
                         case nameof(Columns.First):
                             y = a.First;
                             break;
                         case nameof(Columns.Id):
                             y = a.Id;
                             break;
                         case nameof(Columns.Name):
                             y = a.Name;
                             break;
                         case nameof(Columns.Second):
                             y = a.Second;
                             break;
                         default:
                             throw new System.Exception("esr3323");
                     };

                     return new DataPoint(x, y);
                 }).ToArray());

                series.Add(lineSeries);
            }

            DynamicChart.Series = series;
        }
    }
}