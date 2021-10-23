using SQLite.Service.Factory;
using SQLite.Service.Model;
using Utility.Database;

namespace SQLite.Service.Service
{
    public class ColumnModelService
    {
        private Dictionary<TableKey, IReadOnlyCollection<ColumnModel>> collection = new();
        private readonly ColumnDataFactory factory;

        public ColumnModelService(ColumnDataFactory factory)
        {
            this.factory = factory;
        }

        public IReadOnlyCollection<ColumnModel> GetCollection(TableKey tableKey) =>
            collection[tableKey] = collection.GetValueOrDefault(tableKey) ?? factory.Create(tableKey);
    }

}
