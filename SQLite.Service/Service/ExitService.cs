using SQLite.Service.Repository;

namespace SQLite.Service.Service
{
    public class ExitService
    {
        private readonly TreeService treeService;
        private readonly TabsService tabsService;
        private readonly SeriesRepository seriesRepository;
        private readonly SeriesPairRepository seriesPairRepository;

        public ExitService(TreeService treeService, TabsService tabsService, SeriesRepository seriesRepository, SeriesPairRepository seriesPairRepository)
        {
            this.treeService = treeService;
            this.tabsService = tabsService;
            this.seriesRepository = seriesRepository;
            this.seriesPairRepository = seriesPairRepository;
        }

        public async void Exit()
        {
            await Task.Run(() =>
            {
                treeService.SaveTree();
                tabsService.SaveTabs();
                seriesRepository.PersistAll();
                seriesPairRepository.PersistAll();
            });
        }
    }
}
