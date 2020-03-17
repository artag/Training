using System.Windows.Controls;
using Demo.Infrastructure;

namespace Demo.ToolBar.Views
{
    /// <summary>
    /// Interaction logic for ToolBarView.xaml
    /// </summary>
    public partial class ToolBarView : UserControl, IToolBarView
    {
        public ToolBarView()
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
