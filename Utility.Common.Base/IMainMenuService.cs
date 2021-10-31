namespace Utility.Common.Contracts
{
    public interface IMainMenuService
    {
        IReadOnlyCollection<MenuItem> Collection { get; }

    }
}
