using SQLite.Common;
using SQLite.Common.Contracts;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.ViewModel.Infrastructure.Service
{
    public class TableService
    {
        private readonly ILocaliser localiser;
        private readonly IMessageBoxService messageBoxService;
        private readonly TreeService treeService;

        public TableService(ILocaliser localiser, IMessageBoxService messageBoxService, TreeService treeService)
        {
            this.localiser = localiser;
            this.messageBoxService = messageBoxService;
            this.treeService = treeService;
        }

        public bool DeleteTable(TableItem tableItem)
        {
            var message = localiser["MessageBox_TableDeleteWarning", tableItem.DisplayName];
            var result = messageBoxService.ShowMessage(new(message, localiser["MessageBoxTitle_TableDeletion"], MessageBoxButton.YesNo, MessageBoxImage.Warning));

            if (result != true)
                return false;

            try
            {
                using (var tableHandler = new TableHandler(tableItem.DatabasePath))
                {
                    tableHandler.DropTable(tableItem.DisplayName);
                    treeService.RemoveItemFromTree(tableItem);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Error("Failed to delete table '" + tableItem.DisplayName + "'.", ex);
                return false;
            }
        }
    }
}
