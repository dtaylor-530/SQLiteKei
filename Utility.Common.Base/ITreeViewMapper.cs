using Utility;
using Utility.Common.Base;

namespace SQLite.Service.Mapping
{
    public interface ITreeViewMapper
    {
        TreeItem Map(IKey map);
    }
}