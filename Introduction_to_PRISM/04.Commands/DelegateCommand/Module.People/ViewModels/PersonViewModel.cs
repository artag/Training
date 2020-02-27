using Demo.Business;
using Demo.Infrastructure;
using Module.People.Views;

namespace Module.People.ViewModels
{
    public class PersonViewModel : ViewModelBase, IPersonViewModel
    {
        private Person _person;

        public PersonViewModel(IPersonView view) : base(view)
        {
            CreatePerson();
        }

        public Person Person
        {
            get => _person;
            set
            {
                _person = value;
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
    }
}
