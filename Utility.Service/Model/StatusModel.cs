using System.Reactive.Subjects;
using Utility.Common.Contracts;

namespace Utility.Service;

public class StatusModel : IStatusModel
{
    ReplaySubject<string> replay = new(1);

    public StatusModel()
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
        replay.OnNext(Value = value);
    }

    public string Value { get; private set; }

    public IDisposable Subscribe(IObserver<string> observer)
    {
        return replay.Subscribe(observer);
    }
}
