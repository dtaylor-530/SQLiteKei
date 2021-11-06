using Database.Entity;
using Utility.Database.Common;

namespace Database.Service.Service
{
    public interface ISelectQueryService
    {
        string SelectQuery(ITableKey tableKey, IReadOnlyCollection<SelectItem> selects, IReadOnlyCollection<OrderItem> orders);
        OrderItem ToOrderItem(ITableKey tableKey);
        SelectItem[] ToSelectItems(ITableKey tableKey);
    }
}