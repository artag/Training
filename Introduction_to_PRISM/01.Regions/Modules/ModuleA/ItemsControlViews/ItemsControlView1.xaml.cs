using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ItemsControlView1.xaml
    /// </summary>
    public partial class ItemsControlView1 : UserControl
    {
        public ItemsControlView1()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            buttonOnToolbar.Content = $"Items Control. Guid: {shortGuid}";
        }
    }
}
