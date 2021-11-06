using Dapper;
using SQLiteKei.DataAccess.Database;
using System.Data;
using Utility.Database;
using Utility.Database.SQLite.Common.Models;
using Utility.SQLite.Models;

namespace Utility.SQLite.Database
{
    /// <summary>
    /// A class used for calls to the given SQLite database.
    /// </summary>
    public class DatabaseHandler : DisposableDbHandler, IDatabaseHandler
    {
        internal DatabaseHandler(DatabasePath databasePath) : base(databasePath)
        {
        }

        /// <summary>
        /// Returns the name of the current database.
        /// </summary>
        /// <returns></returns>
        public string Name => Connection.Database;

        public DatabasePath Path => ConnectionPath;

        ///// <summary>
        ///// Returns information about all tables in the current database.
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Table> Tables => from row in SchemaRows("Tables")
        //                                    select new Table
        //                                    {
        //                                        DatabasePath = new DatabasePath(row.ItemArray[0].ToString()),
        //                                        Name = new TableName(row.ItemArray[2].ToString()),
        //                                        CreateStatement = row.ItemArray[6].ToString(),
        //                                    };

        ///// <summary>
        ///// Returns information about all views in the current database.
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<View> Views => from row in SchemaRows("Views")
        //                                  select new View
        //                                  {
        //                                      Name = new TableName(row.ItemArray[2].ToString())
        //                                  };

        ///// <summary>
        ///// Returns information about all indexes in the current database.
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Models.Index> Indexes => from row in SchemaRows("Indexes")
        //                                            let indexName = row.ItemArray[5].ToString()
        //                                            where !indexName.Contains("_PK_")
        //                                            select new Models.Index
        //                                            {
        //                                                Name = new TableName(row.ItemArray[5].ToString())
        //                                            };

        ///// <summary>
        ///// Returns information about all triggers in the current database.
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Trigger> Triggers => from row in SchemaRows("Triggers")
        //                                        select new Trigger
        //                                        {
        //                                            Name = new TableName(row.ItemArray[2].ToString())
        //                                        };

        /// <summary>
        /// Returns information about all tables in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> Tables => Helper.Tables(SchemaRows("Tables"));
        /// <summary>
        /// Returns information about all views in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<View> Views => Helper.Views(SchemaRows("Views"));

        /// <summary>
        /// Returns information about all indexes in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Index> Indexes => Helper.Indexes(SchemaRows("Indexes"));

        /// <summary>
        /// Returns information about all triggers in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Trigger> Triggers => Helper.Triggers(SchemaRows("Triggers"));

        public IEnumerable<DataRow> SchemaRows(string collectionName)
        {
            return Connection.GetSchema(collectionName).AsEnumerable();
        }

        /// <summary>
        /// Executes the specified sql and returns a DataTable with the results.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IReadOnlyCollection<dynamic> ExecuteDynamicQuery(string sql)
        {
            var rows = Connection.Query(sql).AsList();
            return rows;
        }

        /// <summary>
        /// Executes the specified sql and returns a DataTable with the results.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IReadOnlyCollection<T> ExecuteQuery<T>(string sql)
        {
            var rows = Connection.Query<T>(sql).AsList();
            return rows;
        }

        /// <summary>
        /// Executes the specified sql
        /// </summary>
        public DataTable ExecuteAndLoadDataTable(string sql)
        {
            using (var command = Connection.CreateCommand())
            {
                command.CommandText = sql;
                var resultTable = new DataTable();
                resultTable.Load(command.ExecuteReader());

                return resultTable;
            }
        }

        /// <summary>
        /// Executes the given SQL as a non query and returns the number of rows affected.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            using (var command = Connection.CreateCommand())
            {
                command.CommandText = sql;
                var result = command.ExecuteNonQuery();
                return result;
            }
        }

        class Helper
        {

            /// <summary>
            /// Returns information about all tables in the current database.
            /// </summary>
            /// <returns></returns>
            public static IEnumerable<Table> Tables(IEnumerable<DataRow> schemaRows) => from row in schemaRows
                                                                                        select new Table
                                                                                        {
                                                                                            DatabasePath = new DatabasePath(row.ItemArray[0].ToString()),
                                                                                            Name = new TableName(row.ItemArray[2].ToString()),
                                                                                            CreateStatement = row.ItemArray[6].ToString(),
                                                                                        };

            /// <summary>
            /// Returns information about all views in the current database.
            /// </summary>
            /// <returns></returns>
            public static IEnumerable<View> Views(IEnumerable<DataRow> schemaRows) => from row in schemaRows
                                                                                      select new View
                                                                                      {
                                                                                          Name = new TableName(row.ItemArray[2].ToString())
                                                                                      };

            /// <summary>
            /// Returns information about all indexes in the current database.
            /// </summary>
            /// <returns></returns>
            public static IEnumerable<Models.Index> Indexes(IEnumerable<DataRow> schemaRows) => from row in schemaRows
                                                                                                let indexName = row.ItemArray[5].ToString()
                                                                                                where !indexName.Contains("_PK_")
                                                                                                select new Models.Index
                                                                                                {
                                                                                                    Name = new TableName(row.ItemArray[5].ToString())
                                                                                                };

            /// <summary>
            /// Returns information about all triggers in the current database.
            /// </summary>
            /// <returns></returns>
            public static IEnumerable<Trigger> Triggers(IEnumerable<DataRow> schemaRows) => from row in schemaRows
                                                                                            select new Trigger
                                                                                            {
                                                                                                Name = new TableName(row.ItemArray[2].ToString())
                                                                                            };
        }

    }
}