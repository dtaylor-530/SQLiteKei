using ReactiveUI;

namespace Utility.Chart.Entity
{
    public class Column : ReactiveObject, IEquatable<Column?>
    {
        private bool x; private bool y;

        public Column(string name, string tableName, bool isEnabled = false)
        {
            Name = name;
            TableName = tableName;
            IsEnabled = isEnabled;
        }

        public string Name { get; }
        public string TableName { get; }
        public bool IsEnabled { get; }
        public bool X { get => x; set => this.RaiseAndSetIfChanged(ref x, value); }
        public bool Y { get => y; set => this.RaiseAndSetIfChanged(ref y, value); }

        public bool Equals(Column? obj)
        {
            return obj is Column column &&
                   Name == column.Name;
            //&&
            //X == column.X &&
            //Y == column.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, X, Y);
        }

        public static bool operator ==(Column? left, Column? right)
        {
            return EqualityComparer<Column>.Default.Equals(left, right);
        }

        public static bool operator !=(Column? left, Column? right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Column);
        }

    }
}