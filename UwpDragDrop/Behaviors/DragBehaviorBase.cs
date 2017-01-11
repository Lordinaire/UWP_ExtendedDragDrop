using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using UwpDragDrop.DragDrop;

namespace UwpDragDrop.Behaviors
{
    public abstract class DragBehaviorBase : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; set; }

        protected DragPlaceholder Placeholder;

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;

            Placeholder = AssociatedObject as DragPlaceholder;
            if (Placeholder == null)
                return;

            Placeholder.OverlayChanged += OnOverlayChanged;
        }

        public void Detach()
        {
            if (Placeholder != null)
            {
                Placeholder.OverlayChanged += OnOverlayChanged;
                Placeholder.OverlayChanged -= OnOverlayChanged;
            }
        }

        protected virtual void OnOverlayChanged(object sender, FrameworkElement overlay)
        {

        }
    }
}