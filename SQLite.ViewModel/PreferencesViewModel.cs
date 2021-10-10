using System.Windows.Input;
using ReactiveUI;
using SQLite.Common.Contracts;
using static SQLite.Common.Log;


namespace SQLite.ViewModel
{
    public class PreferencesViewModel
    {
        //private readonly ILog log = Log.GetLogger();
        private string selectedLanguage;
        private string selectedTheme;
        private readonly ICommand applySettingsCommand;
        private readonly IThemeService themeService;
        private readonly ILocaliser localiser;

        public PreferencesViewModel(IThemeService themeService, ILocaliser localiser)
        {

            applySettingsCommand = ReactiveCommand.Create(ApplySettings);
            this.themeService = themeService;
            this.localiser = localiser;
        }

        public string[] AvailableLanguages => localiser.AvailableLanguages;

        public string SelectedTheme
        {
            get => selectedTheme ??= themeService.Theme;
            set => selectedTheme = value;
        }

        public string[] AvailableThemes => themeService.Themes;

        public ICommand ApplySettingsCommand => applySettingsCommand;

        public string SelectedLanguage
        {
            get => selectedLanguage ??= localiser.Language;
            set => selectedLanguage = value;
        }

        //private string GetLanguageFromSettings()
        //{
        //    var setting = SQLite.Data.Settings.Default.UILanguage;

        //    switch(setting)
        //    {
        //        case "de-DE":
        //            return Instance["Preferences_Language_German"];
        //        case "en-GB":
        //        default:
        //            return Instance["Preferences_Language_English"];
        //    };
        //}



        private void ApplySettings()
        {
            ApplyLanguage();
            ApplyApplicationTheme();

            //SQLite.Data.Settings.Default.Save();
            //SQLite.Data.Settings.Default.Reload();
        }

        private void ApplyLanguage()
        {
            throw new NotImplementedException();
            //if (selectedLanguage.Equals(GetLanguageFromSettings())) return;

            //if (selectedLanguage.Equals(Instance["Preferences_Language_German"]))
            //{
            //    SQLite.Data.Settings.Default.UILanguage = "de-DE";
            //}
            //else
            //{
            //    SQLite.Data.Settings.Default.UILanguage = "en-GB";
            //}

            //Info("Applied application language " + SQLite.Data.Settings.Default.UILanguage);
        }

        private void ApplyApplicationTheme()
        {
            throw new NotImplementedException();
            //if (selectedTheme.Equals(SQLite.Data.Settings.Default.UITheme)) return;

            //Info("Applying '" + selectedTheme + "' application theme.");

            //try
            //{
            //    themeService.ApplyTheme(selectedTheme);
            //    SQLite.Data.Settings.Default.UITheme = selectedTheme;
            //}
            //catch(Exception ex)
            //{
            //    Error("Could not apply application theme '" + selectedTheme + "'.", ex);
            //}
        }

        public string Title => localiser["WindowTitle_Preferences"];
        public string LanguageHeader => localiser["Preferences_GroupBoxHeader_Languages"];
        public string LanguageKey=> localiser["Preferences_Language"];
        public string LanaguageValue => localiser["Preferences_LanguageChangeInfo"];
        public string ThemeHeader => localiser["Preferences_GroupBoxHeader_Themes"];
        public string ThemeKey=> localiser["Preferences_ApplicationTheme"];
        public string ApplyKey=> localiser["ButtonText_Apply"];
        public string CancelKey=> localiser["ButtonText_Cancel"];
    }
}
