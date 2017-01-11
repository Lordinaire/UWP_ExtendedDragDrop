using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace UwpDragDrop
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<int> _numbers;

        public ObservableCollection<int> Numbers
        {
            get { return _numbers; }
            set { Set(ref _numbers, value); }
        }

        public OperationViewModel Multiply { get; set; }

        public OperationViewModel Add { get; set; }

        public MainViewModel()
        {
            Multiply = new OperationViewModel { Result = 1, Operations = "1" };
            Add = new OperationViewModel { Result = 0, Operations = "0" };

            Numbers = new ObservableCollection<int>();
            for (int i = 0; i < 10; i++)
            {
                Numbers.Add(i);
            }
        }
    }
}