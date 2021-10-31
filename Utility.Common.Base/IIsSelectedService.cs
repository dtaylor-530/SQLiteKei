using Utility.Common.Base;

namespace Utility.Service
{
    public interface IIsSelectedService
    {
        IObservable<IsSelected> Get(IKey key, IObservable<bool> isLoaded);
        void Set(IKey key, IsSelected pairs);
    }
}