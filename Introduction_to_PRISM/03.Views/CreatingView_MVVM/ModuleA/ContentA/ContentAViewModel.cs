using PrismDemo.Infrastructure;

namespace ModuleA
{
    public class ContentAViewModel : IContentAViewModel
    {
        public ContentAViewModel(IContentAView view)
        {
            View = view;
            View.ViewModel = this;
        }

        public IView View { get; set; }

        public string Message { get; set; }
    }
}
