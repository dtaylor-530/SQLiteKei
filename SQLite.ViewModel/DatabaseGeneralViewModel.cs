using SQLite.Common.Contracts;
using Utility.Database;
using Utility.SQLite.Helpers;

namespace SQLite.ViewModel;

public class DatabaseGeneralConfiguration
{
    public DatabaseGeneralConfiguration(ConnectionPath connectionPath)
    {
        this.ConnectionPath = connectionPath;
    }

    public ConnectionPath ConnectionPath { get; }
}

/// <summary>
/// A ViewModel that is used in the main tab view to display data when a database is selected.
/// </summary>
public class DatabaseGeneralViewModel
{
    private readonly ConnectionPath connectionPath;
    private readonly ILocaliser localiser;

    public DatabaseGeneralViewModel(DatabaseGeneralConfiguration configuration, ILocaliser localiser)
    {
        this.connectionPath = configuration.ConnectionPath;
        this.localiser = localiser;

        {
            Name = FilePath.AsFileInfo.Name;
            DisplayName = Path.GetFileNameWithoutExtension(connectionPath.Path);
            TableOverviewData = ConnectionHelper.TablesInformation(configuration.ConnectionPath);

        }
    }

    public string Header => localiser["TabContent_GroupBoxHeader_About"];

    public string DisplayName { get; }

    public string Name { get; }

    public ConnectionPath FilePath => connectionPath;

    public string FileSize => FilePath.Size.ToString();

    public int TableCount => TableOverviewData.Count;

    public long RowCount => TableOverviewData.Sum(a => a.RowCount);

    public IReadOnlyCollection<TableInformation> TableOverviewData { get; }

    public string DatabaseNameKey => localiser["DatabaseGeneralTab_DatabaseName"];
    public string DatabasePathKey => localiser["DatabaseGeneralTab_DatabasePath"];
    public string FileSizeKey => localiser["DatabaseGeneralTab_FileSize"];
    public string TablesKey => localiser["DatabaseGeneralTab_Tables"];
    public string RecordsKey => localiser["DatabaseGeneralTab_Records"];
    public string TableNameKey => localiser["DatabaseGeneralTab_TableName"];
    public string TableColumnsKey => localiser["DatabaseGeneralTab_TableColumns"];
    public string TableRecordsKey => localiser["DatabaseGeneralTab_TableRecords"];
    public string NoTablesFoundKey => localiser["DatabaseGeneralTab_NoTablesFound"];

}