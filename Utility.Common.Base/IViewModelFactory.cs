namespace Utility.Common.Base;

public interface IViewModelFactory
{
    T Build<T>(IKey<T> key) where T : IViewModel;

    //IViewModel Build<T>(IKey<T> configuration);
    IViewModel Build(IKey key);
}
