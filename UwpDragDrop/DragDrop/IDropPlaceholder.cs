using Windows.Foundation;

namespace UwpDragDrop.DragDrop
{
    /// <summary>
    /// Drop placeholder interface
    /// </summary>
    public interface IDropPlaceholder : IDragDropPlaceholder
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is dropable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dropable; otherwise, <c>false</c>.
        /// </value>
        bool IsDropable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is drop succeed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is drop succeed; otherwise, <c>false</c>.
        /// </value>
        bool IsDropSucceed { get; set; }

        /// <summary>
        /// Called when drag started.
        /// </summary>
        void OnDragStarted();

        /// <summary>
        /// Called when drag ended.
        /// </summary>
        void OnDragEnded();

        /// <summary>
        /// Determines whether [is current drop zone valid] [the specified pointer].
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>
        /// True if the zone is valid for a drop, otherwise false
        /// </returns>
        bool IsCurrentDropZoneValid(Point position);

        /// <summary>
        /// Called when [enter drop zone].
        /// </summary>
        void OnEnterDropZone();

        /// <summary>
        /// Called when [exit drop zone].
        /// </summary>
        void OnExitDropZone();

        /// <summary>
        /// Drops the content.
        /// </summary>
        /// <param name="content">The content.</param>
        void DropContent(object content);
    }
}