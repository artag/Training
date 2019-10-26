using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WiredBrainCoffee.CustomersApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ButtonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("Customer added!");
            await messageDialog.ShowAsync();
        }

        private void ButtonDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
