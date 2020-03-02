using System;
using System.ComponentModel;
using Demo.Business;
using Demo.Infrastructure;
using Demo.People.Views;
using Prism.Commands;

namespace Demo.People.ViewModels
{
    public class PersonViewModel : ViewModelBase, IPersonViewModel
    {
        private Person _person;

        public PersonViewModel(IPersonView view) : base(view)
        {
            SaveCommand = new DelegateCommand(Save, CanSave);
        }

        public DelegateCommand SaveCommand { get; }

        private void Save()
        {
            Person.LastUpdated = DateTime.Now;
        }

        private bool CanSave()
        {
            return Person != null && Person.Error == null;
        }

        public string ViewName => $"{Person.LastName}, {Person.FirstName}";

        public Person Person
        {
            get => _person;
            set
            {
                _person = value;
                _person.PropertyChanged += Person_OnPropertyChanged;
                OnPropertyChanged();
            }
        }

        public void CreatePerson(string firstName, string lastName)
        {
            Person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                Age = 0,
            };
        }

        private void Person_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
