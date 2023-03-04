using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LayoutBasics.Controls;

//Class Hierarchy WPF
//Object
//DispatcherObject
//DependencyObject
//Visual
//UIElement
//FrameworkElement (Width,Hight,Margin,etc)
//Panel Class (UI Collection)

//class Hierarchy WinUI2
//Object
//DependencyObject
//UIElement
//FrameworkElement (Width,Hight,Margin,etc)
//Panel Class (UI Collection)

//Layout Process : is based on 3 steps
//1.Measure => calculation of size
//2.Arrange => arranging position
//3.Rendering 
public class DiagonalPanel : Panel
{
    /// <summary>
    /// Calculate the size, as well as size of the children
    /// </summary>
    /// <param name="availableSize"> Size of parent.</param>
    /// <returns>new desired size</returns>
    protected override Size MeasureOverride(Size availableSize)
    {
        var newSize = new Size();

        //InternalChildren panel collection
        foreach (UIElement item in InternalChildren)
        {
            //child can not be bigger then parent
            item.Measure(availableSize);
            newSize.Width += item.DesiredSize.Width;
            newSize.Height += item.DesiredSize.Height;
        }

        return newSize;
    }


    protected override Size ArrangeOverride(Size finalSize)
    {
        var location = new Point();
        foreach (UIElement item in InternalChildren)
        {
            item.Arrange(new Rect(location,item.DesiredSize));
            location.X += item.DesiredSize.Width;
            location.Y += item.DesiredSize.Height;
        }
        return finalSize;
    }
}
