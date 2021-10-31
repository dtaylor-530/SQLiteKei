using System.Windows.Input;

namespace Utility
{
    public class Seperator : MenuObject
    {

    }

    public class MenuObject
    {
    }

    public class MenuItem : MenuObject
    {
        public MenuItem(string header)
        {
            Header = header;

        }

        public string Header { get; }
        public ICommand? Command { get; init; }
        public IReadOnlyCollection<MenuObject> Collection { get; init; } = Array.Empty<MenuObject>();
    }
}
