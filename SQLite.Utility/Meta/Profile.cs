using SQLite.Utility.Converter;
using Utility.Database;
using Utility.Database.SQLite.Common.Abstract;
using Utility.SQLite.Database;

namespace SQLite.Utility.Meta;
public class Profile : AutoMapper.Profile
{
    public Profile()
    {
        //CreateMap<DatabasePath, ConnectionResult>().ConvertUsing(a => ConnectionFactory.CreateDatabase(a));
        //CreateMap<IDatabaseKey, IDatabaseHandler>().ConvertUsing(a => DatabaseHandlerFactory.Build(a));
        //CreateMap<ITableKey, ITableHandler>().ConvertUsing(a => TableHandlerFactory.Build(a));

        //CreateMap<Table, TableHan>().ConvertUsing(a => TableMapping.TableHandler(a));
        CreateMap<ITableHandler, TableInformation>().ConvertUsing(a => TableMapping.TableInformation(a));
        CreateMap<TableHandler, TableInformation>().ConvertUsing(a => TableMapping.TableInformation(a));
        //CreateMap<IEnumerable<ITableHandler>, IEnumerable<TableInformation>>().ConvertUsing(a => a.Select(v => TableMapping.TableInformation(v)));
        //CreateMap<ITableKey, ITableHandler>().ConvertUsing(a => TableHandlerFactory.Build(a));
    }
}
