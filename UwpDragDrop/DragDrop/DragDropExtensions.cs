using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UwpDragDrop.DragDrop
{
    /// <summary>
    /// Drag drop extensions
    /// </summary>
    public static class DragDropExtensions
    {
        /// <summary>
        /// The is draggable property
        /// </summary>
        public static readonly DependencyProperty IsDraggableProperty = DependencyProperty.RegisterAttached(
             "IsDraggable",
                typeof(bool),
                typeof(DragDropExtensions),
                new PropertyMetadata(default(bool)));

        /// <summary>
        /// Sets the is draggable.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetIsDraggable(DependencyObject element, bool value)
        {
            element.SetValue(IsDraggableProperty, value);
        }

        /// <summary>
        /// Gets the is draggable.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>True if draggable, false otherwise</returns>
        public static bool GetIsDraggable(DependencyObject element)
        {
            return (bool)element.GetValue(IsDraggableProperty);
        }
    }
}
