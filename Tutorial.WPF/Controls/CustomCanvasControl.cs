using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tutorial.WPF.Controls
{

    public class CustomCanvasControl : Canvas
    {
        private int leftside = 100;
        private double elementwidth = 100;
        Random Random = new Random();

        // Zoom
        private Double zoomMax = 5;
        private Double zoomMin = 0.5;
        private Double zoomSpeed = 0.01;
        private Double zoom = 1;

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            Canvas_MouseWheel(this, e);
            base.OnMouseWheel(e);
        }
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            zoom += zoomSpeed * e.Delta;
            if (zoom < zoomMin) { zoom = zoomMin; }
            if (zoom > zoomMax) { zoom = zoomMax; }
            this.RenderTransform = new ScaleTransform(zoom, 1);
        }


        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            int items = Children.Count - 1;
            elementwidth = constraint.Width / (items + 1);
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
