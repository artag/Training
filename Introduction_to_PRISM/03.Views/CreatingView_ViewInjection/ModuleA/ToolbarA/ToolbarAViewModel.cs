using PrismDemo.Infrastructure;

namespace ModuleA
{
    public class ToolbarAViewModel : IToolbarAViewModel
    {
        public ToolbarAViewModel(IToolbarAView view)
        {
            View = view;
            View.ViewModel = this;
        }

        public IView View { get; set; }

        public string ButtonText { get; set; }
    }
}
