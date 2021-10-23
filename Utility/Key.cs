namespace Utility;

public abstract class Key : IEquatable<Key>
{
    public abstract bool Equals(Key? other);

    public override bool Equals(object? obj)
    {
        return Equals(obj as Key);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

}
