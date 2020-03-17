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
            GlobalCommands.SaveAllCommand.RegisterCommand(SaveCommand);
        }

        public DelegateCommand SaveCommand { get; }

        public string ViewName => $"{Person.LastName} {Person.FirstName}";

        public Person Person
        {
            get => _person;
            set 
            {
                if (_person != null)
                    _person.PropertyChanged -= Person_PropertyChanged;

                _person = value;
                _person.PropertyChanged += Person_PropertyChanged;

                OnPropertyChanged();
            }
        }

        public void CreatePerson(string firstName, string lastName, int age)
        {
            Person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
            };
        }

        private void Person_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void Save()
        {
            Person.LastUpdated = DateTime.Now;
        }

        private bool CanSave()
        {
            return Person != null && Person.Error == null;
        }
    }
}
