using Avalonia.Controls;
using Avalonia.Interactivity;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace SimpleContactForm
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            var messageBox = MessageBoxManager
                .GetMessageBoxStandardWindow("Save", "  Contact saved.  ", ButtonEnum.Ok);
            messageBox.Show();
        }
    }
}