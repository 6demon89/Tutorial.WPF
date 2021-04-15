using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tutorial.WPF.Controls
{
    public class CustomCanvasControl : Canvas
    {
        public CustomCanvasControl()
        {
        }

        private int leftside = 100;
        private double elementwidth = 100;

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            int items = Children.Count - 1;
            elementwidth = constraint.Width / (items+1);
            leftside = (int)(constraint.Width / (items + 1));

            for (int i = -1; i < items; i++)
            {
                int index = i + 1;
                var _ = GetVisualChild(index);
                _.SetValue(WidthProperty, elementwidth);
                SetLeft(Children[index], (index * leftside));
            }
            return base.MeasureOverride(constraint);
        }
    }
}
