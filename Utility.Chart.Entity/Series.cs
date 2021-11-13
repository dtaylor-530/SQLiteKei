using OxyPlot;

namespace Utility.Chart.Entity
{
    public class Series
    {
        //public Series(string xName, string yName, string tableName, ObservablePoint[] points2)
        //{
        //    XName = xName;
        //    YName = yName;
        //    TableName = tableName;
        //    //Points2 = points2;
        //}

        public Series(string xName, string yName, string tableName, IReadOnlyCollection<DataPoint> points, bool isXDateTime = false, bool isYDateTime = false)
        {
            XName = xName;
            YName = yName;
            TableName = tableName;
            Points = points;
            IsXDateTime = isXDateTime;
            IsYDateTime = isYDateTime;
        }

        //public Series(string xName, string yName, string tableName, IReadOnlyCollection<DateTimePoint> points)
        //{
        //    XName = xName;
        //    YName = yName;
        //    TableName = tableName;
        //    Points3 = points;
        //}

        public string XName { get; }
        public string YName { get; }
        public string TableName { get; }
        //public IReadOnlyCollection<DateTimePoint> Points3 { get; }
        public IReadOnlyCollection<DataPoint> Points { get; }
        public bool IsXDateTime { get; }
        public bool IsYDateTime { get; }
        //public IReadOnlyCollection<ObservablePoint> Points2 { get; }
    }
}
