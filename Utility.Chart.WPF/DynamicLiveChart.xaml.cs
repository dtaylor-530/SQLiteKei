using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using Utility.Chart.Entity;

namespace Utility.Chart.WPF
{
    /// <summary>
    /// Interaction logic for DynamicLiveChart.xaml
    /// </summary>
    public partial class DynamicLiveChart
    {
        public DynamicLiveChart()
        {
            InitializeComponent();
            //CartesianChart.SyncContext = Application.Current.Cont
            //this.CartesianChart.Series = Series;
            this.WhenAnyValue(a => a.Series)
                .OfType<IEnumerable<Series>>()
                .Subscribe(series =>
                {

                    var a = series.Select(a => new ScatterSeries<ObservablePoint> { Values = a.Points.Select(a => new ObservablePoint(a.X, a.Y))/*.Take((int)ts * 5000)*/ }).ToArray();

                    this.CartesianChart.Series = a;

                    //Observable
                    //.Interval(TimeSpan.FromSeconds(0))
                    //.Select(ts =>
                    //{

                    //    var first = series.Select(a => new ScatterSeries<ObservablePoint> { Values = a.Points2/*.Take((int)ts * 5000)*/ }).ToArray();

                    //    return first;

                    //})
                    //.Take(1)
                    //.ObserveOnDispatcher()
                    //.Subscribe(a =>
                    //{

                    //    this.CartesianChart.Series = a;
                    //});

                    //var first = series.Select(a => new ScatterSeries<ObservablePoint> { Values = a.Points2.Take(2000) }).ToArray();
                    //this.CartesianChart.Series = first;
                });

        }

        public IEnumerable Series
        {
            get { return (IEnumerable)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register("Series", typeof(IEnumerable), typeof(DynamicLiveChart), new PropertyMetadata(null));

        //public IEnumerable<ISeries> Series { get; set; } = new ObservableCollection<ISeries>
        //{
        //    new ScatterSeries<ObservablePoint>
        //    {
        //        Values = new ObservableCollection<ObservablePoint>
        //        {
        //            new ObservablePoint(2.2, 5.4),
        //            new ObservablePoint(4.5, 2.5),
        //            new ObservablePoint(4.2, 7.4),
        //            new ObservablePoint(6.4, 9.9),
        //            new ObservablePoint(4.2, 9.2),
        //            new ObservablePoint(5.8, 3.5),
        //            new ObservablePoint(7.3, 5.8),
        //            new ObservablePoint(8.9, 3.9),
        //            new ObservablePoint(6.1, 4.6),
        //            new ObservablePoint(9.4, 7.7),
        //            new ObservablePoint(8.4, 8.5),
        //            new ObservablePoint(3.6, 9.6),
        //            new ObservablePoint(4.4, 6.3),
        //            new ObservablePoint(5.8, 4.8),
        //            new ObservablePoint(6.9, 3.4),
        //            new ObservablePoint(7.6, 1.8),
        //            new ObservablePoint(8.3, 8.3),
        //            new ObservablePoint(9.9, 5.2),
        //            new ObservablePoint(8.1, 4.7),
        //            new ObservablePoint(7.4, 3.9),
        //            new ObservablePoint(6.8, 2.3),
        //            new ObservablePoint(5.3, 7.1),
        //        }
        //    }
        //};
    }
}
