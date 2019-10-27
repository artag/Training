using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using WiredBrainCoffee.CustomersApp.Model;

namespace WiredBrainCoffee.CustomersApp.DataProvider
{
    public class CustomerDataProvider
    {
        private static readonly string s_customersFileName = "customers.json";
        private static readonly StorageFolder s_localFolder = ApplicationData.Current.LocalFolder;

        public async Task<IEnumerable<Customer>> LoadCustomersAsync()
        {
            var storageFile =
                await s_localFolder.TryGetItemAsync(s_customersFileName) as StorageFile;

            List<Customer> customerList = null;

            if (storageFile == null)
            {
                customerList = new List<Customer>()
                {
                    new Customer { FirstName = "Thomas", LastName = "Huber", IsDeveloper = true },
                    new Customer { FirstName = "Anna", LastName = "Rockstar", IsDeveloper = true },
                    new Customer { FirstName = "Julia", LastName = "Master", },
                    new Customer { FirstName = "Urs", LastName = "Meier", IsDeveloper = true },
                    new Customer { FirstName = "Sara", LastName = "Ramone", },
                    new Customer { FirstName = "Elsa", LastName = "Queen", },
                    new Customer { FirstName = "Alex", LastName = "Baier", IsDeveloper = true },
                };
            }
            else
            {
                using (var stream = await storageFile.OpenAsync(FileAccessMode.Read))
                using (var dataReader = new DataReader(stream))
                {
                    var streamSize = (uint) stream.Size;
                    await dataReader.LoadAsync(streamSize);
                    var json = dataReader.ReadString(streamSize);
                    customerList = JsonConvert.DeserializeObject<List<Customer>>(json);
                }
            }

            return customerList;
        }

        public async Task SaveCustomersAsync(IEnumerable<Customer> customers)
        {
            var storageFile = await s_localFolder.CreateFileAsync(
                s_customersFileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            using (var dataWriter = new DataWriter(stream))
            {
                var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
                dataWriter.WriteString(json);
                await dataWriter.StoreAsync();
            }
        }
    }
}
