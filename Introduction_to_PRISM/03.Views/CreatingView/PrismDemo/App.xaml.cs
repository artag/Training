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
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void ConfigureRegionAdapterMappings(
            RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            // Регистрация кастомного Region адаптера для StackPanel.
            regionAdapterMappings.RegisterMapping(
                typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // Регистрация модуля ModuleA из кода (требует reference на модуль).
            var moduleAType = typeof(ModuleAModule);
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = moduleAType.Name,
                ModuleType = moduleAType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable,
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
