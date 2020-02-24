using PrismDemo.Infrastructure;

namespace ModuleA
{
    public interface IToolbarAViewModel : IViewModel
    {
        string ButtonText { get; set; }
    }
}
