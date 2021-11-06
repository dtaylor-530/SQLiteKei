using Database.Entity;
using Utility.Database;

namespace Database.Service.Service
{

    public record struct SqlStatementRoot(TableName TableName, IReadOnlyCollection<ColumnDefinitionItem> ColumnDefinitions, IReadOnlyCollection<ForeignKeyDefinitionItem> ForeignKeyDefinitions);

    public record struct SqlStatementOutput(string? Statement, Exception? Failure);

    public interface ISqlStatementBuilderService
    {
        SqlStatementOutput Create(SqlStatementRoot sqlStatementRoot);
    }
}