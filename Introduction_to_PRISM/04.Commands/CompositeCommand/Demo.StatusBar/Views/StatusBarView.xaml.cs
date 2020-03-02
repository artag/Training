using System.Windows.Controls;
using Demo.Infrastructure;

namespace Demo.StatusBar.Views
{
    /// <summary>
    /// Interaction logic for StatusBarView.xaml
    /// </summary>
    public partial class StatusBarView : UserControl, IStatusBarView
    {
        public StatusBarView()
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
