using Database.Entity;
using ReactiveUI;
using System.Data;
using System.Windows.Input;
using Utility.Common.Base;
using Utility.Database.SQLite.Common.Abstract;

namespace Database.ViewModel
{
    public class QueryEditorViewModel : ReactiveObject
    {
        private readonly ILocaliser localiser;
        private readonly IHandlerService tableHandlerFactory;
        private string sqlStatement = string.Empty;
        private string statusInfo;
        private DatabaseSelectItem selectedDatabase;
        private string selectedText;
        private DataView dataView;

        public QueryEditorViewModel(ILocaliser localiser, IHandlerService tableHandlerFactory)
        {
            this.localiser = localiser;
            this.tableHandlerFactory = tableHandlerFactory;
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
                tableHandlerFactory.Database(selectedDatabase.Key, handler =>
               {
                   try
                   {
                       if (SqlStatement.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
                       {
                           var queryResult = handler.ExecuteAndLoadDataTable(sqlStatement);
                           DataView = queryResult.DefaultView;
                           return string.Format("Rows returned: {0}", queryResult.Rows.Count);
                       }
                       else
                       {
                           var commandResult = handler.ExecuteNonQuery(sqlStatement);
                           return string.Format("Rows affected: {0}", commandResult);
                       }
                   }
                   catch (Exception ex)
                   {
                       return ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                   }
               })
                    .Subscribe(a =>
                    {
                        StatusInfo = a;
                    });
            }
        }
    }
}
