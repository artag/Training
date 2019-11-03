using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WiredBrainCoffee.CustomersApp.Wpf2.DataProvider;
using WiredBrainCoffee.CustomersApp.Wpf2.Model;

namespace WiredBrainCoffee.CustomersApp.Wpf2
{
    public partial class MainWindow : Window
    {
        private readonly CustomerDataProvider _customerDataProvider;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;

            _customerDataProvider = new CustomerDataProvider();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            customerListView.Items.Clear();

            var customers = await _customerDataProvider.LoadCustomersAsync();
            foreach (var customer in customers)
            {
                customerListView.Items.Add(customer);
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            var customers = customerListView.Items.OfType<Customer>();
            _customerDataProvider.SaveCustomersAsync(customers).Wait();
        }

        private void ButtonMove_Click(object sender, RoutedEventArgs e)
        {
            var column = Grid.GetColumn(customerListGrid);
            var newColumn = column == 0 ? 2 : 0;
            Grid.SetColumn(customerListGrid, newColumn);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer { FirstName = "New" };
            customerListView.Items.Add(customer);
            customerListView.SelectedItem = customer;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var customer = customerListView.SelectedItem as Customer;
            if (customer == null)
            {
                return;
            }

            customerListView.Items.Remove(customer);
        }

        private void CustomerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var customer = customerListView.SelectedItem as Customer;
            if (customer == null)
            {
                return;
            }

            customerDetailControl.Customer = customer;
        }
    }
}
