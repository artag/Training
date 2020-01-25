using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for TabControlView2.xaml
    /// </summary>
    public partial class TabControlView2 : UserControl
    {
        public TabControlView2()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            textBlock.Text = $"TabControl. Singleton. Guid: {shortGuid}";
        }
    }
}
