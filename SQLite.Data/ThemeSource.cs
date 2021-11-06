using Utility.Common.Base;

namespace Database.Data;

public class ThemeSource : IThemeSource
{
    private Theme? theme;

    public Theme Theme
    {
        get => theme ??= (Theme)Enum.Parse(typeof(Theme), Settings.Default.UITheme);
        set
        {
            theme = value;
            Settings.Default.UITheme = value.ToString();
            Settings.Default.Save();
        }
    }

    public Theme[] AvailableThemes => (Theme[])Enum.GetValues(typeof(Theme));
}
