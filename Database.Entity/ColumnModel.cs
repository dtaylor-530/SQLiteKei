namespace Database.Entity
{
    //public record Row(double X, double Y);

    //public record RowInt64(long X, long Y);

    public class ColumnModel
    {

        public int Id { get; init; }

        public string Name { get; init; }

        public string DataType { get; init; }

        public bool IsNotNullable { get; init; }

        public object DefaultValue { get; init; }

        public bool IsPrimary { get; init; }

        public string TableName { get; init; }

        public bool IsEnabled => new string[] { "Bool", "Text", "Blob", "Char", "Varchar" }.Any(a => DataType.IndexOf(a, 0, StringComparison.InvariantCultureIgnoreCase) >= 0) == false;

    }
}
//        Blob,
//Bool,
//        Char,
//        DateTime,
//        Double,
//        Float,
//        Integer,
//        Numeric,
//        Real,
//        Text,
//        Varchar