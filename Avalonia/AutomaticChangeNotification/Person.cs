using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutomaticChangeNotification;

public class Person : INotifyPropertyChanged
{
    private string _firstName;

    public Person()
    {
        _firstName = "Init Name";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged();
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
