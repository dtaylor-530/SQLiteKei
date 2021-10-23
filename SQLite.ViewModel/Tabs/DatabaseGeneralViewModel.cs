using SQLite.Common.Contracts;
using SQLite.Service.Service;
using SQLite.ViewModel.Infrastructure;
using Utility.Database;
using Utility.SQLite.Helpers;

namespace SQLite.ViewModel;

public class DatabaseGeneralViewModelTabKey : DatabaseKey
{
    public DatabaseGeneralViewModelTabKey(DatabasePath databasePath) : base(databasePath)
    {
    }
}

/// <summary>
/// A ViewModel that is used in the main tab view to display data when a database is selected.
/// </summary>
public class DatabaseGeneralViewModel : DatabaseViewModel
{
    private readonly ViewModelNameService nameService;
    private readonly ILocaliser localiser;
    private readonly TableInformationsService tableInformationsService;
    private readonly Lazy<IReadOnlyCollection<TableInformation>> tableData;

    public DatabaseGeneralViewModel(DatabaseGeneralViewModelTabKey key, ViewModelNameService nameService, ILocaliser localiser, TableInformationsService tableInformationsService)
        : base(key, string.Empty)
    {
        Key = key;
        this.nameService = nameService;

        this.localiser = localiser;
        //this.tableInformationsService = tableInformationsService;
        tableData = new(() => tableInformationsService.Get(key));
    }

    public override string Name => nameService.Get(Key);

    public string Header => localiser["Tab_GroupBoxHeader_About"];

    public override DatabaseGeneralViewModelTabKey Key { get; }

    public string ConnectionName => Path.GetFileNameWithoutExtension(Key.DatabasePath);

    public DatabasePath FilePath => Key.DatabasePath;

    public string FileSize => FilePath.Size.ToString();

    public int TableCount => TableOverviewData.Count;

    public long RowCount => TableOverviewData.Sum(a => a.RowCount);

    public IReadOnlyCollection<TableInformation> TableOverviewData => ConnectionHelper.TablesInformation(Key.DatabasePath);

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