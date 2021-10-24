using SQLite.Service.Repository;

namespace SQLite.Service.Service
{
    public class ExitService
    {
        private readonly TreeService treeService;
        private readonly TabsService tabsService;
        private readonly SeriesRepository seriesRepository;
        private readonly SeriesPairRepository seriesPairRepository;
        private readonly IsSelectedRepository isSelectedRepository;

        public ExitService(TreeService treeService,
            TabsService tabsService,
            SeriesRepository seriesRepository,
            SeriesPairRepository seriesPairRepository,
            IsSelectedRepository isSelectedRepository)
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
