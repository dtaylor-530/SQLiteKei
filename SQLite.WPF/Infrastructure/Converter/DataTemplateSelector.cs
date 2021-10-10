using System.Windows;

namespace SQLite.WPF.Infrastructure.Converter
{
    public class DataTemplateSelector : System.Windows.Controls.DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var dataTemplateKey = new DataTemplateKey(item.GetType());

            if (container is System.Windows.Controls.Control control &&
                control.FindResource(dataTemplateKey) is DataTemplate dataTemplate)
            {
                return dataTemplate;
            }

            if (Application.Current.FindResource(dataTemplateKey) is DataTemplate appDataTemplate)
            {
                return appDataTemplate;
            }

            //if (container is FrameworkElement element && item != null)
            //{
            //    return element.FindResource("importantTaskTemplate") as DataTemplate;

            //}

            throw new System.Exception("sdfdsfs");
        }

        public static DataTemplateSelector Instance { get; } = new DataTemplateSelector();
    }
}

//var dataTemplateKey = new DataTemplateKey() { DataType = theType; };
//var dataTemplate = yourControl.FindResource(dataTemplateKey);

//if (dataTemplate != null)
//{
//    return dataTemplate;
//}

//return NulloDataTemplate;