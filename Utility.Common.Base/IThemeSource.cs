namespace Utility.Common.Base
{
    public enum Theme
    {
        Dark, Light
    }

    public interface IThemeSource
    {
        Theme Theme { get; set; }
        Theme[] AvailableThemes { get; }
    }
}