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
using System.Windows.Controls;
using Utility.Chart.Entity;

namespace Utility.Chart.WPF
{
    public delegate void ColumnsSelectionsChangedHandler(object source, ColumnsSelectionsChangedEventArgs eventArgs);

    public class ColumnsSelectionsChangedEventArgs : RoutedEventArgs
    {
        public ColumnsSelectionsChangedEventArgs(RoutedEvent routedEvent, object source, IReadOnlyCollection<Column> columnSelections) : base(routedEvent, source)
        {
            ColumnSelections = columnSelections;
        }

        public IReadOnlyCollection<Column> ColumnSelections { get; }
    }

    public partial class ColumnSelectionsGrid : Grid
    {
        public static readonly RoutedEvent ColumnsSelectionChangedEvent =
            EventManager.RegisterRoutedEvent("ColumnsSelectionChanged", RoutingStrategy.Bubble, typeof(ColumnsSelectionsChangedHandler), typeof(ColumnSelectionsGrid));

        //public static readonly DependencyProperty ColumnSelectionsProperty =
        //    DependencyProperty.Register("ColumnSelections", typeof(IEnumerable), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(IEnumerable), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnPropertyKeyProperty =
            DependencyProperty.Register("ColumnPropertyKey", typeof(string), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ColumnEnabledKeyProperty =
            DependencyProperty.Register("ColumnEnabledKey", typeof(string), typeof(ColumnSelectionsGrid), new PropertyMetadata(null));

        public ColumnSelectionsGrid()
        {
            this.InitializeComponent();



            //Rectangle r = DataGrid.GetCellBounds(rowCount - 1, 0); // rowCount is from                                                         
            //Size s = DataGrid.Siz;
            //s.Height = r.Y + r.Height;
            //DataGrid.ClientSize = s;

            CompositeDisposable? disposable = null;

            Columns()
            .Subscribe(columns =>
            {
                this.DataGrid.ItemsSource = columns;
                disposable = Update(columns, disposable);
            });

            IObservable<IEnumerable<Column>> Columns()
            {

                return this.WhenAnyValue(a => a.Columns)
                     .WhereNotNull()
                     .CombineLatest(this.WhenAnyValue(a => a.ColumnPropertyKey), this.WhenAnyValue(a => a.ColumnEnabledKey))
                     .Select(dc =>
                     {
                         var (columns, columnName, enabled) = dc;

                         if (columns == null)
                             return null;
                         if (columns.Cast<object>().Any() == false)
                             return Array.Empty<Column>();

                         var cols = columns.OfType<Column>();
                         if (cols.Any())
                             return cols.ToArray();

                         var (columnNames, columnsEnabled) = NewMethod2(columns, columnName, enabled);

                         if (columnNames != null)
                             return BuildColumns(columnNames, columnsEnabled).ToArray();
                         else
                             throw new Exception("dsfooooooooosd");

                     }).WhereNotNull();

                static IEnumerable<Column> BuildColumns(IReadOnlyCollection<string> columnNames, IReadOnlyCollection<bool>? isEnabledes)
                {
                    isEnabledes ??= Enumerable.Repeat(true, columnNames.Count).ToArray();
                    foreach (var (name, isEnabled) in columnNames.Zip(isEnabledes))
                    {
                        var col = new Column(name, null, isEnabled);
                        yield return col;
                    }
                }

                static (IReadOnlyCollection<string>? names, IReadOnlyCollection<bool>? enabled) NewMethod2(IEnumerable columns, string? columnName, string? enabled)
                {
                    IReadOnlyCollection<string>? columnNames = null;
                    if (columnName != null)
                    {
                        columnNames = columns.Cast<object>().Select(a => (string)(a.TryGetValue(columnName) ?? throw new Exception("g__+++44444"))).ToArray(); ;
                    }
                    else if (columns is not IEnumerable<string> coll)
                        return default;
                    else
                        columnNames = coll.ToArray();

                    IReadOnlyCollection<bool>? columnsEnabled = default;
                    if (enabled != null)
                    {
                        columnsEnabled = columns.Cast<object>().Select(a => (bool)(a.TryGetValue(enabled) ?? throw new Exception("g44568844444"))).ToArray();
                    }
                    return (columnNames, columnsEnabled);
                }

            }
        }

        private CompositeDisposable Update(IEnumerable<Column> columns, CompositeDisposable? disposable)
        {
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

            return disposable;

            void SetColumnSelections(IReadOnlyCollection<Column> columns)
            {
                //if (columns.Count(a => a.X) > 1)
                //    return;
                //if (columns.SingleOrDefault(a => a.X) is not { } singleX)
                //    return;
                //var cs = new ColumnSelections(a.Where(c => c.Y).Select(column => new SeriesPair(singleX.Name, column.Name)).ToArray());
                //ColumnSelections = cs.Collection;
                this.RaiseEvent(new ColumnsSelectionsChangedEventArgs(ColumnsSelectionChangedEvent, this, columns));
            }
        }

        #region properties
        //public IEnumerable ColumnSelections
        //{
        //    get { return (IEnumerable)GetValue(ColumnSelectionsProperty); }
        //    set { SetValue(ColumnSelectionsProperty, value); }
        //}

        public IEnumerable Columns
        {
            get { return (IEnumerable)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
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

    //public class Column : ReactiveObject, IEquatable<Column?>
    //{
    //    private bool x; private bool y;

    //    public Column(string name, string tableName, bool isEnabled = false)
    //    {
    //        Name = name;
    //        TableName = tableName;
    //        IsEnabled = isEnabled;
    //    }

    //    public string Name { get; }
    //    public string TableName { get; }
    //    public bool IsEnabled { get; }
    //    public bool X { get => x; set => this.RaiseAndSetIfChanged(ref x, value); }
    //    public bool Y { get => y; set => this.RaiseAndSetIfChanged(ref y, value); }

    //    public bool Equals(Column? obj)
    //    {
    //        return obj is Column column &&
    //               Name == column.Name;
    //        //&&
    //        //X == column.X &&
    //        //Y == column.Y;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return HashCode.Combine(Name, X, Y);
    //    }

    //    public static bool operator ==(Column? left, Column? right)
    //    {
    //        return EqualityComparer<Column>.Default.Equals(left, right);
    //    }

    //    public static bool operator !=(Column? left, Column? right)
    //    {
    //        return !(left == right);
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return Equals(obj as Column);
    //    }

    //}
}

//.DistinctUntilChanged(new ColumnComparer())
//      .CombineLatest(this
//                        .WhenAnyValue(a => a.ColumnSelections)
//                        .Select(a => a?.Cast<SeriesPair>())
//                        .DistinctUntilChanged(),
//      (a, b) =>
//      {
//          if (b == null)
//              return a;

//          var ewr = from c in b.Cast<SeriesPair>()
//                    join d in a
//                    on c.ColumnX equals d.Name
//                    select new Action(() => d.X = true);

//          var ewr2 = from c in b.Cast<SeriesPair>()
//                     join d in a
//                     on c.ColumnY equals d.Name
//                     select new Action(() => d.Y = true);

//          foreach (var c in ewr.Concat(ewr2))
//          {
//              c.Invoke();
//          }
//          return a;
//      })
//      .DistinctUntilChanged(new ColumnComparer())