﻿//using log4net;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.QueryBuilders;
using System.Data;
using System.Data.SQLite;
using Utility.Database;
using Utility.SQLite.Helpers;

namespace Utility.SQLite.Database;

public class TableHandler : DisposableDbHandler
{
    public TableHandler(ConnectionPath databasePath) : base(databasePath)
    {
    }

    /// <summary>
    /// Gets the column meta data for the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns></returns>
    public Column[] Columns(string tableName)
    {
        using var command = Connection.CreateCommand();
        command.CommandText = "PRAGMA table_info('" + tableName + "');";
        var resultTable = new DataTable();
        resultTable.Load(command.ExecuteReader());
        return resultTable.Columns().ToArray();
    }

    /// <summary>
    /// Gets the column meta data for the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns></returns>
    public DataTable DataTable(string tableName)
    {
        using (var command = Connection.CreateCommand())
        {
            command.CommandText = "PRAGMA table_info('" + tableName + "');";
            var resultTable = new DataTable();
            resultTable.Load(command.ExecuteReader());
            return resultTable;
        }
    }

    /// <summary>
    /// Gets the create statement for the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns></returns>
    /// <exception cref="TableNotFoundException">Could not find table: {tableName}</exception>
    public string CreateStatement(string tableName)
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

    /// <summary>
    /// Gets the row count for the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns></returns>
    public long RowCount(string tableName)
    {
        using (var command = Connection.CreateCommand())
        {
            command.CommandText = QueryBuilder
            .Select("count(*)")
            .From("'" + tableName + "'")
            .Build();

            return Convert.ToInt64(command.ExecuteScalar());
        }
    }

    /// <summary>
    /// Drops the specified table from the given database. Sends a plain command to the database without any further error handling.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    public void DropTable(string tableName)
    {
        using (var command = Connection.CreateCommand())
        {
            command.CommandText = QueryBuilder.Drop(tableName).Build();
            command.ExecuteNonQuery();
            //  Info("Dropped table '" + tableName + "'.");
        }
    }

    /// <summary>
    /// Renames the specified table.
    /// </summary>
    /// <param name="oldName">The old name.</param>
    /// <param name="newName">The new name.</param>
    public void RenameTable(string oldName, string newName)
    {
        using (var command = Connection.CreateCommand())
        {
            command.CommandText = "ALTER TABLE '" + oldName + "' RENAME TO '" + newName + "'";
            command.ExecuteNonQuery();
            // Info("Renamed table '" + oldName + "' to '" + newName + "'.");
        }
    }

    /// <summary>
    /// Deletes all rows from the specified table.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    public void EmptyTable(string tableName)
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
    public void ReindexTable(string tableName)
    {
        using (var command = Connection.CreateCommand())
        {
            command.CommandText = string.Format("REINDEX '{0}'", tableName);
            command.ExecuteNonQuery();
            // logger.Info("Reindexed table '" + tableName + "'.");
        }
    }
}
