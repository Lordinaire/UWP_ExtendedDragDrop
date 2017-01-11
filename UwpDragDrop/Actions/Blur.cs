using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;

namespace UwpDragDrop.Actions
{
    public class Blur : CompositionActionBase
    {
        /// <summary>
        /// The Opacity value of the associated object
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(Fade), new PropertyMetadata(1d));

        /// <summary>
        /// Gets or sets the Opacity.
        /// </summary>
        /// <value>
        /// The Opacity.
        /// </value>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public override object Execute(object sender, object parameter)
        {
            var element = sender as FrameworkElement;
            if (element == null)
                return false;

            if (AnimationExtensions.IsBlurSupported)
            {
                element.Blur(duration: Duration, delay: Delay, value: (float)Value)?.Start();
            }
            return true;
        }
    }
}
