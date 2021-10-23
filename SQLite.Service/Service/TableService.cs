using SQLite.Common;
using SQLite.Common.Contracts;
using SQLite.Service.Model;
using Utility.Database;
using Utility.SQLite.Database;
using static SQLite.Common.Log;

namespace SQLite.Service.Service
{

    public class TableService
    {
        class Mapper
        {
            public static TableHandler Map(TableLeafItem tableItem)
            {
                return new TableHandler(new DatabasePath(tableItem.Key.DatabasePath), tableItem.Name);
            }
        }

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

        public bool DeleteTable(TableLeafItem tableItem)
        {
            var message = localiser["MessageBox_TableDeleteWarning", tableItem.Name];
            var result = messageBoxService.ShowMessage(new(message, localiser["MessageBoxTitle_TableDeletion"], MessageBoxButton.YesNo, MessageBoxImage.Warning));

            if (result != true)
                return false;

            try
            {
                using var tableHandler = Mapper.Map(tableItem);
                tableHandler.DropTable();
                treeService.RemoveItemFromTree(tableItem);
                return true;

            }
            catch (Exception ex)
            {
                Error("Failed to delete table '" + tableItem.Name + "'.", ex);
                return false;
            }
        }

        public void EmptyTable(TableLeafItem tableItem)
        {
            var message = localiser["MessageBox_EmptyTable", tableItem.Name];
            var messageTitle = localiser["MessageBoxTitle_EmptyTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning));

            if (result != true)
                return;

            using var tableHandler = Mapper.Map(tableItem);
            try
            {
                tableHandler.EmptyTable();
            }
            catch (Exception ex)
            {
                Error("Failed to empty table" + tableItem.Name, ex);
                statusService.OnNext(ex.Message);
            }

        }

        public void ReindexTable(TableLeafItem tableItem)
        {
            var message = localiser["MessageBox_ReindexTable", tableItem.Name];
            var messageTitle = localiser["MessageBoxTitle_ReindexTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question));

            if (result != true) return;

            using var tableHandler = Mapper.Map(tableItem);
            try
            {
                tableHandler.ReindexTable();
            }
            catch (Exception ex)
            {
                Error("Failed to empty table" + tableItem.Name, ex);
                statusService.OnNext(ex.Message);
            }

        }

        public void RenameTable(TableLeafItem tableItem, string newName)
        {
            var message = localiser["MessageBox_RenameTable", tableItem.Name];
            var messageTitle = localiser["MessageBoxTitle_RenameTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question));
            if (result != true) return;

            if (string.IsNullOrWhiteSpace(tableItem.Name))
            {
                return;
            }
            try
            {
                using var tableHandler = Mapper.Map(tableItem);
                tableHandler.RenameTable(newName);
            }
            catch
            {
                //TODO decide what should happen in this case
            }
            return;
        }

    }
}
