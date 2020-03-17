using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Demo.Infrastructure
{
    public abstract class ViewModelBase : IViewModel, INotifyPropertyChanged
    {
        protected ViewModelBase(IView view)
        {
            View = view;
            View.ViewModel = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IView View { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
