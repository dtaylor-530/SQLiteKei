using Splat;
using Utility.Common.Base;

namespace Utility.Service
{
    public class ViewModelFactory : IViewModelFactory
    {
        public T Build<T>(IKey<T> key) where T : IViewModel
        {
            if (key != null)
                Locator.CurrentMutable.Register(() => key, key.GetType());
            else
                throw new Exception("99999££%$DFD");
            return Locator.Current.GetService<T>() ?? throw new Exception("££%$DFFDFDFD");
        }

        public IViewModel Build(IKey key)
        {
            if (key != null)
                Locator.CurrentMutable.Register(() => key, key.GetType());
            else
                throw new Exception("££%$DFD");
            return ((IViewModel?)Locator.Current.GetService(key.Type)) ?? throw new Exception("££%$__llddDFFDFDFD");

            //switch (key)
            //{
            //    case AboutViewModelKey:
            //        {
            //            return Build<IAboutViewModel>(key);
            //        }
            //    case PreferencesViewModelKey:
            //        {
            //            return Build<IPreferencesViewModel>(key);
            //        }
            //    default: throw new ArgumentOutOfRangeException("dfv879696");
            //}
        }

    }
}
