using SQLite.Common.Contracts;
using System;
using System.Windows;
using System.Windows.Controls;
using ResizeMode = System.Windows.ResizeMode;
using ResizeMode2 = SQLite.Common.Contracts.ResizeMode;

namespace Utility.WPF.Service;

public class WindowService : IWindowService
{
    public bool? ShowWindow(WindowServiceConfiguration Configuration)
    {
        var style = Application.Current.Resources["WindowStyle"] as Style;
        var window = new Window()
        {
            Style = style,
            ResizeMode = Map(Configuration.ResizeMode),
            Title = Configuration.Title,
            Content = new ContentControl
            {
                Content = Configuration.DataContext
            }
        };

        switch (Configuration.Show)
        {
            case Show.Show:
                window.Show();
                break;
            case Show.ShowDialog:
                return window.ShowDialog();
        }
        return null;
    }

    ResizeMode Map(ResizeMode2 mode)
    {
        switch (mode)
        {
            case ResizeMode2.CanMinimize:
                return ResizeMode.CanMinimize;
            case ResizeMode2.CanResize:
                return ResizeMode.CanResize;
            case ResizeMode2.CanResizeWithGrip:
                return ResizeMode.CanResizeWithGrip;
            case ResizeMode2.NoResize:
                return ResizeMode.NoResize;
            default:
                throw new Exception("d_((fg455555");
        }
    }
}
