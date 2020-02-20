using System.Windows.Controls;
using PrismDemo.Infrastructure;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ContentA.xaml
    /// </summary>
    public partial class ContentA : UserControl, IView
    {
        public ContentA(IContentAViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public IViewModel ViewModel
        {
            get => (IViewModel)DataContext;
            set => DataContext = value;
        }
    }
}
