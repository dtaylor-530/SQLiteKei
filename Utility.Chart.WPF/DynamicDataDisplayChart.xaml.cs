using DynamicDataDisplay;
using DynamicDataDisplay.DataSources;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Utility.Chart.Entity;

namespace Utility.Chart.WPF
{
    /// <summary>
    /// Interaction logic for DynamicDataDisplayChart.xaml
    /// </summary>
    public partial class DynamicDataDisplayChart : UserControl
    {
        public DynamicDataDisplayChart()
        {
            InitializeComponent();

            plotter.Legend.LegendLeft = 10;
            plotter.Legend.LegendRight = double.NaN;
            this.Loaded += DynamicDataDisplayChart_Loaded;

        }

        private void DynamicDataDisplayChart_Loaded(object sender, RoutedEventArgs e)
        {
            this.WhenAnyValue(a => a.Series)

                             .Subscribe(series =>
                             {
                                 plotter.Children.RemoveAll<LineGraph>();
                                 //plotter.RemoveUserElements();
                                 if (series is IEnumerable<Series> seriesSet)
                                 {
                                     foreach (var ser in seriesSet)
                                     {
                                         var dataSource = CreateDataSource(ser);
                                         plotter.AddLineGraph(dataSource, Colors.Black, 1, ser.TableName);
                                         //(dataSource as dynamic).RaiseDataChanged();
                                     }
                                 }

                                 plotter.FitToView();

                             });
        }

        private IPointDataSource CreateDataSource(Series series)
        {
            if (series.Points != null)
            {
                EnumerableDataSource<OxyPlot.DataPoint> ds = new EnumerableDataSource<OxyPlot.DataPoint>(series.Points);
                if (series.IsXDateTime)
                    ds.SetXMapping(ci => dateAxis.ConvertToDouble(new DateTime((long)ci.X)));
                else
                    ds.SetYMapping(ci => ci.X);

                if (series.IsYDateTime)
                    ds.SetXMapping(ci => dateAxis.ConvertToDouble(new DateTime((long)ci.Y)));
                else
                    ds.SetYMapping(ci => ci.Y);
                return ds;
            }

            throw new Exception("df,,,,,ssd");
        }

        public IEnumerable Series
        {
            get { return (IEnumerable)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(IEnumerable), typeof(DynamicDataDisplayChart), new PropertyMetadata(null));
    }
}
