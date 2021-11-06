using Database.Entity;
using SQLite.ViewModel;
using Utility.Database;
using Utility.Entity;

namespace SQLite.Common
{

    public class DatabaseGeneralViewModelTabKey : DatabaseKey<IDatabaseGeneralViewModel>
    {
        public DatabaseGeneralViewModelTabKey(DatabasePath databasePath) : base(databasePath)
        {
        }
    }

    public interface ITableChartViewModel : IDatabaseViewModel { }

    public class TableChartViewModelTabKey : TableTabKey<ITableChartViewModel>
    {
        public TableChartViewModelTabKey(DatabasePath databasePath, TableName tableName) : base(databasePath, tableName)
        {
        }
    }

    public interface ITableGeneralViewModel : IDatabaseViewModel
    {
    }

    public class TableGeneralViewModelTabKey : TableTabViewModelKey<ITableGeneralViewModel>
    {
        public TableGeneralViewModelTabKey(DatabasePath databasePath, TableName tableName) : base(databasePath, tableName)
        {
        }
    }

    public interface ITableRecordsViewModel : IDatabaseViewModel
    {
    }

    public class TableRecordsViewModelTabKey : TableTabViewModelKey<ITableRecordsViewModel>
    {
        public TableRecordsViewModelTabKey(DatabasePath databasePath, TableName tableName) : base(databasePath, tableName)
        {
        }
    }

    public class TableCreatorViewModelKey : Key<ITableCreatorViewModel>
    {
        public TableCreatorViewModelKey() : base()
        {
        }
    }

}
