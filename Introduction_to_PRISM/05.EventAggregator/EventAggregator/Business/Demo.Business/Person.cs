using System;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Demo.Business
{
    public class Person : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _firstName;
        private string _lastName;
        private int _age;
        private DateTime? _lastUpdated;
        private string _error;

        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged();
            }
        }

        public DateTime? LastUpdated
        {
            get => _lastUpdated;
            set
            {
                _lastUpdated = value;
                OnPropertyChanged();
            }
        }

        public string Error
        {
            get => _error;
            private set
            {
                _error = value;
                OnPropertyChanged();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case nameof(FirstName):
                        if (string.IsNullOrEmpty(_firstName))
                        {
                            error = "First Name required";
                        }

                        break;
                    case nameof(LastName):
                        if (string.IsNullOrEmpty(_lastName))
                        {
                            error = "Last Name required";
                        }

                        break;
                    case nameof(Age):
                        if (_age < 18 || _age > 85)
                        {
                            error = "Age out of range";
                        }

                        break;
                }

                Error = error;

                return error;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
