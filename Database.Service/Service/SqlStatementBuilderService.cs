using SQLiteKei.DataAccess.QueryBuilders;

namespace Database.Service.Service
{

    public class SqlStatementBuilderService : ISqlStatementBuilderService
    {
        public SqlStatementOutput Create(SqlStatementRoot sqlStatementRoot)
        {
            //StatusInfo = string.Empty;
            try
            {
                var builder = QueryBuilderFactory.BuildCreate();

                foreach (var definition in sqlStatementRoot.ColumnDefinitions)
                {
                    builder.AddColumn(definition.ColumnName,
                        definition.DataType,
                        definition.IsPrimary,
                        definition.IsNotNull,
                        definition.DefaultValue);
                }

                foreach (var foreignKey in sqlStatementRoot.ForeignKeyDefinitions)
                {
                    if (!string.IsNullOrWhiteSpace(foreignKey.SelectedColumn)
                       && foreignKey.SelectedTable != null
                       && foreignKey.SelectedReferencedColumn != null)
                    {
                        builder.AddForeignKey(foreignKey.SelectedColumn, foreignKey.SelectedTable, foreignKey.SelectedReferencedColumn);
                    }
                }

                var sqlStatement = builder.Build(sqlStatementRoot.TableName);
                return new SqlStatementOutput(sqlStatement, null);
                //IsValidTableDefinition = true;
            }
            catch (Exception ex)
            {
                return new SqlStatementOutput(null, ex);
                //StatusInfo = ex.Message;
                //IsValidTableDefinition = false;
            }
        }
    }
}
