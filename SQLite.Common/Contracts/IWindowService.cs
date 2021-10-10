namespace SQLite.Common.Contracts
{

    public enum ResizeMode
    {
        //
        // Summary:
        //     A window cannot be resized. The Minimize and Maximize buttons are not displayed
        //     in the title bar.
        NoResize,
        //
        // Summary:
        //     A window can only be minimized and restored. The Minimize and Maximize buttons
        //     are both shown, but only the Minimize button is enabled.
        CanMinimize,
        //
        // Summary:
        //     A window can be resized. The Minimize and Maximize buttons are both shown and
        //     enabled.
        CanResize,
        //
        // Summary:
        //     A window can be resized. The Minimize and Maximize buttons are both shown and
        //     enabled. A resize grip appears in the bottom-right corner of the window.
        CanResizeWithGrip
    }
    public enum Show
    {
        Show,
        ShowDialog
    }

    public record WindowServiceConfiguration(string Title, object DataContext, ResizeMode ResizeMode, Show Show);

    public interface IWindowService
    {
        public bool? ShowWindow(WindowServiceConfiguration Configuration);

    }
}
