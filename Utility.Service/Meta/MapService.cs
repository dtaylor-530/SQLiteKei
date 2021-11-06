using AutoMapper;
using Utility.Common.Base;

namespace Utility.Service
{
    public class MapService : IMap
    {
        private readonly System.Lazy<IMapper> mapper;

        public MapService(IEnumerable<Profile> profiles)
        {
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }

            });

            mapper = new System.Lazy<IMapper>(() => config.CreateMapper());
        }

        public TDestination Map<TSource, TDestination>(TSource value)
        {
            try
            {
                return mapper.Value.Map<TDestination>(value);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public TDestination Map<TDestination>(object value)
        {
            try
            {
                return mapper.Value.Map<TDestination>(value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
