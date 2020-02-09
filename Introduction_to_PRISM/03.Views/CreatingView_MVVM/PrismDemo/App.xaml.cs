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
            // Регистрация модуля ModuleA с помощью кода (требуется reference на модуль).
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
