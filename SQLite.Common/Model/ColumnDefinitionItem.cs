using ReactiveUI;
using SQLiteKei.DataAccess.QueryBuilders.Enums;

namespace SQLite.Common.Model
{

    public class ColumnDefinitionItem : ReactiveObject
    {
        private string columnName;
        private DataType dataType;
        private bool isPrimary;
        private bool isNotNull;
        private object defaultValue;

        public ColumnDefinitionItem()
        {
            columnName = "<Name>";
        }

        public string ColumnName
        {
            get { return columnName; }
            set { this.RaiseAndSetIfChanged(ref columnName, value); }
        }

        public bool IsPrimary
        {
            get { return isPrimary; }
            set
            {
                //isPrimary = value;
                if (value)
                    IsNotNull = true;
                this.RaiseAndSetIfChanged(ref isPrimary, value);
            }
        }

        public bool IsNotNull
        {
            get { return isNotNull; }
            set { this.RaiseAndSetIfChanged(ref isNotNull, value); }
        }

        public DataType DataType
        {
            get { return dataType; }
            set { this.RaiseAndSetIfChanged(ref dataType, value); }
        }

        public static IReadOnlyCollection<DataType> DataTypes => Enum.GetValues(typeof(DataType)).Cast<DataType>().ToArray();

        public object DefaultValue
        {
            get { return defaultValue; }
            set { this.RaiseAndSetIfChanged(ref defaultValue, value); }
        }

    }
}