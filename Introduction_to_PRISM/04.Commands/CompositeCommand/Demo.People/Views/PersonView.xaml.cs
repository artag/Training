using System.Windows.Controls;
using Demo.Infrastructure;

namespace Demo.People.Views
{
    /// <summary>
    /// Interaction logic for PeopleView.xaml
    /// </summary>
    public partial class PeopleView : UserControl, IPersonView
    {
        public PeopleView()
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
