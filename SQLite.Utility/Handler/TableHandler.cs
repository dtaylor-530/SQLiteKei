//using log4net;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders;
using System.Data;
using System.Data.SQLite;
using Utility.Database;
using Utility.Database.Common.Models;
using Utility.Database.SQLite.Common.Abstract;

namespace Utility.SQLite.Database;

public class TableHandler : DisposableDbHandler, ITableHandler
{
    private string tableName;

    internal TableHandler(DatabasePath databasePath, string tableName) : base(databasePath)
    {
        this.tableName = tableName;
    }

    public string TableName => tableName;

    /// <summary>
    /// Gets the column meta data for the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns></returns>
    //public Column[] Columns
    //{
    //    get
    //    {
    //        using var command = Connection.CreateCommand();
    //        command.CommandText = "PRAGMA table_info('" + tableName + "');";
    //        var resultTable = new DataTable();
    //        resultTable.Load(command.ExecuteReader());
    //        return resultTable.Columns().ToArray();
    //    }
    //}

    /// <summary>
    /// Returns information about all tables in the current database.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyCollection<Column> Columns => ColumnMapping.Columns(this.DataTable, TableName).ToArray();

    public TableGeneralInformation General
    {
        get
        {
            var columns = Columns;
            return new TableGeneralInformation(CreateStatement, RowCount, columns, columns.Count);
        }
    }

    /// <summary>
    /// Gets the column meta data for the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns></returns>
    public DataTable DataTable
    {
        get
        {
            using (var command = Connection.CreateCommand())
            {
                command.CommandText = "PRAGMA table_info('" + tableName + "');";
                var resultTable = new DataTable();
                resultTable.Load(command.ExecuteReader());
                return resultTable;
            }
        }
    }

    /// <summary>
    /// Gets the create statement for the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns></returns>
    /// <exception cref="TableNotFoundException">Could not find table: {tableName}</exception>
    public string CreateStatement
    {
        get
        {
            var tables = Connection.GetSchema("Tables").AsEnumerable();

            foreach (var table in tables)
            {
                if (table.ItemArray[2].Equals(tableName))
                {
                    return table.ItemArray[6].ToString();
                }
            }

            //Error("Could not find table '" + tableName + "'");
            throw new TableNotFoundException(tableName);
        }
    }

    /// <summary>
    /// Gets the row count for the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns></returns>
    public long RowCount
    {
        get
        {
            using (var command = Connection.CreateCommand())
            {
                command.CommandText = QueryBuilderFactory
                .BuildSelect("count(*)")
                .From("'" + tableName + "'")
                .Build();
                return Convert.ToInt64(command.ExecuteScalar());
            }
        }
    }

    /// <summary>
    /// Drops the specified table from the given database. Sends a plain command to the database without any further error handling.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    public void DropTable()
    {
        using (var command = Connection.CreateCommand())
        {
            command.CommandText = QueryBuilderFactory.BuildDrop(new TableName(tableName)).Build();
            command.ExecuteNonQuery();
            //  Info("Dropped table '" + tableName + "'.");
        }
    }

    /// <summary>
    /// Renames the specified table.
    /// </summary>
    /// <param name="oldName">The old name.</param>
    /// <param name="newName">The new name.</param>
    public void RenameTable(string newName)
    {
        using (var command = Connection.CreateCommand())
        {
            command.CommandText = "ALTER TABLE '" + TableName + "' RENAME TO '" + newName + "'";
            command.ExecuteNonQuery();
            tableName = newName;
            // Info("Renamed table '" + oldName + "' to '" + newName + "'.");
        }
    }

    /// <summary>
    /// Deletes all rows from the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    public void EmptyTable()
    {
        using (var command = Connection.CreateCommand())
        {
            command.CommandText = string.Format("DELETE FROM {0}", tableName);

            try
            {
                command.ExecuteNonQuery();
                //   Info("Emptied table '" + tableName + "'.");
            }
            catch (SQLiteException ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    //    Info("Could not empty table '" + tableName + "'. No such table found.");
                    throw new TableNotFoundException(tableName);
                }
                throw;
            }
        }
    }

    /// <summary>
    /// Reindexes the specified table.
    /// </summary>
    /// <param name="tableName"></param>
    public void ReindexTable()
    {
        using (var command = Connection.CreateCommand())
        {
            command.CommandText = string.Format("REINDEX '{0}'", tableName);
            command.ExecuteNonQuery();
            // logger.Info("Reindexed table '" + tableName + "'.");
        }
    }

    static class ColumnMapping
    {
        public static IEnumerable<Column> Columns(DataTable resultTable, string tableName)
        {
            foreach (DataRow row in resultTable.Rows)
                yield return Map(row, tableName);

            static Column Map(DataRow row, string tableName) => new Column
            {
                Id = Convert.ToInt32(row.ItemArray[0]),
                Name = (string)row.ItemArray[1],
                DataType = (string)row.ItemArray[2],
                IsNotNullable = Convert.ToBoolean(row.ItemArray[3]),
                DefaultValue = row.ItemArray[4],
                IsPrimary = Convert.ToBoolean(row.ItemArray[5]),
                TableName = tableName
            };
        }
    }

}
