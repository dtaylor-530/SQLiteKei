using SQLite.Common.Contracts;
using SQLite.Service.Service;
//using System.Reactive.Linq;
using Utility;

namespace SQLite.ViewModel.Infrastructure
{
    public class ViewModelNameService : IViewModelNameService
    {
        private readonly ILocaliser localiser;

        public ViewModelNameService(ILocaliser localiser)
        {
            this.localiser = localiser;
        }

        public string Get(Key key)
        {
            switch (key)
            {
                case DatabaseGeneralViewModelTabKey { } dk:
                    {
                        return dk.DatabasePath.BaseName;
                    }
                case TableChartViewModelTabKey { } dk:
                    {
                        return "Chart";
                    }
                case TableGeneralViewModelTabKey { } dk:
                    {
                        return localiser["TabHeader_GeneralTable", dk.TableName.Name];
                    }
                case TableRecordsViewModelTabKey { } dk:
                    {
                        return localiser["TabHeader_TableRecords"];
                    }
                default:
                    throw new Exception("d354434t4 ggg");
            }
        }
    }

}
