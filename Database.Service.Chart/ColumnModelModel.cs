using Database.Service.Chart;
using SQLite.Common.Model;
using Utility.Database.Common;
using Utility.Entity;

namespace SQLite.Service.Service
{
    public class ColumnModelModel : IColumnModelService
    {
        private Dictionary<IKey, IReadOnlyCollection<ColumnModel>> collection = new();
        private readonly ColumnDataFactory factory;

        public ColumnModelModel(ColumnDataFactory factory)
        {
            this.factory = factory;
        }

        public IReadOnlyCollection<ColumnModel> GetCollection(ITableKey tableKey) =>
            collection[tableKey] = collection.GetValueOrDefault(tableKey) ?? factory.Create(tableKey);
    }

}
