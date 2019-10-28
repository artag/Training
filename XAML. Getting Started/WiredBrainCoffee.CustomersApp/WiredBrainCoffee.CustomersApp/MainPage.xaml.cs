using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WiredBrainCoffee.CustomersApp.DataProvider;
using WiredBrainCoffee.CustomersApp.Model;

namespace WiredBrainCoffee.CustomersApp
{
    public sealed partial class MainPage : Page
    {
        private readonly CustomerDataProvider _customerDataProvider;

        public MainPage()
        {
            this.InitializeComponent();

            _customerDataProvider = new CustomerDataProvider();

            this.Loaded += MainPage_Loaded;
            App.Current.Suspending += App_Suspending;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            customerListView.Items.Clear();
            
            var customers = await _customerDataProvider.LoadCustomersAsync();
            foreach (var customer in customers)
            {
                customerListView.Items.Add(customer);
            }
        }

        private async void App_Suspending(object sender, SuspendingEventArgs e)
        {
            // Requests that the app suspending operation be delayed.
            var deferral = e.SuspendingOperation.GetDeferral();

            await _customerDataProvider.SaveCustomersAsync(
                customerListView.Items.OfType<Customer>());

            // Notifies the operating system that the app has saved its data
            // and is ready to be suspended.
            deferral.Complete();
        }

        private void ButtonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer { FirstName = "New" };
            customerListView.Items.Add(customer);
            customerListView.SelectedItem = customer;
        }

        private void ButtonDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = customerListView.SelectedItem as Customer;
            if (customer == null)
            {
                return;
            }

            customerListView.Items.Remove(customer);
        }

        private void ButtonMove_Click(object sender, RoutedEventArgs e)
        {
            // Get Attached Properties
            // var column = (int)customerListGrid.GetValue(Grid.ColumnProperty);   // Way 1
            var column = Grid.GetColumn(customerListGrid);                         // Way 2

            var newColumn = column == 0 ? 2 : 0;

            // Set Attached Properties
            // customerListGrid.SetValue(Grid.ColumnProperty, newColumn);          // Way 1
            Grid.SetColumn(customerListGrid, newColumn);                           // Way 2

            moveSymbolIcon.Symbol = newColumn == 0 ? Symbol.Forward : Symbol.Back;
        }

        private void CustomerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var customer = customerListView.SelectedItem as Customer;
            customerDetailControl.Customer = customer;
        }
    }
}
