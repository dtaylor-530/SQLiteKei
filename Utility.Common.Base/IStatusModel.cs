namespace Utility.Common.Contracts
{
    public interface IStatusModel : IObserver<string>, IObservable<string>
    {
        string Value { get; }
    }
}