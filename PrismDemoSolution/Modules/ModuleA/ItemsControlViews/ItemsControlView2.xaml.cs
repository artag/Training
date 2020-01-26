using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ItemsControlView2.xaml
    /// </summary>
    public partial class ItemsControlView2 : UserControl
    {
        public ItemsControlView2()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            buttonOnToolbar.Content = $"Items Control. Singleton. Guid: {shortGuid}";
        }
    }
}
