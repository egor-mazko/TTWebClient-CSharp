using System;
using System.Windows.Input;

namespace TTWebClientUI
{
    public class Command : ICommand
    {
        private bool _isEnabled = true;
        private readonly Action _actionToRun;

        public event EventHandler CanExecuteChanged;

        public Command(Action actionToRun)
        {
            _actionToRun = actionToRun;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            _actionToRun();
        }
    }
}
