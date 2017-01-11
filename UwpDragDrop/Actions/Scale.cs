using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace UwpDragDrop.Actions
{
    public class Scale : CompositionActionBase
    {
        /// <summary>
        /// The scale (x axis) of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register(nameof(ScaleX), typeof(double), typeof(Scale), new PropertyMetadata(1d));

        /// <summary>
        /// The scale (y axis) of the associated object
        /// </summary>
        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register(nameof(ScaleY), typeof(double), typeof(Scale), new PropertyMetadata(1d));

        /// <summary>
        /// The center (x axis) of scale for associated object
        /// </summary>
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(nameof(CenterX), typeof(double), typeof(Scale), new PropertyMetadata(0d));

        /// <summary>
        /// The center (y axis) of scale for associated object
        /// </summary>
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(nameof(CenterY), typeof(double), typeof(Scale), new PropertyMetadata(0d));

        /// <summary>
        /// The target object property
        /// </summary>
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(FrameworkElement), typeof(Scale), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the scale on the x axis.
        /// </summary>
        /// <value>
        /// The scale on the x axis.
        /// </value>
        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale on the y axis.
        /// </summary>
        /// <value>
        /// The scale on the y axis.
        /// </value>
        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale (x axis) of the associated object.
        /// </summary>
        /// <value>
        /// The scale (x axis) of the associated object.
        /// </value>
        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale (y axis) of the associated object.
        /// </summary>
        /// <value>
        /// The scale (y axis) of the associated object.
        /// </value>
        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the target object. This is a dependency property.
        /// </summary>
        public FrameworkElement TargetObject
        {
            get { return (FrameworkElement)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        public override object Execute(object sender, object parameter)
        {
            FrameworkElement element = null;
            if (ReadLocalValue(TargetObjectProperty) != DependencyProperty.UnsetValue)
            {
                element = TargetObject as FrameworkElement;
            }
            else
            {
                element = sender as FrameworkElement;
            }

            if (element == null)
                return false;

            element.Scale(
                    duration: Duration,
                    delay: Delay,
                    centerX: (float)CenterX,
                    centerY: (float)CenterY,
                    scaleX: (float)ScaleX,
                    scaleY: (float)ScaleY)?
                .Start();

            return true;
        }
    }
}