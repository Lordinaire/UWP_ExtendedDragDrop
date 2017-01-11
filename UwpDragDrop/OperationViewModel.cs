using GalaSoft.MvvmLight;

namespace UwpDragDrop
{
    public class OperationViewModel : ViewModelBase
    {
        private int _result;

        public int Result
        {
            get { return _result; }
            set { Set(ref _result, value); }
        }

        private string _operations;

        public string Operations
        {
            get { return _operations; }
            set { Set(ref _operations, value); }
        }
    }
}