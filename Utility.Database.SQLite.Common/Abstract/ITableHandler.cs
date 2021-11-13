//using log4net;
using System.Data;
using Utility.Database.Common.Models;

namespace Utility.Database.SQLite.Common.Abstract;

public readonly struct TableGeneralInformation
{
    public string CreateStatement { get; }

    public TableGeneralInformation(string createStatement, long rowCount, IReadOnlyCollection<Column> columns, int columnCount) : this()
    {
        CreateStatement = createStatement;
        RowCount = rowCount;
        //DataTable = dataTable;
        Columns = columns;
        ColumnCount = columnCount;
    }

    public long RowCount { get; }
    public DataTable DataTable { get; }
    public IReadOnlyCollection<Column> Columns { get; }
    public int ColumnCount { get; }
}

public interface ITableHandler : IDisposable
{

    //  static DataColumn[] Columns { get; }
    IReadOnlyCollection<Column> Columns { get; }
    string CreateStatement { get; }
    DataTable DataTable { get; }
    TableGeneralInformation General { get; }
    long RowCount { get; }
    string TableName { get; }

    void DropTable();
    void EmptyTable();
    void ReindexTable();
    void RenameTable(string newName);
}
