using Utility.Database;
using Utility.Database.SQLite.Common.Abstract;

namespace SQLite.Utility.Converter
{
    class TableMapping
    {
        //public static TableHandler TableHandler(Table table)
        //{
        //    //using (var dbHandler = new DatabaseHandler(FilePath))
        //    //{
        //    //var tables = this.Tables;

        //    var tableHandler = new TableHandler(table.DatabasePath, table.Name);
        //    return tableHandler;

        //    //}
        //}

        //public static IEnumerable<TableInformation> TablesInformation(IEnumerable<TableHandler> tables)
        //{
        //    foreach (var table in tables)
        //    {
        //        yield return TableInformation(table);
        //    }
        //}



        public static TableInformation TableInformation(ITableHandler table)
        {

            //using var tableHandler = new TableHandler(this.ConnectionPath, table.Name);
            var datatable = table.DataTable;
            return new(datatable.Columns.Count, table.TableName, datatable.Rows.Count);
            //}
        }
    }
}
