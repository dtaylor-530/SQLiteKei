using Utility.Common.Base;
using Utility.Common.Contracts;

namespace SQLite.Service.Service
{
    public class ExitService : IExitService
    {
        private readonly IReadOnlyCollection<IRepository> repositories;

        public ExitService(IEnumerable<IRepository> repositories)
        {
            this.repositories = repositories.ToArray();
        }

        public async void Exit()
        {
            await Task.Run(() =>
            {
                foreach (var repo in repositories)
                    repo.PersistAll();
            });
        }
    }
}
