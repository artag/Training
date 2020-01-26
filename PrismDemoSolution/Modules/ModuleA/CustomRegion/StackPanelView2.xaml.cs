using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for StackPanelView2.xaml
    /// </summary>
    public partial class StackPanelView2 : UserControl
    {
        public StackPanelView2()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            button.Content = $"Button. Singleton. Guid: {shortGuid}";
        }
    }
}
