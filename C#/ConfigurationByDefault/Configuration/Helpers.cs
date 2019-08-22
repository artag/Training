using System;
using System.Collections.Generic;

namespace Configuration
{
    static class Helpers
    {
        public static IEnumerable<KeyValuePair<string, string>> CreateInMemorySettings()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("App:Name", "AppNameFromMemory"),
                new KeyValuePair<string, string>("App:BuildDate", "2017-07-13T10:02:02"),
                new KeyValuePair<string, string>("User:Name", "UserNameFromMemory"),
                new KeyValuePair<string, string>("User:BirthDate", "2017-07-12T11:00:00"),
            };
        }

        public static void DisplayValuesFromPoco(AppConfiguration appConfig, string title)
        {
            Console.WriteLine($"--- Title {title}\n");

            var appName = appConfig.App.Name;
            var appBuildDate = appConfig.App.BuildDate;

            var userName = appConfig.User.Name;
            var userBirthDate = appConfig.User.BirthDate;

            Console.WriteLine($"App Name: {appName}");
            Console.WriteLine($"App Build Date: {appBuildDate}");
            Console.WriteLine($"User Name: {userName}");
            Console.WriteLine($"User BirthDate: {userBirthDate}");
            Console.WriteLine("----------------------------------------------------\n");
        }
    }
}
