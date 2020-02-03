using System.Windows;
using ModuleA;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;

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

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // Требуется reference на модуль.
            var moduleATtype = typeof(ModuleAModule);
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = moduleATtype.Name,
                ModuleType = moduleATtype.AssemblyQualifiedName,

                // Initialization options:
                // 1. InitializationMode.WhenAvailable
                // The module will be initialized when it is available on application start-up.
                // 2. InitializationMode.OnDemand
                // The module will be initialized when requested, and not automatically on application start-up.
                InitializationMode = InitializationMode.WhenAvailable,
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
