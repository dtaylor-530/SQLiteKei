namespace Utility.SQLite.Models
{
    public class Column
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string DataType { get; init; }

        public bool IsNotNullable { get; init; }

        public object DefaultValue { get; init; }

        public bool IsPrimary { get; init; }
    }
}
