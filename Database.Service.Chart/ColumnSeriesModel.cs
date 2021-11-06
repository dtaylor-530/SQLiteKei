using DynamicData;
using SQLite.Service.Repository;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Utility.Chart;
using Utility.Entity;

namespace SQLite.Service.Service;

public class ColumnSeriesModel : IColumnSeriesService
{
    private readonly SeriesRepository repository;
    private Dictionary<IKey, List<Series>> collection = new();
    private Subject<IKey> subject = new();
    private Subject<(IKey key, IObserver<Unit> observer)> subject2 = new();

    public ColumnSeriesModel(SeriesRepository repository, ChartSeriesService chartSeriesService)
    {
        this.repository = repository;

        subject
            .ToObservableChangeSet(a => a).ToCollection()
            .CombineLatest(subject2.DistinctUntilChanged(a => a.key).ToObservableChangeSet(a => a.key).ToCollection())
            .Select(a =>
            {
                return from one in a.First
                       join two in a.Second
                       on one equals two.key
                       select two;
            })
            .Subscribe(a =>
            {
                foreach (var pair in a)
                {
                    pair.observer.OnNext(Unit.Default);
                }
            });

        chartSeriesService
              .Subscribe(a =>
              {
                  Set(a.Key, a.Collection.ToList());
                  //this.RaisePropertyChanged(nameof(Series));
              });
    }

    public IReadOnlyCollection<Series> Get(IKey key, IObserver<Unit> observer)
    {
        var aa = collection.GetValueOrDefault(key) ?? repository.Load(key);
        subject2.OnNext((key, observer));
        return collection[key] = aa.ToList();
    }

    void Set(IKey key, List<Series> value)
    {
        collection[key] = collection.GetValueOrDefault(key) ?? new List<Series>();
        collection[key].Clear();
        foreach (var item in value)
            collection[key].Add(item);
        //tableSeriesPairs.OnNext(new(key.TableName, collection[key]));
        repository.Save(key, value);
        subject.OnNext(key);

    }
}
