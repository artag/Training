using System.Windows.Controls;
using PrismDemo.Infrastructure;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ContentAView.xaml
    /// </summary>
    public partial class ContentAView : UserControl, IContentAView
    {
        public ContentAView()
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
