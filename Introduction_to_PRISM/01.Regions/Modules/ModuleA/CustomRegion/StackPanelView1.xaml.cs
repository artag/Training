using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for StackPanelView1.xaml
    /// </summary>
    public partial class StackPanelView1 : UserControl
    {
        public StackPanelView1()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            button.Content = $"Button. Guid: {shortGuid}";
        }
    }
}
