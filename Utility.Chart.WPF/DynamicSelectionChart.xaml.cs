using OxyPlot;
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
    /// Interaction logic for DynamicChart.xaml
    /// </summary>
    public partial class DynamicSelectionChart
    {

        public static readonly DependencyProperty SeriesProperty =
    DependencyProperty.Register("Series", typeof(IEnumerable), typeof(DynamicSelectionChart), new PropertyMetadata(null, Changed));

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public DynamicSelectionChart()
        {
            InitializeComponent();

            this.WhenAnyValue(a => a.Series)
                .WhereNotNull()
                .Combine(new PlotModel())
                .Subscribe(series =>
                {
                    //   Observable
                    //.Interval(TimeSpan.FromSeconds(2))
                    //.Select(ts =>
                    //{

                    //    return (int)((ts + 1) * 10000);

                    //})
                    //.Take(1)
                    //.ObserveOnDispatcher()
                    //.Subscribe(a =>
                    //{
                    AddScatterSeries(series.observable.Cast<Series>().ToArray(), series.observed, null);

                    // });

                });
        }

        private void AddScatterSeries(IEnumerable<Series> series, PlotModel plotModel, int? i)
        {

            plotModel.Series.Clear();
            plotModel.Axes.Clear();
            double xAxisDistance = 0;
            double yAxisDistance = 0;
            HashSet<string> xNames = new();
            HashSet<string> yNames = new();
            foreach (var serie in series)
            {

                var lSeries = new OxyPlot.Series.ScatterSeries();
                if (i.HasValue)
                    lSeries.Points.AddRange(serie.Points.Take(i.Value).Select(a => new OxyPlot.Series.ScatterPoint(a.X, a.Y)));
                else
                    lSeries.Points.AddRange(serie.Points.Select(a => new OxyPlot.Series.ScatterPoint(a.X, a.Y)));

                plotModel.Series.Add(lSeries);
                //lSeries.MarkerSize = 1;
                //lSeries.MarkerType = MarkerType.Plus;

                if (xNames.Add(serie.XName))
                {
                    var xAxis = new OxyPlot.Axes.LinearAxis();
                    xAxis.Position = OxyPlot.Axes.AxisPosition.Bottom;
                    xAxis.AxisDistance = xAxisDistance;
                    xAxis.AxisTitleDistance += xAxisDistance;
                    xAxisDistance += 50;
                    xAxis.Title = serie.XName;
                    plotModel.Axes.Add(xAxis);

                }
                if (yNames.Add(serie.YName))
                {
                    var yAxis = new OxyPlot.Axes.LinearAxis();
                    yAxis.Position = OxyPlot.Axes.AxisPosition.Left;
                    yAxis.Title = serie.YName;
                    yAxis.AxisDistance = yAxisDistance;
                    yAxis.AxisTitleDistance += yAxisDistance;
                    yAxisDistance += 50;
                    plotModel.Axes.Add(yAxis);
                }
            }

            plotModel.InvalidatePlot(true);
            this.PlotView.Model = plotModel;
        }

        public IEnumerable Series
        {
            get { return (IEnumerable)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }
    }

    public static class ObservableHelper
    {

        public static IObservable<(T observable, R observed)> Combine<T, R>(this IObservable<T> observable, R observed)
        {
            return observable
                .Scan((default(T), observed), (a, b) => (b, a.observed))
                .Where(a => a.Item1 != null && a.Item2 != null)!;
        }
    }
}
