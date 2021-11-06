namespace Utility.Common.Base;

public interface ITreeRepository
{
    void Save(IReadOnlyCollection<TreeItem> tree);
    IObservable<IReadOnlyCollection<TreeItem>> Load();
}
