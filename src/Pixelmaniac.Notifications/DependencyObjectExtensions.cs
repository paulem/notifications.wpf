using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Pixelmaniac.Notifications
{
    internal static class DependencyObjectExtensions
    {
        internal static T FindVisualParent<T>(this DependencyObject dependencyObject) where T : class
        {
            for (; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
            {
                var obj = dependencyObject as T;

                if (obj != null)
                    return obj;
            }

            return default(T);
        }

        internal static T FindVisualChild<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            return dependencyObject.FindVisualChildren<T>().FirstOrDefault();
        }

        internal static IEnumerable<T> FindVisualChildren<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                if (child is T)
                    yield return (T)child;
                foreach (T visualChild in child.FindVisualChildren<T>())
                    yield return visualChild;
            }
        }
    }
}
