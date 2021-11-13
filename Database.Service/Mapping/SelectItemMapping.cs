using Database.Entity;
using Utility.Database.Common.Models;

namespace Database.Service.Mapping
{
    class SelectItemMapping
    {
        public static SelectItem Map(Column column)
        {
            return new SelectItem
            {
                ColumnName = column.Name,
                IsSelected = true
            };

        }

    }
}
