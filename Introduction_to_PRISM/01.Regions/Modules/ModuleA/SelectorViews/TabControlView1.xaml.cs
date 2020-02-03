using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for TabControlView1.xaml
    /// </summary>
    public partial class TabControlView1 : UserControl
    {
        public TabControlView1()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            textBlock.Text = $"TabControl. Guid: {shortGuid}";
        }

        private void RepeatButton_OnClick(object sender, RoutedEventArgs e)
        {
            var numberString = Regex.Match(repeatButton.Content.ToString(), @"\d+").Value;
            var successfullyParsed = int.TryParse(numberString, out var i);
            if (successfullyParsed)
            {
                i++;
                repeatButton.Content = $"Pressed {i} times";
            }
        }
    }
}
