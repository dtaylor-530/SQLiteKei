using DynamicData;
using SQLite.Service.Repository;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Utility;

namespace SQLite.Service.Service;

public struct IsSelected
{
    public IsSelected(bool value)
    {
        Value = value;
    }

    public bool Value { get; }

    public override string? ToString()
    {
        return nameof(IsSelected) + " " + Value.ToString();
    }
}

public class DefaultKey : Key { }

public class IsSelectedService
{
    private readonly IsSelectedRepository repository;
    private readonly Dictionary<Key, IsSelected> collection = new();
    private readonly Subject<(Key key, IsSelected s)> subject = new();
    private readonly Subject<(Key key, IsSelected s)> subject3 = new();
    private readonly Subject<(Key key, bool isloaded)> subject2 = new();
    private readonly ReplaySubject<(Key key, IsSelected s)> rsubject = new();
    Dictionary<Key, bool> isInitialised = new();

    public IsSelectedService(IsSelectedRepository repository)
    {
        this.repository = repository;

        var aa = subject
            .ToObservableChangeSet(a => a.key)
            .ToCollection()
            .CombineLatest(subject2.ToObservableChangeSet(a => a.key).ToCollection())
            .Select(a => a.First.Join(a.Second, a => a.key, a => a.key, (a, b) => a))
            .SelectMany(a => a);

        subject3
            .Concat(aa)
            .Select(a => a)
            .Subscribe(rsubject);

        rsubject.Subscribe(a =>
        {
            collection[a.key] = a.s;
            repository.Save(a.key, a.s);
        });
    }

    public IObservable<IsSelected> Get(Key key, IObservable<bool> isLoaded)
    {
        isLoaded.Where(a => a).Subscribe(a => { subject3.OnCompleted(); subject2.OnNext((key, a)); });

        var success = collection.TryGetValue(key, out var value);
        if (success == false)
        {
            value = repository.Load(key);
        }

        //collection[key] = value;

        subject3.OnNext((key, value));

        return rsubject.Where(a => a.key == key).Select(a => a.s);

    }

    public void Set(Key key, IsSelected pairs)
    {

        subject.OnNext((key, pairs));

    }
}
