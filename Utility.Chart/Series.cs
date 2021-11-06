using OxyPlot;
using System.Collections.Generic;

namespace Utility.Chart
{
    public class Series
    {
        public Series(string xName, string yName, IReadOnlyCollection<DataPoint> points)
        {
            XName = xName;
            YName = yName;
            Points = points;
        }

        public string XName { get; }
        public string YName { get; }
        public IReadOnlyCollection<DataPoint> Points { get; }
    }
}
