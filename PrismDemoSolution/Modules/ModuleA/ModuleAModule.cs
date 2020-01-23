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

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register(typeof(ToolbarView));
            containerRegistry.RegisterSingleton(typeof(ToolbarView2));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            AddViewsToItemsControl(containerProvider);
            AddViewToContentControl();
        }

        /// <summary>
        /// Добавить Views в Shell, в ItemsControl (может содержать несколько контролов).
        /// </summary>
        /// <param name="containerProvider"></param>
        private void AddViewsToItemsControl(IContainerProvider containerProvider)
        {
            var region = _regionManager.Regions[RegionNames.ToolbarRegion];

            // Создаются разные объекты одного типа и в ItemsControl можно добавить их все.
            region.Add(containerProvider.Resolve(typeof(ToolbarView)));
            region.Add(containerProvider.Resolve(typeof(ToolbarView)));

            // Получаем singleton. ItemsControl может содержать только 1 объект этого типа.
            // (Попытка добавления этого же объекта (singleton же!) вызовет исключение).
            region.Add(containerProvider.Resolve(typeof(ToolbarView2)));
        }

        /// <summary>
        /// Добавить Views в Shell, в ContentControl (может содержать один контрол).
        /// </summary>
        private void AddViewToContentControl()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ContentView));
        }
    }
}
