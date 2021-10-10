using SQLite.Common.Contracts;


namespace SQLite.ViewModel
{
    public class UnhandledExceptionViewModel
    {
        private readonly ILocaliser localiser;

        public UnhandledExceptionViewModel(ILocaliser localiser)
        {
            this.localiser = localiser;
        }

        public string Title => localiser["UnhandledException_Title"];

        public string UnhandledExceptionKey => localiser["UnhandledException"];
    }
}
