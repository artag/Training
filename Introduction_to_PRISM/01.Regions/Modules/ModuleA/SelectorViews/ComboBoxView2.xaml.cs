using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ComboBoxView2.xaml
    /// </summary>
    public partial class ComboBoxView2 : UserControl
    {
        public ComboBoxView2()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            textBlock.Text = $"CheckBox. Singleton. Guid: {shortGuid}";
        }
    }
}
