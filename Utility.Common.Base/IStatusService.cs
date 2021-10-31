namespace Utility.Common.Contracts
{
    public interface IStatusService : IObservable<string>, IObserver<string>
    {
    }
}