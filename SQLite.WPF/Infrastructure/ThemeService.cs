using SQLite.Common.Contracts;
using SQLite.Data;
using SQLite.Views;
using System;
using System.Windows;
using System.Windows.Markup;
using static SQLite.Common.Log;

namespace SQLite.WPF.Infrastructure
{
    public class ThemeService : IThemeService
    {
        private readonly IThemeSource themeSource;

        public ThemeService(IThemeSource themeSource)
        {
            this.themeSource = themeSource;
        }

        public string Theme { get; }
        public string[] Themes { get; }

        /// <summary>
        /// Loads the user theme defined in the user settings.
        /// </summary>
        public void ApplyCurrentUserTheme()
        {
            ApplyTheme(themeSource.Theme.ToString());
        }

        /// <summary>
        /// Loads the theme.
        /// </summary>
        /// <param name="themeName">Name of the theme.</param>
        public void ApplyTheme(string themeName)
        {
            Info($"Loading '{themeName}' theme.");
            var assembly = typeof(About).Assembly;
            var themePath = $"{assembly.GetName().Name}.Resources.UIThemes.{themeName}.{themeName}.Embedded.xaml";

            using (var s = assembly.GetManifestResourceStream(themePath))
            {
                ResourceDictionary dic = (ResourceDictionary)XamlReader.Load(s);
                foreach (var dict in Application.Current.Resources.MergedDictionaries)
                {
                    if (dict.Source.ToString().Contains("Embedded"))
                        Application.Current.Resources.MergedDictionaries.Remove(dict);
                }

                Application.Current.Resources.MergedDictionaries.Add(dic);
            }
            themeSource.Theme = (Theme)Enum.Parse(typeof(Theme), themeName);
        }
    }
}
