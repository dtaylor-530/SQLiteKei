using DynamicData;
using Fasterflect;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using Utility.Chart;

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

    public partial class ColumnSelectionsGrid
    {
        public static readonly RoutedEvent ColumnsSelectionChangedEvent =
            EventManager.RegisterRoutedEvent("ColumnsSelectionChanged", RoutingStrategy.Bubble, typeof(ColumnsSelectionsChangedHandler), typeof(ColumnSelectionsGrid));

        public static readonly DependencyProperty ColumnSelectionsProperty =
            DependencyProperty.Register("ColumnSelections", typeof(IEnumerable), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnDetailsProperty =
            DependencyProperty.Register("ColumnDetails", typeof(IEnumerable), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnPropertyKeyProperty =
            DependencyProperty.Register("ColumnPropertyKey", typeof(string), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnEnabledKeyProperty =
            DependencyProperty.Register("ColumnEnabledKey", typeof(string), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public ColumnSelectionsGrid()
        {
            this.InitializeComponent();

            CompositeDisposable? disposable = null;

            this.WhenAnyValue(a => a.ColumnDetails)
                .WhereNotNull()
                .CombineLatest(this.WhenAnyValue(a => a.ColumnPropertyKey), this.WhenAnyValue(a => a.ColumnEnabledKey))
                .Select(c =>
                {
                    string[]? columnNames = null;
                    if (ColumnPropertyKey != null)
                    {
                        columnNames = c.First.Cast<object>().Select(a => (string)(a.TryGetValue(c.Second) ?? throw new Exception("g__+++44444"))).ToArray(); ;
                    }
                    else if (c.First is not IEnumerable<string> coll)
                        return null;
                    else
                        columnNames = coll.ToArray();

                    bool[]? columnsEnabled = null;
                    if (ColumnEnabledKey != null)
                    {
                        columnsEnabled = c.First.Cast<object>().Select(a => (bool)(a.TryGetValue(c.Third) ?? throw new Exception("g44568844444"))).ToArray();
                    }

                    return BuildColumns(columnNames, columnsEnabled);

                })
                .WhereNotNull()
                .DistinctUntilChanged(new ColumnComparer())
                      .CombineLatest(this
                                        .WhenAnyValue(a => a.ColumnSelections)
                                        .Select(a => a?.Cast<SeriesPair>())
                                        .DistinctUntilChanged(),
                      (a, b) =>
                      {
                          if (b == null)
                              return a;

                          var ewr = from c in b.Cast<SeriesPair>()
                                    join d in a
                                    on c.ColumnX equals d.Name
                                    select new Action(() => d.X = true);

                          var ewr2 = from c in b.Cast<SeriesPair>()
                                     join d in a
                                     on c.ColumnY equals d.Name
                                     select new Action(() => d.Y = true);

                          foreach (var c in ewr.Concat(ewr2))
                          {
                              c.Invoke();
                          }
                          return a;
                      })
                      .DistinctUntilChanged(new ColumnComparer())
                .Subscribe(columns =>
                {
                    this.DataGrid.ItemsSource = columns;

                    disposable?.Dispose();
                    disposable = new();
                    Subject<Column> subject = new();

                    subject
                    .ToObservableChangeSet(a => a.Name)
                    .ToCollection()
                    .Where(a => a.Count > 0)
                    .Subscribe(a =>
                    {
                        SetColumnSelections(a);
                    })
                    .DisposeWith(disposable);

                    foreach (var col in columns)
                    {
                        col
                        .WhenAny((a) => a.X, a => a.Y, (a, b) => a.Sender)
                        .Subscribe(subject);
                    }
                });

            void SetColumnSelections(IReadOnlyCollection<Column> a)
            {
                if (a.Count(a => a.X) > 1)
                    return;
                if (a.SingleOrDefault(a => a.X) is not { } singleX)
                    return;
                var cs = new ColumnSelections(a.Where(c => c.Y).Select(a => new SeriesPair(singleX.Name, a.Name)).ToArray());
                ColumnSelections = cs.Collection;
                this.RaiseEvent(new ColumnsSelectionsChangedEventArgs(ColumnsSelectionChangedEvent, this, cs));
            }

            static List<Column> BuildColumns(IReadOnlyCollection<string> columnNames, IReadOnlyCollection<bool>? isEnabledes = null)
            {
                List<Column> columns = new();

                isEnabledes ??= Enumerable.Repeat(true, columnNames.Count).ToArray();
                foreach (var (name, isEnabled) in columnNames.Zip(isEnabledes))
                {
                    var col = new Column(name, isEnabled);
                    columns.Add(col);
                }

                return columns;
            }
        }

        #region properties
        public IEnumerable ColumnSelections
        {
            get { return (IEnumerable)GetValue(ColumnSelectionsProperty); }
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

        public string ColumnEnabledKey
        {
            get { return (string)GetValue(ColumnEnabledKeyProperty); }
            set { SetValue(ColumnEnabledKeyProperty, value); }
        }

        public event ColumnsSelectionsChangedHandler ColumnsSelectionChanged
        {
            add { AddHandler(ColumnsSelectionChangedEvent, value); }
            remove { RemoveHandler(ColumnsSelectionChangedEvent, value); }
        }
        #endregion properties

    }

    class ColumnComparer : IEqualityComparer<IReadOnlyCollection<Column>>
    {
        public bool Equals(IReadOnlyCollection<Column>? x, IReadOnlyCollection<Column>? y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode([DisallowNull] IReadOnlyCollection<Column> obj)
        {
            return obj.Count;
        }
    }

    public class Column : ReactiveObject, IEquatable<Column?>
    {
        private bool x; private bool y;

        public Column(string name, bool isEnabled)
        {
            Name = name;
            IsEnabled = isEnabled;
        }

        public string Name { get; }
        public bool IsEnabled { get; }
        public bool X { get => x; set => this.RaiseAndSetIfChanged(ref x, value); }
        public bool Y { get => y; set => this.RaiseAndSetIfChanged(ref y, value); }

        public bool Equals(Column? obj)
        {
            return obj is Column column &&
                   Name == column.Name;
            //&&
            //X == column.X &&
            //Y == column.Y;
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
