using Demo.Infrastructure;
using Demo.ToolBar.Views;

namespace Demo.ToolBar.ViewModels
{
    public class ToolBarViewModel : ViewModelBase, IToolBarViewModel
    {
        public ToolBarViewModel(IToolBarView view) : base(view)
        {
        }
    }
}
