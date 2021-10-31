using SQLite.ViewModel;
using SQLite.Views;
using SQLite.Views.UserControls;
using SQLite.WPF.Infrastructure.IKriv.Windows.Mvvm;
using SQLite.WPF.Views.Tabs;

namespace SQLite.WPF.Meta;

public static class BootStrapper
{
    public static void RegisterViews()
    {

        DataTemplateManager.RegisterDataTemplate<TableGeneralViewModel, TableGeneralTab>();
        DataTemplateManager.RegisterDataTemplate<TableRecordsViewModel, TableRecordsTab>();
        DataTemplateManager.RegisterDataTemplate<TableChartViewModel, TableChartTab>();
        DataTemplateManager.RegisterDataTemplate<DatabaseGeneralViewModel, DatabaseGeneralTab>();

        DataTemplateManager.RegisterDataTemplate<QueryEditorViewModel, QueryEditor>();
        DataTemplateManager.RegisterDataTemplate<SelectQueryViewModel, SelectQueryUserControl>();
        DataTemplateManager.RegisterDataTemplate<TableCreatorViewModel, TableCreator>();

    }

}
