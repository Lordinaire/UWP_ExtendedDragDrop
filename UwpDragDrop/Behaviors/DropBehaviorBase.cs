using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using UwpDragDrop.DragDrop;

namespace UwpDragDrop.Behaviors
{
    public abstract class DropBehaviorBase : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; set; }

        protected DropPlaceholder Placeholder;

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;

            Placeholder = AssociatedObject as DropPlaceholder;
            if (Placeholder == null)
                return;
        }

        public void Detach()
        {
            if (Placeholder != null)
            {
            }
        }

        protected virtual void OnOverlayChanged(object sender, FrameworkElement overlay)
        {

        }
    }
}