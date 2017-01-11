using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace UwpDragDrop.DragDrop
{
    /// <summary>
    /// Drag placeholder class
    /// </summary>
    public class DragPlaceholder : UserControl, IDragPlaceholder
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is drag started.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is drag started; otherwise, <c>false</c>.
        /// </value>
        public bool IsDragStarted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is draggable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is draggable; otherwise, <c>false</c>.
        /// </value>
        public bool IsDraggable { get; set; }

        /// <summary>
        /// Gets or sets the drop placeholder.
        /// </summary>
        /// <value>
        /// The drop placeholder.
        /// </value>
        public List<IDropPlaceholder> DropPlaceholders { get; set; }

        /// <summary>
        /// The _transform overlay
        /// </summary>
        private TranslateTransform _transformOverlay;

        /// <summary>
        /// The is drag in progress
        /// </summary>
        private bool _isDragInProgress;

        /// <summary>
        /// The pointer position
        /// </summary>
        private Point _pointerPosition;

        /// <summary>
        /// The _is event launched
        /// </summary>
        private bool _isEventLaunched;

        #endregion

        #region Dependency properties

        #region Overlay

        /// <summary>
        /// The overlay property
        /// </summary>
        public static readonly DependencyProperty OverlayProperty = DependencyProperty.Register(
                "Overlay",
                typeof(FrameworkElement),
                typeof(DragPlaceholder),
                new PropertyMetadata(default(FrameworkElement), OnOverlayChanged));

        private static void OnOverlayChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var placeholder = dependencyObject as DragPlaceholder;
            if (placeholder != null)
            {
                placeholder.OnOverlayChanged(placeholder.Overlay);
            }
        }

        /// <summary>
        /// Gets or sets the overlay.
        /// </summary>
        /// <value>
        /// The overlay.
        /// </value>
        public FrameworkElement Overlay
        {
            get { return (FrameworkElement)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        #endregion Overlay

        #region Data

        /// <summary>
        /// The data property
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
             "Data",
                typeof(object),
                typeof(DragPlaceholder),
                new PropertyMetadata(default(object)));

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        #endregion Data

        #region Group name

        /// <summary>
        /// The group name property
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
             "GroupName",
                typeof(string),
                typeof(DragPlaceholder),
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
                DragDropManager.AddOrUpdateGroup(e.NewValue.ToString(), (IDragDropPlaceholder)sender, DragDropGestureType.Drag);
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

        #endregion Group name

        #endregion Dependency properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DragPlaceholder"/> class.
        /// </summary>
        public DragPlaceholder()
        {
            IsDraggable = true;
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SetContentDraggable();
            SetOverlayDraggable();
        }

        /// <summary>
        /// Sets the content draggable.
        /// </summary>
        private void SetContentDraggable()
        {
            if (Content != null && Overlay != null)
            {
                Content.PointerPressed += OnPointerPressed;
                Content.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                Content.ManipulationStarting += OnManipulationStarting;
                Content.ManipulationStarted += OnManipulationStarted;
                Content.ManipulationDelta += OnManipulationDelta;
                Content.ManipulationCompleted += OnManipulationCompleted;
            }
        }

        /// <summary>
        /// Handles the PointerPressed event of the Content control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PointerRoutedEventArgs"/> instance containing the event data.</param>
        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(this).Position;
            var ttv = TransformToVisual(Window.Current.Content);
            _pointerPosition = ttv.TransformPoint(point);
        }

        /// <summary>
        /// Sets the overlay draggable.
        /// </summary>
        private void SetOverlayDraggable()
        {
            if (Overlay != null)
            {
                _transformOverlay = new TranslateTransform();
                _transformOverlay.X = -10;
                _transformOverlay.Y = -10;
                Overlay.RenderTransform = _transformOverlay;
                Overlay.IsHitTestVisible = false;
                Overlay.SetValue(Canvas.ZIndexProperty, 99);
            }
        }

        #endregion Constructor

        #region Drag

        /// <summary>
        /// Called when [manipulation starting].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ManipulationStartingRoutedEventArgs"/> instance containing the event data.</param>
        private void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            if (!IsDraggable)
            {
                return;
            }

            _isDragInProgress = false;

            FrameworkElement elementToDrag = null;
            if (IsDraggable)
            {
                elementToDrag = this;
            }
            else
            {
                elementToDrag = GetDraggableControl<FrameworkElement>(this);
                if (elementToDrag == null)
                {
                    _isDragInProgress = false;
                    return;
                }
            }

            var transformToVisual = elementToDrag.TransformToVisual(Window.Current.Content);
            var point = transformToVisual.TransformPoint(new Point(0, 0));
            var bounds = new Rect(point, new Size(elementToDrag.ActualWidth, elementToDrag.ActualHeight));

            if (bounds.Contains(point))
            {
                _isDragInProgress = true;
            }

            DropPlaceholders = DragDropManager.GetDropPlaceholder(GroupName);
        }

        /// <summary>
        /// Called when [manipulation started].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ManipulationStartedRoutedEventArgs"/> instance containing the event data.</param>
        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (!IsDraggable || !_isDragInProgress || DropPlaceholders == null)
            {
                return;
            }

            SetData();
            IsDragStarted = true;

            var point = e.Position;
            var ttv = TransformToVisual(Window.Current.Content);
            _pointerPosition = ttv.TransformPoint(point);

            _transformOverlay.X = 0;
            _transformOverlay.Y = 0;
            Overlay.SetValue(Canvas.LeftProperty, _pointerPosition.X);
            Overlay.SetValue(Canvas.TopProperty, _pointerPosition.Y);
            Overlay.DataContext = Data;

            Overlay.Loaded -= OnOverlayLoaded;
            Overlay.Loaded += OnOverlayLoaded;

            if (DragDropManager.DragDropCanvas.Children.Contains(Overlay))
            {
                DragDropManager.DragDropCanvas.Children.Remove(Overlay);
            }

            _isEventLaunched = false;
        }

        /// <summary>
        /// Called when [overlay loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnOverlayLoaded(object sender, object e)
        {
            _transformOverlay.X -= Overlay.ActualWidth / 2;
            _transformOverlay.Y -= Overlay.ActualHeight / 2;
        }

        /// <summary>
        /// Called when [manipulation delta].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ManipulationDeltaRoutedEventArgs"/> instance containing the event data.</param>
        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!IsDraggable || !_isDragInProgress)
            {
                return;
            }

            // USED TO SEE IF MOUSE MOVED OUT OF BOUND BEFORE STARTING THE DRAG
            if (!_isEventLaunched)
            {
                var mousePos = Window.Current.CoreWindow.PointerPosition;

                var ttv = TransformToVisual(Window.Current.Content);
                var topLeftPos = ttv.TransformPoint(new Point(0, 0));

                if (mousePos.X < topLeftPos.X || mousePos.X > topLeftPos.X + ActualWidth ||
                    mousePos.Y < topLeftPos.Y || mousePos.Y > topLeftPos.Y + ActualHeight)
                {
                    _isEventLaunched = true;
                    DragDropManager.DragDropCanvas.Children.Add(Overlay);
                    DragDropManager.SetOverlay(GroupName, Overlay);

                    foreach (var placeholder in DropPlaceholders)
                    {
                        placeholder.OnDragStarted();
                    }
                }
            }

            _transformOverlay.X += e.Delta.Translation.X;
            _transformOverlay.Y += e.Delta.Translation.Y;
        }

        /// <summary>
        /// Called when [drag completed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ManipulationCompletedRoutedEventArgs"/> instance containing the event data.</param>
        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            IsDragStarted = false;

            if (DragDropManager.DragDropCanvas.Children.Contains(Overlay))
            {
                DragDropManager.DragDropCanvas.Children.Remove(Overlay);
            }

            if (!IsDraggable || !_isDragInProgress || !_isEventLaunched)
            {
                return;
            }

            Overlay.Loaded -= OnOverlayLoaded;
            DragDropManager.SetOverlay(GroupName, null);

            foreach (var placeholder in DropPlaceholders)
            {
                placeholder.OnDragEnded();
                if (placeholder.IsDropable)
                {
                    placeholder.IsDropSucceed = true;
                    SetData();
                    placeholder.DropContent(DragDropManager.GetData(GroupName));
                }
                else
                {
                    _transformOverlay.X = 0;
                    _transformOverlay.Y = 0;
                }
            }
        }

        #endregion Drag

        #region IDragPlaceholder

        /// <summary>
        /// Sets the data.
        /// </summary>
        public virtual void SetData()
        {
            DragDropManager.SetData(GroupName, Data);
        }

        #endregion IDragPlaceholder

        #region Events

        public event EventHandler<FrameworkElement> OverlayChanged;

        protected void OnOverlayChanged(FrameworkElement overlay)
        {
            OverlayChanged?.Invoke(this, overlay);
        }

        #endregion

        /// <summary>
        /// Gets the draggable control.
        /// </summary>
        /// <typeparam name="T">FrameworkElement type</typeparam>
        /// <param name="root">The root.</param>
        /// <returns>Draggable control, null otherwise</returns>
        public T GetDraggableControl<T>(DependencyObject root) where T : FrameworkElement
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                var isDraggable = (bool)child.GetValue(DragDropExtensions.IsDraggableProperty);
                var control = child as T;

                if (isDraggable)
                {
                    return control;
                }

                control = GetDraggableControl<T>(child);

                if (control != null)
                {
                    return control;
                }
            }

            return null;
        }


    }
}
