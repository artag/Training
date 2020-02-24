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
            var regionManager = containerProvider.Resolve<IRegionManager>();

            // Демонстрация установки свойства.
            var contentAViewModel = containerProvider.Resolve<IContentAViewModel>();
            contentAViewModel.Message = "ContentA First View Model";

            // Регистрация View при помощи View Injection. Способ #1.
            var contentRegion = regionManager.Regions[RegionNames.ContentRegion];
            contentRegion.Add(contentAViewModel.View);

            // Демонстрация установки свойства.
            var toolbarAViewModel = containerProvider.Resolve<IToolbarAViewModel>();
            toolbarAViewModel.ButtonText = "ToolbarA View Model";

            // Регистрация View при помощи View Injection. Способ #2.
            regionManager.AddToRegion(RegionNames.ToolbarRegion, toolbarAViewModel.View);

            // Демонстрация установки второго View вместе с ViewModel для ContentA.
            // Использование Deactivate() вместе с установкой нового View.
            var contentAViewModel2 = containerProvider.Resolve<IContentAViewModel>();
            contentAViewModel2.Message = "ContentA Second View Model";

            contentRegion.Deactivate(contentAViewModel.View);
            contentRegion.Add(contentAViewModel2.View);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Использование MVVM. Регистрация View и ViewModel через интерфейсы.
            containerRegistry.Register<IContentAView, ContentA>();
            containerRegistry.Register<IContentAViewModel, ContentAViewModel>();

            containerRegistry.Register<IToolbarAView, ToolbarA>();
            containerRegistry.Register<IToolbarAViewModel, ToolbarAViewModel>();
        }
    }
}
