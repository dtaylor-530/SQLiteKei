using Database.Entity;
using Database.Service.Service;
using ReactiveUI;
using SQLite.ViewModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Utility.Common.Base;
using Utility.Database;

namespace Database.ViewModel
{
    public class SelectQueryViewModelKey : TableKey<ISelectQueryViewModel>
    {
        public SelectQueryViewModelKey(DatabasePath databaseName, TableName tableName) : base(databaseName, tableName)
        {
        }
    }

    public interface ISelectQueryViewModel : IDatabaseViewModel
    {
        string SelectQuery { get; }
    }

    /// <summary>
    /// The main ViewModel for the GenerateSelectQuery window
    /// </summary>
    public class SelectQueryViewModel : TableViewModel<ISelectQueryViewModel>

    {
        private readonly SelectQueryViewModelKey key;
        private readonly ISelectQueryService selectQueryService;
        private readonly ILocaliser localiser;
        //private readonly DatabasePath connectionPath;
        //private readonly string tableName;

        //private SelectQueryBuilder? selectQueryBuilder;
        private string selectQuery;

        public SelectQueryViewModel(SelectQueryViewModelKey key, ISelectQueryService selectQueryService, ILocaliser localiser) : base(key)
        {
            this.key = key;
            this.selectQueryService = selectQueryService;
            this.localiser = localiser;
            //connectionPath = key.DatabasePath;
            //tableName = key.TableName;
            Selects = new();
            selectQueryService.ToSelectItems(key)
                .Subscribe(a =>
                {
                    foreach (var b in a)
                        Selects.Add(b);
                });

            Initialize();
            AddOrderStatementCommand = ReactiveCommand.Create(() => selectQueryService.ToOrderItem(key));
        }

        public string CancelKey => localiser["ButtonText_Cancel"];
        public string ExecuteKey => localiser["ButtonText_Execute"];

        public string SelectQuery
        {
            get { return selectQuery; }
            set { this.RaiseAndSetIfChanged(ref selectQuery, value); }
        }

        public ObservableCollection<SelectItem> Selects { get; }

        public ObservableCollection<OrderItem> Orders { get; } = new ObservableCollection<OrderItem>();

        public ICommand AddOrderStatementCommand { get; }

        public override string Name => "Select Query";

        void UpdateSelectQuery()
        {
            SelectQuery = selectQueryService.SelectQuery(key, Selects, Orders);
        }

        private void Initialize()
        {
            Selects.CollectionChanged += (s, e) => CollectionContentChanged(e);
            Orders.CollectionChanged += (s, e) => UpdateSelectQuery();
            UpdateSelectQuery();

            //InitializeItems();
            //UpdateSelectQuery();

            //void InitializeItems()
            //{
            //    using (var tableHandler = new TableHandler(connectionPath, tableName))
            //    {
            //        var columns = tableHandler.DataTable.Columns();

            //        foreach (var column in columns)
            //        {
            //            Selects.Add(new SelectItem
            //            {
            //                ColumnName = column.Name,
            //                IsSelected = true
            //            });
            //        }
            //    }
            //}

            void CollectionContentChanged(NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Remove:
                        foreach (INotifyPropertyChanged item in e.OldItems!)
                        {
                            item.PropertyChanged -= (s, e) => UpdateSelectQuery();
                        }
                        break;
                    case NotifyCollectionChangedAction.Add:
                        foreach (INotifyPropertyChanged item in e.NewItems!)
                        {
                            item.PropertyChanged += (s, e) => UpdateSelectQuery();
                        }
                        break;

                    default:
                        throw new Exception("gd33__De");
                }
            }

            //void UpdateSelectQuery()
            //{
            //    selectQueryBuilder = new SelectQueryBuilder();

            //    var canBeWildcard = DetermineIfSelectCanBeWildcard();

            //    if (canBeWildcard)
            //    {
            //        selectQueryBuilder.AddSelect("*");
            //    }
            //    else
            //    {
            //        foreach (var select in Selects)
            //        {
            //            if (select.IsSelected)
            //            {
            //                if (string.IsNullOrWhiteSpace(select.Alias))
            //                    selectQueryBuilder.AddSelect(select.ColumnName);
            //                else
            //                    selectQueryBuilder.AddSelect(select.ColumnName, select.Alias);
            //            }
            //        }
            //    }

            //    AddWhereClauses();
            //    AddOrderClauses();
            //    SelectQuery = selectQueryBuilder.From(tableName).Build();

            //    void AddWhereClauses()
            //    {
            //        foreach (var select in Selects)
            //        {
            //            if (!string.IsNullOrWhiteSpace(select.CompareValue))
            //            {
            //                WhereClause where;

            //                if (selectQueryBuilder.WhereClauses.Any())
            //                    where = selectQueryBuilder.And(select.ColumnName);
            //                else
            //                    where = selectQueryBuilder.Where(select.ColumnName);

            //                selectQueryBuilder = AddClause(where, select.CompareType, select.CompareValue) as SelectQueryBuilder;
            //            }
            //        }

            //        static ConditionalQueryBuilder AddClause(WhereClause clause, string compareType, string compareValue)
            //        {
            //            return compareType switch
            //            {
            //                "=" => clause.Is(compareValue),
            //                ">" => clause.IsGreaterThan(compareValue),
            //                ">=" => clause.IsGreaterThanOrEqual(compareValue),
            //                "&lt;" => clause.IsLessThan(compareValue),
            //                "&lt;=" => clause.IsLessThanOrEqual(compareValue),
            //                "Like" => clause.IsLike(compareValue),
            //                "Contains" => clause.Contains(compareValue),
            //                "Begins with" => clause.BeginsWith(compareValue),
            //                "Ends with" => clause.EndsWith(compareValue),
            //                _ => throw new NotImplementedException(),
            //            };
            //        }
            //    }

            //    void AddOrderClauses()
            //    {
            //        foreach (var clause in from clause in Orders
            //                               where !string.IsNullOrEmpty(clause.SelectedColumn)
            //                               select clause)
            //        {
            //            selectQueryBuilder.OrderBy(clause.SelectedColumn, clause.IsDescending);
            //        }
            //    }
        }

        ///// <summary>
        ///// Determines if select statement can be wildcard. This is the case when all columns are selected and no aliases are defined.
        ///// </summary>
        //bool DetermineIfSelectCanBeWildcard()
        //{
        //    var hasUnselectedColumns = Selects.Any(c => !c.IsSelected);
        //    var hasAliases = false;

        //    if (!hasUnselectedColumns)
        //        hasAliases = Selects.Any(c => !string.IsNullOrWhiteSpace(c.Alias));

        //    return !hasUnselectedColumns && !hasAliases;
        //}
    }

    //void AddOrderStatement()
    //{

    //    using (TableHandler? tableHandler = new(connectionPath, tableName))
    //    {
    //        var orderItem = new OrderItem();
    //        var columns = tableHandler.DataTable.Columns();

    //        foreach (var column in columns)
    //        {
    //            orderItem.Columns.Add(column.Name);
    //        }

    //        Orders.Add(orderItem);
    //    }
    //}
}
