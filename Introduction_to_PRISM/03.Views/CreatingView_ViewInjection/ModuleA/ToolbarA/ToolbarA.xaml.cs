using System.Windows.Controls;
using PrismDemo.Infrastructure;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ToolbarA.xaml
    /// </summary>
    public partial class ToolbarA : UserControl, IToolbarAView
    {
        public ToolbarA()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get => (IToolbarAViewModel)DataContext;
            set => DataContext = value;
        }
    }
}
