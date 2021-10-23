using System.Collections.ObjectModel;

namespace SQLite.Common.Contracts;

public interface ITreeRepository
{
    void Save(IReadOnlyCollection<DatabaseTreeItem> tree);
    IReadOnlyCollection<DatabaseTreeItem> Load();
}
