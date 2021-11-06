using Database.Entity;
using SQLite.Utility.Factory;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.DataAccess.QueryBuilders.Base;
using SQLiteKei.DataAccess.QueryBuilders.Where;
using Utility.Common.Base;
using Utility.Database.Common;

namespace Database.Service.Service
{
    /// <summary>
    /// The main ViewModel for the GenerateSelectQuery window
    /// </summary>
    public class SelectQueryService : ISelectQueryService
    {
        private readonly IMap map;
        private readonly IHandlerService handlerService;

        public SelectQueryService(IMap map, IHandlerService handlerService)
        {
            this.map = map;
            this.handlerService = handlerService;
        }

        public SelectItem[] ToSelectItems(ITableKey tableKey)
        {
            return handlerService.Table(tableKey, tableHandler =>
            {
                var columns = tableHandler.Columns;
                var selects = map.Map<SelectItem[]>(columns);
                return selects;
            });

        }

        public OrderItem ToOrderItem(ITableKey tableKey)
        {

            return handlerService.Table(tableKey, tableHandler =>
            {
                var orderItem = new OrderItem();
                var columns = tableHandler.Columns;
                foreach (var column in columns)
                {
                    orderItem.Columns.Add(column.Name);
                }

                return orderItem;
            });
        }

        public string SelectQuery(ITableKey tableKey, IReadOnlyCollection<SelectItem> selects, IReadOnlyCollection<OrderItem> orders)
        {
            var selectQuery = Helper.SelectQuery(selects, orders).From(tableKey.TableName).Build();
            return selectQuery;
        }

        class Helper
        {
            public static SelectQueryBuilder SelectQuery(IReadOnlyCollection<SelectItem> selects, IReadOnlyCollection<OrderItem> orders)
            {
                var selectQueryBuilder = new SelectQueryBuilder();

                var canBeWildcard = DetermineIfSelectCanBeWildcard(selects);

                if (canBeWildcard)
                {
                    selectQueryBuilder.AddSelect("*");
                }
                else
                {
                    foreach (var select in selects)
                    {
                        if (select.IsSelected)
                        {
                            if (string.IsNullOrWhiteSpace(select.Alias))
                                selectQueryBuilder.AddSelect(select.ColumnName);
                            else
                                selectQueryBuilder.AddSelect(select.ColumnName, select.Alias);
                        }
                    }
                }

                AddWhereClauses();
                AddOrderClauses(selectQueryBuilder, orders);

                return selectQueryBuilder;

                void AddWhereClauses()
                {
                    foreach (var select in selects)
                    {
                        if (!string.IsNullOrWhiteSpace(select.CompareValue))
                        {
                            WhereClause where;

                            if (selectQueryBuilder.WhereClauses.Any())
                                where = selectQueryBuilder.And(select.ColumnName);
                            else
                                where = selectQueryBuilder.Where(select.ColumnName);

                            selectQueryBuilder = AddClause(where, select.CompareType, select.CompareValue) as SelectQueryBuilder;
                        }
                    }

                    static ConditionalQueryBuilder AddClause(WhereClause clause, string compareType, string compareValue)
                    {
                        return compareType switch
                        {
                            "=" => clause.Is(compareValue),
                            ">" => clause.IsGreaterThan(compareValue),
                            ">=" => clause.IsGreaterThanOrEqual(compareValue),
                            "&lt;" => clause.IsLessThan(compareValue),
                            "&lt;=" => clause.IsLessThanOrEqual(compareValue),
                            "Like" => clause.IsLike(compareValue),
                            "Contains" => clause.Contains(compareValue),
                            "Begins with" => clause.BeginsWith(compareValue),
                            "Ends with" => clause.EndsWith(compareValue),
                            _ => throw new NotImplementedException(),
                        };
                    }
                }

                static void AddOrderClauses(SelectQueryBuilder selectQueryBuilder, IReadOnlyCollection<OrderItem> orders)
                {
                    foreach (var clause in from clause in orders
                                           where !string.IsNullOrEmpty(clause.SelectedColumn)
                                           select clause)
                    {
                        selectQueryBuilder.OrderBy(clause.SelectedColumn, clause.IsDescending);
                    }
                }

                /// <summary>
                /// Determines if select statement can be wildcard. This is the case when all columns are selected and no aliases are defined.
                /// </summary>
                static bool DetermineIfSelectCanBeWildcard(IReadOnlyCollection<SelectItem> selects)
                {
                    var hasUnselectedColumns = selects.Any(c => !c.IsSelected);
                    var hasAliases = false;

                    if (!hasUnselectedColumns)
                        hasAliases = selects.Any(c => !string.IsNullOrWhiteSpace(c.Alias));

                    return !hasUnselectedColumns && !hasAliases;
                }
            }
        }
    }
}
