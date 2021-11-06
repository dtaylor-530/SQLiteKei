namespace Utility.Common.Base
{
    public interface IMap
    {

        TDestination Map<TSource, TDestination>(TSource value);

        TDestination Map<TDestination>(object value);
    }
}
