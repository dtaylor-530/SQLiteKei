using Splat;

namespace SQLite.ViewModel.Infrastructure.Factory
{
    public class ViewModelFactory
    {

        public T Build<T>(object? configuration = null)
        {
            if (configuration != null)
                Locator.CurrentMutable.Register(() => configuration, configuration.GetType());
            return Locator.Current.GetService<T>() ?? throw new Exception("££%$DFFDFDFD");
        }

    }
}
