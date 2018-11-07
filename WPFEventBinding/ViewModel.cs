using System.ComponentModel;

namespace WPFEventBinding
{
    class ViewModel : INotifyPropertyChanged
    {
        private int _value = 0;

        #region Properties

        public int DefaultValue => 0;

        public int MinimumValue => -100;

        public int MaximumValue => 100;

        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;

                    OnPropertyChanged(nameof(Value));
                    IncreaseValueCommand?.RaiseCanExecuteChanged();
                    DecreaseValueCommand?.RaiseCanExecuteChanged();
                    ResetValueCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Command Properties

        public RelayCommand IncreaseValueCommand { get; }

        public RelayCommand DecreaseValueCommand { get; }

        public RelayCommand ResetValueCommand { get; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            IncreaseValueCommand = new RelayCommand(_ => Value++, _ => Value <= MaximumValue);
            DecreaseValueCommand = new RelayCommand(_ => Value--, _ => Value >= MinimumValue);
            ResetValueCommand = new RelayCommand(_ => Value = DefaultValue, _ => Value != DefaultValue);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
