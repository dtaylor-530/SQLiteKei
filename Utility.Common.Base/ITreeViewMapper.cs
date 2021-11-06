using Utility.Common.Base;
using Utility.Entity;

namespace SQLite.Service.Mapping
{
    public interface ITreeViewMapper
    {
        TreeItem Map(Key map);
    }
}