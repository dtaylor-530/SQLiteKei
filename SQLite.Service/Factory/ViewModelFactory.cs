using Splat;
using Utility;

namespace SQLite.ViewModel.Infrastructure.Factory
{

    public interface IConfiguration { }

    public class ViewModelFactory
    {

        public T Build<T>(Key configuration)
        {
            if (configuration != null)
                Locator.CurrentMutable.Register(() => configuration, configuration.GetType());
            return Locator.Current.GetService<T>() ?? throw new Exception("££%$DFFDFDFD");
        }

    }
}
