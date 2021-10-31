using SQLite.Common.Contracts;
using Utility.Common.Base;
using Utility.Common.Contracts;
using Utility.Database;
using Utility.SQLite.Database;
using static Utility.Common.Base.Log;

namespace SQLite.Service.Service
{

    public class TableService : ITableService
    {
        class Mapper
        {
            public static TableHandler Map(ITableKey tableItem)
            {
                return new TableHandler(new DatabasePath(tableItem.DatabasePath), tableItem.TableName);
            }
        }

        private readonly ILocaliser localiser;
        private readonly IMessageBoxService messageBoxService;
        private readonly ITreeService treeService;
        private readonly IStatusService statusService;

        public TableService(ILocaliser localiser, IMessageBoxService messageBoxService, ITreeService treeService, IStatusService statusService)
        {
            this.localiser = localiser;
            this.messageBoxService = messageBoxService;
            this.treeService = treeService;
            this.statusService = statusService;
        }

        public bool DeleteTable(ITableKey tableItem)
        {
            var message = localiser["MessageBox_TableDeleteWarning", tableItem.TableName];
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
                Error("Failed to delete table '" + tableItem.TableName + "'.", ex);
                return false;
            }
        }

        public void EmptyTable(ITableKey tableItem)
        {
            var message = localiser["MessageBox_EmptyTable", tableItem.TableName];
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
                Error("Failed to empty table" + tableItem.TableName, ex);
                statusService.OnNext(ex.Message);
            }

        }

        public void ReindexTable(ITableKey tableItem)
        {
            var message = localiser["MessageBox_ReindexTable", tableItem.TableName];
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
                Error("Failed to empty table" + tableItem.TableName, ex);
                statusService.OnNext(ex.Message);
            }

        }

        public void RenameTable(ITableKey tableItem, string newName)
        {
            var message = localiser["MessageBox_RenameTable", tableItem.TableName];
            var messageTitle = localiser["MessageBoxTitle_RenameTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question));
            if (result != true) return;

            if (string.IsNullOrWhiteSpace(tableItem.TableName))
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
