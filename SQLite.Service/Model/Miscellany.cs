using Utility.Chart;
using Utility.Database;
using Utility.SQLite.Models;

namespace SQLite.Service.Model
{
    public record Row(double X, double Y);

    public record RowInt64(long X, long Y);

    public record TableSeriesPairs(TableName TableName, IReadOnlyCollection<SeriesPair> Collection);

    public interface ISeriesPairChanges : IObservable<TableSeriesPairs>
    {
    }

    public class ColumnModel : Column
    {
        public ColumnModel(Column columnData)
        {
            this.Name = columnData.Name;
            this.DataType = columnData.DataType;
            this.DefaultValue = columnData.DefaultValue;
            this.IsNotNullable = columnData.IsNotNullable;
            this.IsPrimary = columnData.IsPrimary;
        }

        public bool IsEnabled => new string[] { "Blob", "Char", "Varchar" }.Any(a => DataType.Equals(a, StringComparison.InvariantCultureIgnoreCase)) == false;

    }
}