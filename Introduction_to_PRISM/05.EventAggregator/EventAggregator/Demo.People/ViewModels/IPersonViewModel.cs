using Demo.Infrastructure;

namespace Demo.People.ViewModels
{
    public interface IPersonViewModel : IViewModel
    {
        void CreatePerson(string firstName, string lastName, int age);
    }
}
