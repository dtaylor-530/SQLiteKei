using OxyPlot;
using OxyPlot.Series;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;

namespace Utility.WPF.Chart
{

    /// <summary>
    /// Interaction logic for DynamicChart.xaml
    /// </summary>
    public partial class DynamicSelectionChart
    {

        public static readonly DependencyProperty SeriesProperty =
    DependencyProperty.Register("Series", typeof(IEnumerable), typeof(DynamicSelectionChart), new PropertyMetadata(null));

        public DynamicSelectionChart()
        {
            InitializeComponent();

            this.WhenAnyValue(a => a.Series)
                .WhereNotNull()
                .Combine(new PlotModel())
                .Subscribe(series =>
                {
                    RefreshPlot(series.observable, series.observed);
                });
        }

        private void RefreshPlot(IEnumerable series, PlotModel plotModel)
        {
            plotModel.Series.Clear();
            plotModel.Axes.Clear();
            double xAxisDistance = 0;
            double yAxisDistance = 0;
            HashSet<string> xNames = new();
            HashSet<string> yNames = new();
            foreach (var serie in series)
            {
                if (serie is Utility.Chart.Series series1)
                {
                    var lSeries = new LineSeries();
                    lSeries.Points.AddRange(series1.Points);
                    plotModel.Series.Add(lSeries);

                    if (xNames.Add(series1.XName))
                    {
                        var xAxis = new OxyPlot.Axes.LinearAxis();
                        xAxis.Position = OxyPlot.Axes.AxisPosition.Bottom;
                        xAxis.AxisDistance = xAxisDistance;
                        xAxis.AxisTitleDistance += xAxisDistance;
                        xAxisDistance += 50;
                        xAxis.Title = series1.XName;
                        plotModel.Axes.Add(xAxis);

                    }
                    if (yNames.Add(series1.YName))
                    {
                        var yAxis = new OxyPlot.Axes.LinearAxis();
                        yAxis.Position = OxyPlot.Axes.AxisPosition.Left;
                        yAxis.Title = series1.YName;
                        yAxis.AxisDistance = yAxisDistance;
                        yAxis.AxisTitleDistance += yAxisDistance;
                        yAxisDistance += 50;
                        plotModel.Axes.Add(yAxis);
                    }
                }
                else
                    throw new Exception("34234fffff");

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
                .Skip(1)!;
        }
    }
}
