using System.Collections;
using System.Data;

namespace SQLite.Common.Contracts
{
    public interface IListCollectionService
    {
        IEnumerable Collection { get; }

        void Refresh();
        void SetFilter(string value);
        void SetSource(DataTable dataTable);
    }
}