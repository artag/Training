using System.Windows.Controls;
using PrismDemo.Infrastructure;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ToolbarAView.xaml
    /// </summary>
    public partial class ToolbarAView : UserControl, IToolbarAView
    {
        public ToolbarAView()
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
