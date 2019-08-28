namespace Immutability
{
    public class UserProfile
    {
        private User _user;
        private string _address;

        // Здесь side effect
        public void UpdateUser(int userId, string name)
        {
            _user = new User(userId, name);
        }
    }

    public class User
    {
        public int Id { get; }
        public string Name { get; }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
