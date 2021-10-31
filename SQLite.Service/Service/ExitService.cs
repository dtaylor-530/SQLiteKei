using SQLite.Service.Repository;
using Utility.Common.Base;
using Utility.Common.Contracts;

namespace SQLite.Service.Service
{
    public class ExitService : IExitService
    {
        private readonly ITreeService treeService;
        private readonly TabsService tabsService;
        private readonly SeriesRepository seriesRepository;
        private readonly SeriesPairRepository seriesPairRepository;
        private readonly IIsSelectedRepository isSelectedRepository;

        public ExitService(ITreeService treeService,
            TabsService tabsService,
            SeriesRepository seriesRepository,
            SeriesPairRepository seriesPairRepository,
            IIsSelectedRepository isSelectedRepository)
        {
            this.treeService = treeService;
            this.tabsService = tabsService;
            this.seriesRepository = seriesRepository;
            this.seriesPairRepository = seriesPairRepository;
            this.isSelectedRepository = isSelectedRepository;
        }

        public async void Exit()
        {
            await Task.Run(() =>
            {
                treeService.SaveTree();
                tabsService.SaveTabs();
                seriesRepository.PersistAll();
                seriesPairRepository.PersistAll();
                isSelectedRepository.PersistAll();
            });
        }
    }
}
