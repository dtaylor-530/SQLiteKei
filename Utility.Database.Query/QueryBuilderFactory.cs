using Utility.Database;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// Allows to build SQL query strings.
    /// </summary>
    public abstract class QueryBuilderFactory
    {

        /// Begins a SELECT statement.
        /// Begins a SELECT statement.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public static SelectQueryBuilder BuildSelect(string? column = null)
        {
            return new SelectQueryBuilder(column);
        }

        /// <summary>
        /// Begins a SELECT statement. If no columns are defined, it defaults to a wildcard selection.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static SelectQueryBuilder BuildSelect(string column, string alias)
        {
            return new SelectQueryBuilder(column, alias);
        }

        /// <summary>
        /// Begins a CREATE statement for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static CreateQueryBuilder BuildCreate()
        {
            return new CreateQueryBuilder();
        }

        /// <summary>
        /// Begins a DROP statement for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static DropQueryBuilder BuildDrop(TableName tableName)
        {
            return new DropQueryBuilder(tableName);
        }
    }
}
