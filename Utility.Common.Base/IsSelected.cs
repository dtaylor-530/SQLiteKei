namespace Utility.Common.Base
{
    public struct IsSelected
    {
        public IsSelected(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public override string? ToString()
        {
            return nameof(IsSelected) + " " + Value.ToString();
        }
    }

}
