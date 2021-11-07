using Utility.Entity;

namespace Utility.Common.Contracts
{
    public interface IMainToolBarModel
    {
        IReadOnlyCollection<PanelObject> Collection { get; }
    }
}