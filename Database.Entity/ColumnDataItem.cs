﻿namespace SQLite.Common.Model
{
    public class ColumnDataItem
    {
        public string Name { get; set; }

        public string DataType { get; set; }

        public bool IsNotNullable { get; set; }

        public object DefaultValue { get; set; }

        public bool IsPrimary { get; set; }
    }
}