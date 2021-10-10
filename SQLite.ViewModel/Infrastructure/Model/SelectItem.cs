using ReactiveUI;

namespace SQLite.ViewModel
{
    /// <summary>
    /// A view model that is used to represent a build a user's select statement.
    /// </summary>
    public class SelectItem : ReactiveObject
    {
        private bool isSelected;
        private string compareType;
        private string compareValue;
        private string alias;


        public string ColumnName { get; set; }

        public string Alias
        {
            get { return alias; }
            set { this.RaiseAndSetIfChanged(ref alias, value); }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { this.RaiseAndSetIfChanged(ref isSelected, value); }
        }

        public string CompareType
        {
            get { return compareType; }
            set { this.RaiseAndSetIfChanged(ref compareType, value); }
        }

        public string CompareValue
        {
            get { return compareValue; }
            set { this.RaiseAndSetIfChanged(ref compareValue, value); }
        }
    }
}
