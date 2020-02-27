using System.Windows.Controls;
using Demo.Infrastructure;

namespace Module.People.Views
{
    /// <summary>
    /// Interaction logic for PersonView.xaml
    /// </summary>
    public partial class PersonView : UserControl, IPersonView
    {
        public PersonView()
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
