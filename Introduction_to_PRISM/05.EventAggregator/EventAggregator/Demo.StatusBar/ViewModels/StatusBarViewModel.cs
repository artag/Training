using Demo.Infrastructure;
using Demo.StatusBar.Views;
using Prism.Events;

namespace Demo.StatusBar.ViewModels
{
    public class StatusBarViewModel : ViewModelBase, IStatusBarViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private string _message;

        public StatusBarViewModel(IStatusBarView view, IEventAggregator eventAggregator)
            : base(view)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PersonUpdatedEvent>().Subscribe(OnPersonUpdated);
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

        private void OnPersonUpdated(string fullName)
        {
            Message = $"{fullName} was updated.";
        }
    }
}
