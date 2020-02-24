using PrismDemo.Infrastructure;

namespace ModuleA
{
    public interface IContentAViewModel : IViewModel
    {
        string Message { get; set; }
    }
}
