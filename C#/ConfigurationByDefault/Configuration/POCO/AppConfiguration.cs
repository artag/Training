using System;

namespace Configuration
{
    class AppConfiguration
    {
        public AppSettings App { get; set; }
        public UserSettings User { get; set; }
    }

    class AppSettings
    {
        public string Name { get; set; }
        public DateTime BuildDate { get; set; }
    }

    class UserSettings
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
