using SQLite.Service.Repository;
using Utility.Common.Base;
using Utility.Common.Contracts;

namespace SQLite.Service.Service
{
    public class ExitService : IExitService
    {
        private readonly ITreeModel treeService;
        private readonly TabsRepository tabsRepository;
        private readonly SeriesRepository seriesRepository;
        private readonly SeriesPairRepository seriesPairRepository;
        private readonly IIsSelectedRepository isSelectedRepository;

        public ExitService(ITreeModel treeModel,
            TabsRepository tabsRepository,
            SeriesRepository seriesRepository,
            SeriesPairRepository seriesPairRepository,
            IIsSelectedRepository isSelectedRepository)
        {
            this.treeService = treeModel;
            this.tabsRepository = tabsRepository;
            this.seriesRepository = seriesRepository;
            this.seriesPairRepository = seriesPairRepository;
            this.isSelectedRepository = isSelectedRepository;
        }

        public async void Exit()
        {
            await Task.Run(() =>
            {
                treeService.SaveTree();
                tabsRepository.PersistAll();
                seriesRepository.PersistAll();
                seriesPairRepository.PersistAll();
                isSelectedRepository.PersistAll();
            });
        }
    }
}
