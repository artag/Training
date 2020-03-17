using Demo.Infrastructure;
using Demo.StatusBar.Views;

namespace Demo.StatusBar.ViewModels
{
    public class StatusBarViewModel : ViewModelBase, IStatusBarViewModel
    {
        private string message;

        public StatusBarViewModel(IStatusBarView view) : base(view)
        {
        }

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }
    }
}
