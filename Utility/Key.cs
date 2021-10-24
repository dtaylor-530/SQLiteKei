namespace Utility;

public abstract class Key : IEquatable<Key>
{
    public virtual bool Equals(Key? other)
    {
        return this.GetType().Equals(other?.GetType());
    }


    public override bool Equals(object? obj)
    {
        return Equals(obj as Key);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

}
