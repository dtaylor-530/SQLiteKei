using System.Collections.ObjectModel;

namespace SQLite.Common.Contracts;

public interface ITreeRepository
{
    void Save(IReadOnlyCollection<TreeItem> tree);
    IReadOnlyCollection<TreeItem> Load();
}
