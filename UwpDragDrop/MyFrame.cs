using Windows.UI.Xaml.Controls;

namespace UwpDragDrop
{
    /// <summary>
    /// The frame used by the application
    /// </summary>
    public sealed class MyFrame : Frame
    {
        /// <summary>
        /// Gets or sets the drag drop canvas.
        /// </summary>
        /// <value>
        /// The drag drop canvas.
        /// </value>
        public Canvas DragDropCanvas { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="MyFrame"/> class.
        /// </summary>
        public MyFrame()
        {
            DefaultStyleKey = typeof(MyFrame);
        }

        /// <summary>
        /// On apply template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DragDropCanvas = GetTemplateChild("PART_DragDropCanvas") as Canvas;
        }
    }
}