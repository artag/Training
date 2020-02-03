using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ListBoxView2.xaml
    /// </summary>
    public partial class ListBoxView2 : UserControl
    {
        public ListBoxView2()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            textBlock.Text = $"ListBox. Singleton. Guid: {shortGuid}";
        }
    }
}
