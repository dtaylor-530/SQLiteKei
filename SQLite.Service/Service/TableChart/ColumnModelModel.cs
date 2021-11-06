using SQLite.Common.Model;
using SQLite.Service.Factory;
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
