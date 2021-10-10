using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Models;
using System.Data;
using Utility.Database;
using Utility.SQLite.Models;

namespace Utility.SQLite.Database
{
    /// <summary>
    /// A class used for calls to the given SQLite database.
    /// </summary>
    public class DatabaseHandler : DisposableDbHandler
    {
        public DatabaseHandler(ConnectionPath databasePath) : base(databasePath)
        {
        }

        /// <summary>
        /// Returns the name of the current database.
        /// </summary>
        /// <returns></returns>
        public string Name => Connection.Database;

        public ConnectionPath Path => ConnectionPath;

        /// <summary>
        /// Returns information about all tables in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> Tables => from row in GetSchema("Tables")
                                            select new Table
                                            {
                                                DatabaseName = row.ItemArray[0].ToString(),
                                                Name = row.ItemArray[2].ToString(),
                                                CreateStatement = row.ItemArray[6].ToString(),
                                            };

        /// <summary>
        /// Returns information about all views in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<View> Views => from row in GetSchema("Views")
                                          select new View
                                          {
                                              Name = row.ItemArray[2].ToString()
                                          };

        /// <summary>
        /// Returns information about all indexes in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Index> Indexes => from row in GetSchema("Indexes")
                                                    let indexName = row.ItemArray[5].ToString()
                                                    where !indexName.Contains("_PK_")
                                                    select new Models.Index
                                                    {
                                                        Name = row.ItemArray[5].ToString()
                                                    };

        /// <summary>
        /// Returns information about all triggers in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Trigger> Triggers => from row in GetSchema("Triggers")
                                                select new Trigger
                                                {
                                                    Name = row.ItemArray[2].ToString()
                                                };

        private IEnumerable<DataRow> GetSchema(string collectionName)
        {
            return Connection.GetSchema(collectionName).AsEnumerable();
        }

        /// <summary>
        /// Executes the specified sql and returns a DataTable with the results.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public DataTable ExecuteReader(string sql)
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
    }
}