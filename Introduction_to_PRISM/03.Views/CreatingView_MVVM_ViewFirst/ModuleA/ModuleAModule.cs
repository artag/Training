using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismDemo.Infrastructure;

namespace ModuleA
{
    public class ModuleAModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleAModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Демонстрация View Discovery.
            _regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(ToolbarA));
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ContentA));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // View регистрируются не через интерфейс.
            containerRegistry.Register<ContentA>();
            containerRegistry.Register<ToolbarA>();

            // ViewModel регистрируется через интерфейс.
            containerRegistry.Register<IContentAViewModel, ContentAViewModel>();
            containerRegistry.Register<IToolbarAViewModel, ToolbarAViewModel>();
        }
    }
}
