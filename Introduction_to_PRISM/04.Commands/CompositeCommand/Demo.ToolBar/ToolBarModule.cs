using Demo.Infrastructure;
using Demo.ToolBar.ViewModels;
using Demo.ToolBar.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Demo.ToolBar
{
    public class ToolBarModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            var vm = containerProvider.Resolve<IToolBarViewModel>();
            regionManager.AddToRegion(RegionNames.ToolBarRegion, vm.View);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IToolBarViewModel, ToolBarViewModel>();
            containerRegistry.Register<IToolBarView, ToolBarView>();
        }
    }
}
