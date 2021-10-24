using ReactiveUI;
using SQLite.Common.Contracts;
using SQLite.Service.Service;
using SQLite.ViewModel.Infrastructure;
using SQLite.ViewModel.Infrastructure.Factory;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Utility.Database;
using static SQLite.Common.Log;

namespace SQLite.ViewModel
{
    public class TableRecordsViewModelTabKey : TableTabKey
    {
        public TableRecordsViewModelTabKey(DatabasePath databasePath, TableName tableName) : base(databasePath, tableName)
        {
        }
    }

    public class TableRecordsViewModel : DatabaseViewModel
    {
        private string searchString;
        //private readonly TableRecordsConfiguration configuration;
        private readonly ILocaliser localiser;
        private readonly IListCollectionService listCollectionService;
        private readonly IWindowService windowService;
        private readonly ViewModelFactory viewModelFactory;
        private readonly StatusService statusService;
        private readonly ViewModelNameService nameService;
        private readonly DatabaseService databaseService;

        public TableRecordsViewModel(
            TableRecordsViewModelTabKey key,
            ILocaliser localiser,
            IListCollectionService listCollectionService,
            IWindowService windowService,
            ViewModelFactory viewModelFactory,
            StatusService statusService,
            ViewModelNameService nameService,
            IsSelectedService isSelectedService,
            DatabaseService databaseService) : base(key, isSelectedService)
        {
            this.Key = key;
            this.WhenAnyValue(a => a.SearchString)
                .WhereNotNull()
                .Subscribe(filter =>
                {
                    listCollectionService.SetFilter(filter);
                    listCollectionService.Refresh();
                });

            //this.configuration = configuration;
            this.localiser = localiser;
            this.listCollectionService = listCollectionService;
            this.windowService = windowService;
            this.viewModelFactory = viewModelFactory;
            this.statusService = statusService;
            this.nameService = nameService;
            this.databaseService = databaseService;
            this.SelectCommand = ReactiveCommand.Create(ExecuteSelect);

            Execute($"Select * from {key.TableName.Name} limit 20");
        }

        public override string Name => nameService.Get(this.Key);
        public override TableRecordsViewModelTabKey Key { get; }

        public IEnumerable Collection => listCollectionService.Collection;

        public string SearchString
        {
            get => searchString;
            set => this.RaiseAndSetIfChanged(ref searchString, value);

        }

        public string FilterKey => localiser["TableRecordsTab_Filter"];

        private void ExecuteSelect()
        {

            var selectQuery = viewModelFactory.Build<SelectQueryViewModel>(new SelectQueryViewModelKey(Key.DatabasePath, Key.TableName));
            bool? vb = windowService.ShowWindow(new("GDFGDFG", selectQuery, ResizeMode.NoResize, Show.ShowDialog));

            // Todo fix WindowService so that the result is meaningful
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
                var resultTable = databaseService.SelectCurrentToDataTable(selectQuery);
                listCollectionService.SetSource(resultTable);
                statusService.OnNext(string.Format("Rows returned: {0}", resultTable.Rows.Count));
            }
            catch (Exception ex)
            {
                Error("Select query failed.", ex);
                statusService.OnNext(ErrorMessage(ex));
            }

            this.RaisePropertyChanged(nameof(Collection));

            static string ErrorMessage(Exception ex)
            {
                var oneLineMessage = Regex.Replace(ex.Message, @"\n", " ");
                oneLineMessage = Regex.Replace(oneLineMessage, @"\t|\r", "");
                oneLineMessage = oneLineMessage.Replace("SQL logic error or missing database ", "SQL Error - ");
                return oneLineMessage;
            }

        }

        public ICommand SelectCommand { get; }

    }
}
