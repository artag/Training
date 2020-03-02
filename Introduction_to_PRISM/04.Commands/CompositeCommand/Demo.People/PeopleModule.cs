using Demo.Infrastructure;
using Demo.People.ViewModels;
using Demo.People.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Demo.People
{
    public class PeopleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            var vm = containerProvider.Resolve<IPersonViewModel>();
            vm.CreatePerson("Bob", "Smith");
            regionManager.AddToRegion(RegionNames.ContentRegion, vm.View);

            vm = containerProvider.Resolve<IPersonViewModel>();
            vm.CreatePerson("Karl", "Sums");
            regionManager.AddToRegion(RegionNames.ContentRegion, vm.View);

            vm = containerProvider.Resolve<IPersonViewModel>();
            vm.CreatePerson("Jeff", "Lock");
            regionManager.AddToRegion(RegionNames.ContentRegion, vm.View);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IPersonViewModel, PersonViewModel>();
            containerRegistry.Register<IPersonView, PeopleView>();
        }
    }
}
