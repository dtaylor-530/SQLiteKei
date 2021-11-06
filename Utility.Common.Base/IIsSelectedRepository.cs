using Utility.Entity;

namespace Utility.Common.Base
{
    public interface IIsSelectedRepository : IRepository
    {
        IsSelected Load(IKey key);

        void Save(IKey key, IsSelected pairs);
    }
}