using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EMM.Core
{
    public static class ListExtension
    {
        public static List<T> GetSelectedElement<T>(this IList<T> list) where T : IViewModel
        {
            return list.Where<T>(i => i.IsSelected == true).ToList();
        }


        /// <summary>
        /// Move element(s) in a list
        /// </summary>
        /// <param name="step">Move step. Negative to go up, positive to go down</param>
        /// <param name="list">List to move elements from</param>
        public static void MoveSelectedElement<T>(this ObservableCollection<T> list, int step) where T : IViewModel
        {
            int distance = Math.Abs(step);

            var selectedElements = GetSelectedElement(list);

            if (selectedElements.Count == 0)
                return;

            if (step > 0)
                selectedElements.Reverse();

            foreach (var element in selectedElements)
            {
                var oldIndex = list.IndexOf(element);

                if ((oldIndex == 0 && step < 0) || (oldIndex == list.Count - 1 && step > 0))
                    return;

                if (oldIndex < distance && step < 0)
                    step++;
                else if (list.Count - oldIndex - 1 < distance && step > 0)
                    step--;

                var newIndex = list.IndexOf(element) + step;
                list.Move(oldIndex, newIndex);
            }
        }


        /// <summary>
        /// Get the paste index base on selection from the list
        /// </summary>
        /// <typeparam name="T">Type of the list</typeparam>
        /// <param name="list">the list to get index</param>
        /// <returns></returns>
        public static int GetPasteIndex<T>(this IList<T> list, int selectedIndex) where T : IViewModel
        {
            int pasteIndex;
            var selectedItems = GetSelectedElement(list);

            //if list is empty then return -1
            if (list.Count == 0)
                return -1;

            //No selected, paste to the end of the list
            if (selectedIndex < 0)
                pasteIndex = list.Count - 1;
            //Multiple selected, paste after last selected
            else if (selectedItems.Count > 1)
                pasteIndex = list.IndexOf(selectedItems.LastOrDefault());
            //Else paste after the currently selected
            else pasteIndex = selectedIndex;

            return (pasteIndex >= 0) ? pasteIndex : -1;
        }
    }
}
