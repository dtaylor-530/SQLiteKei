using SQLite.Common.Contracts;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Windows.Data;

namespace SQLite.WPF.Infrastructure
{
    public class ListCollectionService : IListCollectionService
    {
        private ListCollectionView dataGridCollection;
        private string searchString;

        public ListCollectionService()
        {
        }

        public void SetFilter(string value) => this.searchString = value;

        public void Refresh() => dataGridCollection.Refresh();

        public void SetSource(DataTable dataTable)
        {
            dataGridCollection = new ListCollectionView(dataTable.DefaultView);
            if (dataGridCollection.CanFilter)
                dataGridCollection.Filter = Filter;

            bool Filter(object obj)
            {
                if (string.IsNullOrEmpty(searchString))
                {
                    return true;
                }

                if (obj is DataRowView { Row: { } row })
                {
                    return row.ItemArray
                        .Select(entry => entry?.ToString())
                        .Any(value => value?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true);
                }
                throw new Exception("40099095jfrfdfddf");
            }
        }

        public IEnumerable Collection => dataGridCollection;
    }
}
