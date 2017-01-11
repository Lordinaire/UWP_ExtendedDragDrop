namespace UwpDragDrop.DragDrop
{
    /// <summary>
    /// Drag placeholder interface
    /// </summary>
    public interface IDragPlaceholder : IDragDropPlaceholder
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is drag started.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is drag started; otherwise, <c>false</c>.
        /// </value>
        bool IsDragStarted { get; set; }

        /// <summary>
        /// Sets the data.
        /// </summary>
        void SetData();
    }
}