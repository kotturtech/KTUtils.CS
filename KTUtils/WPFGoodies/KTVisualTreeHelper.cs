using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace KotturTech.WPFGoodies
{
    /// <summary>
    /// KTVisualTreeHelper - Contains helper methods for Visual Tree search, on top of VisualTreeHelper class
    /// </summary>
    public static class KTVisualTreeHelper
    {
        /// <summary>
        /// Finds all children of a Visual, of specified type
        /// </summary>
        /// <typeparam name="T">Type of children to find</typeparam>
        /// <param name="root">Root element</param>
        /// <param name="collection">Collection that shall contain the search results</param>
        public static void FindChildrenOfType<T>(Visual root, IList<T> collection) where T : Visual
        {
            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                // Retrieve child visual at specified index value.
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(root, i);

                // Do processing of the child visual object.
                if(childVisual is T)
                    collection.Add(childVisual as T);

                // Enumerate children of the child visual object.
                FindChildrenOfType<T>(childVisual, collection);
            }
        }

        /// <summary>
        /// Finds all children of a root Visual, of specified type
        /// </summary>
        /// <typeparam name="T">Type of children to find</typeparam>
        /// <param name="root">Root visual</param>
        /// <returns>Collection that contains search results. If nothing found, the collection will be empty.</returns>
        public static IEnumerable<T> FindChildrenOfType<T>(Visual root) where T : Visual
        {
            var list = new List<T>();
            KTVisualTreeHelper.FindChildrenOfType(root, list);
            return list;
        }
    }
}
