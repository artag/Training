using System.Windows.Controls;
using PrismDemo.Infrastructure;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for ContentA.xaml
    /// </summary>
    public partial class ContentA : UserControl, IContentAView
    {
        public ContentA()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get => (IContentAViewModel)DataContext;
            set => DataContext = value; 
        }
    }
}
