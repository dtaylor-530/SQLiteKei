using Microsoft.Xaml.Behaviors;
using SQLite.Common;
using System.Linq;
using System.Windows;

namespace SQLite.WPF.Infrastructure
{

    public class LoadedBehavior : StyleBehavior<FrameworkElement, LoadedBehavior>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            if (AssociatedObject.IsLoaded)
            {

            }
            base.OnAttached();
        }
        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            base.OnDetaching();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.DataContext is IViewModel vm)
                vm.IsLoaded = true;
        }
    }

    /// <summary>
    /// For attaching a behavior to a Style
    /// <a href="https://stackoverflow.com/questions/1647815/how-to-add-a-blend-behavior-in-a-style-setter"></a>
    /// Roma Borodov
    /// </summary>
    public class StyleBehavior<TComponent, TBehavior> : Behavior<TComponent>
            where TComponent : DependencyObject
            where TBehavior : StyleBehavior<TComponent, TBehavior>, new()
    {
        public static DependencyProperty IsEnabledForStyleProperty =
            DependencyProperty.RegisterAttached("IsEnabledForStyle", typeof(bool),
            typeof(StyleBehavior<TComponent, TBehavior>), new FrameworkPropertyMetadata(false, OnIsEnabledForStyleChanged));

        public bool IsEnabledForStyle
        {
            get => (bool)GetValue(IsEnabledForStyleProperty);
            set { SetValue(IsEnabledForStyleProperty, value); }
        }

        private static void OnIsEnabledForStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uie && e.NewValue is bool newValue)
            {
                var behaviors = Interaction.GetBehaviors(uie);

                if (behaviors.FirstOrDefault(b => b is TBehavior) is { } existing)
                {
                    if (newValue)
                        behaviors.Remove(existing);
                }
                else if (newValue)
                {
                    behaviors.Add(new TBehavior());
                }
            }
        }
    }

}
