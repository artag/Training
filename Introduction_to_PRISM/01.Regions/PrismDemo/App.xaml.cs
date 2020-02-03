using System.Windows;
using System.Windows.Controls;
using ModuleA;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using PrismDemo.Infrastructure;

namespace PrismDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var catalog = new ModuleCatalog();
            catalog.AddModule(typeof(ModuleAModule));

            return catalog;
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping(
                typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
        }
    }
}
