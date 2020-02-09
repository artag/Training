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
            // Здесь используется View Injection

            // View Injection. Способ 1.
            _regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(ToolbarA));

            // View Injection. Способ 2.
            var contentAView = containerProvider.Resolve<ContentA>();
            _regionManager.Regions[RegionNames.ContentRegion].Add(contentAView);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Регистрация User Control'ов. Без использования MVVM.
            containerRegistry.Register<ContentA>();
            containerRegistry.Register<ToolbarA>();
        }
    }
}
