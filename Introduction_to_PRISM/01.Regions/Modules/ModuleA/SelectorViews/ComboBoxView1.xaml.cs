using System;
using System.Windows.Controls;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ComboBoxView1.xaml
    /// </summary>
    public partial class ComboBoxView1 : UserControl
    {
        public ComboBoxView1()
        {
            InitializeComponent();

            var guid = Guid.NewGuid().ToString();
            var shortGuid = guid.Substring(guid.Length - 4);
            textBlock.Text = $"CheckBox. Guid: {shortGuid}";
        }
    }
}
