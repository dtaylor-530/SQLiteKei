using Database.ViewModel;
using Database.WPF.Infrastructure.IKriv.Windows.Mvvm;
using Database.WPF.UserControls;
using Database.WPF.Views.Tabs;

namespace Database.WPF.Meta;

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
