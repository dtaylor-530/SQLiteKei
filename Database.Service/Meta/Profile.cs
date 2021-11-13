using Database.Entity;
using Database.Service.Mapping;
using Utility.Database.Common.Models;

namespace Database.Service.Meta
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {

            CreateMap<Column, SelectItem>().ConvertUsing(a => SelectItemMapping.Map(a));
            CreateMap<Column, ColumnModel>();
        }
    }
}
