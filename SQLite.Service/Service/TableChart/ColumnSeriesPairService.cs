using SQLite.Common.Contracts;
using SQLite.Service.Repository;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using Utility;
using Utility.Chart;
using Utility.Database;

namespace SQLite.Service.Service
{
    public class ColumnSeriesPairService : IColumnSeriesPairService
    {

        private readonly SeriesPairRepository repository;
        private Dictionary<IKey, ObservableCollection<SeriesPair>> collection = new();
        readonly ReplaySubject<TableSeriesPairs> subject = new(1);

        public ColumnSeriesPairService(SeriesPairRepository repository)
        {
            this.repository = repository;
        }

        public IReadOnlyCollection<SeriesPair> Get(ITableKey key)
        {
            var value = collection.GetValueOrDefault(key);
            if (value == default)
            {
                value = new ObservableCollection<SeriesPair>(repository.Load(key));
                subject.OnNext(new(key, value));
            }
            return collection[key] = value;
        }

        public void Set(ITableKey key, IReadOnlyCollection<SeriesPair> pairs)
        {
            collection[key] = collection.GetValueOrDefault(key) ?? new ObservableCollection<SeriesPair>();
            collection[key].Clear();
            foreach (var item in pairs)
                collection[key].Add(item);

            subject.OnNext(new(key, collection[key]));
            repository.Save(key, pairs.ToList());

        }

        public IDisposable Subscribe(IObserver<TableSeriesPairs> observer)
        {
            return subject.Subscribe(observer);
        }
    }

}
