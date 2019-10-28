using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WiredBrainCoffee.CustomersApp.Model;

namespace WiredBrainCoffee.CustomersApp.Controls
{
    public sealed partial class CustomerDetailControl : UserControl
    {
        private Customer _customer;

        public CustomerDetailControl()
        {
            this.InitializeComponent();
        }

        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;

                txtFirstName.Text = _customer?.FirstName ?? string.Empty;
                txtLastName.Text = _customer?.LastName ?? string.Empty;
                chkIsDeveloper.IsChecked = _customer?.IsDeveloper;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCustomer();
        }

        private void CheckBox_IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateCustomer();
        }

        private void UpdateCustomer()
        {
            if (Customer == null)
            {
                return;
            }

            Customer.FirstName = txtFirstName.Text;
            Customer.LastName = txtLastName.Text;
            Customer.IsDeveloper = chkIsDeveloper.IsChecked.GetValueOrDefault();
        }
    }
}
