using System;
using System.ComponentModel;
using Demo.Business;
using Demo.Infrastructure;
using Module.People.Views;
using Prism.Commands;

namespace Module.People.ViewModels
{
    public class PersonViewModel : ViewModelBase, IPersonViewModel
    {
        private Person _person;

        public PersonViewModel(IPersonView view) : base(view)
        {
            CreatePerson();

            // Имена делегатов так криво названы мной. В реальных проектах надо более грамотно называть.
            // Используются для демонстрации использования DelegateCommand в различных конфигурациях.
            // В DelegateCommand<T>, T может быть либо reference, либо Nullable типа.
            SaveCommand = new DelegateCommand(Save, CanSave);
            SaveCommandObject = new DelegateCommand<object>(SaveObject, CanSaveObject);
            SaveCommandNullable = new DelegateCommand<int?>(SaveNullable, CanSaveNullable);
            SaveCommandPerson = new DelegateCommand<Person>(SavePerson, CanSavePerson);
        }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand<object> SaveCommandObject { get; }

        public DelegateCommand<int?> SaveCommandNullable { get; }

        public DelegateCommand<Person> SaveCommandPerson { get; }

        public Person Person
        {
            get => _person;
            set
            {
                _person = value;
                _person.PropertyChanged += Person_PropertyChanged;
                OnPropertyChanged();
            }
        }

        private void CreatePerson()
        {
            Person = new Person()
            {
                FirstName = "Bob",
                LastName = "Smith",
                Age = 46,
            };
        }

        private void Save()
        {
            Person.LastUpdated = DateTime.Now;
        }

        private bool CanSave()
        {
            return Person.Error == null;
        }

        private void SaveObject(object value)
        {
            Person.LastUpdated = DateTime.Now.AddYears(Convert.ToInt32(value));
        }

        private bool CanSaveObject(object value)
        {
            return Person.Error == null;
        }

        private void SaveNullable(int? arg)
        {
            Person.LastUpdated = arg.HasValue
                ? DateTime.Now.AddYears(arg.Value)
                : DateTime.Now;
        }

        private bool CanSaveNullable(int? arg)
        {
            return Person.Error == null;
        }

        private void SavePerson(Person person)
        {
            Person.LastUpdated = DateTime.Now.AddYears(person.Age);
        }

        private bool CanSavePerson(Person person)
        {
            return Person.Error == null;
        }

        private void Person_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Проверка на возможность запуска команды.
            // (Обновляется статус кнопки к которой привязана команда).
            SaveCommand.RaiseCanExecuteChanged();
            SaveCommandObject.RaiseCanExecuteChanged();
            SaveCommandNullable.RaiseCanExecuteChanged();
            SaveCommandPerson.RaiseCanExecuteChanged();
        }
    }
}
