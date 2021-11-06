using System.Diagnostics;

namespace Utility.ViewModel
{
    public class WebNavigationService : IWebNavigationService
    {
        public void Navigate(Uri uri)
        {
            Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        }
    }

}
