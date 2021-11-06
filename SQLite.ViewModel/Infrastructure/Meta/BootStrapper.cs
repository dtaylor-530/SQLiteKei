using Autofac;
using Database.ViewModel;

namespace SQLite.ViewModel.Meta
{
    public static class BootStrapper
    {
        public static ContainerBuilder Register(this ContainerBuilder containerBuilder)
        {

            foreach (var type in new[] {
                typeof(DatabaseGeneralViewModel),
                typeof(TableChartViewModel),
                typeof(TableGeneralViewModel),
                typeof(TableRecordsViewModel),

                typeof(DatabaseViewModel),
                typeof(QueryEditorViewModel),
                typeof(SelectQueryViewModel),
                typeof(TableCreatorViewModel),
            })
            {
                containerBuilder.RegisterType(type).AsImplementedInterfaces().AsSelf();
            }

            return containerBuilder;
        }
    }
}
