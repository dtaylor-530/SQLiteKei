using Utility.Common.Base;
using Utility.Entity;

namespace SQLite.Service.Mapping
{
    public interface ITreeViewMapper
    {
        IObservable<TreeItem> Map(Key map);
    }
}