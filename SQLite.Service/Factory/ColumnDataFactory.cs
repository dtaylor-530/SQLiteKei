using SQLite.Service.Model;
using System.Linq;
using System.Reactive.Linq;
using Utility.Database;
using Utility.SQLite.Database;

namespace SQLite.Service.Factory
{
    public class ColumnDataFactory
    {
        public ColumnModel[] Create(TableKey configuration)
        {
            using (var dbHandler = new TableHandler(configuration.DatabasePath, configuration.TableName))
            {
                return dbHandler.Columns.Select(a => new ColumnModel(a)).ToArray();
            }
        }
    }

}
