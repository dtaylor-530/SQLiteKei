using Utility.Common.Base;

namespace SQLite.Common.Contracts;

public interface ITreeRepository
{
    void Save(IReadOnlyCollection<TreeItem> tree);
    IReadOnlyCollection<TreeItem> Load();
}
