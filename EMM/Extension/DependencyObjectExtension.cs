using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EMM
{
    public static class DependencyObjectExtension
    {
        public static T FindParentOfType<T>(this DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentDepObj = child;
            do
            {
                parentDepObj = VisualTreeHelper.GetParent(parentDepObj);
                T parent = parentDepObj as T;
                if (parent != null) return parent;
            }
            while (parentDepObj != null);
            return null;
        }

        /// <summary>
        /// Find the top most level parent of type T. Return null if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T FindTopLevelParentOfType<T>(this DependencyObject child) where T : DependencyObject
        {
            var stack = new Stack<T>();

            var parent = child.FindParentOfType<T>();

            if (parent != null)
            {
                stack.Push(parent);
            }

            while(stack.Count > 0)
            {
                var currentItem = stack.Pop();

                parent = currentItem.FindParentOfType<T>();

                if (parent != null)
                {
                    stack.Push(parent);
                    continue;
                }

                parent = currentItem;
            }

            return parent;
        }
    }
}
