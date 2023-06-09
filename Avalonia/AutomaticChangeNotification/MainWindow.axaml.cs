using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AutomaticChangeNotification
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Person();
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            var person = (Person)DataContext;
            person.FirstName = "My New Name";
        }
    }
}