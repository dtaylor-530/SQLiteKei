using Utility;
using Utility.Common.Base;

namespace SQLite.Service.Repository
{

    public interface IIsSelectedRepository
    {
        IsSelected Load(IKey key);
        void PersistAll();
        void Save(IKey key, IsSelected pairs);
    }
}