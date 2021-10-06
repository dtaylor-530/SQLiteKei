using log4net;
using SQLite.WPF;
using SQLiteKei.Views;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using log = SQLiteKei.Helpers.Log; 

namespace SQLiteKei.Helpers
{
    public class ThemeHelper
    {

        /// <summary>
        /// Loads the user theme defined in the user settings.
        /// </summary>
        public static void LoadCurrentUserTheme()
        {
            LoadTheme(SQLite.WPF.Settings.Default.UITheme);
        }

        /// <summary>
        /// Loads the theme.
        /// </summary>
        /// <param name="themeName">Name of the theme.</param>
        public static void LoadTheme(string themeName)
        {
            log.Logger.Info("Loading '" + themeName + "' theme.");
            var assembly = typeof(About).Assembly;
            var themePath = string.Format("{0}.Resources.Themes.{1}.{1}.xaml", assembly.GetName().Name, themeName);

            using (var s = assembly.GetManifestResourceStream(themePath))
            {
                ResourceDictionary dic = (ResourceDictionary)XamlReader.Load(s);

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dic);
            }
        }
    }
}
