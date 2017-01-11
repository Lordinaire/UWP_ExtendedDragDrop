using System;
using Windows.UI.Xaml;
using UwpDragDrop.DragDrop;

namespace UwpDragDrop
{
    public class AddDropPlaceholder : DropPlaceholder
    {
        public OperationViewModel ViewModel { get; set; }

        public AddDropPlaceholder()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnLoaded;
            ViewModel = DataContext as OperationViewModel;
        }

        public override void DropContent(object content)
        {
            base.DropContent(content);

            var number = (int)content;
            ViewModel.Result += number;
            ViewModel.Operations = string.Concat(ViewModel.Operations, " + ", number);
        }
    }
}
