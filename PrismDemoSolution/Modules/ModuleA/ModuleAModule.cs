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
            // Views for Items Control Region
            containerRegistry.Register(typeof(ItemsControlView1));
            containerRegistry.RegisterSingleton(typeof(ItemsControlView2));

            // Views for Selector Region
            containerRegistry.Register<ComboBoxView1>();
            containerRegistry.RegisterSingleton<ComboBoxView2>();
            containerRegistry.Register<ListBoxView1>();
            containerRegistry.RegisterSingleton<ListBoxView2>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            AddViewsToItemsControl(containerProvider);
            AddViewToContentControl();
            AddViewToSelectorRegion(containerProvider);
        }

        /// <summary>
        /// Добавить Views в Shell, в Items Control (может содержать несколько контролов).
        /// </summary>
        /// <param name="containerProvider"></param>
        private void AddViewsToItemsControl(IContainerProvider containerProvider)
        {
            var region = _regionManager.Regions[RegionNames.ItemsControlRegion];

            // Создаются разные объекты одного типа и в ItemsControl можно добавить их все.
            region.Add(containerProvider.Resolve(typeof(ItemsControlView1)));
            region.Add(containerProvider.Resolve(typeof(ItemsControlView1)));

            // Получаем singleton. ItemsControl может содержать только 1 объект этого типа.
            // (Попытка добавления этого же объекта (singleton же!) вызовет исключение).
            region.Add(containerProvider.Resolve(typeof(ItemsControlView2)));
        }

        /// <summary>
        /// Добавить Views в Shell, в Content Control (может содержать один контрол).
        /// </summary>
        private void AddViewToContentControl()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ContentControlRegion, typeof(ContentControlView));
        }

        /// <summary>
        /// Добавить Views в Shell, в Selector Control.
        /// </summary>
        private void AddViewToSelectorRegion(IContainerProvider containerProvider)
        {
            var comboBoxRegion = _regionManager.Regions[RegionNames.ComboBoxRegion];
            comboBoxRegion.Add(containerProvider.Resolve<ComboBoxView1>());
            comboBoxRegion.Add(containerProvider.Resolve<ComboBoxView1>());
            comboBoxRegion.Add(containerProvider.Resolve<ComboBoxView2>());

            var listBoxRegion = _regionManager.Regions[RegionNames.ListBoxRegion];
            listBoxRegion.Add(containerProvider.Resolve<ListBoxView1>());
            listBoxRegion.Add(containerProvider.Resolve<ListBoxView1>());
            listBoxRegion.Add(containerProvider.Resolve<ListBoxView2>());

            var tabControlRegion = _regionManager.Regions[RegionNames.TabControlRegion];
            tabControlRegion.Add(containerProvider.Resolve<TabControlView1>());
            tabControlRegion.Add(containerProvider.Resolve<TabControlView1>());
            tabControlRegion.Add(containerProvider.Resolve<TabControlView2>());
        }
    }
}
