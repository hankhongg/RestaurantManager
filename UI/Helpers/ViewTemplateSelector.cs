using System.Windows;
using System.Windows.Controls;

namespace RestaurantManager.Helpers
{
    public class ViewTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is string viewName && container is FrameworkElement element)
            {
                var template = element.TryFindResource(viewName) as DataTemplate;
                if (template != null)
                    return template;

                MessageBox.Show($"Template '{viewName}' not found!");
            }

            return null; 
        }
    }
}
