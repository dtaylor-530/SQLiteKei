using SQLite.Common;
using SQLite.Common.Contracts;
using Utility.Common.Base;
using Utility.Common.Contracts;
using Utility.Database;
using Utility.SQLite.Helpers;

namespace SQLite.ViewModel;

/// <summary>
/// A ViewModel that is used in the main tab view to display data when a database is selected.
/// </summary>
public class DatabaseGeneralViewModel : DatabaseViewModel<IDatabaseGeneralViewModel>, IDatabaseGeneralViewModel
{
    private readonly IViewModelNameService nameService;
    private readonly ILocaliser localiser;
    private readonly Lazy<IReadOnlyCollection<TableInformation>> tableData;
    DatabaseGeneralViewModelTabKey key;

    public DatabaseGeneralViewModel(
        DatabaseGeneralViewModelTabKey key,
        IViewModelNameService nameService,
        ILocaliser localiser,
        ITableInformationsService tableInformationsService)
        : base(key)
    {
        this.key = key;
        this.nameService = nameService;
        this.localiser = localiser;
        tableData = new(() => tableInformationsService.Get(key));
    }

    public override string Name => nameService.Get(key);

    public string Header => localiser["Tab_GroupBoxHeader_About"];

    //public override DatabaseGeneralViewModelTabKey Key { get; }

    public string ConnectionName => Path.GetFileNameWithoutExtension(key.DatabasePath);

    public DatabasePath FilePath => key.DatabasePath;

    public string FileSize => FilePath.Size.ToString();

    public int TableCount => TableOverviewData.Count;

    public long RowCount => TableOverviewData.Sum(a => a.RowCount);

    public IReadOnlyCollection<TableInformation> TableOverviewData => tableData.Value;

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
