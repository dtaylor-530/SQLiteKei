using SQLite.Common.Model;
using SQLite.Service.Factory;
using Utility;
using Utility.Database;

namespace SQLite.Service.Service
{
    public class ColumnModelService : IColumnModelService
    {
        private Dictionary<IKey, IReadOnlyCollection<ColumnModel>> collection = new();
        private readonly ColumnDataFactory factory;

        public ColumnModelService(ColumnDataFactory factory)
        {
            this.factory = factory;
        }

        public IReadOnlyCollection<ColumnModel> GetCollection(ITableKey tableKey) =>
            collection[tableKey] = collection.GetValueOrDefault(tableKey) ?? factory.Create(tableKey);
    }

}
