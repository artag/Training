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
            // Регистрация модулей в регионах View Injection (2 способа).
            // Способ 1.
            var toolbarAViewModel = containerProvider.Resolve<IToolbarAViewModel>();
            _regionManager.AddToRegion(RegionNames.ToolbarRegion, toolbarAViewModel.View);

            // Способ 2. Здесь меняется свойство у ViewModel перед созданием View.
            var contentAViewModel = containerProvider.Resolve<IContentAViewModel>();
            contentAViewModel.Message = "Changed message";

            var region = _regionManager.Regions[RegionNames.ContentRegion];
            region.Add(contentAViewModel.View);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IContentAView, ContentAView>();
            containerRegistry.Register<IContentAViewModel, ContentAViewModel>();

            containerRegistry.Register<IToolbarAView, ToolbarAView>();
            containerRegistry.Register<IToolbarAViewModel, ToolbarAViewModel>();
        }
    }
}
