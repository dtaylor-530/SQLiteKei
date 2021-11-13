namespace Utility.Entity;

public interface IType
{
    public Type Type { get; }
}

public interface IKey : IEquatable<IKey>, IType
{
}

public interface IKey<T> : IKey
{

}

public abstract class Key : IKey
{
    public Key(Type type)
    {
        Type = type;
    }

    public Type Type { get; }

    public virtual bool Equals(IKey? other)
    {
        return GetType().Equals(other?.GetType());
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Key);
    }

    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }
}
public class Key<T> : Key, IKey<T>
{
    public Key() : base(typeof(T))
    {
    }
}
