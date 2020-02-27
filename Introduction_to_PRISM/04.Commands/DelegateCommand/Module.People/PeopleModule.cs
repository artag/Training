using Demo.Infrastructure;
using Module.People.ViewModels;
using Module.People.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Module.People
{
    public class PeopleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            var vm = containerProvider.Resolve<IPersonViewModel>();
            regionManager.AddToRegion(RegionNames.ContentRegion, vm.View);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IPersonViewModel, PersonViewModel>();
            containerRegistry.Register<IPersonView, PersonView>();
        }
    }
}
