using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.Service.Model;
using System.Data;
using System.Windows.Input;
using Utility.SQLite.Database;

namespace SQLite.ViewModel
{
    public class QueryEditorViewModel : ReactiveObject
    {
        private readonly ILocaliser localiser;
        private string sqlStatement = string.Empty;
        private string statusInfo;
        private DatabaseSelectItem selectedDatabase;
        private string selectedText;
        private DataView dataView;

        public QueryEditorViewModel(ILocaliser localiser)
        {
            this.localiser = localiser;
            ExecuteCommand = ReactiveCommand.Create(Execute);
        }

        public ICommand ExecuteCommand { get; }

        public string Title => localiser["WindowTitle_QueryEditor"];
        public string DatabaseKey => localiser["QueryEditor_Label_Database"];
        public string ExecuteKey => localiser["ButtonText_Execute"];
        public string CancelKey => localiser["ButtonText_Cancel"];

        public string SqlStatement
        {
            get => sqlStatement;
            set => this.RaiseAndSetIfChanged(ref sqlStatement, value);
        }

        public string StatusInfo
        {
            get => statusInfo;
            set => this.RaiseAndSetIfChanged(ref statusInfo, value);
        }

        public List<DatabaseSelectItem> Databases { get; } = new List<DatabaseSelectItem>();

        public DatabaseSelectItem SelectedDatabase
        {
            get => selectedDatabase;
            set => this.RaiseAndSetIfChanged(ref selectedDatabase, value);
        }

        public string SelectedText
        {
            get => selectedText;
            set => this.RaiseAndSetIfChanged(ref selectedText, value);
        }

        public DataView DataView
        {
            get => dataView;
            set => this.RaiseAndSetIfChanged(ref dataView, value);
        }

        public void Execute()
        {
            StatusInfo = string.Empty;

            if (SelectedDatabase == null)
            {
                StatusInfo = localiser["TableCreator_NoDatabaseSelected"];
                return;
            }

            if (string.IsNullOrEmpty(SqlStatement))
                return;

            if (string.IsNullOrEmpty(selectedText))
            {
                ExecuteSql(SqlStatement);
            }
            else
            {
                ExecuteSql(SelectedText);
            }

            void ExecuteSql(string sqlStatement)
            {

                using var dbHandler = new DatabaseHandler(selectedDatabase.Path);
                try
                {
                    if (SqlStatement.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var queryResult = dbHandler.ExecuteAndLoadDataTable(sqlStatement);
                        DataView = queryResult.DefaultView;
                        StatusInfo = string.Format("Rows returned: {0}", queryResult.Rows.Count);
                    }
                    else
                    {
                        var commandResult = dbHandler.ExecuteNonQuery(sqlStatement);
                        StatusInfo = string.Format("Rows affected: {0}", commandResult);
                    }
                }
                catch (Exception ex)
                {
                    StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                }
            }
        }
    }
}
