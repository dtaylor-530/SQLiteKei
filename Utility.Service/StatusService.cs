using System.Reactive.Subjects;
using Utility.Common.Contracts;

namespace Utility.Service;

public class StatusService : IStatusService
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
