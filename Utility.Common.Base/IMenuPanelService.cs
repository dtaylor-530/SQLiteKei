namespace Utility.Common.Contracts
{
    public interface IMenuPanelService
    {
        IReadOnlyCollection<PanelObject> Collection { get; }
    }
}