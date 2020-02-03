using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ListBoxView1.xaml
    /// </summary>
    public partial class ListBoxView1 : UserControl
    {
        public ListBoxView1()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            textBlock.Text = $"ListBox. Guid: {shortGuid}";
        }
    }
}
