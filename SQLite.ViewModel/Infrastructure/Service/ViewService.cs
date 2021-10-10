using SQLite.Common.Contracts;
using SQLite.ViewModel.Infrastructure.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ViewModel.Infrastructure.Service
{
    public class ViewService
    {
        private readonly ViewModelFactory viewModelFactory;
        private readonly ILocaliser localiser;
        private readonly IWindowService windowService;
        private readonly TreeService treeService;

        public ViewService(ViewModelFactory viewModelFactory, ILocaliser localiser, IWindowService windowService, TreeService treeService)
        {
            this.viewModelFactory = viewModelFactory;
            this.localiser = localiser;
            this.windowService = windowService;
            this.treeService = treeService;
        }

        public void OpenQueryEditor()
        {
            var preferences = viewModelFactory.Build<PreferencesViewModel>();
            windowService.ShowWindow(new(localiser["WindowTitle_Preferences"], preferences, ResizeMode.NoResize, Show.ShowDialog));
        }
        public void OpenTableCreator()
        {
            var tableCreatorViewModel = viewModelFactory.Build<TableCreatorViewModel>();
            windowService.ShowWindow(new(localiser["WindowTitle_Preferences"], tableCreatorViewModel, ResizeMode.NoResize, Show.ShowDialog));

            treeService.RefreshTree();
        }
    }
}
