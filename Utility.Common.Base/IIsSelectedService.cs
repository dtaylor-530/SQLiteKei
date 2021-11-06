using Utility.Common.Base;
using Utility.Entity;

namespace Utility.Service
{
    public interface IIsSelectedService
    {
        IObservable<IsSelected> Get(IKey key, IObservable<bool> isLoaded);
        void Set(IKey key, IsSelected pairs);
    }
}