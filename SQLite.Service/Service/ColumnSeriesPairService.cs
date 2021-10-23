using SQLite.Service.Model;
using SQLite.Service.Repository;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using Utility.Chart;
using Utility.Database;

namespace SQLite.Service.Service
{
    public class ColumnSeriesPairService : IObservable<TableSeriesPairs>
    {

        private readonly SeriesPairRepository repository;
        private Dictionary<TableKey, ObservableCollection<SeriesPair>> collection = new();
        readonly ReplaySubject<TableSeriesPairs> subject = new(1);

        public ColumnSeriesPairService(SeriesPairRepository repository)
        {
            this.repository = repository;
        }

        public IReadOnlyCollection<SeriesPair> Get(TableKey key)
        {
            var aa = collection.GetValueOrDefault(key) ?? new ObservableCollection<SeriesPair>(repository.Load(key));
            return collection[key] = aa;
        }

        public void Set(TableKey key, IReadOnlyCollection<SeriesPair> pairs)
        {
            collection[key] = collection.GetValueOrDefault(key) ?? new ObservableCollection<SeriesPair>();
            collection[key].Clear();
            foreach (var item in pairs)
                collection[key].Add(item);

            subject.OnNext(new(key.TableName, collection[key]));
            repository.Save(key, pairs.ToList());

        }

        public IDisposable Subscribe(IObserver<TableSeriesPairs> observer)
        {
            return subject.Subscribe(observer);
        }
    }

}
