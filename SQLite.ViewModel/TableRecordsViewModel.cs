using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Factory;
using SQLite.ViewModel.Infrastructure.Service;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Utility.Database;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.ViewModel
{
    public class TableRecordsConfiguration
    {
        public TableRecordsConfiguration(string tableName, ConnectionPath connectionPath)
        {
            TableName = tableName;
            ConnectionPath = connectionPath;
        }
        public string TableName { get; }

        public ConnectionPath ConnectionPath { get; }

    }

    public class TableRecordsViewModel : ReactiveObject
    {
        private string searchString;
        private readonly TableRecordsConfiguration configuration;
        private readonly ILocaliser localiser;
        private readonly IListCollectionService listCollectionService;
        private readonly IWindowService windowService;
        private readonly ViewModelFactory viewModelFactory;
        private readonly StatusService statusService;
        private readonly TreeService treeService;

        public TableRecordsViewModel(
            TableRecordsConfiguration configuration,
            ILocaliser localiser,
            IListCollectionService listCollectionService,
            IWindowService windowService,
            ViewModelFactory viewModelFactory,
            StatusService statusService,
            TreeService treeService)
        {
            this.WhenAnyValue(a => a.SearchString)
                .WhereNotNull()
                .Subscribe(filter =>
                {
                    listCollectionService.SetFilter(filter);
                    listCollectionService.Refresh();
                });

            this.configuration = configuration;
            this.localiser = localiser;
            this.listCollectionService = listCollectionService;
            this.windowService = windowService;
            this.viewModelFactory = viewModelFactory;
            this.statusService = statusService;
            this.treeService = treeService;
            this.SelectCommand = ReactiveCommand.Create(ExecuteSelect);

            Execute($"Select * from {configuration.TableName} limit 20");
        }

        public IEnumerable Collection => listCollectionService.Collection;

        public string SearchString
        {
            get => searchString;
            set => this.RaiseAndSetIfChanged(ref searchString, value);

        }

        public string FilterKey => localiser["TableRecordsTab_Filter"];

        private void ExecuteSelect()
        {

            var selectQuery = viewModelFactory.Build<SelectQueryViewModel>(new SelectQueryConfiguration(configuration.TableName, configuration.ConnectionPath));
            bool? vb = windowService.ShowWindow(new("GDFGDFG", selectQuery, ResizeMode.NoResize, Show.ShowDialog));

            //if (vb == true)
            //{
            statusService.OnNext(string.Empty);
            Execute(selectQuery.SelectQuery);
            //}

        }

        void Execute(string selectQuery)
        {
            Info("Executing select query from SelectQuery window.\n" + selectQuery);
            try
            {
                var dbHandler = new DatabaseHandler(treeService.SelectedItem.DatabasePath);
                var resultTable = dbHandler.ExecuteReader(selectQuery);
                listCollectionService.SetSource(resultTable);
                statusService.OnNext(string.Format("Rows returned: {0}", resultTable.Rows.Count));
            }
            catch (Exception ex)
            {
                Error("Select query failed.", ex);
                var oneLineMessage = Regex.Replace(ex.Message, @"\n", " ");
                oneLineMessage = Regex.Replace(oneLineMessage, @"\t|\r", "");
                oneLineMessage = oneLineMessage.Replace("SQL logic error or missing database ", "SQL Error - ");

                statusService.OnNext(oneLineMessage);
            }

            this.RaisePropertyChanged(nameof(Collection));
        }

        public ICommand SelectCommand { get; }

    }
}
