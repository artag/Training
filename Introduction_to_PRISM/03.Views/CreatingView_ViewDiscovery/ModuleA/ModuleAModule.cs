using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismDemo.Infrastructure;

namespace ModuleA
{
    public class ModuleAModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<RegionManager>();

            // Используется View Discovery.
            regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(ToolbarA));
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ContentA));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IContentAView, ContentA>();
            containerRegistry.Register<IContentAViewModel, ContentAViewModel>();

            containerRegistry.Register<IToolbarAView, ToolbarA>();
            containerRegistry.Register<IToolbarAViewModel, ToolbarAViewModel>();
        }
    }
}
