using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WiredBrainCoffee.CustomersApp.Wpf2.Model;

namespace WiredBrainCoffee.CustomersApp.Wpf2.Controls
{
    public partial class CustomerDetailControl : UserControl
    {
        private Customer _customer;
        private bool _isSettingCustomer = false;

        public CustomerDetailControl()
        {
            InitializeComponent();
        }

        [TypeConverter(typeof(CustomerConverter))]
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;

                _isSettingCustomer = true;

                txtFirstName.Text = _customer?.FirstName ?? "";
                txtLastName.Text = _customer?.LastName ?? "";
                chkIsDeveloper.IsChecked = _customer?.IsDeveloper ?? false;

                _isSettingCustomer = false;
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
            if (_isSettingCustomer || Customer == null)
            {
                return;
            }

            Customer.FirstName = txtFirstName.Text;
            Customer.LastName = txtLastName.Text;
            Customer.IsDeveloper = chkIsDeveloper.IsChecked.GetValueOrDefault();
        }
    }
}
