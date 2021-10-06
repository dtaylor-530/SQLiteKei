using log4net;

using SQLiteKei.Helpers;

using System.Collections.Generic;
using System;
using log = SQLiteKei.Helpers.Log;
using System.Windows.Input;
using ReactiveUI;

namespace SQLiteKei.ViewModels.PreferencesWindow
{
    /// <summary>
    /// The ViewModel for the preference window
    /// </summary>
    public class PreferencesViewModel
    {
        //private readonly ILog log = Log.GetLogger();
        private string selectedLanguage;
        private string selectedTheme;
        private readonly ICommand applySettingsCommand;


        public PreferencesViewModel()
        {
            AvailableLanguages = new List<string>
            {
                LocalisationHelper.GetString("Preferences_Language_German"),
                LocalisationHelper.GetString("Preferences_Language_English")
            };

            AvailableThemes = new List<string>
            {
                "Dark", "Light"
            };

            applySettingsCommand = ReactiveCommand.Create(ApplySettings);
        }

        public List<string> AvailableLanguages { get; set; }

        public string SelectedTheme
        {
            get
            {
                if (string.IsNullOrEmpty(selectedTheme))
                    selectedTheme = SQLite.WPF.Settings.Default.UITheme;

                return selectedTheme;
            }
            set { selectedTheme = value; }
        }

        public List<string> AvailableThemes { get; set; }

        public ICommand ApplySettingsCommand => applySettingsCommand;

        public string SelectedLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(selectedLanguage))
                    selectedLanguage = GetLanguageFromSettings();

                return selectedLanguage;
            }
            set { selectedLanguage = value; }
        }

        private string GetLanguageFromSettings()
        {
            var setting = SQLite.WPF.Settings.Default.UILanguage;

            switch(setting)
            {
                case "de-DE":
                    return LocalisationHelper.GetString("Preferences_Language_German");
                case "en-GB":
                default:
                    return LocalisationHelper.GetString("Preferences_Language_English");
            };
        }



        private void ApplySettings()
        {
            ApplyLanguage();
            ApplyApplicationTheme();

            SQLite.WPF.Settings.Default.Save();
            SQLite.WPF.Settings.Default.Reload();
        }

        private void ApplyLanguage()
        {
            if (selectedLanguage.Equals(GetLanguageFromSettings())) return;

            if (selectedLanguage.Equals(LocalisationHelper.GetString("Preferences_Language_German")))
            {
                SQLite.WPF.Settings.Default.UILanguage = "de-DE";
            }
            else
            {
                SQLite.WPF.Settings.Default.UILanguage = "en-GB";
            }

            log.Logger.Info("Applied application language " + SQLite.WPF.Settings.Default.UILanguage);
        }

        private void ApplyApplicationTheme()
        {
            if (selectedTheme.Equals(SQLite.WPF.Settings.Default.UITheme)) return;

            log.Logger.Info("Applying '" + selectedTheme + "' application theme.");

            try
            {
                ThemeHelper.LoadTheme(selectedTheme);
                SQLite.WPF.Settings.Default.UITheme = selectedTheme;
            }
            catch(Exception ex)
            {
                log.Logger.Error("Could not apply application theme '" + selectedTheme + "'.", ex);
            }
        }
    }
}
