namespace Utility.Common.Base;

public interface ITreeRepository
{
    void Save(IReadOnlyCollection<TreeItem> tree);
    IReadOnlyCollection<TreeItem> Load();
}
