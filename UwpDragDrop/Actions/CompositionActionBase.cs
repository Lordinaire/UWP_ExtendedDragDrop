using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Xaml.Interactivity;

namespace UwpDragDrop.Actions
{
    /// <summary>
    /// A base class for all behaviors using composition.It contains some of the common propeties to set on a visual.
    /// </summary>
    public abstract class CompositionActionBase : DependencyObject, IAction
    {
        /// <summary>
        /// The duration of the animation.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof(Duration), typeof(double), typeof(CompositionBehaviorBase), new PropertyMetadata(1d));

        /// <summary>
        /// The delay of the animation.
        /// </summary>
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof(Delay), typeof(double), typeof(CompositionBehaviorBase), new PropertyMetadata(0d));

        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>
        /// The delay.
        /// </value>
        public double Delay
        {
            get { return (double)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public abstract object Execute(object sender, object parameter);
    }
}