using System.Windows;

namespace Utility.WPF.Common.Converter
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

            throw new System.Exception("sdfdsfs");
        }

        public static DataTemplateSelector Instance { get; } = new DataTemplateSelector();
    }
}
