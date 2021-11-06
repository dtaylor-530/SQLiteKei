using Database.Entity;
using Utility.Database.Common;

namespace Database.Service.Service
{
    public interface ISelectQueryService
    {
        string SelectQuery(ITableKey tableKey, IReadOnlyCollection<SelectItem> selects, IReadOnlyCollection<OrderItem> orders);
        IObservable<OrderItem> ToOrderItem(ITableKey tableKey);
        IObservable<SelectItem[]> ToSelectItems(ITableKey tableKey);
    }
}