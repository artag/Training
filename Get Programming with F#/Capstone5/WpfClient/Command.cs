using System;
using System.Windows.Input;

namespace WpfClient
{
    public class Command<T> : ICommand
    {
        private readonly Action<T> _command;
        private readonly Func<bool> _canExecute;
        private readonly Func<object, Tuple<bool, T>> _tryParse;

        public event EventHandler CanExecuteChanged;

        public Command(Action<T> command, Func<object, Tuple<bool, T>> tryParse, Func<bool> canExecute = null)
        {
            _command = command;
            _tryParse = tryParse;
            _canExecute = canExecute ?? (() => true);
        }

        public void Refresh()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            var (successParsed, parsedValue) = _tryParse(parameter);
            if (successParsed)
            {
                _command(parsedValue);
                this.Refresh();
            }
        }
    }
}
