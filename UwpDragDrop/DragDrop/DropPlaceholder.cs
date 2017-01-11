using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UwpDragDrop.DragDrop
{
    /// <summary>
    /// Drop placeholder class
    /// </summary>
    public class DropPlaceholder : UserControl, IDropPlaceholder
    {
        #region Properties

        /// <summary>
        /// The bounds
        /// </summary>
        private Rect _bounds;

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <value>
        /// The bounds.
        /// </value>
        public Rect Bounds
        {
            get { return _bounds; }
        }

        /// <summary>
        /// Gets or sets the last position.
        /// </summary>
        /// <value>
        /// The last position.
        /// </value>
        public Point LastPosition { get; set; }

        /// <summary>
        /// Gets or sets the drag placeholders.
        /// </summary>
        /// <value>
        /// The drag placeholders.
        /// </value>
        public List<IDragPlaceholder> DragPlaceholders { get; set; }

        #endregion

        #region Dependency properties

        #region Group name

        /// <summary>
        /// The group name property
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
             "GroupName",
                typeof(string),
                typeof(DropPlaceholder),
                new PropertyMetadata(default(string), GroupNameChanged));

        /// <summary>
        /// Groups the name changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void GroupNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                DragDropManager.AddOrUpdateGroup(e.NewValue.ToString(), (IDragDropPlaceholder)sender, DragDropGestureType.Drop);
            }
        }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler DropZoneEnter;

        public event EventHandler DropZoneExit;

        public event EventHandler DragStarted;

        public event EventHandler DragEnded;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DropPlaceholder"/> class.
        /// </summary>
        public DropPlaceholder()
        {
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Window.Current.CoreWindow.PointerMoved += OnPointerMoved;
            LayoutUpdated += OnLayoutUpdated;
        }

        /// <summary>
        /// Called when [content layout updated].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnLayoutUpdated(object sender, object e)
        {
            GetBoundsFromElement(this);
        }

        #endregion

        #region Drop

        /// <summary>
        /// Called when pointer moved.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="Windows.UI.Core.PointerEventArgs"/> instance containing the event data.</param>
        private void OnPointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            DragPlaceholders = DragDropManager.GetDragPlaceholder(GroupName);
            if (DragPlaceholders == null)
            {
                return;
            }

            var activePlaceholder = DragPlaceholders.FirstOrDefault(p => p.IsDragStarted);
            if (activePlaceholder == null)
            {
                return;
            }

            var position = new Point(args.CurrentPoint.Position.X - _bounds.X, args.CurrentPoint.Position.Y - _bounds.Y);
            IsDropable = IsCurrentDropZoneValid(position);

            if (IsDropable && activePlaceholder.IsDragStarted)
            {
                OnEnterDropZone();
            }
            else
            {
                OnExitDropZone();
            }
        }

        /// <summary>
        /// Sets the bounds from element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The bounds.</returns>
        public Rect GetBoundsFromElement(FrameworkElement element)
        {
            var transformToVisual = element.TransformToVisual(Window.Current.Content);
            var point = transformToVisual.TransformPoint(new Point(0, 0));
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        /// <summary>
        /// Sets the bounds.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        public void SetBounds(Rect bounds)
        {
            _bounds = bounds;
        }

        #endregion

        #region IDropPlaceholder

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dropable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dropable; otherwise, <c>false</c>.
        /// </value>
        public bool IsDropable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is drop succeed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is drop succeed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDropSucceed { get; set; }

        /// <summary>
        /// Called when drag started.
        /// </summary>
        public virtual void OnDragStarted()
        {
            DragStarted?.Invoke(this, null);
            SetBounds(GetBoundsFromElement(this));
        }

        public virtual void OnDragEnded()
        {
            DropZoneExit?.Invoke(this, null);
            DragEnded?.Invoke(this, null);
        }

        /// <summary>
        /// Determines whether the current drop zone is valid.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>
        /// True if the zone is valid for a drop, otherwise false
        /// </returns>
        public virtual bool IsCurrentDropZoneValid(Point position)
        {
            if (position.X >= 0
                && position.X <= _bounds.Width
                && position.Y >= 0
                && position.Y <= _bounds.Height)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called when enter drop zone.
        /// </summary>
        public virtual void OnEnterDropZone()
        {
            DropZoneEnter?.Invoke(this, null);
        }

        /// <summary>
        /// Called when exit drop zone.
        /// </summary>
        public virtual void OnExitDropZone()
        {
            DropZoneExit?.Invoke(this, null);
        }

        /// <summary>
        /// Drops the content.
        /// </summary>
        /// <param name="content">The content.</param>
        public virtual void DropContent(object content)
        {
            
        }

        #endregion
    }
}
