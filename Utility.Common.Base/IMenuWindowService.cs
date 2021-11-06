namespace Utility.Common.Contracts
{

    public enum WindowType
    {
        About, Preferences
    }

    public record struct WindowRequest(WindowType WindowType);

    public interface IWindowConverter : IObserver<WindowRequest> { }

    public interface IMenuWindowService
    {
        void OpenAbout();
        void OpenPreferences();
    }
}
