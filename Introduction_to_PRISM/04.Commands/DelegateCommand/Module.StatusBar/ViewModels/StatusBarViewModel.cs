using Demo.Infrastructure;
using Module.StatusBar.Views;

namespace Module.StatusBar.ViewModels
{
    public class StatusBarViewModel : ViewModelBase, IStatusBarViewModel
    {
        private string _message = "Ready";

        public StatusBarViewModel(IStatusBarView view) : base(view)
        {
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
    }
}
