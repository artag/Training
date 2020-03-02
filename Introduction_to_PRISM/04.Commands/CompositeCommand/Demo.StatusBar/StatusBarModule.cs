using Demo.Infrastructure;
using Demo.StatusBar.ViewModels;
using Demo.StatusBar.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Demo.StatusBar
{
    public class StatusBarModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            var vm = containerProvider.Resolve<IStatusBarViewModel>();
            regionManager.AddToRegion(RegionNames.StatusBarRegion, vm.View);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IStatusBarViewModel, StatusBarViewModel>();
            containerRegistry.Register<IStatusBarView, StatusBarView>();
        }
    }
}
