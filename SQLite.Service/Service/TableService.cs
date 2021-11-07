using Database.Common.Contracts;
using Utility.Common.Base;
using Utility.Common.Contracts;
using Utility.Database.Common;
using Utility.Database.SQLite.Common.Abstract;
using static Utility.Common.Base.Log;

namespace SQLite.Service.Service
{

    public class TableService : ITableService
    {
        private readonly ILocaliser localiser;
        private readonly IMessageBoxService messageBoxService;
        private readonly ITreeModel treeModel;
        private readonly IStatusModel statusService;
        private readonly IHandlerService tableHandlerFactory;

        public TableService(ILocaliser localiser, IMessageBoxService messageBoxService, ITreeModel treeModel, IStatusModel statusService, IHandlerService tableHandlerFactory)
        {
            this.localiser = localiser;
            this.messageBoxService = messageBoxService;
            this.treeModel = treeModel;
            this.statusService = statusService;
            this.tableHandlerFactory = tableHandlerFactory;
        }

        public bool DeleteTable(ITableKey tableKey)
        {
            var message = localiser["MessageBox_TableDeleteWarning", tableKey.TableName];
            var result = messageBoxService.ShowMessage(new(message, localiser["MessageBoxTitle_TableDeletion"], MessageBoxButton.YesNo, MessageBoxImage.Warning));

            if (result != true)
                return false;

            try
            {
                tableHandlerFactory.Table(tableKey, handler =>
                {
                    handler.DropTable();
                    return new object();
                });
                this.treeModel.OnNext(new(Removes: new[] { tableKey }));
                return true;

            }
            catch (Exception ex)
            {
                Error("Failed to delete table '" + tableKey.TableName + "'.", ex);
                return false;
            }
        }

        public void EmptyTable(ITableKey tableKey)
        {
            var message = localiser["MessageBox_EmptyTable", tableKey.TableName];
            var messageTitle = localiser["MessageBoxTitle_EmptyTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning));

            if (result != true)
                return;

            tableHandlerFactory.Table(tableKey, handler =>
            {
                try
                {
                    handler.EmptyTable();
                }
                catch (Exception ex)
                {
                    Error("Failed to empty table" + tableKey.TableName, ex);
                    statusService.OnNext(ex.Message);
                }
                return new object();
            });

        }

        public void ReindexTable(ITableKey tableKey)
        {
            var message = localiser["MessageBox_ReindexTable", tableKey.TableName];
            var messageTitle = localiser["MessageBoxTitle_ReindexTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question));

            if (result != true) return;

            tableHandlerFactory.Table(tableKey, handler =>
            {
                try
                {
                    handler.ReindexTable();
                }
                catch (Exception ex)
                {
                    Error("Failed to reindex table" + tableKey.TableName, ex);
                    statusService.OnNext(ex.Message);
                }
                return new object();
            });
        }

        public void RenameTable(ITableKey tableKey, string newName)
        {
            var message = localiser["MessageBox_RenameTable", tableKey.TableName];
            var messageTitle = localiser["MessageBoxTitle_RenameTable"];
            var result = messageBoxService.ShowMessage(new(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question));
            if (result != true) return;

            if (string.IsNullOrWhiteSpace(tableKey.TableName))
            {
                return;
            }

            tableHandlerFactory.Table(tableKey, handler =>
            {
                try
                {
                    handler.RenameTable(newName);
                }
                catch (Exception ex)
                {
                    Error("Failed to rename table" + tableKey.TableName, ex);
                    statusService.OnNext(ex.Message);
                }
                return new object();
            });
        }
    }
}
