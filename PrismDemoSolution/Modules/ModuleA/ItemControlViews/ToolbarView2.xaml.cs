using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ToolbarView2.xaml
    /// </summary>
    public partial class ToolbarView2 : UserControl
    {
        public ToolbarView2()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            buttonOnToolbar.Content = $"Register as Singleton. Guid: {shortGuid}";
        }
    }
}
