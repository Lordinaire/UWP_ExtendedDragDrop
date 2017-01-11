using Windows.UI.Xaml;

namespace UwpDragDrop.Behaviors
{
    public class DragTransparentBehavior : DragBehaviorBase
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(DragTransparentBehavior), new PropertyMetadata(default(double)));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        protected override void OnOverlayChanged(object sender, FrameworkElement overlay)
        {
            if (overlay != null)
            {
                overlay.Opacity = Value;
            }
        }
    }
}
