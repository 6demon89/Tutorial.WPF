using System;
using System.Windows;
using System.Windows.Controls;

namespace LayoutBasics.Controls
{
    public class SimpleCanvas : Panel
    {


        public static double GetTop(DependencyObject obj)
        {
            return (double)obj.GetValue(TopProperty);
        }

        public static void SetTop(DependencyObject obj, double value)
        {
            obj.SetValue(TopProperty, value);
        }

        // Using a DependencyProperty as the backing store for Top.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopProperty =
            DependencyProperty.RegisterAttached("Top", 
                typeof(double), 
                typeof(SimpleCanvas), 
                new PropertyMetadata(0.0,TopPropertyChanged)
                );

        private static void TopPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var child = d as FrameworkElement;
            if(child != null)
            {
                var canvas = child.Parent as SimpleCanvas;
                if(canvas != null)
                {
                    canvas.InvalidateArrange();
                    canvas.UpdateLayout();
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var location = new Point();
            foreach (UIElement item in InternalChildren)
            {
                location.Y = GetTop(item);
                item.Arrange(new Rect(location, item.DesiredSize));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement item in InternalChildren)
                item.Measure(availableSize);
            return base.MeasureOverride(availableSize);
        }
    }
}
