using System;
using System.Collections.Generic;
using System.Data;
using Utility.Database;
using Utility.SQLite.Database;
using Utility.SQLite.Models;

namespace Utility.SQLite.Helpers
{

    public record struct TableInformation(int ColumnCount, string Name, long RowCount);

    public static class ConnectionHelper
    {
        public static IEnumerable<Column> Columns(this DataTable resultTable)
        {
            foreach (DataRow row in resultTable.Rows)
                yield return Map(row);
        }

        private static Column Map(DataRow row) => new Column
        {
            Id = Convert.ToInt32(row.ItemArray[0]),
            Name = (string)row.ItemArray[1],
            DataType = (string)row.ItemArray[2],
            IsNotNullable = Convert.ToBoolean(row.ItemArray[3]),
            DefaultValue = row.ItemArray[4],
            IsPrimary = Convert.ToBoolean(row.ItemArray[5])
        };

        public static IReadOnlyCollection<TableInformation> TablesInformation(DatabasePath FilePath)
        {
            using (var dbHandler = new DatabaseHandler(FilePath))
            {
                var tables = dbHandler.Tables;
                List<TableInformation> ti = new();

                foreach (var table in tables)
                {
                    using var tableHandler = new TableHandler(FilePath, table.Name);
                    var tableRowCount = tableHandler.RowCount;
                    var columns = tableHandler.Columns;
                    ti.Add(new(columns.Length, table.Name, tableRowCount));
                }
                return ti;
            }
        }

        //public static IReadOnlyCollection<TableInformation> TablesInformation(TableHandler dbHandler)
        //{
        //    List<TableInformation> list = new();

        //        foreach (var column in dbHandler.Columns(dbHandler.Name))
        //        {
        //        list.Add(Converter.MapToColumnData(column));
        //        }

        //}
    }
}
