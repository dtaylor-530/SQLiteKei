using DynamicData;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Utility.Common.Base;

namespace Utility.Service;

public class DefaultKey : Key { public DefaultKey() : base(null) { } }

public class IsSelectedService : IIsSelectedService
{
    private readonly IsSelectedRepository repository;
    private readonly Dictionary<IKey, IsSelected> collection = new();
    private readonly Subject<(IKey key, IsSelected s)> subject = new();
    private readonly Subject<(IKey key, IsSelected s)> subject3 = new();
    private readonly Subject<(IKey key, bool isloaded)> subject2 = new();
    private readonly ReplaySubject<(IKey key, IsSelected s)> rsubject = new();
    //Dictionary<Key, bool> isInitialised = new();

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

    public IObservable<IsSelected> Get(IKey key, IObservable<bool> isLoaded)
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

    public void Set(IKey key, IsSelected pairs)
    {

        subject.OnNext((key, pairs));

    }
}
