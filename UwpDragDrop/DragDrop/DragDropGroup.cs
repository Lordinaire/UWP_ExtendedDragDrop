using System.Collections.Generic;
using Windows.UI.Xaml;

namespace UwpDragDrop.DragDrop
{
    /// <summary>
    /// Drag / Drop group class
    /// </summary>
    public class DragDropGroup
    {
        /// <summary>
        /// Gets or sets the drag panels.
        /// </summary>
        /// <value>
        /// The drag panels.
        /// </value>
        public List<IDragPlaceholder> DragPanels { get; set; }

        /// <summary>
        /// Gets or sets the drop panels.
        /// </summary>
        /// <value>
        /// The drop panels.
        /// </value>
        public List<IDropPlaceholder> DropPanels { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the overlay.
        /// </summary>
        /// <value>
        /// The overlay.
        /// </value>
        public FrameworkElement Overlay { get; set; }
    }
}