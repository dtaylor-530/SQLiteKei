using Database.Service.Chart;
using SQLite.Common.Model;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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

        public IObservable<IReadOnlyCollection<ColumnModel>> GetCollection(ITableKey tableKey)
        {
            collection[tableKey] = collection.GetValueOrDefault(tableKey);
            if (collection[tableKey] == null)
            {
                Subject<IReadOnlyCollection<ColumnModel>> subject = new();

                factory
                    .Create(tableKey)
                    .Subscribe(a =>
                   {
                       collection[tableKey] = a;
                       subject.OnNext(a);
                   });

                return subject;
            }
            return Observable.Return(collection[tableKey]);
        }
    }

}
