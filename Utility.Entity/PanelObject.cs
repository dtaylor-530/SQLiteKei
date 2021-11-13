using System.Windows.Input;

namespace Utility.Entity
{
    public class ImageButton : PanelObject
    {
        public ImageButton(ICommand command, string toolTip, string source)
        {
            Command = command;
            ToolTip = toolTip;
            Source = source;
        }

        public ICommand Command { get; }
        public string ToolTip { get; }
        public string Source { get; }
    }

    public class PanelObject
    {

    }
    public class SeperatorItem : PanelObject
    {

    }

}
