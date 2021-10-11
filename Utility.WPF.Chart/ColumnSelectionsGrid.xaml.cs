using DynamicData;
using Fasterflect;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;

namespace Utility.WPF.Chart
{
    public delegate void ColumnsSelectionsChangedHandler(object source, ColumnsSelectionsChangedEventArgs eventArgs);

    public class ColumnsSelectionsChangedEventArgs : RoutedEventArgs
    {
        public ColumnsSelectionsChangedEventArgs(RoutedEvent routedEvent, object source, ColumnSelections columnSelections) : base(routedEvent, source)
        {
            ColumnSelections = columnSelections;
        }

        public ColumnSelections ColumnSelections { get; }
    }

    public record SeriesPair(string ColumnX, string ColumnY);

    public record ColumnSelections(IReadOnlyCollection<SeriesPair> Collection);

    public partial class ColumnSelectionsGrid
    {
        public static readonly RoutedEvent ColumnsSelectionChangedEvent =
            EventManager.RegisterRoutedEvent("ColumnsSelectionChanged", RoutingStrategy.Bubble, typeof(ColumnsSelectionsChangedHandler), typeof(ColumnSelectionsGrid));

        public static readonly DependencyProperty ColumnSelectionsProperty =
            DependencyProperty.Register("ColumnSelections", typeof(IReadOnlyCollection<SeriesPair>), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnDetailsProperty =
            DependencyProperty.Register("ColumnDetails", typeof(IEnumerable), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnPropertyKeyProperty =
            DependencyProperty.Register("ColumnPropertyKey", typeof(string), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public ColumnSelectionsGrid()
        {
            this.InitializeComponent();
            CompositeDisposable? disposables = null;
            this.WhenAnyValue(a => a.ColumnDetails)
                .WhereNotNull()
                .CombineLatest(this.WhenAnyValue(a => a.ColumnPropertyKey)
                .WhereNotNull())
                .Subscribe(c =>
                {
                    IEnumerable<string> columnNames = null;
                    if (ColumnPropertyKey != null)
                    {
                        columnNames = c.First.Cast<object>().Select(a => (string)(a.TryGetValue(c.Second) ?? throw new Exception("g44444")));
                    }
                    else if (c.First is not IEnumerable<string> coll)
                        return;
                    else
                        columnNames = coll;

                    List<Column> columns = new();

                    disposables?.Dispose();
                    disposables = new();
                    ReplaySubject<Column> replaySubject = new();
                    foreach (var name in columnNames)
                    {
                        var col = new Column(name);
                        columns.Add(col);
                        col.WhenAny((a) => a.X, a => a.Y, (a, b) => a.Sender)
                        .Subscribe(replaySubject)
                         .DisposeWith(disposables);
                    }

                    replaySubject
                      .ToObservableChangeSet(a => a.Name)
                        .ToCollection()
                        .Where(a => a.Count > 0)
                        .Subscribe(a =>
                        {
                            if (a.Count(a => a.X) > 1)
                                return;
                            if (a.SingleOrDefault(a => a.X) is not { } singleX)
                                return;
                            var cs = new ColumnSelections(a.Where(c => c.Y).Select(a => new SeriesPair(singleX.Name, a.Name)).ToArray());
                            ColumnSelections = cs.Collection;
                            this.RaiseEvent(new ColumnsSelectionsChangedEventArgs(ColumnsSelectionChangedEvent, this, cs));
                        });

                    this.DataGrid.ItemsSource = columns;
                });
        }

        #region properties
        public IReadOnlyCollection<SeriesPair> ColumnSelections
        {
            get { return (IReadOnlyCollection<SeriesPair>)GetValue(ColumnSelectionsProperty); }
            set { SetValue(ColumnSelectionsProperty, value); }
        }

        public IEnumerable ColumnDetails
        {
            get { return (IEnumerable)GetValue(ColumnDetailsProperty); }
            set { SetValue(ColumnDetailsProperty, value); }
        }

        public string ColumnPropertyKey
        {
            get { return (string)GetValue(ColumnPropertyKeyProperty); }
            set { SetValue(ColumnPropertyKeyProperty, value); }
        }

        public event ColumnsSelectionsChangedHandler ColumnsSelectionChanged
        {
            add { AddHandler(ColumnsSelectionChangedEvent, value); }
            remove { RemoveHandler(ColumnsSelectionChangedEvent, value); }
        }
        #endregion properties

        public class Column : ReactiveObject, IEquatable<Column?>
        {
            private bool x; private bool y;

            public Column(string name)
            {
                Name = name;
            }

            public string Name { get; }
            public bool X { get => x; set => this.RaiseAndSetIfChanged(ref x, value); }
            public bool Y { get => y; set => this.RaiseAndSetIfChanged(ref y, value); }

            public bool Equals(Column? obj)
            {
                return obj is Column column &&
                       Name == column.Name &&
                       X == column.X &&
                       Y == column.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Name, X, Y);
            }

            public static bool operator ==(Column? left, Column? right)
            {
                return EqualityComparer<Column>.Default.Equals(left, right);
            }

            public static bool operator !=(Column? left, Column? right)
            {
                return !(left == right);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Column);
            }
        }
    }
}
