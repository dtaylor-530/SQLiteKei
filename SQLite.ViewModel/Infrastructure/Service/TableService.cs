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
        private readonly StatusService statusService;

        public TableService(ILocaliser localiser, IMessageBoxService messageBoxService, TreeService treeService, StatusService statusService)
        {
            this.localiser = localiser;
            this.messageBoxService = messageBoxService;
            this.treeService = treeService;
            this.statusService = statusService;
        }

        public bool DeleteTable(TableItem tableItem)
        {
            var message = localiser["MessageBox_TableDeleteWarning", tableItem.DisplayName];
            var result = messageBoxService.ShowMessage(new(message, localiser["MessageBoxTitle_TableDeletion"], MessageBoxButton.YesNo, MessageBoxImage.Warning));

            if (result != true)
                return false;

            try
            {
                using (var tableHandler = new TableHandler(tableItem.DatabasePath, tableItem.DisplayName))
                {
                    tableHandler.DropTable();
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

        public void EmptyTable(TableItem tableItem)
        {
            var message = localiser["MessageBox_EmptyTable", tableItem.DisplayName];
            var messageTitle = localiser["MessageBoxTitle_EmptyTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning));

            if (result != true)
                return;

            using (var tableHandler = new TableHandler(tableItem.DatabasePath, tableItem.DisplayName))
            {
                try
                {
                    tableHandler.EmptyTable();
                }
                catch (Exception ex)
                {
                    Error("Failed to empty table" + tableItem.DisplayName, ex);
                    statusService.OnNext(ex.Message);
                }

            }

        }

        public void ReindexTable(TableItem tableItem)
        {
            var message = localiser["MessageBox_ReindexTable", tableItem.DisplayName];
            var messageTitle = localiser["MessageBoxTitle_ReindexTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question));

            if (result != true) return;

            using (var tableHandler = new TableHandler(tableItem.DatabasePath, tableItem.DisplayName))
            {
                try
                {
                    tableHandler.ReindexTable();
                }
                catch (Exception ex)
                {
                    Error("Failed to empty table" + tableItem.DisplayName, ex);
                    statusService.OnNext(ex.Message);
                }
            }
        }

        public void RenameTable(TableItem tableItem, string newName)
        {
            if (string.IsNullOrWhiteSpace(tableItem.DisplayName))
            {
                return;
            }
            try
            {
                using (var tableHandler = new TableHandler(tableItem.DatabasePath, tableItem.DisplayName))
                {
                    tableHandler.RenameTable(newName);
                }
            }
            catch
            {
                //TODO decide what should happen in this case
            }
            return;
        }
    }
}
