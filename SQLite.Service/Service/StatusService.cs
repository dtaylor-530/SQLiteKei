using System.Reactive.Subjects;

namespace SQLite.Service.Service;

public class StatusService : IObservable<string>, IObserver<string>
{
    ReplaySubject<string> replay = new(1);

    public StatusService()
    {

    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(string value)
    {
        replay.OnNext(value);
    }

    public IDisposable Subscribe(IObserver<string> observer)
    {
        return replay.Subscribe(observer);
    }
}
