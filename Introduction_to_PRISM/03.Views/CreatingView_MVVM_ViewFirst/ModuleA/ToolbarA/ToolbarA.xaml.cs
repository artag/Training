using System.Windows.Controls;
using PrismDemo.Infrastructure;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ToolbarA.xaml
    /// </summary>
    public partial class ToolbarA : UserControl, IView
    {
        public ToolbarA()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get => (IViewModel)DataContext;
            set => DataContext = value;
        }
    }
}
